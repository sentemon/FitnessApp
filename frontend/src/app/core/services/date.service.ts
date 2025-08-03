import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DateService {
  constructor() { }


  formatMessageDate(dateInput: string | Date): string {
    const date = new Date(dateInput);
    const now = new Date();

    const diffMs = now.getTime() - date.getTime();
    const diffMinutes = Math.floor(diffMs / (1000 * 60));
    const diffHours = Math.floor(diffMs / (1000 * 60 * 60));

    if (diffMs < 0) {
      return this.formatDate(date, true);
    }

    if (diffMinutes < 1) {
      return "Just now";
    }

    if (diffMinutes < 60) {
      return `${diffMinutes} minute${diffMinutes > 1 ? 's' : ''} ago`;
    }

    if (diffHours < 12) {
      return `${diffHours} hour${diffHours > 1 ? 's' : ''} ago`;
    }

    if (date.toDateString() === now.toDateString()) {
      return "Today";
    }

    const yesterday = new Date(now);
    yesterday.setDate(now.getDate() - 1);
    if (date.toDateString() === yesterday.toDateString()) {
      return "Yesterday";
    }

    if (date.getFullYear() !== now.getFullYear()) {
      return this.formatDate(date, true);
    }

    return this.formatDate(date, false);
  }

  private formatDate(date: Date, withYear: boolean): string {
    const day = ('0' + date.getDate()).slice(-2);
    const month = ('0' + (date.getMonth() + 1)).slice(-2);
    if (withYear) {
      return `${day}-${month}-${date.getFullYear()}`;
    }
    return `${day}-${month}`;
  }
}
