import { ReactNode } from 'react';
import styled from 'styled-components';

const Container = styled.div`
    display: flex;
`;

interface FlexBoxProps {
    children: ReactNode;
}
export const FlexBox = (props: FlexBoxProps) => {
    return <Container>{props.children}</Container>;
};
