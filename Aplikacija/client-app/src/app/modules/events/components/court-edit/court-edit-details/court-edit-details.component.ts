import {Component, EventEmitter, Input, OnChanges, OnDestroy, Output, SimpleChanges} from '@angular/core';
import {GeocodingService} from "../../../services/geocoding/geocoding.service";
import {EventsService} from "../../../services/events/events.service";
import {Router} from "@angular/router";
import {NgForm} from "@angular/forms";
import {CourtCreateData} from "../../../interfaces/court-create-data";
import {CourtsService} from "../../../services/courts/courts.service";
import {Court} from "../../../models/court";
import {Observable} from "rxjs";

@Component({
  selector: 'app-court-edit-details',
  templateUrl: './court-edit-details.component.html',
  styleUrl: './court-edit-details.component.css'
})
export class CourtEditDetailsComponent implements OnChanges, OnDestroy{
  isLoading = false;
  @Input() selectedCourt?: Court;
  @Output() editFinished = new EventEmitter<string>();
  addressInput = '';
  nameInput = '';
  imageInput = '';
  typeInput ='';

  geocodingObs: Observable<any> = new Observable<any>();
  constructor(private geocodingService: GeocodingService,
              private router: Router,
              private courtsService: CourtsService) {
  }
  ngOnChanges(changes: SimpleChanges) {
    this.addressInput = this.selectedCourt!.address;
    this.nameInput = this.selectedCourt!.name;
    this.imageInput = this.selectedCourt!.image ?? '';
    this.typeInput = this.selectedCourt!.sport;
  }
  ngOnDestroy() {
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

        if(responseData.features.length === 0){
          alert("Address doesn't exist");
        }else {
          const courtData: CourtCreateData = {
            name: name,
            sport: sport,
            address: address,
            image: imageLink,
            longitude: responseData.features[0].center[0],
            latitude: responseData.features[0].center[1]
          }

          this.courtsService.updateCourt(this.selectedCourt?.id! ,courtData);
          this.editFinished.next("over");
          //this.router.navigate(['/events']);
        }
        this.isLoading = false;
        form.reset();
      },
      error: err => {
        console.log(err);
        this.isLoading = false;
        alert("Error while connecting to API");
      }
    })



  }

}
