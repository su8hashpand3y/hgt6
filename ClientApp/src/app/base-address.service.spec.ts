import { TestBed, inject } from '@angular/core/testing';

import { BaseAddressService } from './base-address.service';

describe('BaseAddressService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [BaseAddressService]
    });
  });

  it('should be created', inject([BaseAddressService], (service: BaseAddressService) => {
    expect(service).toBeTruthy();
  }));
});
