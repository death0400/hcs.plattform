import { HcsPlatformModule } from './hcs-platform.module';

describe('HcsPlatformModule', () => {
  let hcsPlatformModule: HcsPlatformModule;

  beforeEach(() => {
    hcsPlatformModule = new HcsPlatformModule();
  });

  it('should create an instance', () => {
    expect(hcsPlatformModule).toBeTruthy();
  });
});
