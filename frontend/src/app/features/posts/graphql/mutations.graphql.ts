import gql from "graphql-tag";

export const CREATE_POST = gql`
  mutation CreatePost($title: String!, $description: String!, $file: Upload) {
    createPost(input: {
      title: $title,
      description: $description,
      contentType: IMAGE,
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
