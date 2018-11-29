import { HttpClient, HttpResponse } from '@angular/common/http';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { Inject, Optional } from '@angular/core';
import { ModuleWithProviders } from '@angular/compiler/src/core';
import { I18nIndex } from './I18nIndex';
import { Observable, Observer, combineLatest } from 'rxjs';


export class PlatformTranslateLoader implements TranslateLoader {
    constructor(private http: HttpClient, private index: I18nIndex) { }
    protected prifix = '/assets/i18n';
    protected pathObject(dist: Object, source: Object) {
        if (source !== undefined && source !== null) {
            for (const key of Object.keys(source)) {
                const value = source[key];
                if (typeof value === 'object') {
                    if (dist[key] === undefined) {
                        dist[key] = {};
                    }
                    this.pathObject(dist[key], value);
                } else {
                    dist[key] = value;
                }
            }
        }
    }
    /**
     * Gets the translations from the server
     * @param lang
     * @returns {any}
     */
    public getTranslation(lang: string): any {
        return Observable.create((observer: Observer<any>) => {
            let timeouted = false;
            const timer = setTimeout(() => {
                timeouted = true;
                observer.error('getTranslation timeout!');
                observer.complete();
            }, 10000);


            this.http.get(`${this.prifix}/${lang}.json`).subscribe(resp => {
                const observers = this.index.names.map(x => this.http.get(`${this.prifix}/${x}/${lang}.json`));
                combineLatest(observers).subscribe(responses => {
                    responses.forEach(x => this.pathObject(resp, x));
                    if (!timeouted) {
                        clearTimeout(timer);
                        observer.next(resp);
                        observer.complete();
                    }
                });
            });
        });
    }
}
