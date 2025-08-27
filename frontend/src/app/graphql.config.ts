import {HttpLink} from "apollo-angular/http";
import {environment} from "../environments/environment";
import {InMemoryCache} from "@apollo/client/core";

export function graphql(httpLink: HttpLink) {
  return {
    auth: {
      name: 'auth',
      link: httpLink.create({
        uri: environment.auth_service,
        withCredentials: true
      }),
      cache: new InMemoryCache()
    },
    posts: {
      name: 'posts',
      link: httpLink.create({
        uri: environment.post_service,
        withCredentials: true
      }),
      cache: new InMemoryCache()
    },
    workouts: {
      name: 'workouts',
      link: httpLink.create({
        uri: environment.workout_service,
        withCredentials: true
      }),
      cache: new InMemoryCache()
    },
    chats: {
      name: 'chats',
      link: httpLink.create({
        uri: environment.chat_service,
        withCredentials: true
      }),
      cache: new InMemoryCache()
    },
  }
}
