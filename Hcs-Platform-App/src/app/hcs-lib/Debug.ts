import { environment } from '../../environments/environment';
export class Debug {
    public log(message: any, ...optionalParameters: any[]) {
        if (!environment.production) {
            console.log.apply(this, [message].concat(optionalParameters));
        }
    }
}
