import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PlayerPageComponent } from './components/player-page/player-page.component';
import {RouterModule} from "@angular/router";
import {PlayerGuard} from "../auth/services/guards/player.guard";
import {MatGridListModule} from "@angular/material/grid-list";
import {MatButtonModule} from "@angular/material/button";
import {MatCardModule} from "@angular/material/card";
import {MatIconModule} from "@angular/material/icon";
import {MatDialogModule} from "@angular/material/dialog";
import {MatInputModule} from "@angular/material/input";
import {FormsModule} from "@angular/forms";
import { EditBioDialogComponent } from './components/edit-bio-dialog/edit-bio-dialog.component';
import { ReviewsListComponent } from './components/reviews-list/reviews-list.component';



@NgModule({
  declarations: [
  
    EditBioDialogComponent,
       PlayerPageComponent,
       ReviewsListComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild([
      {path: 'page/:username', canActivate: [PlayerGuard], component: PlayerPageComponent}
    ]),
    MatGridListModule,
    MatButtonModule,
    MatCardModule,
    MatIconModule,
    MatDialogModule,
    MatInputModule,
    FormsModule
  ]
})
export class PlayerModule { }
