export interface CourtDataFormat{
  id: string;
  sport: string;
  name: string;
  latitude: number;
  longitude: number;
  address: string;
  image: string;
  isHall: boolean;
  startTime: number;
  endTime: number;
  pricePerHour: number;
}
