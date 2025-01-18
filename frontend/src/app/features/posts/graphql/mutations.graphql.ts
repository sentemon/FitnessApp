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
