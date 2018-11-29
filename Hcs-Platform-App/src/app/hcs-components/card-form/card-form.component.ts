import { Component, OnInit, Input, ChangeDetectionStrategy } from '@angular/core';

@Component({
  selector: 'hcs-card-form',
  templateUrl: './card-form.component.html',
  styleUrls: ['./card-form.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CardFormComponent implements OnInit {

  @Input() public title: string;
  constructor() { }

  ngOnInit() {
  }

}
