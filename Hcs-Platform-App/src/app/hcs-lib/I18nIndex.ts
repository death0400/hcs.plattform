export class I18nIndex {
    private _names: string[];
    public get names(): string[] {
        return this._names;
    }
    constructor(...names: string[]) {
        this._names = names;
    }
}
