import { Observable, combineLatest, ReplaySubject } from 'rxjs';

export class MenuItem {
    public expand = true;
    constructor(
        public title: string,
        public icon: string,
        public route: any[],
        public click?: () => void,
        public children?: MenuItem[],
        public visible?: () => boolean,
        public color?: string
    ) {
        if (children && children.length) {
            this.visible = () => children.filter(x => x.visible).map(x => x.visible()).filter(x => x).length > 0;
        }
    }
}
