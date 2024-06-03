import {Component, OnDestroy, OnInit} from '@angular/core';
import {CourtsService} from "../../services/courts/courts.service";
import {Court} from "../../models/court";
import {Subscription} from "rxjs";
import {MatDialog} from "@angular/material/dialog";
import {DeleteDialogComponent} from "./delete-dialog/delete-dialog.component";

@Component({
  selector: 'app-court-edit',
  templateUrl: './court-edit.component.html',
  styleUrl: './court-edit.component.css'
})
export class CourtEditComponent implements OnInit, OnDestroy{
  courtsSubscription?: Subscription;
  courts: Court[] = [];
  editMode = false;
  selectedCourt?: Court;
  constructor(private courtsService: CourtsService, private dialog: MatDialog) {
  }
  ngOnInit() {
    this.courtsSubscription = this.courtsService.courtsChanged.subscribe((courts: Court[]) =>{
      this.courts = courts;
    })
    this.courtsService.getCourts();
  }

  ngOnDestroy() {
    this.courtsSubscription?.unsubscribe();
  }

  onClickEdit(court: Court){
    this.selectedCourt = court;
    this.editMode = true;
    console.log(this.selectedCourt);
  }

  onClickDelete(court: Court){
    const dialogRef = this.dialog.open(DeleteDialogComponent);

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.courtsService.deleteCourt(court.id);
      }
    })
  }


}
