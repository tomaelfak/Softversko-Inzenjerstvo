import {Component, Inject} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";

@Component({
  selector: 'app-edit-bio-dialog',
  templateUrl: './edit-bio-dialog.component.html',
  styleUrl: './edit-bio-dialog.component.css'
})
export class EditBioDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<EditBioDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: {bio: string},
  ) {}

  onNoClick(): void {
    this.dialogRef.close();
  }

}