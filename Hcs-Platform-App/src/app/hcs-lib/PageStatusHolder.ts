import { ActivatedRoute, Router, NavigationStart, NavigationEnd, ResolveEnd, ResolveStart } from '@angular/router';
import { Debug } from './Debug';
import { Location } from '@angular/common';
import { FormGroup, FormBuilder } from '@angular/forms';
import { StaticClassProvider } from '@angular/core/src/di/provider';
export interface IPageStatusAccessor<T> {
    value: T;
}
export interface IPageFormAccessor {
    formGroup?: FormGroup;
    save?: () => void;
}
export class PageStatusAccessor<T> implements IPageStatusAccessor<T> {
    private _value: T;
    constructor(private valueGetter: () => T, private valueSetter: (value: T) => void) {
        this._value = this.valueGetter();
    }
    public get value() {
        return this.valueGetter();
    }
    public set value(value: T) {
        if (this._value !== value) {
            this._value = value;
            this.valueSetter(value);
        }
    }
}
export interface IPageStatusHolderUrlObject {
    url: string;
    clear?: boolean;
}
export class PageStatusHolder {
    static provider: StaticClassProvider = {
        provide: PageStatusHolder,
        useClass: PageStatusHolder,
        deps: [Router, Debug, FormBuilder, Location]
    };
    private siteState: any;
    private pageState: any;
    private currentPath: string;
    private pageKey: string;
    private clear = false;
    constructor(private router: Router, private debug: Debug, private formBuilder: FormBuilder, private location: Location) {
        this.siteState = JSON.parse(localStorage.getItem('site-state-container') || '{}');
        debug.log('PageStatusHolder constructor');
        router.events.subscribe(routeEvent => {
            if (routeEvent instanceof ResolveEnd) {
                const urlObj: IPageStatusHolderUrlObject = { url: routeEvent.url };
                if (/[?&]clear=1(&|$)/.test(routeEvent.url)) {
                    urlObj.url = urlObj.url.replace(/[?&]clear=1(&|$)/, '');
                    urlObj.clear = true;
                    this.clear = true;
                }
                this.currentPath = urlObj.url;
                this.pageKey = this.getPageKey();
                this.readState();
                this.clear = false;
                if (urlObj.clear) {
                    const parts = urlObj.url.split('?');
                    setTimeout(() => this.location.replaceState(parts[0], parts[1]));
                }
            }

        });
    }
    public clearNext() {
        this.clear = true;
    }
    public getAccessor<T>(key: string, scope: 'page' | 'site' = 'page'): IPageStatusAccessor<T> {
        this.debug.log('get accessor ' + key);
        let valueSetter: (value: T) => void;
        let valueGetter: () => T;
        if (scope === 'page') {
            valueSetter = (value) => {
                this.pageState[key] = value;
                this.saveStatePage();
            };
            valueGetter = () => this.pageState[key];
        } else {
            valueSetter = (value) => {
                this.siteState[key] = value;
                this.saveStateSite();
            };
            valueGetter = () => this.siteState[key];
        }
        const accessor = new PageStatusAccessor<T>(valueGetter, valueSetter);
        return accessor;
    }
    public bindFormGroup(group: FormGroup, name = 'form'): IPageFormAccessor {
        const accessor: IPageFormAccessor = {};
        accessor.formGroup = group;
        const formValueAccessor = this.getAccessor(name);
        accessor.formGroup.reset(formValueAccessor.value || {});
        accessor.save = () => {
            formValueAccessor.value = accessor.formGroup.value;
        };
        return accessor;
    }
    public getSavedFormGroup(config: { [key: string]: any }, name = 'form'): IPageFormAccessor {
        return this.bindFormGroup(this.formBuilder.group(config), name);
    }
    private getPageKey() {
        return `page-state-container.${this.currentPath}`;
    }
    private readState() {
        if (this.clear) {
            localStorage.removeItem(this.pageKey);
        }
        this.pageState = JSON.parse(localStorage.getItem(this.pageKey) || '{}');
        this.debug.log('read state');
    }
    private saveStateSite() {
        localStorage.setItem('site-state-container', JSON.stringify(this.siteState));
    }
    private saveStatePage() {
        localStorage.setItem(this.pageKey, JSON.stringify(this.pageState));
    }
}
