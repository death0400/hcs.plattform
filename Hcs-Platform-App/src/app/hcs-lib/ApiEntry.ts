import 'reflect-metadata';
export function Property() {
    return function (target: Object, propertyKey: string) {
        const type = Reflect.getMetadata('design:type', target, propertyKey);
        const typeName = type.name;
        if (!target['$$properties']) {
            target['$$properties'] = {};
        }
        if (!(propertyKey in target['$$properties'])) {
            let attr;
            attr = getPropertyTypeAttr(type, attr, propertyKey, typeName);
            Object.defineProperty(target['$$properties'], propertyKey, attr);
        }
        if (!target['$$properties_name']) {
            target['$$properties_name'] = {};
        }
        if (!(propertyKey in target['$$properties_name'])) {
            let attr;
            attr = getPropertyNameAttr(type, attr, propertyKey, typeName);
            Object.defineProperty(target['$$properties_name'], propertyKey, attr);
        }
    };
}
export interface IApiEntry {
    apiEntry: {
        path?: string,
        identityProperty?: string,
        expand: string[],
        select: string[]
    };
}
function getPropertyTypeAttr(type: any, attr: any, key: string, typeName: any) {
    switch (type) {
        case Array:
        case String:
        case Boolean:
        case Function:
        case String:
        case Date:
        case Symbol:
        case Number:
            attr = {
                enumerable: false,
                configurable: false,
                value: type.name
            };
            break;
        default:
            attr = {
                enumerable: false,
                configurable: false,
                get: function () {
                    const obj = new type();
                    return obj.$$properties;
                }
            };
    }
    return attr;
}
function getPropertyNameAttr(type: any, attr: any, key: string, typeName: any) {
    switch (type) {
        case Array:
        case String:
        case Boolean:
        case Function:
        case String:
        case Date:
        case Symbol:
        case Number:
            attr = {
                enumerable: false,
                configurable: false,
                get: function () {
                    let k = key;
                    if (this['$$prefix']) {
                        k = this['$$prefix'] + '.' + key;
                    }
                    return k;
                }
            };
            break;
        default:
            attr = {
                enumerable: false,
                configurable: false,
                get: function () {
                    const obj = new type();
                    const names = obj.$$properties_name;
                    if (names) {
                        names['$$prefix'] = this['$$prefix'] ? (this['$$prefix'] + '.' + typeName) : typeName;
                        return names;
                    }
                }
            };
    }
    return attr;
}

function getOrApplyapiEntry(target: Object | Function): IApiEntry {
    const tar: IApiEntry = (typeof target === 'object' ? target.constructor : target) as any;
    if (!('apiEntry' in tar)) {
        tar['apiEntry'] = {
            select: [],
            expand: []
        };
    }
    return tar;
}
export function Key() {
    return function (target: Object, propertyKey: string) {
        Property()(target, propertyKey);
        const obj = getOrApplyapiEntry(target);
        obj.apiEntry.identityProperty = propertyKey;
    };
}
export function Expand(select?: string) {
    return function (target: Object, propertyKey: string) {
        const obj = getOrApplyapiEntry(target);
        let expand = propertyKey;
        if (select) {
            expand += `($select=${select})`;
        }
        obj.apiEntry.expand.push(expand);
    };
}

export function Select(...path: string[]) {
    return function (target: Object, propertyKey: string) {
        const obj = getOrApplyapiEntry(target);
        if (path.length) {
            path.map(x => propertyKey + '.' + x).forEach(x => obj.apiEntry.select.push(x));
        } else {
            obj.apiEntry.select.push(propertyKey);
        }
    };
}
export function ApiEntry(path: string) {
    return function ViewModel(target) {
        const obj = getOrApplyapiEntry(target);
        obj.apiEntry.path = path;
    };
}

