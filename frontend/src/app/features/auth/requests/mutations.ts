import gql from 'graphql-tag';

export const LOGIN = gql`
    mutation Login($username: String!, $password: String!) {
      login(input: { username: $username, password: $password }) {
        accessToken
        expiresIn
        refreshExpiresIn
        refreshToken
        tokenType
        notBeforePolicy
        sessionState
        scope
      }
    }
`;
