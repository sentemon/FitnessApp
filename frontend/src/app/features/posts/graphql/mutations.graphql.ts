import gql from "graphql-tag";

export const CREATE_POST = `
  mutation CreatePost($title: String!, $description: String!, $contentType: ContentType! $file: Upload) {
    createPost(input: {
      title: $title,
      description: $description,
      contentType: $contentType,
      file: $file
    }) {
      id
      title
      description
      contentUrl
      contentType
      likeCount
      commentCount
      createdAt
      userImageUrl
      username
    }
  }
`;

export const DELETE_POST = gql`
  mutation DeletePost($id: String!) {
    deletePost(id: $id)
  }
`;

export const CREATE_COMMENT = gql`
  mutation CreateComment($postId: String!, $content: String!) {
    createComment(
      input: {
        postId: $postId
        content: $content
      }
    ) {
      id
      postId
      userId
      username
      content
      createdAt
    }
  }
`;

export const GET_ALL_COMMENTS = gql`
  query AllComments($postId: String!, $first: Int!) {
    allComments(postId: $postId, first: $first) {
      id
      postId
      userId
      username
      content
      createdAt
    }
  }
`;

export const ADD_LIKE = gql`
  mutation AddLike($postId: String!) {
    addLike(postId: $postId) {
      id
      postId
      userId
      createdAt
    }
  }
`;

export const DELETE_LIKE = gql`
  mutation DeleteLike($postId: String!) {
    deleteLike(postId: $postId)
  }
`;

export const IS_POST_LIKED = gql`
  query IsPostLiked($postId: String!) {
    isPostLiked(postId: $postId)
  }
`;
