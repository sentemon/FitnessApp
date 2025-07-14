import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import {HubConnection, LogLevel} from "@microsoft/signalr";
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
      console.log('Chat ID is required for SignalR connection');
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
      .configureLogging(LogLevel.Error)
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
      .catch(err => console.log('SignalR connection error: ', err));

    this.hubConnection.on('ReceiveMessage', (message: Message) => {
      this.onReceiveMessage.next(message);
    });

    this.hubConnection.onclose(() => {
      setTimeout(() => this.createConnection(url, token), 3000);
    });
  }

  public stopConnection(): void {
    this.hubConnection?.stop().catch(err => console.log('SignalR connection error: ', err));
  }

  public sendMessage(receiverId: string, message: string): void {
    if (this.isConnected()) {
      this.hubConnection.invoke('SendMessage', receiverId, message)
        .catch(err => console.log('Error while sending message: ', err));
    } else {
      console.log("Message didn't send — connection not ready");
    }
  }

  private isConnected(): boolean {
    return this.hubConnection?.state === signalR.HubConnectionState.Connected;
  }
}
