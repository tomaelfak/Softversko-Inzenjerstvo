import {Component, EventEmitter, OnDestroy, OnInit, Output} from '@angular/core';
import {HallsService} from "../../../services/halls/halls.service";
import {Court} from "../../../models/court";
import {Subscription} from "rxjs";
import {MatDialog} from "@angular/material/dialog";
import {DeleteDialogComponent} from "../../court-edit/delete-dialog/delete-dialog.component";

@Component({
  selector: 'app-halls-list',
  templateUrl: './halls-list.component.html',
  styleUrl: './halls-list.component.css'
})
export class HallsListComponent implements OnInit, OnDestroy {
    @Output() editHallEvent = new EventEmitter<Court>();
    @Output() showEventsEvent = new EventEmitter<Court>();


    hallsChangedSubscription?: Subscription;
    halls: Court[] = [];

    constructor(private hallsService: HallsService, private dialog: MatDialog) { }

    ngOnInit(): void {
      this.hallsChangedSubscription = this.hallsService.hallsChanged.subscribe((halls) => {
        this.halls = halls;
      })

      this.hallsService.getHalls();
    }

    ngOnDestroy(): void {
      this.hallsChangedSubscription?.unsubscribe();
    }

    editHall(hall: Court) {
      this.editHallEvent.emit(hall);
    }

    deleteHall(hallId: string) {
      const dialogRef = this.dialog.open(DeleteDialogComponent);

      dialogRef.afterClosed().subscribe(result => {
        if (result) {
          this.hallsService.deleteCourt(hallId);
        }
      })

    }

    showEvents(hall: Court) {
      this.showEventsEvent.emit(hall);
    }

}
