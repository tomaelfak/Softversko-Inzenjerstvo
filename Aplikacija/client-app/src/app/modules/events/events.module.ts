import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {EventsRoutingModule} from "./events-routing.module";
import { EventsDisplayComponent } from './components/events-display/events-display.component';
import { EventNewComponent } from './components/event-new/event-new.component';
import { HallsComponent } from './components/halls/halls.component';
import { EventsMapComponent } from './components/events-map/events-map.component';
import { EventsNearbyComponent } from './components/events-nearby/events-nearby.component';
import { EventDetailComponent } from './components/event-detail/event-detail.component';
import {MatGridList, MatGridTile, MatGridTileHeaderCssMatStyler} from "@angular/material/grid-list";
import {MatCardModule} from "@angular/material/card";
import {MatButtonToggle, MatButtonToggleGroup} from "@angular/material/button-toggle";
import {ControlComponent, MapComponent, MarkerComponent, NavigationControlDirective} from '@maplibre/ngx-maplibre-gl';
import {MatSelectModule} from "@angular/material/select";
import {MatDividerModule} from "@angular/material/divider";
import {MatButtonModule} from "@angular/material/button";
import { CourtNewComponent } from './components/court-new/court-new.component';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {MatInput} from "@angular/material/input";
import {MatProgressBar} from "@angular/material/progress-bar";
import {MatStepperModule} from "@angular/material/stepper";
import {MatDatepickerModule} from "@angular/material/datepicker";
import {MatNativeDateModule} from "@angular/material/core";
import { CourtEditComponent } from './components/court-edit/court-edit.component';
import {MatDialogModule} from "@angular/material/dialog";
import { DeleteDialogComponent } from './components/court-edit/delete-dialog/delete-dialog.component';
import { CourtEditDetailsComponent } from './components/court-edit/court-edit-details/court-edit-details.component';
import { EventScheduleComponent } from './components/event-new/event-schedule/event-schedule.component';
import {MatListModule} from "@angular/material/list";
import { HallNewComponent } from './components/halls/hall-new/hall-new.component';
import { HallEventsListComponent } from './components/halls/hall-events-list/hall-events-list.component';
import { HallsListComponent } from './components/halls/halls-list/halls-list.component';
import {MatSliderModule} from "@angular/material/slider";
import { HallEditComponent } from './components/halls/hall-edit/hall-edit.component';
import {MatExpansionModule} from "@angular/material/expansion";
import {MatIconModule} from "@angular/material/icon";
import {MatTableModule} from "@angular/material/table";
import {MatSortModule} from "@angular/material/sort";
import {MatPaginator} from "@angular/material/paginator";




@NgModule({
  declarations: [
    EventsDisplayComponent,
    EventNewComponent,
    HallsComponent,
    EventsMapComponent,
    EventsNearbyComponent,
    EventDetailComponent,
    CourtNewComponent,
    CourtEditComponent,
    DeleteDialogComponent,
    CourtEditDetailsComponent,
    EventScheduleComponent,
    HallNewComponent,
    HallEventsListComponent,
    HallsListComponent,
    HallEditComponent
  ],
  imports: [
    CommonModule,
    EventsRoutingModule,
    MatGridList,
    MatGridTile,
    MatGridTileHeaderCssMatStyler,
    MatButtonToggleGroup,
    MatButtonToggle,
    MapComponent,
    ControlComponent,
    NavigationControlDirective,
    MarkerComponent,
    MatSelectModule,
    MatCardModule,
    MatDividerModule,
    MatButtonModule,
    FormsModule,
    ReactiveFormsModule,
    MatInput,
    MatProgressBar,
    MatStepperModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatDialogModule,
    MatListModule,
    MatSliderModule,
    MatExpansionModule,
    MatIconModule,
    MatTableModule,
    MatSortModule,
    MatPaginator

  ]
})
export class EventsModule { }
