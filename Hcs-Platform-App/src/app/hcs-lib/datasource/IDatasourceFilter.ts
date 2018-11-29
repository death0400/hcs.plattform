export type IFilterOperator = '=' | '!=' | '>' | '>=' | '<' | '<=' | 'in' | 'notIn' | 'contains' | 'startswith' | 'endwith';
export interface IDatasourceFilter {
  property: string;
  operator: IFilterOperator;
  parameter: any;
  clone(): IDatasourceFilter;
  toString(): string;

}
