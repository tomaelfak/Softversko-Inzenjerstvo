import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TeamsComponent } from './components/teams/teams.component';
import { TableComponent } from './components/table/table.component';
import {TeamsRoutingModule} from "./teams-routing.module";



@NgModule({
  declarations: [
    TeamsComponent,
    TableComponent
  ],
  imports: [
    CommonModule,
    TeamsRoutingModule
  ]
})
export class TeamsModule { }
