import { Component, OnInit, Output, EventEmitter, Input, Inject, Optional } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { HCS_FUNCTION_NAME, HCS_FUNCTION_ROUTE } from '../../../hcs-lib/Tokens';
import { FunctionRoute } from '../../FunctionRoute';

@Component({
  selector: 'hcs-edit-button',
  templateUrl: './edit-button.component.html',
  styleUrls: ['./edit-button.component.scss']
})
export class EditButtonComponent implements OnInit {
  @Input() public key: any;
  @Input() public queryParams: any;
  constructor(private router: Router, private functionRoute: FunctionRoute, @Inject(HCS_FUNCTION_NAME) public fn: string, @Optional() @Inject(HCS_FUNCTION_ROUTE) public fr: string) { }
  public editClick() {
    this.router.navigate(this.functionRoute.getFunctionRoute(this.fn, this.fr).concat([this.key, 'edit']), { queryParamsHandling: 'merge', queryParams: this.queryParams });
  }
  ngOnInit() {
  }

}
