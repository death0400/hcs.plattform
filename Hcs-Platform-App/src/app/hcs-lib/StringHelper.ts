export module StringHelper {
    export function camelCaseToDash(str) {
        return str.replace(/([a-zA-Z])(?=[A-Z])/g, '$1-').toLowerCase();
    }
}
