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
      lastSeenAt
      imageUrl
      followersCount
      followingCount
      createdAt
    }
  }
`;

export const GET_USER_BY_USERNAME = gql`
  query UserByUsername($username: String!) {
    userByUsername(username: $username) {
      id
      firstName
      lastName
      username
      email
      lastSeenAt
      imageUrl
      followersCount
      followingCount
    }
  }
`;

export const IS_AUTHENTICATED = gql`
  query IsAuthenticated {
    isAuthenticated
  }
`;

export const SEARCH_USERS = gql`
  query SearchUsers($search: String!) {
    searchUsers(search: $search) {
      firstName
      lastName
      username
      email
      lastSeenAt
      imageUrl
    }
  }
`;

export const GET_FOLLOWERS = gql`
  query Followers($userId: String!) {
    followers(userId: $userId) {
      id
      firstName
      lastName
      username {
        value
      }
      lastSeenAt
      imageUrl
      createdAt
    }
  }
`;

export const GET_FOLLOWING = gql`
  query Following($userId: String!) {
    following(userId: $userId) {
      id
      firstName
      lastName
      username {
        value
      }
      lastSeenAt
      imageUrl
      createdAt
    }
  }
`;

export const IS_FOLLOWING = gql`
  query IsFollowing($targetUserId: String!) {
    isFollowing(targetUserId: $targetUserId)
  }
`;
