import {Component, OnDestroy, OnInit} from '@angular/core';
import {Court} from "../../models/court";
import {Subscription} from "rxjs";
import {EventsService} from "../../services/events/events.service";
import {CourtsService} from "../../services/courts/courts.service";
import {Event} from "../../models/event";
import {AuthService} from "../../../auth/services/auth.service";
import {NearbyEvent} from "../../interfaces/nearby-event";
import {NearbyEventsService} from "../../services/events/nearby-events.service";

@Component({
  selector: 'app-event-detail',
  templateUrl: './event-detail.component.html',
  styleUrl: './event-detail.component.css'
})
export class EventDetailComponent implements OnInit, OnDestroy{
  selectedCourt: Court | null =  null;
  selectedCourtSubscription?: Subscription;
  eventJoinedSubscription?: Subscription;

  eventList: Event[] = [];

  constructor(private eventsService: EventsService,
              private courtsService: CourtsService,
              private authService: AuthService,
              private eventsNearbyService: NearbyEventsService) { }

  ngOnInit() {
    this.selectedCourtSubscription = this.courtsService.courtSelected.subscribe(
      court => {
        this.selectedCourt = court;
        this.eventsService.fetchCourtDetails(court.id).subscribe({
          next: courtDetails => {
            this.eventList = courtDetails.activities;
          },
          error: error => {
            console.error(error);
          }
        })
      }
    );

    this.eventJoinedSubscription = this.eventsService.eventJoinedNearbyToDetail.subscribe( data =>{
        if(this.selectedCourt?.id === data.courtId){
          this.eventList.forEach(event => {
            if(event.id === data.eventId){
              event.participants.push({
                appUserId: this.authService.user.value!.id,
                username: this.authService.user.value!.username
              });
            }
          })
        }
      });

  }

  ngOnDestroy() {
    this.selectedCourtSubscription?.unsubscribe();
    this.eventJoinedSubscription?.unsubscribe();
  }


  joinEvent(event: Event) {
    this.eventsService.participateInEvent(event.id).subscribe({
      next: () => {
        event.participants.push({
          appUserId: this.authService.user.value!.id,
          username: this.authService.user.value!.username
        });
        this.eventsService.eventJoinedDetailToNearby.next({eventId: event.id, courtId: this.selectedCourt!.id});
      },
      error: error => {
        console.error(error);
      }
    })
  }

  leaveEvent(event: Event) {
    this.eventsService.participateInEvent(event.id).subscribe({
      next: () => {
        event.participants = event.participants.filter(participant => participant.username !== this.authService.user.value!.username);
        this.updateNearbyEventsLeaving(event);
      },
      error: error => {
        console.error(error);
      }
    })
  }

  checkToJoin(event: Event) {
    let canJoin = true;
    event.participants.forEach(participant => {
      if (participant.username === this.authService.user.value?.username){
        canJoin = false;
      }
    })
    return canJoin;
  }
  checkToLeave(event: Event) {
    let canLeave = true;
    if(this.authService.user.value?.username === event.hostName){
      return true;
    }
    event.participants.forEach(participant => {
      if (participant.username === this.authService.user.value?.username){
        canLeave = false;
      }
    })
    return canLeave;
  }

  hasExpired(event: Event) {
    const today = new Date();
    const hours = today.getHours();
    today.setHours(0, 0, 0, 0);
    const eventDate = new Date(event.date);
    eventDate.setHours(0, 0, 0, 0);
    if(eventDate < today){
      return true;
    }
    if(eventDate.getDate() === today.getDate()
      && eventDate.getMonth() === today.getMonth()
      && eventDate.getFullYear() === today.getFullYear()
      && event.timeSlot.startTime < hours){
      return true;
    }
    return false;

  }

  updateNearbyEventsLeaving(event: Event){
    const nearbyEvent: NearbyEvent = {
      eventId: event.id,
      courtId: this.selectedCourt!.id,
      courtName: this.selectedCourt!.name,
      courtImage: this.selectedCourt!.image,
      maximumPlayers: event.maxParticipants,
      participantUsernames: event.participants.map((participant) => participant.username),
      startTime: event.timeSlot.startTime,
      endTime: event.timeSlot.endTime,
      sport: event.sport,
      latitude: this.selectedCourt!.latitude,
      longitude: this.selectedCourt!.longitude,
      date: new Date(event.date)
    };

    this.eventsNearbyService.eventLeft(nearbyEvent);

  }

}
