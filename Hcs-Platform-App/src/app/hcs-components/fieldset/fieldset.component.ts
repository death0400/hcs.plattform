import { Component, OnInit, Input, HostBinding } from '@angular/core';

@Component({
  selector: 'hcs-fieldset',
  templateUrl: './fieldset.component.html',
  styleUrls: ['./fieldset.component.scss']
})
export class FieldsetComponent implements OnInit {

  @Input() public legend: string;
  constructor() { }

  ngOnInit() {
  }

}
