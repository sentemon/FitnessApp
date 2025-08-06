import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DeviceService {
  constructor() { }

  public isMobile(): boolean {
    return window.innerWidth < 768;
  }

  public isTablet(): boolean {
    return window.innerWidth >= 768 && window.innerWidth < 992;
  }

  public isDesktop(): boolean {
    return window.innerWidth >= 992;
  }
}
