import {EventEmitter, Injectable} from '@angular/core';
import {Court} from "../../models/court";
import {BehaviorSubject, Subject} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {CourtDataFormat} from "../../interfaces/court-data-format";
import {CourtCreateData} from "../../interfaces/court-create-data";
import {AuthService} from "../../../auth/services/auth.service";
import {HallCreateData} from "../../interfaces/hall-create-data";
import {HallsService} from "../halls/halls.service";

@Injectable({
  providedIn: 'root'
})
export class CourtsService {

  courtsChanged: Subject<Court[] > = new Subject<Court[]>();
  courts:Court[] =[];
  //   [
  //   new Court("1", "football", false, "teren1", 43.315243, 21.908565, "adresa1"),
  //   new Court("2", "football", true, "teren2", 43.320842, 21.930529, "adresa1", "https://www.hyndburnleisure.co.uk/wp-content/uploads/2023/08/footballpitch-1024x1024.jpeg"),
  //   new Court("3", "basketball", true, "teren3", 43.325954, 21.907874, "adresa1"),
  //   new Court("4", "basketball", false, "teren4", 43.316090, 21.925030, "adresa1"),
  //   new Court("5", "tennis", false, "teren5", 43.326636, 21.900686, "adresa1"),
  //   new Court("6", "tennis", true, "teren6", 43.313265, 21.910447, "adresa1"),
  // ];
  baseUrl = "http://localhost:5000/api/";

  courtSelected: EventEmitter<Court> = new EventEmitter<Court>();

  constructor(private http: HttpClient, private authService: AuthService, private hallsService: HallsService) { }

  getCourts(){
    if(this.courts.length===0){
      this.fetchCourts();
    }else{
      this.courtsChanged.next(this.courts.slice());
    }
  }

  fetchCourts(){
      this.http.get<CourtDataFormat[]>(this.baseUrl + "Court").subscribe(data => {
        this.courts = data.map(court => {
          return new Court(court.id, court.sport, court.isHall, court.name, court.latitude, court.longitude, court.address, court.startTime, court.endTime, court.pricePerHour, court.image,);
        });
        this.courtsChanged.next(this.courts.slice());
      })
  }


  addNewCourt(data: CourtCreateData){
    this.http.post<any>(this.baseUrl+"Court",{
      sport: data.sport,
      name: data.name,
      latitude: data.latitude,
      longitude: data.longitude,
      address: data.address,
      image: data.image,
      managerId: this.authService.user.value?.id
    }).subscribe(data => {
        this.fetchCourts();
    })

  }

  updateCourt(id: string, data: CourtCreateData){
    this.http.put<any>(this.baseUrl + "Court/" + id,{
      sport: data.sport,
      name: data.name,
      latitude: data.latitude,
      longitude: data.longitude,
      address: data.address,
      image: data.image
    }).subscribe({
      next: data => {
        const index = this.courts.findIndex(court => {
          return court.id === id
        });
        this.courts[index].sport = data.sport;
        this.courts[index].name = data.name;
        this.courts[index].image = data.image;
        this.courts[index].address = data.address;
        this.courts[index].latitude = data.latitude;
        this.courts[index].longitude = data.longitude;

         this.courtsChanged.next(this.courts.slice());
      }, error: error => {
        console.log(error);
      }
    });


  }

  deleteCourt(id: string){
    this.http.delete<any>(this.baseUrl+"Court/" + id).subscribe(response => {
      this.courts = this.courts.filter(court => court.id !== id);
      this.courtsChanged.next(this.courts.slice());
    });
  }

  addNewHall(data: HallCreateData){
    this.http.post<any>(this.baseUrl+"Court",{
      sport: data.sport,
      name: data.name,
      latitude: data.latitude,
      longitude: data.longitude,
      address: data.address,
      image: data.image,
      managerId: this.authService.user.value?.id,
      isHall: true,
      startTime: data.startTime,
      endTime: data.endTime,
      pricePerHour: data.price
    }).subscribe(data => {
      this.fetchCourts();
      this.hallsService.fetchHalls();
    })

  }

  updateHall(id: string, data: HallCreateData){
    this.http.put<any>(this.baseUrl + "Court/" + id,{
      sport: data.sport,
      name: data.name,
      latitude: data.latitude,
      longitude: data.longitude,
      address: data.address,
      image: data.image,
      managerId: this.authService.user.value?.id,
      isHall: true,
      startTime: data.startTime,
      endTime: data.endTime,
      pricePerHour: data.price
    }).subscribe({
      next: data => {
        const index = this.courts.findIndex(court => {
          return court.id === id
        });
        this.courts[index].sport = data.sport;
        this.courts[index].name = data.name;
        this.courts[index].image = data.image;
        this.courts[index].address = data.address;
        this.courts[index].latitude = data.latitude;
        this.courts[index].longitude = data.longitude;
        this.courts[index].startTime = data.startTime;
        this.courts[index].endTime = data.endTime;
        this.courts[index].price = data.price;


        this.courtsChanged.next(this.courts.slice());

        this.hallsService.fetchHalls();
      }, error: error => {
        console.log(error);
      }
    });


  }

}
