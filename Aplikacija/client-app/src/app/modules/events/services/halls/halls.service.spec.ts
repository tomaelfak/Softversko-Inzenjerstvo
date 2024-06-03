import { TestBed } from '@angular/core/testing';

import { HallsService } from './halls.service';

describe('HallsService', () => {
  let service: HallsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(HallsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
