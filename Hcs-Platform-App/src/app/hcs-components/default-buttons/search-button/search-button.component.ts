import { Component, OnInit, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'hcs-search-button',
  templateUrl: './search-button.component.html',
  styleUrls: ['./search-button.component.scss']
})
export class SearchButtonComponent implements OnInit {

  @Output() click = new EventEmitter();
  constructor() { }

  ngOnInit() {
  }

}
