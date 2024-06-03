import { Event } from '../models/event';
 export interface CourtDetails{
  id: string;
  sport: string;
  isHall: boolean;
  name: string;
  latitude: number;
  longitude: number;
  address: string;
  image?: string;
  activities: Event[];
  startTime: number;
  endTime: number;
  price: number;
}
