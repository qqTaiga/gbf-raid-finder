import styled from 'styled-components';

const Button = styled.div`
    position: absolute;
    bottom: 0;
    left: 0;
    width: 50px;
    height: 50px;
`;

interface ThemeButtonProps {
    toggleTheme: () => void;
}

export const ThemeButton = (props: ThemeButtonProps) => {
    return <Button onClick={props.toggleTheme}>Toggle</Button>;
};
