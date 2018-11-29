import { IDatasourceFilter, IFilterOperator } from '../IDatasourceFilter';

export class OdataFilter implements IDatasourceFilter {

  public not: boolean;
  public stringOrDateFunction: 'length' | 'indexof' | 'replace' | 'substring' | 'tolower' | 'toupper' | 'trim' | 'concat'
    | 'day' | 'hour' | 'minute' | 'month' | 'second' | 'year' | 'round' | 'floor' | 'ceiling';
  public functionParameters: any[] = [];
  public propertyType: string;
  constructor(public property: string, public operator: IFilterOperator, public parameter: any) {

  }
  public getProperty(): string {
    if (this.stringOrDateFunction) {
      let pl = [];
      pl.push(this.property.replace(/\./g, '/'));
      pl = pl.concat(this.functionParameters);
      return `${this.not ? 'not ' : ''}${this.stringOrDateFunction}(${pl.join(',')})`;
    }
    return this.property.replace(/\./g, '/');
  }
  clone(): IDatasourceFilter {
    return Object.assign(new OdataFilter(this.property, this.operator, this.parameter), this);
  }
  public toString(): string {
    if (!this[this.operator]) {
      throw Error(`operator ${this.operator} not exists`);
    }
    if (!this.parameter === undefined) {
      throw Error('parameter is undefined, use null to compare null,\'\' to compare empty');
    }
    return this[<string>this.operator](this.getEscapedParameter(this.parameter));
  }
  private formatDate(date: Date) {
    return date.toISOString();
  }
  public getEscapedParameter(param: any): string {
    if (this.propertyType) {
      if (typeof param !== 'number' && param !== undefined && param !== null) {
        switch (this.propertyType) {
          case 'Number':
            const parsed = parseFloat(param.toString());
            if (!Number.isNaN(parsed)) {
              param = parsed;
            }
            break;
          case 'String':
            param = param.toString();
            break;
        }
      }
    }
    let escapedParam = param;
    if (escapedParam === null || escapedParam === undefined) {
      escapedParam = 'null';
    } else if (this.propertyType === 'String') {
      escapedParam = `'${escapedParam.replace(/'/g, '\'\'')}'`;
    } else if (escapedParam instanceof Date) {
      escapedParam = this.formatDate(escapedParam);
    } else if (typeof escapedParam === 'object' && '_d' in escapedParam && escapedParam._d instanceof Date) {
      escapedParam = this.formatDate(escapedParam._d);
    } else if (typeof escapedParam === 'string') {
      escapedParam = `'${escapedParam.replace(/'/g, '\'\'')}'`;
    } else {
      escapedParam = escapedParam.toString();
    }
    return escapedParam;
  }
  private getSimpleOperatorFilter = (operator: string, param: any) => `${this.getProperty()} ${operator} ${param}`;

  // operators
  '=' = (param: string) => this.getSimpleOperatorFilter('eq', param);
  '!=' = (param: string) => this.getSimpleOperatorFilter('ne', param);
  '>' = (param: string) => this.getSimpleOperatorFilter('gt', param);
  '>=' = (param: string) => this.getSimpleOperatorFilter('ge', param);
  '<' = (param: string) => this.getSimpleOperatorFilter('lt', param);
  '<=' = (param: string) => this.getSimpleOperatorFilter('le', param);
  contains(param: string): string {
    if (typeof this.parameter !== 'string') {
      throw Error('contains only support string');
    }
    if (!this.parameter) {
      return null;
    }
    return `contains(${this.getProperty()},${param})`;
  }
  startswith(param: string): string {
    if (typeof this.parameter !== 'string') {
      throw Error('startswith only support string');
    }
    if (!this.parameter) {
      return null;
    }
    return `startswith(${this.getProperty()},${param})`;
  }
  endwith(param: string): string {
    if (typeof this.parameter !== 'string') {
      throw Error('endwith only support string');
    }
    if (!this.parameter) {
      return null;
    }
    return `endwith(${this.getProperty()},${param})`;
  }
  in(param: any[]): string {
    if (this.parameter instanceof Array) {
      if (this.parameter.length === 0) {
        throw Error('empty array is not allowed');
      }
      return `(${this.parameter.map(item => `${this.getProperty()} eq ${this.getEscapedParameter(item)}`).join(' or ')})`;
    } else {
      throw Error('parameter not Array');
    }
  }
  notIn(param: any[]): string {
    if (this.parameter instanceof Array) {
      if (this.parameter.length === 0) {
        throw Error('empty array is not allowed');
      }
      return `(${this.parameter.map(item => `${this.getProperty()} ne ${this.getEscapedParameter(item)}`).join(' and ')})`;
    } else {
      throw Error('parameter not Array');
    }
  }
}
