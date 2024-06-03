import {Component, OnDestroy, OnInit} from '@angular/core';
import {AbstractControl, FormBuilder, FormControl, ValidationErrors, Validators} from "@angular/forms";
import {ActivatedRoute, Router} from "@angular/router";
import {Observable, Subject, Subscription} from "rxjs";
import {EventsService} from "../../services/events/events.service";

@Component({
  selector: 'app-event-new',
  templateUrl: './event-new.component.html',
  styleUrl: './event-new.component.css'
})
export class EventNewComponent implements OnInit, OnDestroy{
  firstFormGroup = this._formBuilder.group({
    firstCtrl: ['', Validators.required],
  });
  secondFormGroup = this._formBuilder.group({
    secondCtrl: ['']
  });
  thirdFormGroup = this._formBuilder.group({
    thirdCtrl: ['', [Validators.required, Validators.min(1), Validators.max(30)]],
  });
  fourthFormGroup = this._formBuilder.group({
    fourthCtrl: ['', Validators.required],
  });

  courtId: string = "";
  sport: string = "";
  occupiedSlots: number[] = [];
  selectedSlots: number[] = [];
  selectedSlotsChange :Subject<number[]>;
  selectedSlotsSubscription?: Subscription;

  constructor(private _formBuilder: FormBuilder,
              private route: ActivatedRoute,
              private eventsService: EventsService,
              private router: Router) {
    this.selectedSlotsChange = new Subject<number[]>();
  }

  ngOnInit(): void {
    this.courtId = this.route.snapshot.params['courtId'];
    this.route.params.subscribe(params => {
      this.courtId = params['courtId'];
      this.sport = params['sport'];
    });

    this.secondFormGroup.get('secondCtrl')!.setAsyncValidators(this.isSlotSelected.bind(this));
    this.secondFormGroup.get('secondCtrl')!.updateValueAndValidity();
  }

  ngOnDestroy(): void {
    this.selectedSlotsSubscription?.unsubscribe();
  }


  dateFilter = (d: Date | null): boolean => {
    const date = d || new Date();
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    return date >= today;
  }

  onSelectedSlotsChange(selectedSlots: number[]): void {
    this.selectedSlots = selectedSlots;
    this.secondFormGroup.get('secondCtrl')!.updateValueAndValidity();
    this.selectedSlotsChange.next(this.selectedSlots);
  }

  isSlotSelected(control: AbstractControl): Promise<ValidationErrors | null> | Observable<ValidationErrors | null> {
    return new Promise((resolve, reject) => {
      // Subscribe to the selectedSlotsChange event
      this.selectedSlotsSubscription = this.selectedSlotsChange.subscribe(selectedSlots => {
        //console.log(selectedSlots);
        // If no slots are selected, return an error
        if (!selectedSlots || selectedSlots.length === 0) {
          resolve({'noSelectedSlots': true});
        } else {
          // If slots are selected, the control is valid
          resolve(null);
        }
      });
    });
  }

  finish(){
    const date: Date =new Date(this.firstFormGroup.value.firstCtrl!);
    const utcDate = new Date(Date.UTC(date.getFullYear(), date.getMonth(), date.getDate()));
    const startTime = this.selectedSlots[0];
    const endTime = this.selectedSlots[this.selectedSlots.length - 1] + 1;
    const numberOfPlayers: number = +this.thirdFormGroup.value.thirdCtrl!;
    const description = this.fourthFormGroup.value.fourthCtrl!;

    this.eventsService.addNewEvent(this.courtId, this.sport, utcDate, startTime, endTime, numberOfPlayers, description)
      .subscribe({
        next: (data => {
          this.router.navigate(['/events']);
        }),
        error: (error => {
          console.log(error)
        })
      });
  }

  onDateChange(event: any) {
    this.occupiedSlots = [];
    const date = event.value;
    const utcDate = new Date(Date.UTC(date.getFullYear(), date.getMonth(), date.getDate()));

    const today = new Date();
    if(date.getDate() === today.getDate()){
      for(let i = 0; i <= today.getHours(); i++){
        this.occupiedSlots.push(i);
      }
    }

    this.eventsService.fetchScheduledSlots(this.courtId, utcDate)
      .subscribe({
        next: (data => {
          //console.log(data);
          data.forEach(slot => {
            for (let i = slot.startTime; i < slot.endTime; i++) {
              this.occupiedSlots.push(i);
            }
          })
        }),
        error: (error => {
          console.log(error);
        })
      });
  }


}
