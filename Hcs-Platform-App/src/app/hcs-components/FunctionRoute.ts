import { StringHelper } from '../hcs-lib/StringHelper';


export class FunctionRoute {
    public getFunctionRoute(functionName: string, functionRoute: string) {
        if (functionRoute) {
            const parts = functionRoute.split('/');
            parts[0] = '/' + parts[0];
            return parts;
        } else {
            const parts = functionName.split('.').map(x => StringHelper.camelCaseToDash(x));
            return ['/' + parts[0], parts[1]];
        }
    }
}
