import {TimeSlot} from "../interfaces/time-slot";

export class Event{
  constructor(
    public id: string,
    public name: string,
    public description: string,
    public sport: string,
    public date: string,
    public numberOfParticipants: number,
    public maxParticipants: number,
    public timeSlot: TimeSlot,
    public isCancelled: boolean,
    public hostName: string,
    public participants: {appUserId: string, username: string}[]
  ) {
  }
}
