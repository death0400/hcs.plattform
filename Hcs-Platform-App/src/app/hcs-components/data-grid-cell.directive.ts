import { Directive, Input, TemplateRef, ViewContainerRef, OnChanges, SimpleChanges, ChangeDetectorRef, OnDestroy } from '@angular/core';
import { DataGridColumDirective } from './data-grid-colum.directive';
import { Subscription } from 'rxjs';

@Directive({
  selector: '[hcsDataGridCell]'
})
export class DataGridCellDirective implements OnChanges, OnDestroy {

  private hasView = false;
  @Input() public columnIndex: number;
  @Input() public context: any;
  // tslint:disable-next-line:no-input-rename
  @Input('hcsDataGridCell') public templateRef: TemplateRef<any>;

  constructor(private viewContainerRef: ViewContainerRef, private cdr: ChangeDetectorRef) { }
  ngOnChanges(changes: SimpleChanges): void {
    this.updateView();
  }
  ngOnDestroy(): void {

  }
  private updateView() {
    if (this.hasView) {
      this.viewContainerRef.clear();
    }
    this.viewContainerRef.createEmbeddedView(this.templateRef, this.context);
    this.hasView = true;
  }
}
