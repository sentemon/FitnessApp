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

export const GET_CHAT_BY_ID = gql`
  query ChatById($chatId: String!) {
    chatById(chatId: $chatId) {
      id
      createdAt
      userChats {
        userId
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
        id
        senderId
        chatId
        content
        sentAt
        updatedAt
        isRead
        sender {
          id
          firstName
          lastName
          username
          imageUrl
          createdAt
        }
      }
    }
  }
`;
