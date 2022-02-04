import React from "react";
import "./MenuItem.scss";

interface MenuItemProps {
  isToggle?: boolean;
  onToggle?: () => void;
  title: string;
}

const MenuItem: React.FC<MenuItemProps> = ({
  isToggle,
  onToggle,
  children,
  title,
}) => {
  return (
    <div
      className={`menu-item ${isToggle && "active"}`}
      onClick={onToggle}
      title={title}
    >
      {children}
    </div>
  );
};

export default MenuItem;
