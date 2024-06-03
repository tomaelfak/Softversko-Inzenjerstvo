import {AfterViewInit, Component, Input, OnChanges, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {EventsService} from "../../../services/events/events.service";
import {Event} from "../../../models/event";
import {MatDialog} from "@angular/material/dialog";
import {DeleteDialogComponent} from "../../court-edit/delete-dialog/delete-dialog.component";
import {MatTable, MatTableDataSource} from "@angular/material/table";
import {MatPaginator} from "@angular/material/paginator";
import {MatSort} from "@angular/material/sort";

@Component({
  selector: 'app-hall-events-list',
  templateUrl: './hall-events-list.component.html',
  styleUrl: './hall-events-list.component.css'
})
export class HallEventsListComponent implements OnInit, OnDestroy, OnChanges, AfterViewInit{
  @Input() hallId?: string;

  displayedColumns: string[] = ['sport', 'user', 'date', 'time', 'participants', 'actions'];
  @ViewChild(MatTable, {static: true}) table!: MatTable<any>;
  @ViewChild(MatPaginator, {static: true}) paginator!: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort!: MatSort;

  events: Event[] = [];

  dataSource = new MatTableDataSource<Event>(this.events);



  constructor(private eventsService: EventsService, private dialog: MatDialog) {
  }

  ngOnInit(): void {

  }

  ngOnDestroy() {
  }

  ngOnChanges() {
    this.eventsService.fetchCourtDetails(this.hallId!).subscribe({
      next: (courtDetails) => {
        this.events = courtDetails.activities;
        this.table.renderRows();
      },
      error: (error) => {
        console.error(error);
      }
    })
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator!;
    this.dataSource.sort = this.sort!;
  }

  onDelete(id: string){
    const dialogRef = this.dialog.open(DeleteDialogComponent);

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.eventsService.deleteEvent(id).subscribe({
          next: () => {
            this.events = this.events.filter(event => event.id !== id);
            this.table.renderRows();
          },
          error: (error) => {
            console.error(error);
          }
        })
      }
    })



  }


}
