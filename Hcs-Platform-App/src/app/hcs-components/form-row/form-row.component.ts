import { Component, OnInit, Input, HostBinding } from '@angular/core';

@Component({
  selector: 'hcs-form-row',
  templateUrl: './form-row.component.html',
  styleUrls: ['./form-row.component.scss']
})
export class FormRowComponent implements OnInit {

  @Input() @HostBinding('class.no-margin') public noMargin = false;
  @Input() @HostBinding('class.no-wrap') public noWrap = false;
  constructor() { }

  ngOnInit() {
  }

}
