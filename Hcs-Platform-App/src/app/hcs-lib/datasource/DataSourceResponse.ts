export class DataSourceResponse<T> {
    constructor(public data: T[], public totalCount: number) {

    }
}
