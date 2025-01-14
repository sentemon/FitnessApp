import { ApolloClientOptions, InMemoryCache, ApolloLink, HttpLink } from '@apollo/client/core';
import { setContext } from '@apollo/client/link/context';
import {inject} from "@angular/core";
import {CookieService} from "./core/services/cookie.service";
import {environment} from "../environments/environment";

export function createApolloClientOptions(): ApolloClientOptions<any> {
  const cookieService = inject(CookieService);

  const httpLink = new HttpLink({
    uri: environment.auth_service,
    credentials: 'include',
  });

  const authLink = setContext(() => {
    const token = cookieService.get("token");
    return {
      headers: {
        Authorization: token ? `Bearer ${token}` : '',
      }
    };
  });

  const link = ApolloLink.from([authLink, httpLink]);

  return {
    link: link,
    cache: new InMemoryCache()
  };
}
