import { Directive, ElementRef, Input } from '@angular/core';

@Directive({
  selector: '[hcsSyncWidth]',
  exportAs: 'syncWidth'
})
export class SyncWidthDirective {

  constructor(private el: ElementRef) {

  }

  // tslint:disable-next-line:no-input-rename
  @Input('hcsSyncWidth') syncWith: SyncWidthDirective;
  public getWidth() {
    return getComputedStyle(this.el.nativeElement as HTMLElement).width;
  }
  public sync() {
    if (this.syncWith) {
      const el = (this.el.nativeElement as HTMLElement);
      el.style.minWidth = el.style.maxWidth = this.syncWith.getWidth();
    }
  }
}
