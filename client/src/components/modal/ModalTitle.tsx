import styled from 'styled-components';

const Header = styled.div``;

const CloseButton = styled.div``;

interface ModalHeaderProps {
    showCloseButton: boolean;
}

export const ModalTitle = (props: ModalHeaderProps) => {
    return (
        <Header>
            atre
            {props.showCloseButton ? <CloseButton></CloseButton> : ''}
        </Header>
    );
};
