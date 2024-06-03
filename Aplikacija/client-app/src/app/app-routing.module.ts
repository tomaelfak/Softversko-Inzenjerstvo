import { NgModule } from '@angular/core';
import {PreloadAllModules, RouterModule, Routes} from '@angular/router';

const appRoutes: Routes = [
  { path: '', redirectTo: '/events', pathMatch: 'full'},
  { path: 'events', loadChildren: () => import('./modules/events/events.module').then(m => m.EventsModule)},
  { path: 'teams', loadChildren: () => import('./modules/teams/teams.module').then(m => m.TeamsModule)},
  { path: 'auth', loadChildren: () => import('./modules/auth/auth.module').then(m => m.AuthModule)},
  { path: 'player', loadChildren: () => import('./modules/player/player.module').then(m => m.PlayerModule)}
];

@NgModule({
  imports: [RouterModule.forRoot(appRoutes, {preloadingStrategy: PreloadAllModules})],
  exports: [RouterModule]
})
export class AppRoutingModule { }
