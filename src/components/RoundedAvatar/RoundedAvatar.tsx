import React from "react";

interface RoundedAvatarProps {
  style?: any;
  src: string;
}

const RoundedAvatar: React.FC<RoundedAvatarProps> = ({ style, src }) => {
  return (
    <img
      src={src}
      style={{ width: 48, height: 48, borderRadius: "50%", ...style }}
    />
  );
};

export default RoundedAvatar;
