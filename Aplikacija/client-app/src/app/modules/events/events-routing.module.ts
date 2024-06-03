import {RouterModule, Routes} from "@angular/router";
import {NgModule} from "@angular/core";
import {EventsDisplayComponent} from "./components/events-display/events-display.component";
import {EventNewComponent} from "./components/event-new/event-new.component";
import {HallsComponent} from "./components/halls/halls.component";
import {AuthGuard} from "../auth/services/guards/auth.guard";
import {PlayerGuard} from "../auth/services/guards/player.guard";
import {ManagerGuard} from "../auth/services/guards/manager.guard";
import {CourtNewComponent} from "./components/court-new/court-new.component";
import {AdminGuard} from "../auth/services/guards/admin.guard";
import {CourtEditComponent} from "./components/court-edit/court-edit.component";
import {PlayerAdminGuard} from "../auth/services/guards/player-admin.guard";

const routes: Routes = [
  { path: '',
    component: EventsDisplayComponent,
    children: [ ],
    canActivate: [AuthGuard, PlayerAdminGuard]
  },
  { path: 'new/:courtId/:sport', canActivate: [AuthGuard, PlayerGuard], component: EventNewComponent},
  { path: 'halls', canActivate: [AuthGuard, ManagerGuard], component: HallsComponent},
  { path: 'court/new', canActivate: [AuthGuard], component: CourtNewComponent},
  { path: 'court/edit', canActivate: [AuthGuard], component: CourtEditComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EventsRoutingModule{}
