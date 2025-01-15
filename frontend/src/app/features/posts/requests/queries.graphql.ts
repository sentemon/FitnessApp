import gql from "graphql-tag";

export const GET_POST = gql`
  query Post($id: String!) {
    post(id: $id) {
      title
      description
      contentUrl
      contentType
      likeCount
      commentCount
      createdAt
      userImageUrl
      username
      id
    }
  }
`;

export const GET_ALL_POSTS = gql`
  query AllPost($first: Int!, $lastPostId: String!) {
    allPost(first: $first, lastPostId: $lastPostId) {
      id
      userId
      title
      description
      contentUrl
      contentType
      likeCount
      commentCount
      createdAt
    }
  }
`;
