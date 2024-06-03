import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HallEventsListComponent } from './hall-events-list.component';

describe('HallEventsListComponent', () => {
  let component: HallEventsListComponent;
  let fixture: ComponentFixture<HallEventsListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [HallEventsListComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(HallEventsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
