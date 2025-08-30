import { Injectable } from '@angular/core';
import {Apollo, ApolloBase} from "apollo-angular";
import {UPDATE_ACTIVITY_STATUS} from "../graphql/mutations";
import {toResult} from "../../../core/extensions/graphql-result-wrapper";

@Injectable({
  providedIn: 'root'
})
export class ActivityStatusService {
  private authClient: ApolloBase;

  private lastPing = 0;
  private readonly PING_INTERVAL = 5 * 60 * 1000; // 5 minutes

  constructor(apollo: Apollo) {
    this.authClient = apollo.use("auth");
  }

  init() {
    const handler = this.onActivity.bind(this);

    document.addEventListener('mousemove', handler);
    document.addEventListener('keydown', handler);
    document.addEventListener('click', handler);
  }

  private onActivity(): void {
    const now = Date.now();
    if (now - this.lastPing < this.PING_INTERVAL)
      return;

    this.lastPing = now;

    this.authClient.mutate({
      mutation: UPDATE_ACTIVITY_STATUS
    }).pipe(
      toResult<Date>("updateActivityStatus"),
    ).subscribe(result => {
      if (!result.isSuccess) {
        console.log(result.error.message);
      }
    });
  }
}
