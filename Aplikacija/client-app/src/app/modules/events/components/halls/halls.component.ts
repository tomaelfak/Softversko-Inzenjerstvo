import { Component } from '@angular/core';
import {HallsService} from "../../services/halls/halls.service";
import {Court} from "../../models/court";

@Component({
  selector: 'app-halls',
  templateUrl: './halls.component.html',
  styleUrl: './halls.component.css'
})
export class HallsComponent {
  showNewHallForm = false;
  showEditHallForm = false;
  showEventsList = false;
  hallToEdit?: Court;
  hallToShowId?: string;


  constructor(private hallsService: HallsService) {}


  onClickAdd(){
    this.showNewHallForm = true;
    this.showEditHallForm = false;
    this.showEventsList = false;
  }

  onHallCreated(){
    this.showNewHallForm = false;
  }

  onClickEdit(hall: Court){
    this.hallToEdit = hall;
    this.showEditHallForm = true;
    this.showNewHallForm = false;
    this.showEventsList = false;
  }

  onHallEdited(){
    this.showEditHallForm = false;
  }

  onShowEventsList(court: Court){
    this.hallToShowId = court.id;
    this.showEventsList = true;
    this.showNewHallForm = false;
    this.showEditHallForm = false;
  }




}
