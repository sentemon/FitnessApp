import {ApplicationConfig, inject, provideZoneChangeDetection} from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import {provideHttpClient, withFetch} from '@angular/common/http';
import { provideApollo } from 'apollo-angular';
import {environment} from "../environments/environment";
import {loadDevMessages, loadErrorMessages} from "@apollo/client/dev";
import { createApolloClientOptions } from "./apollo.config";

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

      return createApolloClientOptions();
    })
  ]
};
