import {Component, EventEmitter, Output} from '@angular/core';
import {GeocodingService} from "../../../services/geocoding/geocoding.service";
import {EventsService} from "../../../services/events/events.service";
import {NgForm} from "@angular/forms";
import {CourtsService} from "../../../services/courts/courts.service";
import {HallCreateData} from "../../../interfaces/hall-create-data";

@Component({
  selector: 'app-hall-new',
  templateUrl: './hall-new.component.html',
  styleUrl: './hall-new.component.css'
})
export class HallNewComponent {
  @Output() hallCreated = new EventEmitter<boolean>();

  isLoading = false;

  startTime: number = 0;
  endTime: number = 24;
  constructor(private geocodingService: GeocodingService,
              private eventsService: EventsService,
              private CourtsService: CourtsService) {
  }


  onSubmit(form: NgForm){
    if(form.invalid){
      return;
    }
    this.isLoading = true;
    const sport = form.value.sport;
    const name = form.value.name;
    const address = form.value.address;
    const startTime = this.startTime;
    const endTime = this.endTime;
    const pricePerHour = form.value.pricePerHour;
    let imageLink: string | undefined = undefined;
    if(form.value.image !== ""){
      imageLink = form.value.image;
    }
    let geocodingObs = this.geocodingService.searchByName(address);

    geocodingObs.subscribe({
      next: responseData => {
        this.isLoading = false;
        if(responseData.features.length === 0){
          alert("Address doesn't exist");
          form.reset();
        }else {
          const courtData: HallCreateData = {
            name: name,
            sport: sport,
            address: address,
            image: imageLink,
            longitude: responseData.features[0].center[0],
            latitude: responseData.features[0].center[1],
            startTime: startTime,
            endTime: endTime,
            price: pricePerHour
          }
          form.reset();
          this.CourtsService.addNewHall(courtData);
          this.hallCreated.emit(true);
        }


      },
      error: err => {
        console.log(err);
        this.isLoading = false;
        alert("Error while connecting to API");
      }
    });



  }
}
