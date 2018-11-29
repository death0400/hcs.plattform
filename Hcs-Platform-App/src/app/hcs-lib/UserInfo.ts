import { DataSourceFactory } from './datasource/DataSourceFactory';
import { StaticClassProvider } from '@angular/core/src/di/provider';
import { PlatformUserInfo } from '../hcs-models/PlatformUserInfo';
import { Observable, ReplaySubject } from 'rxjs';

export class UserInfo {
    public static provider: StaticClassProvider = {
        provide: UserInfo,
        useClass: UserInfo,
        deps: [DataSourceFactory]
    };
    constructor(private df: DataSourceFactory) {

    }
    public getUserInfo(id: number): Observable<PlatformUserInfo> {
        const replay = new ReplaySubject<PlatformUserInfo>();
        const cached = sessionStorage.getItem('platform-user-' + id);
        if (cached) {
            replay.next(JSON.parse(cached));
            replay.complete();
        } else {
            this.df.getDataSource(PlatformUserInfo).get(id).subscribe(user => {
                sessionStorage.setItem('platform-user-' + id, JSON.stringify(user));
                replay.next(user);
                replay.complete();
            });
        }
        return replay;
    }
}
