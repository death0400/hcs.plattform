import { Component, OnInit, Input, Inject, Optional } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FunctionRoute } from '../../FunctionRoute';
import { HCS_FUNCTION_NAME, HCS_FUNCTION_ROUTE } from '../../../hcs-lib/Tokens';

@Component({
  selector: 'hcs-copy-button',
  templateUrl: './copy-button.component.html',
  styleUrls: ['./copy-button.component.scss']
})
export class CopyButtonComponent implements OnInit {
  @Input() public key: any;
  @Input() public queryParams: any;
  constructor(private router: Router, private functionRoute: FunctionRoute, @Inject(HCS_FUNCTION_NAME) public fn: string, @Optional() @Inject(HCS_FUNCTION_ROUTE) public fr: string) { }
  public clickCopy() {
    this.router.navigate(this.functionRoute.getFunctionRoute(this.fn, this.fr).concat(['new', this.key]), { queryParamsHandling: 'merge', queryParams: this.queryParams });
  }
  ngOnInit() {
  }

}
