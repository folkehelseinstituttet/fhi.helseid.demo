import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-not-found',
  templateUrl: './not-found.component.html',
  styleUrls: ['./not-found.component.css']
})
export class NotFoundComponent implements OnInit {

  constructor() {
    console.warn(`on ctor`)
   }

  ngOnInit() {
    // console.error("on init");
  }

}
