import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DateService {
  constructor() {}

  formatMessageDate(dateInput: string | Date): string {
    const date = new Date(dateInput);
    const now = new Date();

    const diffMs = now.getTime() - date.getTime();
    const diffMinutes = Math.floor(diffMs / (1000 * 60));
    const diffHours = Math.floor(diffMs / (1000 * 60 * 60));

    if (diffMs < 0)
      return this.formatDate(date, true);

    if (diffMinutes < 1)
      return "Just now";

    if (diffMinutes < 60)
      return this.plural(diffMinutes, 'minute');

    if (diffHours < 12)
      return this.plural(diffHours, 'hour');

    if (this.isSameDay(date, now))
      return "Today";

    if (this.isYesterday(date, now))
      return "Yesterday";

    return this.formatDate(date, date.getFullYear() !== now.getFullYear());
  }

  formatLastSeenDate(dateInput: string | Date): string {
    const date = new Date(dateInput);
    const now = new Date();

    const diffMs = now.getTime() - date.getTime();
    const diffMinutes = Math.floor(diffMs / (1000 * 60));
    const diffHours = Math.floor(diffMs / (1000 * 60 * 60));

    if (diffMinutes < 5)
      return "Online";

    if (diffMinutes < 60)
      return this.plural(diffMinutes, 'minute');

    if (diffHours < 24)
      return this.plural(diffHours, 'hour');

    return this.formatDate(date, date.getFullYear() !== now.getFullYear());
  }


  private plural(value: number, unit: string): string {
    return `${value} ${unit}${value === 1 ? '' : 's'} ago`;
  }

  private formatDate(date: Date, withYear: boolean): string {
    const day = date.getDate().toString().padStart(2, '0');
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const year = date.getFullYear();
    return withYear ? `${day}.${month}.${year}` : `${day}.${month}`;
  }

  private isSameDay(a: Date, b: Date): boolean {
    return a.toDateString() === b.toDateString();
  }

  private isYesterday(date: Date, now: Date): boolean {
    const yesterday = new Date(now);
    yesterday.setDate(now.getDate() - 1);
    return date.toDateString() === yesterday.toDateString();
  }
}
