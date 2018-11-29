import { IFilterOperator } from '../hcs-lib/datasource/IDatasourceFilter';

export interface IReferenceDialogFilter {
    operator: IFilterOperator;
    field: string;
    name: string;
    type?: string;
    data?: any;
}
