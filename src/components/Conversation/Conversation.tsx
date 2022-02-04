import { useLazyQuery } from "@apollo/client";
import React, { useEffect } from "react";
import {
  GET_MESSAGES_AMONG_USERS,
  GET_MESSAGES_BETWEEN_USER_AND_GROUP,
} from "../../features/apolloClient";
import { AuthState } from "../../features/authSlice";
import {
  addConversations,
  Conversation,
  Message,
} from "../../features/conversationsSlice";
import { useAppDispatch, useAppSelector } from "../../features/hooks";
import { Theme } from "../../features/themeSlice";
import { renameKeys, toLocalDateTime } from "../../utils/utils";
import RoundedAvatar from "../RoundedAvatar/RoundedAvatar";
import "./Conversation.scss";
import moment from "moment";
import { Group, User } from "../../pages/Home/Home";
import { Loading } from "../Loading/Loading";

interface ConversationProps {
  userOrGroup: User | Group;
}

const ConversationCpn: React.FC<ConversationProps> = ({ userOrGroup }) => {
  const dispatch = useAppDispatch();
  const id = userOrGroup.id;
  const userInfo: AuthState = useAppSelector((state) => state.auth);

  const [getMessagesAmongUsers, { loading }] = useLazyQuery(
    GET_MESSAGES_AMONG_USERS,
    {
      onCompleted: (data) => {
        const conversation: Conversation = {
          id: id,
          messages: data.messages.map((mess) =>
            renameKeys({ toUser: "toUserOrGroup" }, mess)
          ),
        };
        dispatch(addConversations(conversation));
      },
      fetchPolicy: 'no-cache'
    }
  );

  const [getMessagesBetweenUserAndGroup] = useLazyQuery(
    GET_MESSAGES_BETWEEN_USER_AND_GROUP,
    {
      onCompleted: (data) => {
        const conversation: Conversation = {
          id: id,
          messages: data.messages.map((mess) =>
            renameKeys({ toGroup: "toUserOrGroup" }, mess)
          ),
        };
        dispatch(addConversations(conversation));
      },
    }
  );

  const theme: Theme = useAppSelector((state) =>
    state.theme.find((ele: Theme) => ele.conversationId === id)
  );

  const conversation: Conversation | undefined = useAppSelector((state) =>
    state.conversations.find((ele: Conversation) => ele.id === id)
  );

  useEffect(() => {
    if (!conversation && "imageUrl" in userOrGroup) {
      getMessagesAmongUsers({
        variables: {
          idFrom: userInfo.id,
          idTo: id,
        },
      });
    } else if (!conversation && !("imageUrl" in userOrGroup)) {
      getMessagesBetweenUserAndGroup({
        variables: {
          groupId: id,
        },
      });
    }
  }, [conversation, getMessagesAmongUsers, getMessagesBetweenUserAndGroup, id]);

  return (
    <ol className="messages">
      {conversation && conversation.messages.length === 0 && (
        <div className="no-message">Lets start chating!</div>
      )}
      {loading && (
        <Loading
          style={{
            position: "relative",
            left: "50%",
            top: "-50%",
            transform: "translateX(-50%)",
          }}
        />
      )}
      {conversation &&
        conversation.messages.length !== 0 &&
        conversation.messages.map((message: Message) =>
          message.sendByUser.id === userInfo.id ? (
            <li
              key={message.id}
              className="mine"
              style={{ backgroundImage: theme && theme.linearGradient }}
            >
              <div className="content">
                {message.content}
                <span className="tooltip-left">
                  {moment(toLocalDateTime(message.date)).format(
                    "dddd, MMMM Do YYYY, h:mm a"
                  )}
                </span>
              </div>
            </li>
          ) : (
            <li>
              <RoundedAvatar
                src={message.sendByUser.imageUrl}
                style={{ marginRight: 6, width: 28, height: 28 }}
              />
              <div className="content">
                {message.content}
                <span className="tooltip-right">
                  {moment(toLocalDateTime(message.date)).format(
                    "dddd, MMMM Do YYYY, h:mm a"
                  )}
                </span>
              </div>
            </li>
          )
        )}
    </ol>
  );
};

export default ConversationCpn;
