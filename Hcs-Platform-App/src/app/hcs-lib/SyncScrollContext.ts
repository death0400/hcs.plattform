
export class SyncScrollContext {
    public components: { component: any, getKey: () => string }[] = [];
    public regist(componentRef: { component: any, getKey: () => string }) {
        this.components.push(componentRef);
    }
    public setScroll(key: string, left: number, top: number) {
        return this.components.filter(x => x.getKey() === key).forEach(c => {
            c.component.setScroll(left, top);
        });
    }
}
