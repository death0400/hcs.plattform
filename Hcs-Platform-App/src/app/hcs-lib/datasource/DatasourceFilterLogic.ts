import { IDatasourceFilter } from './IDatasourceFilter';
export class DatasourceFilterLogic {
  public subGroups: DatasourceFilterLogic[] = [];
  public filters: any[] = [];
  constructor(public logic: 'and' | 'or') {

  }
  public clone(): DatasourceFilterLogic {
    const root = Object.assign(new DatasourceFilterLogic(this.logic), this);
    root.filters = [];
    root.subGroups = [];
    for (const f of this.filters) {
      if (f.clone) {
        root.filters.push(f.clone());
      } else if (typeof f === 'object') {
        root.filters.push(Object.assign({}, f));
      } else {
        root.filters.push(f);
      }
    }
    for (const g of this.subGroups) {
      root.subGroups.push(g.clone());
    }
    return root;
  }
}
