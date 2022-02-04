import React from "react";
import RoundedAvatar from "../RoundedAvatar/RoundedAvatar";
import "./ConversationItem.scss";

interface ConversationItemProps {
  src: string;
  name: string;
  latestMessage?: string;
  onToggle?: () => void;
  isToggle?: boolean;
}

const ConversationItem: React.FC<ConversationItemProps> = ({
  src,
  name,
  latestMessage,
  onToggle,
  isToggle,
}) => {
  return (
    <div className={`conversation-item ${isToggle && 'active'}`} onClick={onToggle}>
      <RoundedAvatar src={src} />
      <div className="main-content">
        <div>{name}</div>
        <div>{latestMessage}</div>
      </div>
    </div>
  );
};

export default ConversationItem;
