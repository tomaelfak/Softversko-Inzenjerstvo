import { Component, OnInit, ViewChild, ElementRef, AfterViewInit, OnDestroy } from '@angular/core';
import {Map, MapStyle, config, Marker} from '@maptiler/sdk';
import {Court} from "../../models/court";
import {MatButtonToggleGroup} from "@angular/material/button-toggle";
import {EventsService} from "../../services/events/events.service";
import {CourtsService} from "../../services/courts/courts.service";
import {Subscription} from "rxjs";
@Component({
  selector: 'app-events-map',
  templateUrl: './events-map.component.html',
  styleUrl: './events-map.component.css'
})
export class EventsMapComponent implements OnInit, OnDestroy{
  map: Map | undefined;

  courts: Court[] = [];
  courtsSubscription?: Subscription;
  selectedSport: string = "all";
  selectedCourt: string = "all";


  @ViewChild('map')
  private mapContainer!: ElementRef<HTMLElement>;

  constructor(private eventsService: EventsService, private courtsService: CourtsService) {}

  ngOnInit() {
    //config.apiKey = 'd96hSHCLErl8rkEEoIrM';
    this.courtsSubscription = this.courtsService.courtsChanged.subscribe((courts: Court[]) =>{
      this.courts = courts;
    });
    this.courtsService.getCourts();
    this.onShowChanged();
  }

  ngOnDestroy() {
    this.courtsSubscription?.unsubscribe();
  }

  addMarker(latitude: number, longitude: number, isHall: boolean, courtId: string){
    const markerColor = isHall ? "#a86932" : "#326da8";

    new Marker({color: markerColor})
      .setLngLat([longitude, latitude])
      .on('click', (e) => {
        console.log(e.lngLat)
        console.log(courtId);
      })
      .addTo(this.map!);
  }

  onShowChanged(){
    this.courts.forEach(court => {
      if(this.selectedSport==='all' && this.selectedCourt==='all'){
        court.toShow = true;
      }else if(this.selectedSport!=='all' &&this.selectedCourt==='all'){
        court.toShow = court.sport === this.selectedSport;
      }else if(this.selectedSport!=='all' && this.selectedCourt==='hall'){
        court.toShow = court.sport === this.selectedSport && court.isHall;
      }else if(this.selectedSport!=='all' &&this.selectedCourt==='street'){
        court.toShow = court.sport === this.selectedSport && !court.isHall;
      }else if(this.selectedSport==='all' && this.selectedCourt==='hall'){
        court.toShow = court.isHall;
      }else{
        court.toShow = !court.isHall;
      }
    })
  }

  onMarkerClick(clickedCourt: Court){
    this.courtsService.courtSelected.emit(clickedCourt);
  }

}
