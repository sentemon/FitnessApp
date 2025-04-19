import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import {HubConnection} from "@microsoft/signalr";
import {Message} from "../models/message.model";

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private hubConnection!: HubConnection;

  public onReceiveMessage: ((message: Message) => void) | null = null;

  constructor() { }

  public startConnection(chatId: string | null, accessToken: string): void {
    if (!chatId) {
      console.error('Chat ID is required for SignalR connection');
      return;
    }

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`http://localhost:8000/chat/chat?chatId=${chatId}`, {
        withCredentials: true,
        accessTokenFactory: () => accessToken
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('SignalR connection started'))
      .catch(err => console.error('SignalR connection error: ', err));

    this.hubConnection.on('ReceiveMessage', (message: Message) => {
      if (this.onReceiveMessage) {
        this.onReceiveMessage(message);
      }
    });

    this.hubConnection.onclose(error => {
      console.warn('SignalR connection closed. Attempting to reconnect...', error);
      setTimeout(() => this.startConnection(chatId, accessToken), 3000);
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
