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

export const UPDATE_ACTIVITY_STATUS = gql`
  mutation UpdateActivityStatus {
    updateActivityStatus
  }
`

export const FOLLOW = gql`
  mutation Follow($targetUserId: String!) {
    follow(targetUserId: $targetUserId)
  }
`;

export const UNFOLLOW = gql`
  mutation Unfollow($targetUserId: String!) {
    unfollow(targetUserId: $targetUserId)
  }
`;

export const DELETE_USER = gql`
  mutation DeleteUser {
    deleteUser
  }
`;
