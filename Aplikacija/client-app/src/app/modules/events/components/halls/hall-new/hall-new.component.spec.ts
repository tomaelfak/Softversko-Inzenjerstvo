import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HallNewComponent } from './hall-new.component';

describe('HallNewComponent', () => {
  let component: HallNewComponent;
  let fixture: ComponentFixture<HallNewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [HallNewComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(HallNewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
