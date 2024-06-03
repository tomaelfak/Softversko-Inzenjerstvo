import { Injectable } from '@angular/core';
import {AuthService} from "../../../auth/services/auth.service";
import {GeocodingService} from "../geocoding/geocoding.service";
import {Court} from "../../models/court";
import {Event} from "../../models/event";
import {EventsService} from "./events.service";
import {CourtsService} from "../courts/courts.service";
import {Subject, Subscription} from "rxjs";
import {NearbyEvent} from "../../interfaces/nearby-event";

@Injectable({
  providedIn: 'root'
})
export class NearbyEventsService {
  nearbyEvents: NearbyEvent[] = [];
  eventsListChanged: Subject<NearbyEvent[]> = new Subject<NearbyEvent[]>();

  courtsSubscription?: Subscription;
  geoSubscription?: Subscription;

  userLatitude: number = 0;
  userLongitude: number = 0;
  constructor(private authService: AuthService,
              private geoService: GeocodingService,
              private eventService: EventsService,
              private courtsService: CourtsService) { }


  getNearbyEvents(){
    if(this.nearbyEvents.length === 0){
      this.fetchNearbyEvents();
    }else {
      this.eventsListChanged.next(this.nearbyEvents);
    }
  }

  fetchNearbyEvents(){
    this.geoSubscription = this.geoService.searchByName(this.authService.user.value!.address).subscribe((response) => {
      if(response.features.length > 0) {

        this.userLatitude = response.features[0].center[1];
        this.userLongitude = response.features[0].center[0];

        this.courtsSubscription = this.courtsService.courtsChanged.subscribe((courts: Court[]) =>{
          this.nearbyEvents = [];
          courts.forEach((court) => {
            const distance = this.getDistanceInMeters(this.userLatitude, this.userLongitude, court.latitude, court.longitude);
            if(distance < 1000){
              this.eventService.fetchCourtDetails(court.id).subscribe((court) => {
                court.activities.forEach((event) => {
                  if(new Date(event.date) >= new Date()) {
                    console.log(event);
                    this.filterEvents(court, event);
                  }
                });


              });
            }
          });
          //after all courts are checked, emit the nearby events
          this.eventsListChanged.next(this.nearbyEvents);

        });

        this.courtsService.getCourts();


      }
    });

  }

  private getDistanceInMeters(lat1: number, lon1: number, lat2: number, lon2: number)
  {
    // distance between latitudes
    // and longitudes
    let dLat = (lat2 - lat1) * Math.PI / 180.0;
    let dLon = (lon2 - lon1) * Math.PI / 180.0;

    // convert to radians
    lat1 = (lat1) * Math.PI / 180.0;
    lat2 = (lat2) * Math.PI / 180.0;

    // apply formulae
    let a = Math.pow(Math.sin(dLat / 2), 2) +
      Math.pow(Math.sin(dLon / 2), 2) *
      Math.cos(lat1) *
      Math.cos(lat2);
    let rad = 6371;
    let c = 2 * Math.asin(Math.sqrt(a));
    return rad * c * 1000;
  }

  private filterEvents(court: Court, event: Event){
    const eventDate = new Date(event.date);
    const today = new Date();
    const hours = today.getHours();
    eventDate.setHours(0, 0, 0, 0);
    today.setHours(0, 0, 0, 0);

    if(eventDate < today ){
      return;
    }
    if(eventDate.getDate() === today.getDate()
      && eventDate.getMonth() === today.getMonth()
      && eventDate.getFullYear() === today.getFullYear()
      && event.timeSlot.startTime < hours) {
      return;
    }
    if(event.participants.length >= event.maxParticipants){
      return;
    }
    for(let participant of event.participants){
      if(participant.username === this.authService.user.value!.username){
        return;
      }
    }


    const nearbyEvent: NearbyEvent = {
      eventId: event.id,
      courtId: court.id,
      courtName: court.name,
      courtImage: court.image,
      maximumPlayers: event.maxParticipants,
      participantUsernames: event.participants.map((participant) => participant.username),
      startTime: event.timeSlot.startTime,
      endTime: event.timeSlot.endTime,
      sport: event.sport,
      date: new Date(event.date)
    };

    this.nearbyEvents.push(nearbyEvent);

  }

  removeEvent(eventId: string){
    this.nearbyEvents = this.nearbyEvents.filter((event) => event.eventId !== eventId);
  }

  eventLeft(nearbyEvent: NearbyEvent){
    if(this.getDistanceInMeters(this.userLatitude, this.userLongitude, nearbyEvent.latitude!, nearbyEvent.longitude!) < 1000){
      this.nearbyEvents.push(nearbyEvent);
      this.eventsListChanged.next(this.nearbyEvents);
    }

  }

  unsubscribeFromCourts(){
    this.courtsSubscription?.unsubscribe();
    this.geoSubscription?.unsubscribe();
  }





}
