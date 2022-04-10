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
`;

const ModalContent = styled.div`
    background: white;
    height: 90%;
    width: 70%;
    overflow-y: scroll;
`;

const ModalCloseButton = styled.button`
    border: 1px;
`;

interface ModalProps {
    children: React.ReactNode;
    isOpen: boolean;
    onClose: () => void;
}

export const Modal = (props: ModalProps) => {
    if (!props.isOpen) return null;

    return (
        <ModalBoard onClick={props.onClose}>
            <ModalContent onClick={(e) => e.stopPropagation()}>
                {props.children}
                <ModalCloseButton onClick={props.onClose}>Close</ModalCloseButton>
            </ModalContent>
        </ModalBoard>
    );
};
