import {Component, EventEmitter, Input, OnChanges, Output} from '@angular/core';
import {GeocodingService} from "../../../services/geocoding/geocoding.service";
import {EventsService} from "../../../services/events/events.service";
import {NgForm} from "@angular/forms";
import {HallCreateData} from "../../../interfaces/hall-create-data";
import {CourtsService} from "../../../services/courts/courts.service";
import {Court} from "../../../models/court";

@Component({
  selector: 'app-hall-edit',
  templateUrl: './hall-edit.component.html',
  styleUrl: './hall-edit.component.css'
})
export class HallEditComponent implements OnChanges{
  @Output() hallEdited = new EventEmitter<boolean>();
  @Input() selectedHall?: Court;

  isLoading = false;

  startTime: number = 0;
  endTime: number = 24;
  nameInput: string = "";
  sportInput: string = "";
  addressInput: string = "";
  pricePerHourInput: number = 0;
  imageInput: string = "";

  constructor(private geocodingService: GeocodingService,
              private eventsService: EventsService,
              private courtsService: CourtsService) {}

  ngOnChanges() {
    this.startTime = this.selectedHall?.startTime!;
    this.endTime = this.selectedHall?.endTime!;
    this.nameInput = this.selectedHall?.name!;
    this.sportInput = this.selectedHall?.sport!;
    this.addressInput = this.selectedHall?.address!;
    this.pricePerHourInput = this.selectedHall?.price!;
    this.imageInput = this.selectedHall?.image ?? "";

  }


  onSubmit(form: NgForm) {
    if (form.invalid) {
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
    if (form.value.image !== "") {
      imageLink = form.value.image;
    }
    let geocodingObs = this.geocodingService.searchByName(address);

    geocodingObs.subscribe({
      next: responseData => {
        this.isLoading = false;
        if (responseData.features.length === 0) {
          alert("Address doesn't exist");
          form.reset();
        } else {
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
          this.courtsService.updateHall(this.selectedHall?.id!, courtData);
          this.hallEdited.emit(true);
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
