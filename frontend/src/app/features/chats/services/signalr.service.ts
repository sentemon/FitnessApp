import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import {HubConnection} from "@microsoft/signalr";
import {Message} from "primeng/message";

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private hubConnection!: HubConnection;

  public onReceiveMessage: ((message: Message) => void) | null = null;

  constructor() { }

  public startConnection(chatId: string, accessToken: string): void {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`http://localhost:8000/chat/chat`)
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('SignalR connection started'))
      .catch(err => console.error('SignalR connection error: ', err));

    this.hubConnection.on('ReceiveMessage', (message) => {
      if (this.onReceiveMessage) {
        this.onReceiveMessage(message);
      }
    });
  }

  public stopConnection(): void {
    if (this.hubConnection) {
      this.hubConnection.stop().then(() => console.log('SignalR connection stopped'));
    }
  }

  public sendMessage(chatId: string, message: string): void {
    if (this.isConnected()) {
      this.hubConnection.invoke('SendMessage', chatId, message)
        .catch(err => console.error('Error while sending message: ', err));
    }
  }

  private isConnected(): boolean {
    return this.hubConnection?.state === signalR.HubConnectionState.Connected;
  }
}
