import { ReplaySubject } from 'rxjs';

export class HttpClientStatus {
    loading = new ReplaySubject<boolean>();
    emitLoading(value: boolean) {
        this.loading.next(value);
    }
}
