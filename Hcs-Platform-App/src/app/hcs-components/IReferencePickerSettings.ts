import { IReferenceDialogFilter } from './IReferenceDialogFilter';
import { IReferenceDialogColumn } from './IReferenceDialogColumn';
import { IDataSource } from '../hcs-lib/datasource/IDataSource';
import { ISortingState } from './data-grid/data-grid.component';


export interface IReferencePickerSettings {
    displayFormatter?: (entity: any) => string;
    displayField?: string;
    filters?: IReferenceDialogFilter[];
    columns: IReferenceDialogColumn[];
    datasource: IDataSource<any>;
    sortState?: ISortingState[];
}
