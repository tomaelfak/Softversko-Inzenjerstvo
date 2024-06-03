export interface NearbyEvent {
  eventId: string;
  courtId: string;
  courtName: string;
  courtImage?: string;
  maximumPlayers: number;
  participantUsernames: string[];
  startTime: number;
  endTime: number;
  sport: string;
  date: Date;
  longitude?: number;
  latitude?: number;
}
