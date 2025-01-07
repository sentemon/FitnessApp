import { ApolloClientOptions, InMemoryCache, ApolloLink, HttpLink } from '@apollo/client/core';
import { setContext } from '@apollo/client/link/context';
import { environment } from '../environments/environment';

export function createApolloClientOptions(): ApolloClientOptions<any> {
  const httpLink = new HttpLink({
    uri: environment.auth_service,
    credentials: 'include',
  });

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

  const link = ApolloLink.from([authLink, httpLink]);

  return {
    link: link,
    cache: new InMemoryCache()
  };
}
