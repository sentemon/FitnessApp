import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import {HubConnection} from "@microsoft/signalr";
import {Message} from "../models/message.model";
import {Subject} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private hubConnection!: HubConnection;
  public onReceiveMessage: Subject<Message> = new Subject();

  constructor() { }

  public startConnection(chatId: string | null, accessToken: string): void {
    if (!chatId) {
      console.error('Chat ID is required for SignalR connection');
      return;
    }

    this.createConnection(`http://localhost:8000/chat/chat?chatId=${chatId}`, accessToken);
  }

  public startTempConnection(receiverId: string, accessToken: string): void {
    this.createConnection(`http://localhost:8000/chat/chat?receiverId=${receiverId}`, accessToken);
  }

  private createConnection(url: string, token: string) {
    if (this.hubConnection) {
      this.hubConnection.off('ReceiveMessage');
    }

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(url, {
        withCredentials: true,
        accessTokenFactory: () => token
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('SignalR connection started'))
      .catch(err => console.error('SignalR connection error: ', err));

    this.hubConnection.on('ReceiveMessage', (message: Message) => {
      this.onReceiveMessage.next(message);
    });

    this.hubConnection.onclose(error => {
      console.warn('SignalR connection closed. Reconnecting...', error);
      setTimeout(() => this.createConnection(url, token), 3000);
    });
  }

  public stopConnection(): void {
    if (this.hubConnection) {
      this.hubConnection.stop().then(() => console.log('SignalR connection stopped'));
    }
  }

  public sendMessage(receiverId: string, message: string): void {
    if (this.isConnected()) {
      this.hubConnection.invoke('SendMessage', receiverId, message)
        .catch(err => console.error('Error while sending message: ', err));
    } else {
      console.log("Message didn't send — connection not ready");
    }
  }

  private isConnected(): boolean {
    return this.hubConnection?.state === signalR.HubConnectionState.Connected;
  }
}
