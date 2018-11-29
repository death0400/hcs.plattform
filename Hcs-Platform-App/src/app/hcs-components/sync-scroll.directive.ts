import { Directive, Input, ElementRef, HostListener } from '@angular/core';
import { SyncScrollContext } from '../hcs-lib/SyncScrollContext';

@Directive({
  selector: '[hcsSyncScroll]'
})
export class SyncScrollDirective {

  // tslint:disable-next-line:no-input-rename
  @Input('hcsSyncScroll') id: string;
  @HostListener('scroll') onscroll(event) {
    const el = this.el.nativeElement as HTMLElement;
    this.context.setScroll(this.id, el.scrollLeft, el.scrollTop);
  }
  @HostListener('wheel') onwheel(event) {
    const el = this.el.nativeElement as HTMLElement;
    this.context.setScroll(this.id, el.scrollLeft, el.scrollTop);
  }
  public setScroll(left: number, top: number) {
    const el = this.el.nativeElement as HTMLElement;
    el.scrollTop = top;
    el.scrollLeft = left;
  }
  constructor(private context: SyncScrollContext, private el: ElementRef) {
    context.regist({ component: this, getKey: () => this.id });
  }

}
