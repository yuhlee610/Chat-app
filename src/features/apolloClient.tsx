import { ApolloClient, InMemoryCache, gql } from "@apollo/client";
import { split, HttpLink } from "@apollo/client";
import { getMainDefinition } from "@apollo/client/utilities";
import { WebSocketLink } from "@apollo/client/link/ws";
import { setContext } from '@apollo/client/link/context';
import { store } from "./store";

const wsLink = new WebSocketLink({
  uri: "wss://localhost:44313/graphql",
  options: {
    reconnect: true,
  },
});

const httpLink = new HttpLink({
  uri: "https://localhost:44313/graphql/",
});

const authLink = setContext((_, { headers }) => {
  // get the authentication token from local storage if it exists
  const token = store.getState().auth.token
  // return the headers to the context so httpLink can read them
  return {
    headers: {
      ...headers,
      authorization: token ? `Bearer ${token}` : "",
    }
  }
});

const splitLink = split(
  ({ query }) => {
    const definition = getMainDefinition(query);
    return (
      definition.kind === "OperationDefinition" &&
      definition.operation === "subscription"
    );
  },
  wsLink,
  // httpLink
  authLink.concat(httpLink)
);

export const client = new ApolloClient({
  link: splitLink,
  cache: new InMemoryCache(),
});

// export const client = new ApolloClient({
//   uri: "https://localhost:44313/graphql/",
//   cache: new InMemoryCache(),
// });

export const CREATE_ACCOUNT_IF_NOT_EXISTS = gql`
  mutation ($userInfo: AddUserPayloadInput!) {
    createUserAndToken(userInput: $userInfo) {
      user {
        id
        name
        imageUrl
        email
      }
      token
    }
  }
`;

export const GET_MEMBERS = gql`
  query {
    users {
      id
      name
      imageUrl
      email
    }
  }
`;

export const GET_CONTACT_USERS = gql`
  query ($id: String!) {
    contactUsers(idUser: $id) {
      user {
        id
        name
        imageUrl
        email
      }
      latestMessage {
        content
      }
    }
  }
`;

export const GET_CONTACT_GROUPS = gql`
  query ($id: String!) {
    contactGroups(idUser: $id) {
      group {
        id
        name
        host {
          id
          name
          imageUrl
          email
        }
        groupUsers {
          user {
            id
            name
            imageUrl
            email
          }
        }
      }
      latestMessage {
        content
      }
      numOfMembers
    }
  }
`;

export const GROUP_CREATED_SUBSCRIPTION = gql`
  subscription ($id: String!) {
    groupCreated(id: $id) {
      group {
        id
        name
        host {
          id
          name
          imageUrl
          email
        }
        groupUsers {
          user {
            id
            name
            imageUrl
            email
          }
        }
      }
      latestMessage {
        content
      }
      numOfMembers
    }
  }
`;

export const CREATE_GROUP = gql`
  mutation ($name: String!, $hostId: String!, $groupUserIds: [String]!) {
    createGroup(
      groupInput: { name: $name, hostId: $hostId, groupUserIds: $groupUserIds }
    ) {
      group {
        id
        name
        host {
          id
          name
          imageUrl
          email
        }
        groupUsers {
          user {
            id
            name
            imageUrl
            email
          }
        }
      }
      latestMessage {
        content
      }
      numOfMembers
    }
  }
`;

export const GET_MESSAGES_AMONG_USERS = gql`
  query ($idFrom: String!, $idTo: String!) {
    messages(
      order: { date: DESC }
      where: {
        sendByUserId: { or: [{ eq: $idFrom }, { eq: $idTo }] }
        toUserId: { or: [{ eq: $idFrom }, { eq: $idTo }] }
      }
    ) {
      id
      content
      toUser {
        id
        name
        email
        imageUrl
      }
      sendByUser {
        id
        name
        email
        imageUrl
      }
      date
    }
  }
`;

export const GET_MESSAGES_BETWEEN_USER_AND_GROUP = gql`
  query ($groupId: String!) {
    messages(order: { date: DESC }, where: { toGroupId: { eq: $groupId } }) {
      id
      content
      toGroup {
        id
        name
      }
      sendByUser {
        id
        name
        email
        imageUrl
      }
      date
    }
  }
`;

export const MESSAGE_CREATED_SUBSCRIPTION = gql`
  subscription ($id: String!) {
    messageCreated(id: $id) {
      id
      content
      sendByUser {
        id
        name
        imageUrl
        email
      }
      toUser {
        id
        name
        imageUrl
        email
      }
      toGroup {
        id
        name
      }
      date
    }
  }
`;

export const CREATE_MESSAGE = gql`
  mutation (
    $content: String!
    $type: Type!
    $sendByUserId: String!
    $groupOrUserId: String!
  ) {
    createMessage(
      messageInput: {
        content: $content
        type: $type
        sendByUserId: $sendByUserId
        groupOrUserId: $groupOrUserId
      }
    ) {
      id
    }
  }
`;

export const EXIT_GROUP = gql`
  mutation ($userId: String!, $groupId: String!) {
    exitGroup(userId: $userId, groupId: $groupId) {
      group {
        id
        name
        host {
          id
          name
          imageUrl
          email
        }
        groupUsers {
          user {
            id
            name
            imageUrl
            email
          }
        }
      }
      latestMessage {
        content
      }
      numOfMembers
    }
  }
`;

export const GROUP_EXITED_SUBSCRIPTION = gql`
  subscription ($userId: String!) {
    groupExited(userId: $userId) {
      group {
        id
        name
        host {
          id
          name
          imageUrl
          email
        }
        groupUsers {
          user {
            id
            name
            imageUrl
            email
          }
        }
      }
      latestMessage {
        content
      }
      numOfMembers
    }
  }
`;

export const REMOVE_MEMBERS = gql`
  mutation ($userIds: [String]!, $groupId: String!) {
    removeMembers(userIds: $userIds, groupId: $groupId) {
      group {
        id
        name
        host {
          id
          name
          imageUrl
          email
        }
        groupUsers {
          user {
            id
            name
            imageUrl
            email
          }
        }
      }
      latestMessage {
        content
      }
      numOfMembers
    }
  }
`;

export const MEMBERS_REMOVED_SUBSCRIPTION = gql`
  subscription ($userId: String!) {
    groupRemoveMembers(userId: $userId) {
      group {
        id
        name
        host {
          id
          name
          imageUrl
          email
        }
        groupUsers {
          user {
            id
            name
            imageUrl
            email
          }
        }
      }
      latestMessage {
        content
      }
      numOfMembers
    }
  }
`;

export const ADD_MEMBERS = gql`
  mutation ($userIds: [String]!, $groupId: String!) {
    addMembers(userIds: $userIds, groupId: $groupId) {
      group {
        id
        name
        host {
          id
          name
          imageUrl
          email
        }
        groupUsers {
          user {
            id
            name
            imageUrl
            email
          }
        }
      }
      latestMessage {
        content
      }
      numOfMembers
    }
  }
`;

export const MEMBERS_ADDED_SUBSCRIPTION = gql`
  subscription ($userId: String!) {
    groupAddMembers(userId: $userId) {
      group {
        id
        name
        host {
          id
          name
          imageUrl
          email
        }
        groupUsers {
          user {
            id
            name
            imageUrl
            email
          }
        }
      }
      latestMessage {
        content
      }
      numOfMembers
    }
  }
`;
