import { Directive, Input, ElementRef } from '@angular/core';

@Directive({
  selector: '[hcsSyncCellWidth]',
  exportAs: 'syncCellWidth'
})
export class SyncCellWidthDirective {

  constructor(private el: ElementRef) {

  }

  // tslint:disable-next-line:no-input-rename
  @Input('hcsSyncCellWidth') syncWith: SyncCellWidthDirective;
  private getCells() {
    const element = this.el.nativeElement as HTMLElement;
    if (element.tagName.toLowerCase() === 'tr') {
      return Array.from(element.querySelectorAll('td,th')) as HTMLElement[];
    } else {

    } return Array.from(element.querySelector('tr:first-child').querySelectorAll('td,th')) as HTMLElement[];
  }
  public sync() {
    if (this.syncWith) {
      const cells = this.getCells();
      const targetWidths = this.syncWith.getCellWidths();
      if (cells.length === targetWidths.length) {
        for (let i = 0, c = cells.length; i < c; i++) {
          cells[i].style.minWidth = cells[i].style.maxWidth = targetWidths[i];
        }

      } else {
        console.warn(`can't sync ${cells.length} cells with ${targetWidths.length} cells`);
      }
    }
  }
  public getCellWidths() {
    return this.getCells().map((x: HTMLElement) => getComputedStyle(x).width);
  }
}
