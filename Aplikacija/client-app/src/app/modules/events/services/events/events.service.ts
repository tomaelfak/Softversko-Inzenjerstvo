import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {CourtDetails} from "../../interfaces/court-details";
import {TimeSlot} from "../../interfaces/time-slot";
import {Subject} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class EventsService {

  eventJoinedNearbyToDetail: Subject<{eventId: string, courtId: string}> = new Subject<{eventId: string, courtId: string}>();
  eventJoinedDetailToNearby: Subject<{eventId: string, courtId: string}> = new Subject<{eventId: string, courtId: string}>();

  constructor(private http: HttpClient) {
  }

  baseUrl = 'http://localhost:5000/api/Activities';

  addNewEvent(courtId: string, sport: string, date: Date, startTime: number, endTime: number, numberOfPlayers: number, description: string){
    const url = `${this.baseUrl}/${courtId}?StartTime=${startTime}&EndTime=${endTime}`;
    const body = {
      title: 'New Event',
      date: date.toISOString(),
      description: description,
      sport: sport,
      maxParticipants: numberOfPlayers,
    };

    return this.http.post<any>(url, body);
  }

  fetchCourtDetails(courtId: string) {
    return this.http.get<CourtDetails>(`http://localhost:5000/api/Court/${courtId}`);
  }

  participateInEvent(eventId: string) {
    return this.http.post<any>(`http://localhost:5000/api/Activities/${eventId}/participate`, {});
  }

  fetchScheduledSlots(courtId: string, date: Date) {
    const url = `http://localhost:5000/api/Court/${courtId}/scheduledslots?date=${date.toISOString()}`;
    return this.http.get<TimeSlot[]>(url);
  }

  deleteEvent(eventId: string){
    return this.http.delete(`http://localhost:5000/api/Activities/${eventId}`);
  }



}
