import React from "react";
import RoundedAvatar from "../RoundedAvatar/RoundedAvatar";
import "./ChatHeader.scss";

interface ChatHeaderProps {
  src: string;
  name: string;
  email?: string;
  numOfMembers?: number;
  direction?: "vertical" | "horizontal";
}

const ChatHeader: React.FC<ChatHeaderProps> = ({
  src,
  name,
  email,
  numOfMembers,
  direction = "horizontal",
}) => {
  return (
    <header className={`chat-header-${direction}`}>
      <RoundedAvatar
        src={src}
        style={
          direction === "vertical" && {
            width: 80,
            height: 80,
            boxShadow:
              "rgba(50, 50, 93, 0.25) 0px 6px 12px -2px, rgba(0, 0, 0, 0.3) 0px 3px 7px -3px",
          }
        }
      />
      <div className="title">
        <div>{name}</div>
        <div>{email ? `Email: ${email}` : `Members: ${numOfMembers}`}</div>
      </div>
    </header>
  );
};

export default ChatHeader;
