import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditBioDialogComponent } from './edit-bio-dialog.component';

describe('EditBioDialogComponent', () => {
  let component: EditBioDialogComponent;
  let fixture: ComponentFixture<EditBioDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [EditBioDialogComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(EditBioDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
