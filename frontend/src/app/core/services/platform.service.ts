import { Injectable } from '@angular/core';
import { Capacitor } from  '@capacitor/core';

@Injectable({
  providedIn: 'root'
})
export class PlatformService {
  constructor() { }

  isWeb(): boolean {
    return Capacitor.getPlatform() === 'web';
  }

  isMobile(): boolean {
    return !this.isWeb();
  }

  isIos(): boolean {
    return Capacitor.getPlatform() === 'ios';
  }

  isAndroid(): boolean {
    return Capacitor.getPlatform() === 'android';
  }
}
