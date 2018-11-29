import { HttpClient } from '@angular/common/http';
import { LoginResult } from '../hcs-models/LoginResult';
import { Subject, Observable, of, BehaviorSubject, ReplaySubject } from 'rxjs';
import { Router } from '@angular/router';
import { PasswordHash } from './PasswordHash';
import { first } from 'rxjs/operators';

export class UserState {
    token: string;
    name: string;
    id: number;
    roles: string[];
    public get isLogin(): boolean {
        return !!this.token;
    }
    constructor(private httpClient: HttpClient, private router: Router, private passwordHash: PasswordHash) {
        this.readSavedData();
        console.log('init user state');
    }
    private loading = false;

    public logout() {
        this.clearLoginData();
        this.router.navigateByUrl('/login');
    }
    public login(account: string, password: string): Subject<LoginResult> {
        this.clearLoginData();
        const sub = new Subject<LoginResult>();
        this.httpClient.post('api/Login', {
            Account: account,
            Password: this.passwordHash.hash(password)
        }).subscribe((resp: LoginResult) => {
            if (resp.Succeeded) {
                this.saveLoginData(resp);
                this.readSavedData();
            }
            sub.next(resp);
        }, resp => {
            sub.next(resp.error);
        });
        return sub;
    }
    private saveLoginData(login: LoginResult) {
        localStorage.setItem('user-id', login.User.Id.toString());
        localStorage.setItem('user-name', login.User.Name);
        localStorage.setItem('user-token', login.Token);
        localStorage.setItem('user-roles', JSON.stringify(login.Roles));
    }
    private readSavedData() {
        const ids = localStorage.getItem('user-id');
        if (ids) {
            this.id = parseInt(ids, 10);
        }
        this.name = localStorage.getItem('user-name');
        this.token = localStorage.getItem('user-token');
        this.roles = JSON.parse(localStorage.getItem('user-roles') || '[]');
    }
    private clearLoginData() {
        this.id = undefined;
        this.name = undefined;
        this.token = undefined;
        localStorage.removeItem('user-id');
        localStorage.removeItem('user-name');
        localStorage.removeItem('user-token');
        localStorage.removeItem('user-roles');
    }
}
