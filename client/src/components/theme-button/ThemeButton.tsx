import styled from 'styled-components';

const Button = styled.div`
    width: 50px;
    height: 50px;
`;

interface ThemeButtonProps {
    toggleTheme: () => void;
}

const ThemeButton = (props: ThemeButtonProps) => {
    return <Button onClick={props.toggleTheme}>Toggle</Button>;
};

export default ThemeButton;
