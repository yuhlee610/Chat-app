import React, { useCallback, useEffect } from "react";
import "./Modal.scss";

interface ModalProps {
  title: string;
  openModal: boolean;
  onToggleModal: () => void;
}

const Modal: React.FC<ModalProps> = ({
  title,
  children,
  openModal,
  onToggleModal,
}) => {
  const clickOutside = useCallback((event: MouseEvent) => {
    if ((event.target as HTMLDivElement).className.includes("modal")) {
      onToggleModal();
    }
  }, []);
  useEffect(() => {
    window.addEventListener("click", clickOutside);
    return () => {
      window.removeEventListener("click", clickOutside);
    };
  }, [clickOutside]);

  useEffect(() => {
    if (!openModal) {
      window.removeEventListener("click", clickOutside);
    }
  }, [clickOutside, openModal]);

  return (
    <div className={`modal`}>
      <div className="content">
        <div className="title">
          <span>{title}</span>
          <span className="close" onClick={onToggleModal}>
            &times;
          </span>
        </div>
        <div className="body">{children}</div>
      </div>
    </div>
  );
};

export default Modal;
