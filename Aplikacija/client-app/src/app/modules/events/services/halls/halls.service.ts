import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Court} from "../../models/court";
import {Subject} from "rxjs";
import {CourtDataFormat} from "../../interfaces/court-data-format";

@Injectable({
  providedIn: 'root'
})
export class HallsService {
  halls: Court[] = [];

  hallsChanged = new Subject<Court[]>();

  baseUrl = 'http://localhost:5000/api/';


  constructor(private http: HttpClient) { }

  getHalls() {
    if(this.halls.length === 0) {
      this.fetchHalls();
    } else {
      this.hallsChanged.next(this.halls.slice());
    }
  }

  fetchHalls() {
    this.http.get<CourtDataFormat[]>('http://localhost:5000/api/Court/OwnCourts').subscribe({
      next: (data) =>{
        this.halls = [];
        data.forEach((courtData) => {
          this.halls.push(new Court(
            courtData.id,
            courtData.sport,
            courtData.isHall,
            courtData.name,
            courtData.latitude,
            courtData.longitude,
            courtData.address,
            courtData.startTime,
            courtData.endTime,
            courtData.pricePerHour,
            courtData.image));
        });
        this.hallsChanged.next(this.halls.slice());
      },
      error: (error) => {
        console.log(error);
      }
    });
  }

  deleteCourt(id: string){
    this.http.delete<any>(this.baseUrl+"Court/" + id).subscribe(response => {
      this.halls = this.halls.filter(court => court.id !== id);
      this.hallsChanged.next(this.halls.slice());
    });
  }

}
