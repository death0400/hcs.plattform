import { Component, OnInit, ViewChild } from '@angular/core';
import { HCS_FUNCTION_NAME } from '../../hcs-lib/Tokens';
import { PlatformUser } from '../../models/hcs-basic-model/PlatformUser';
import { FormGroup, FormControl, Validators, FormArray } from '@angular/forms';
import { DataSourceFactory } from '../../hcs-lib/datasource/DataSourceFactory';
import { IHttpDataSource } from '../../hcs-lib/datasource/IHttpDataSource';
import { FormState, FormPageComponent } from '../../hcs-components/form-page/form-page.component';
import { ActivatedRoute } from '@angular/router';
import { HcsValidators } from '../../hcs-lib/HcsValidators';
import { TranslateService } from '@ngx-translate/core';
import { IDataSource } from '../../hcs-lib/datasource/IDataSource';
import { PlatformGroup } from '../../models/hcs-basic-model/PlatformGroup';
import { PlatformUserGroup } from '../../models/hcs-basic-model/PlatformUserGroup';
import { BatchUpdateModel } from '../../hcs-platform-model/BatchUpdateModel';
import { HttpClient } from '@angular/common/http';
import { UserState } from '../../hcs-lib/UserState';
import { PasswordHash } from '../../hcs-lib/PasswordHash';
@Component({
  selector: 'hcs-platform-user-form',
  templateUrl: './platform-user-form.component.html',
  styleUrls: ['./platform-user-form.component.scss'],
  providers: [
    {
      provide: HCS_FUNCTION_NAME,
      useValue: 'Basic.PlatformUser'
    }
  ]
})
export class PlatformUserFormComponent implements OnInit {

  formGroup: FormGroup;
  groupFormGroup: FormGroup;
  datasource: IHttpDataSource<PlatformUser>;
  groups: PlatformGroup[];
  groupGruopModel = {};
  @ViewChild(FormPageComponent) formPage: FormPageComponent<PlatformUser>;
  constructor(private passwordHash: PasswordHash, private datasourceFactory: DataSourceFactory, route: ActivatedRoute, translate: TranslateService, private http: HttpClient, public currentUser: UserState) {
    this.datasource = datasourceFactory.getDataSource(PlatformUser);
    translate.get('models.BaseModels.PlatformUser.Password').subscribe(name => {
      this.formGroup = new FormGroup({
        Id: new FormControl(),
        Name: new FormControl(null, Validators.required),
        Account: new FormControl(null, Validators.required),
        Email: new FormControl(null, Validators.email),
        Status: new FormControl(null, Validators.required),
        Password: new FormControl(null, (c) => route.snapshot.params['id'] ? null : Validators.required(c)),
        ConfirmPassword: new FormControl(null, HcsValidators.sameAs('Password', name))
      });
    });
    const groupGroupMap = {};
    this.datasourceFactory.getDataSource(PlatformGroup).getObservable().subscribe(x => {
      this.groups = x.data;
      x.data.forEach(y => {
        this.groupGruopModel[y.Id] = {
          PlatformGroupId: y.Id,
          Checked: false
        };
        groupGroupMap[y.Id] = new FormGroup({
          PlatformGroupId: new FormControl(y.Id),
          Checked: new FormControl()
        });
      });
      this.groupFormGroup = new FormGroup(groupGroupMap);
      this.formPage.isReadonlyState.subscribe(r => {
        if (r) {
          this.groupFormGroup.disable({ onlySelf: false });
        } else {
          this.groupFormGroup.enable({ onlySelf: false });
        }
      });
      route.params.subscribe(p => {
        this.readUserGroup();
      });
      this.formPage.modelSaved.subscribe(id => {
        this.saveUserGroups();
      });
    });

  }
  public saveUserGroups() {
    // tslint:disable-next-line:triple-equals
    if (this.formPage.dataId == this.currentUser.id) {
      return;
    }
    const body = new BatchUpdateModel<PlatformUserGroup>();
    if (this.groupFormGroup.valid) {
      Object.keys(this.groupFormGroup.value).forEach(pk => {
        if (this.groupFormGroup.value[pk]['Checked']) {
          const value = this.groupFormGroup.value[pk] as PlatformUserGroup;
          value.PlatformUserId = this.formPage.dataId;
          body.Entities.push(value);
        }
      });
      this.http.put('api/entity/updateGroups/' + this.formPage.dataId, body).subscribe(x => {
        this.readUserGroup();
      });
    }
  }
  private readUserGroup() {
    const ds = this.datasourceFactory.getDataSource(PlatformUserGroup);
    ds.where(x => x.PlatformUserId, '=', this.formPage.copyIdOrDataId).getObservable().subscribe(gp => {
      const m = Object.assign({}, this.groupGruopModel);
      gp.data.forEach(d => {
        m[d.PlatformGroupId] = {
          PlatformGroupId: d.PlatformGroupId,
          Checked: true
        };
      });
      this.groupFormGroup.reset(m);
    });
  }

  post() {
    return (model: PlatformUser, state: FormState) => {
      model = Object.assign({}, model);
      if (model.Password) {
        model.Password = this.passwordHash.hash(model.Password);
      }
      return model;
    };
  }
  ngOnInit() {
  }
}
