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

export const REGISTER = gql`
  mutation Register($firstName: String!, $lastName: String!, $username: String!, $email: String!, $password: String!) {
    register(input: {
      firstName: $firstName,
      lastName: $lastName,
      username: $username,
      email: $email,
      password: $password
    }) {
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


export const LOGOUT = gql`
  mutation Logout($refreshToken: String!) {
    logout(
      refreshToken: $refreshToken
    )
  }
`;
