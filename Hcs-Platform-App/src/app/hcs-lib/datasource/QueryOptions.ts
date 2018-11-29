import { DatasourceFilterLogic } from './DatasourceFilterLogic';
import { DatasourceOrder } from './DatasourceOrder';
export class QueryOptions {
  public filterLogic: DatasourceFilterLogic = new DatasourceFilterLogic('and');
  public orderby: DatasourceOrder[] = [];
  public take: number;
  public skip: number;
  public expand: string;
  public select: string;
  public apply: string;
  public clone(newFilter?: DatasourceFilterLogic): QueryOptions {
    const newed = new QueryOptions();
    newed.filterLogic = newFilter || this.filterLogic.clone();
    newed.skip = this.skip;
    newed.take = this.take;
    newed.expand = this.expand;
    newed.select = this.select;
    newed.orderby = this.orderby.map(x => x.clone());
    newed.apply = this.apply;
    return newed;
  }
}
