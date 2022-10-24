import { ReactNode } from 'react';
import styled from 'styled-components';

const Header = styled.div``;

const CloseButton = styled.div`
    float: right;
`;

interface ModalHeaderProps {
    children: ReactNode;
    onClickCloseButton?: () => void;
}

export const ModalTitle = (props: ModalHeaderProps) => {
    return (
        <Header>
            {props.children}
            {props.onClickCloseButton && (
                <CloseButton onClick={props.onClickCloseButton}>X</CloseButton>
            )}
        </Header>
    );
};
