import { DatasourceOrder } from '../DatasourceOrder';
import { DatasourceFilterLogic } from '../DatasourceFilterLogic';
import { QueryOptions } from '../QueryOptions';
interface IOdataExpand {
  [property: string]: {
    select?: string[];
    expand?: IOdataExpand;
  };
}
export class OdataUrlBuilder {
  public static getOrderString(order: DatasourceOrder) {
    const direction = order.direction ? ' ' + order.direction : '';

    return order.property.split(',').map(x => `${x}${direction}`).join(',').replace(/\./g, '/');
  }
  public static getFilterString(filter: DatasourceFilterLogic) {
    const arr = (<Object[]>filter.subGroups).concat(<Object[]>filter.filters).map(f => {
      const fs = f.toString();
      return fs ? `(${fs})` : null;
    }).filter(f => f != null);
    return arr.join(` ${filter.logic.toString()} `);
  }
  constructor(public options: QueryOptions) {

  }
  protected mapExpand(obj: IOdataExpand, select: string[]) {
    if (!(select[0] in obj)) {
      obj[select[0]] = {};
    }
    const current = obj[select[0]];
    if (select.length > 2) {
      if (!('expand' in current)) {
        current.expand = {};
      }
      this.mapExpand(current.expand, select.slice(1));
    } else if (select.length > 1) {
      if (!current.select) {
        current.select = [];
      }
      current.select.push(select[1]);
    }
  }
  protected buildExpand(obj: IOdataExpand) {
    return Object.keys(obj).map(x => {
      let ref = x;
      const sub = [];
      if (obj[x].select && obj[x].select.length) {
        sub.push(`$select=${obj[x].select.join(',')}`);
      }
      if (obj[x].expand && Object.keys(obj[x].expand)) {
        sub.push(`$expand=${this.buildExpand(obj[x].expand)}`);
      }
      if (sub.length) {
        ref += `(${sub.join(';')})`;
      }
      return ref;
    }).join(',');
  }
  public apply(params: { [key: string]: string }) {
    let filter: string;
    if (this.options.filterLogic.filters.length > 0 && (filter = OdataUrlBuilder.getFilterString(this.options.filterLogic))) {
      params['$filter'] = filter;
    }
    if (this.options.skip) {
      params['$skip'] = this.options.skip.toString();
    }
    if (this.options.take) {
      params['$top'] = this.options.take.toString();
    }
    if (this.options.orderby && this.options.orderby.length > 0) {
      params['$orderby'] = this.options.orderby.map(o => OdataUrlBuilder.getOrderString(o)).join(',');
    }
    if (this.options.apply) {
      params['$apply'] = this.options.apply;
    }
    const expandMap: IOdataExpand = {};
    if (this.options.select) {
      const raw = this.options.select.split(',');
      raw.filter(x => /\//g.test(x)).forEach(x => this.mapExpand(expandMap, x.split('/')));
      const distinct = {};
      raw.filter(x => !/\//g.test(x)).forEach(x => distinct[x] = true);
      params['$select'] = Object.keys(distinct).join(',');
    }
    if (this.options.expand) {
      this.options.expand.split(',').forEach(x => this.mapExpand(expandMap, [x]));
    }
    const expandKeys = Object.keys(expandMap);
    if (expandKeys.length) {
      params['$expand'] = this.buildExpand(expandMap);
    }
    if (this.options.take || this.options.skip) {
      params['$count'] = 'true';
    }
  }

}
