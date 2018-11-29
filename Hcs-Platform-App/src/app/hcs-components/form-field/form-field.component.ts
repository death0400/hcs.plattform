import { Component, OnInit, Input } from '@angular/core';
import { Permission } from '../../hcs-lib/Permission';

@Component({
  selector: 'hcs-form-field',
  templateUrl: './form-field.component.html',
  styleUrls: ['./form-field.component.scss'],
  providers: [Permission.provider]
})
export class FormFieldComponent implements OnInit {
  @Input() label: string;
  constructor() { }
  ngOnInit() {
  }

}
