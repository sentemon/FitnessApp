import gql from "graphql-tag";

export const GET_ALL_CHATS = gql`
  query AllChats {
    allChats {
      id
      createdAt
      userChats {
        user {
          id
          firstName
          lastName
          username
          imageUrl
          createdAt
        }
      }
      messages {
        content
        sentAt
        updatedAt
        isRead
      }
    }
  }
`;
