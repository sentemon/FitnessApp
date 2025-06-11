import gql from 'graphql-tag';

export const GET_CURRENT_USER = gql`
  query CurrentUser {
    currentUser {
      id
      firstName
      lastName
      username {
        value
      }
      email {
        value
      }
      imageUrl
      createdAt
    }
  }
`;

export const GET_USER_BY_USERNAME = gql`
  query UserByUsername($username: String!) {
    userByUsername(username: $username) {
      firstName
      lastName
      username
      email
      imageUrl
    }
  }
`;

export const IS_AUTHENTICATED = gql`
  query IsAuthenticated {
    isAuthenticated
  }
`
