import { HttpInterceptor, HttpHandler, HttpRequest, HttpEvent, HttpResponse, HttpErrorResponse, HttpHeaders, HTTP_INTERCEPTORS } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';
import { HttpClientStatus } from './HttpClientStatus';
import { UserState } from './UserState';
import { Dialog } from '../hcs-components/dialogs/Dialog';
import { StaticClassProvider } from '@angular/core/src/di/provider';
export class HcsHttpInterceptor implements HttpInterceptor {
    public static provider: StaticClassProvider = {
        provide: HTTP_INTERCEPTORS,
        useClass: HcsHttpInterceptor,
        deps: [HttpClientStatus, UserState, Dialog],
        multi: true
    };
    requesting = 0;
    requestingStatus = false;
    private timout;
    constructor(private httpStatus: HttpClientStatus, private user: UserState, private dialog: Dialog) {

    }
    private requestStart() {
        if (this.timout) {
            clearTimeout(this.timout);
        }
        this.requesting++;
        if (this.requesting) {
            if (this.requestingStatus === false) {
                this.httpStatus.emitLoading(true);
            }
            this.requestingStatus = true;
        }
    }
    private requestEnd() {
        if (this.timout) {
            clearTimeout(this.timout);
        }
        this.requesting--;
        if (this.requesting === 0) {
            this.timout = setTimeout(() => {
                this.requestingStatus = false;
                this.httpStatus.emitLoading(false);
            }, 100);
        }
    }
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        this.requestStart();
        let headers = new HttpHeaders();
        if (this.user.isLogin) {
            headers = new HttpHeaders({ 'Authorization': `Bearer ${this.user.token}` });
        }
        const authReq = req.clone({ headers: headers });
        return next.handle(authReq).pipe(tap(event => {
            if (event instanceof HttpResponse) {
                this.requestEnd();
            }
        }, (err: any) => {
            if (err instanceof HttpErrorResponse) {
                this.requestEnd();
                if (err.status === 401 || err.status === 403) {
                    this.user.logout();
                } else if (err.status === 500 && err.error) {
                    if (!(typeof err.error === 'object' && 'ValidationErrors' in err.error)) {
                        this.dialog.httpError(err);
                    }

                }
            }
        }));
    }


}
