import { Component, OnInit, Input } from '@angular/core';
import { ErrorHelper } from '../../hcs-lib/ErrorHelper';

@Component({
  selector: 'hcs-error-summary',
  templateUrl: './error-summary.component.html',
  styleUrls: ['./error-summary.component.scss']
})
export class ErrorSummaryComponent implements OnInit {

  constructor() { }
  @Input() public errors: ErrorHelper;
  ngOnInit() {
  }

}
