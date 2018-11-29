import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormControl, Validators, FormArray } from '@angular/forms';
import { IHttpDataSource } from '../../hcs-lib/datasource/IHttpDataSource';
import { PlatformGroup } from '../../models/hcs-basic-model/PlatformGroup';
import { DataSourceFactory } from '../../hcs-lib/datasource/DataSourceFactory';
import { ActivatedRoute } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { HCS_FUNCTION_NAME } from '../../hcs-lib/Tokens';
import { OdataDataSource } from '../../hcs-lib/datasource/odata/OdataDataSource';
import { PlatformFunction } from '../../hcs-platform-model/PlatformFunction';
import { IDataSource } from '../../hcs-lib/datasource/IDataSource';
import { PlatformGroupRole } from '../../models/hcs-basic-model/PlatformGroupRole';
import { HttpClient } from '@angular/common/http';
import { FormPageComponent } from '../../hcs-components/form-page/form-page.component';
import { BatchUpdateModel } from '../../hcs-platform-model/BatchUpdateModel';

@Component({
  selector: 'hcs-platform-group-form',
  templateUrl: './platform-group-form.component.html',
  styleUrls: ['./platform-group-form.component.scss'],
  providers: [
    {
      provide: HCS_FUNCTION_NAME,
      useValue: 'Basic.PlatformGroup'
    }
  ]
})
export class PlatformGroupFormComponent implements OnInit {
  formGroup: FormGroup;
  datasource: IHttpDataSource<PlatformGroup>;
  permissionDataSource: IDataSource<PlatformFunction>;
  groupRoleDatasource: IDataSource<PlatformGroupRole>;
  permissionGroup: FormGroup;
  permissionModel: { [key: string]: PlatformGroupRole } = {};
  @ViewChild(FormPageComponent) formPage: FormPageComponent<PlatformGroup>;
  constructor(datasourceFactory: DataSourceFactory, route: ActivatedRoute, translate: TranslateService, private http: HttpClient) {
    this.datasource = datasourceFactory.getDataSource(PlatformGroup);
    this.formGroup = new FormGroup({
      Id: new FormControl(),
      Name: new FormControl(null, Validators.required),
      IsEnabled: new FormControl(null, Validators.required)
    });
    this.groupRoleDatasource = datasourceFactory.getDataSource(PlatformGroupRole);
    datasourceFactory.getDataSource(PlatformFunction).getObservable().subscribe(x => {
      const permissions = {};
      x.data.forEach(p => {
        p.Permissions.forEach(pp => {
          permissions[`${p.Code}.${pp}`] = new FormGroup({
            Permission: new FormControl(undefined, Validators.required),
            Id: new FormControl(null, Validators.required),
            FunctionCode: new FormControl(p.Code, Validators.required),
            FunctionRoleCode: new FormControl(pp, Validators.required),
            PlatformGroupId: new FormControl(this.formPage.dataId, Validators.required)
          });
          this.permissionModel[`${p.Code}.${pp}`] = {
            Id: 0,
            PlatformGroupId: 0,
            Permission: 0,
            FunctionCode: p.Code,
            FunctionRoleCode: pp
          };
        });
      });
      this.permissionGroup = new FormGroup(permissions);
      this.permissionDataSource = datasourceFactory.getDataSource(x.data);
      this.formPage.isReadonlyState.subscribe(r => {
        if (r) {
          this.permissionGroup.disable({ onlySelf: false });
        } else {
          this.permissionGroup.enable({ onlySelf: false });
        }
      });
      route.params.subscribe(p => {
        if (this.formPage.copyIdOrDataId) {
          this.readGroupRoles();
        } else {
          this.permissionGroup.reset(this.permissionModel);
        }
      });
      this.formPage.modelSaved.subscribe(id => {
        if (this.formPage.state === 'copy' || this.permissionGroup.touched) {
          this.savePermission();
        }
      });
    });

  }
  private readGroupRoles() {
    const pm = Object.assign({}, this.permissionModel);
    this.groupRoleDatasource.where(x => x.PlatformGroupId, '=', this.formPage.copyIdOrDataId).getObservable().subscribe(r => {
      r.data.forEach(rr => {
        pm[`${rr.FunctionCode}.${rr.FunctionRoleCode}`] = rr;
        if (this.formPage.state === 'copy') {
          rr.Id = 0;
        }
      });
      this.permissionGroup.reset(pm);
    });
  }

  public savePermission() {
    const body = new BatchUpdateModel<PlatformGroupRole>();
    if (this.permissionGroup.valid) {
      Object.keys(this.permissionGroup.value).forEach(pk => {
        const value = this.permissionGroup.value[pk] as PlatformGroupRole;
        value.PlatformGroupId = this.formPage.dataId;
        body.Entities.push(value);
      });
      this.http.put('api/entity/updateRoles/' + this.formPage.dataId, body).subscribe(x => {
        this.readGroupRoles();
      });
    }

  }
  public getControl(code: string) {
    const ctrl = this.permissionGroup.controls[code].get('Permission');
    return ctrl;
  }
  ngOnInit() {
  }

}
