import { ModalBoard, ModalCloseButton, ModalContent } from 'components/modal/Modal.styles';
import { ModalProps } from 'types';

const Modal = (props: ModalProps) => {
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

export default Modal;
