import {Component, OnDestroy, OnInit} from '@angular/core';
import {NearbyEvent} from "../../interfaces/nearby-event";
import {NearbyEventsService} from "../../services/events/nearby-events.service";
import {Subscription} from "rxjs";
import {EventsService} from "../../services/events/events.service";

@Component({
  selector: 'app-events-nearby',
  templateUrl: './events-nearby.component.html',
  styleUrl: './events-nearby.component.css'
})
export class EventsNearbyComponent implements OnInit, OnDestroy{

  eventList: NearbyEvent[] = []
  validAddress: boolean = false;
  eventListChangedSubscription: Subscription = new Subscription();
  eventJoinedSubscription?: Subscription;

  constructor(private nearbyEventsService: NearbyEventsService, private eventsService: EventsService) { }

  ngOnInit() {
    this.eventListChangedSubscription = this.nearbyEventsService.eventsListChanged.subscribe((events: NearbyEvent[]) => {
      this.eventList = events;
      this.validAddress = true;
    });
    this.nearbyEventsService.getNearbyEvents();

    this.eventJoinedSubscription = this.eventsService.eventJoinedDetailToNearby.subscribe((data) => {
      this.eventList = this.eventList.filter((event) => event.eventId !== data.eventId);
      this.nearbyEventsService.removeEvent(data.eventId);
    });

  }

  ngOnDestroy() {
    this.eventListChangedSubscription.unsubscribe();
    this.eventJoinedSubscription?.unsubscribe();
    this.nearbyEventsService.unsubscribeFromCourts();
  }

  quickJoinEvent(eventId: string, courtId: string){
    this.eventsService.participateInEvent(eventId).subscribe({
      next: (response) => {
        this.eventList = this.eventList.filter((event) => event.eventId !== eventId);
        this.nearbyEventsService.removeEvent(eventId);
        this.eventsService.eventJoinedNearbyToDetail.next({eventId: eventId, courtId: courtId});
      },
      error: (error) => {
        console.log(error);
      }
    });
    }

}
