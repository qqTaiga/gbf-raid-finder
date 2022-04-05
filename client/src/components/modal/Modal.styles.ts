import styled from 'styled-components';

export const ModalBoard = styled.div`
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

export const ModalContent = styled.div`
    width: 500px;
    background: white;
`;

export const ModalCloseButton = styled.button`
    border: 1px;
`;
