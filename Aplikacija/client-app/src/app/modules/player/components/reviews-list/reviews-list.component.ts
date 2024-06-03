import {Component, OnDestroy, OnInit} from '@angular/core';
import {Review} from "../../interfaces/review";
import {ActivatedRoute} from "@angular/router";

@Component({
  selector: 'app-reviews-list',
  templateUrl: './reviews-list.component.html',
  styleUrl: './reviews-list.component.css'
})
export class ReviewsListComponent implements OnInit, OnDestroy{

  reviews: Review[] = [];

  constructor(private route: ActivatedRoute) {
  }

  ngOnInit() {
  }

  ngOnDestroy() {
  }


  addReview() {

  }


  getStars(rating: number): any[] {
    return Array(Math.round(rating)).fill(1);
  }

}
