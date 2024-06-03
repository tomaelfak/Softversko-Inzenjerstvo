import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HallsListComponent } from './halls-list.component';

describe('HallsListComponent', () => {
  let component: HallsListComponent;
  let fixture: ComponentFixture<HallsListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [HallsListComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(HallsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
