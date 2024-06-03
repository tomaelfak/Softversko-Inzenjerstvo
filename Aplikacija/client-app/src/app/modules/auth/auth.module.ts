import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthComponent } from './components/auth/auth.component';
import {RouterModule} from "@angular/router";
import {FormsModule} from "@angular/forms";
import {MatFormFieldModule} from "@angular/material/form-field";
import {MatInput} from "@angular/material/input";
import {MatGridList, MatGridTile} from "@angular/material/grid-list";
import {MatButtonModule} from "@angular/material/button";
import {MatProgressSpinner} from "@angular/material/progress-spinner";
import { AdminPanelComponent } from './components/admin-panel/admin-panel.component';
import {MatTableModule} from "@angular/material/table";
import {MatPaginator} from "@angular/material/paginator";
import {MatSort, MatSortHeader} from "@angular/material/sort";
import {AuthGuard} from "./services/guards/auth.guard";
import {AdminGuard} from "./services/guards/admin.guard";



@NgModule({
  declarations: [
    AuthComponent,
    AdminPanelComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild([
      {path: '', component: AuthComponent},
      {path: 'admin-panel', canActivate: [AuthGuard, AdminGuard], component: AdminPanelComponent}
    ]),
    FormsModule,
    MatFormFieldModule,
    MatInput,
    MatGridList,
    MatGridTile,
    MatButtonModule,
    MatProgressSpinner,
    MatTableModule,
    MatPaginator,
    MatSort,
    MatSortHeader
  ],
})
export class AuthModule { }
