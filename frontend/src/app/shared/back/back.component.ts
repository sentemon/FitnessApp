import { Component } from '@angular/core';
import { Location } from "@angular/common";

@Component({
  selector: 'app-back',
  templateUrl: './back.component.html',
  styleUrl: './back.component.scss'
})
export class BackComponent {
  constructor(private location: Location) { }

  back(): void {
    this.location.back();
  }
}
