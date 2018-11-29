import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '../../../../../node_modules/@angular/material';

@Component({
  selector: 'hcs-alert',
  templateUrl: './alert.component.html',
  styleUrls: ['./alert.component.scss']
})
export class AlertComponent implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) public data: { title: string, message: string }) { }

  ngOnInit() {
  }

}
