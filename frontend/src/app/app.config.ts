import {ApplicationConfig, inject, provideZoneChangeDetection} from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import {provideHttpClient, withFetch} from '@angular/common/http';
import { provideApollo } from 'apollo-angular';
import {ApolloClient, ApolloLink, InMemoryCache} from '@apollo/client/core';
import {setContext} from "@apollo/client/link/context";
import {environment} from "../environments/environment";
import {HttpLink} from "apollo-angular/http";
import {loadDevMessages, loadErrorMessages} from "@apollo/client/dev";

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(withFetch()),
    provideApollo(() => {
      if (!environment.production) {
        loadDevMessages();
        loadErrorMessages();
      }
      const httpLink = inject(HttpLink);

      const authLink = setContext(() => {
        const token = document.cookie
          .split('; ')
          .find(row => row.startsWith('token='))
          ?.split('=')[1];
        return {

          headers: {
            Authorization: token ? `Bearer ${token}` : '',
          }
        };
      });

      const link = ApolloLink.from([
        authLink,
        httpLink.create({ uri: environment.auth_service }),
        httpLink.create({ uri: environment.post_service })
      ]);

      return {
        link: link,
        cache: new InMemoryCache(),
      };
    })
  ]
};
