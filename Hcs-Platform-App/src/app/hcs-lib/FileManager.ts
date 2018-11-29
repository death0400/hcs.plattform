import { PlatformFileInfo } from '../hcs-models/PlatformFileInfo';
import { DomSanitizer } from '@angular/platform-browser';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Router, NavigationStart } from '@angular/router';
import { Observable, of, combineLatest, ReplaySubject } from 'rxjs';
import { tap, first } from 'rxjs/operators';
import { StaticClassProvider } from '@angular/core/src/di/provider';

export class FileManager {
    public static provider: StaticClassProvider = { provide: FileManager, useClass: FileManager, deps: [HttpClient, DomSanitizer, Router] };
    cacheData: { [key: string]: Promise<PlatformFileInfo> } = {};
    constructor(protected http: HttpClient, protected sanitizer: DomSanitizer, protected router: Router) {
        router.events.subscribe(event => {
            if (event instanceof NavigationStart) {
                this.cacheData = {};
            }
        });
    }
    public upload(file: File | Blob, name: string, dir: string): Observable<string> {
        const formData = new FormData();
        formData.append('file', file, name);
        return this.http.post(`api/file${dir ? `/${dir}` : ''}`, formData, { responseType: 'text' });
    }

    public getFileInfo(codes: string[]): Observable<{ [key: string]: PlatformFileInfo }> {
        const map = {};
        const waits = [];
        codes.forEach(x => {
            if (this.cacheData[x]) {
                waits.push(this.cacheData[x].then(v => {
                    map[x] = v;
                }));
            } else {
                this.cacheData[x] = new Promise((resolve) => {
                    this.http.get(`api/file/${x}`).subscribe((resp: PlatformFileInfo) => {
                        resolve(resp);
                    }, error => {
                        resolve(null);
                    });
                });
                waits.push(this.cacheData[x].then(v => {
                    map[x] = v;
                }));
            }
        });
        const s = new ReplaySubject<{ [key: string]: PlatformFileInfo }>();
        Promise.all(waits).then(() => {
            s.next(map);
            s.complete();
        });

        return s;
    }
    public getFileUrl(key: string, file: PlatformFileInfo) {
        return this.sanitizer.bypassSecurityTrustUrl(`api/file/${key}/${encodeURI(file.Name)}`);
    }
    public downloadFile(fileName: string, fileUrl: string, params: any, headers?: any) {
        this.http.get(fileUrl, { params: params, headers: headers || {}, responseType: 'blob' }).pipe(first()).subscribe(resp => {
            const url = window.URL.createObjectURL(resp);
            const a = document.createElement('a');
            document.body.appendChild(a);
            a.href = url;
            a.download = fileName;
            a.click();
            setTimeout(() => {
                a.remove();
                window.URL.revokeObjectURL(url);
            }, 100);

        });
    }
}
