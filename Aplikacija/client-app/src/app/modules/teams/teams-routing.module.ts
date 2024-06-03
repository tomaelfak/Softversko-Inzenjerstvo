import {NgModule} from "@angular/core";
import {RouterModule, Routes} from "@angular/router";
import {TableComponent} from "./components/table/table.component";
import {TeamsComponent} from "./components/teams/teams.component";
import {AuthGuard} from "../auth/services/guards/auth.guard";
import {PlayerGuard} from "../auth/services/guards/player.guard";

const routes: Routes = [
  { path: '',
    component: TeamsComponent,
    children: [ ],
    canActivate: [AuthGuard, PlayerGuard]
  },
  { path: 'table', canActivate: [], component: TableComponent}
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TeamsRoutingModule {}
