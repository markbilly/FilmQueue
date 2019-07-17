import { TestBed } from '@angular/core/testing';

import { WatchNextService } from './watch-next.service';

describe('WatchNextService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: WatchNextService = TestBed.get(WatchNextService);
    expect(service).toBeTruthy();
  });
});
