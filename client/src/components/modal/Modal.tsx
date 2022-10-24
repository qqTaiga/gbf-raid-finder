import { ReactNode } from 'react';
import styled from 'styled-components';

const ModalBoard = styled.div`
    position: fixed;
    left: 0;
    top: 0;
    right: 0;
    bottom: 0;
    background-color: grey;
    display: flex;
    align-items: center;
    justify-content: center;
    opacity: 0.6;
`;

const ModalContent = styled.div`
    background: ${({ theme }) => theme.backgroundColor};
    color: ${({ theme }) => theme.color}
    height: 90%;
    width: 70%;
    overflow-y: scroll;
    padding: 10px 40px;
    border: 1px solid ${({ theme }) => theme.color};
`;

interface ModalProps {
    children: ReactNode;
    isOpen: boolean;
    onClose: () => void;
}

export const Modal = (props: ModalProps) => {
    if (!props.isOpen) return null;

    return (
        <ModalBoard onClick={props.onClose}>
            <ModalContent onClick={(e) => e.stopPropagation()}>{props.children}</ModalContent>
        </ModalBoard>
    );
};
