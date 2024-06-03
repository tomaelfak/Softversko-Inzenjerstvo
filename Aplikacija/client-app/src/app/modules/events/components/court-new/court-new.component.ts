import { Component } from '@angular/core';
import {NgForm} from "@angular/forms";
import {GeocodingService} from "../../services/geocoding/geocoding.service";
import {CourtCreateData} from "../../interfaces/court-create-data";
import {EventsService} from "../../services/events/events.service";
import {Router} from "@angular/router";
import {CourtsService} from "../../services/courts/courts.service";

@Component({
  selector: 'app-court-new',
  templateUrl: './court-new.component.html',
  styleUrl: './court-new.component.css'
})
export class CourtNewComponent {
  isLoading = false;
  constructor(private geocodingService: GeocodingService,
              private eventsService: EventsService,
              private router: Router,
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
          const courtData: CourtCreateData = {
            name: name,
            sport: sport,
            address: address,
            image: imageLink,
            longitude: responseData.features[0].center[0],
            latitude: responseData.features[0].center[1]
          }
          form.reset();
          this.CourtsService.addNewCourt(courtData);
          this.router.navigate(['/events']);
        }


      },
      error: err => {
        console.log(err);
        this.isLoading = false;
        alert("Error while connecting to API");
      }
    })



  }

}
