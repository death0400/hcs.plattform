import { Component, OnInit, Input, Inject, Optional } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FunctionRoute } from '../../FunctionRoute';
import { HCS_FUNCTION_NAME, HCS_FUNCTION_ROUTE } from '../../../hcs-lib/Tokens';

@Component({
  selector: 'hcs-create-button',
  templateUrl: './create-button.component.html',
  styleUrls: ['./create-button.component.scss']
})
export class CreateButtonComponent implements OnInit {
  constructor(private router: Router, private functionRoute: FunctionRoute, @Inject(HCS_FUNCTION_NAME) public fn: string, @Optional() @Inject(HCS_FUNCTION_ROUTE) public fr: string) { }
  @Input() queryParams: any;
  ngOnInit() {
  }
  clickCreate() {
    this.router.navigate(this.functionRoute.getFunctionRoute(this.fn, this.fr).concat(['new']), { queryParamsHandling: 'merge', queryParams: this.queryParams });
  }
}
