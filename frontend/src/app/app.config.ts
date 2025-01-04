import { ApplicationConfig, provideZoneChangeDetection, inject } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient} from '@angular/common/http';
import { provideApollo } from 'apollo-angular';
import { HttpLink } from 'apollo-angular/http';
import {ApolloLink, InMemoryCache} from '@apollo/client/core';
import {environment} from "../environments/environment";
import {setContext} from "@apollo/client/link/context";
import {DOCUMENT} from "@angular/common";

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(),
    provideApollo(() => {
      const httpLink = inject(HttpLink);
      const document = inject(DOCUMENT);

      const authLink = setContext(() => {
        const token = document.cookie
          .split('; ')
          .find(row => row.startsWith('token='))
          ?.split('=')[1];
        return {
          headers: {
            Authorization: token ? `Bearer ${token}` : '',
          },
        };
      });

      const link = ApolloLink.from([
        authLink,
        httpLink.create({ uri: environment.auth_service }),
      ]);

      return {
        link,
        cache: new InMemoryCache(),
      };
    })
  ]
};
