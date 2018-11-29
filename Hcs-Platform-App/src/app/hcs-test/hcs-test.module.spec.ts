import { HcsTestModule } from './hcs-test.module';

describe('HcsTestModule', () => {
  let hcsTestModule: HcsTestModule;

  beforeEach(() => {
    hcsTestModule = new HcsTestModule();
  });

  it('should create an instance', () => {
    expect(hcsTestModule).toBeTruthy();
  });
});
