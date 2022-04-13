import { ReactNode } from 'react';
import styled from 'styled-components';

const Header = styled.div``;

const CloseButton = styled.div``;

interface ModalHeaderProps {
    children: ReactNode;
    showCloseButton: boolean;
}

export const ModalTitle = (props: ModalHeaderProps) => {
    return (
        <Header>
            {props.children}
            {props.showCloseButton ? <CloseButton></CloseButton> : ''}
        </Header>
    );
};
