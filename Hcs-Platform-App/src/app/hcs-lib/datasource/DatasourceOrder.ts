export class DatasourceOrder {
  public clone(): DatasourceOrder {
    return new DatasourceOrder(this.property, this.direction);
  }
  constructor(public property: string, public direction?: 'asc' | 'desc') {

  }

}
