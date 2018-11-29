import { HcsBasicModule } from './hcs-basic.module';

describe('HcsBasicModule', () => {
  let hcsBasicModule: HcsBasicModule;

  beforeEach(() => {
    hcsBasicModule = new HcsBasicModule();
  });

  it('should create an instance', () => {
    expect(hcsBasicModule).toBeTruthy();
  });
});
