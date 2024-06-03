import {Component, OnDestroy, OnInit} from '@angular/core';
import {Court} from "../../models/court";
import {EventsService} from "../../services/events/events.service";
import {Subscription} from "rxjs";
import {AuthService} from "../../../auth/services/auth.service";

@Component({
  selector: 'app-events-display',
  templateUrl: './events-display.component.html',
  styleUrl: './events-display.component.css'
})
export class EventsDisplayComponent {
  constructor(public authService: AuthService) {
  }

}
