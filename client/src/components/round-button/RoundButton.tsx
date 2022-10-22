import styled from 'styled-components';

const Button = styled.div`
    background-color: purple;
    padding: 15px;
    width: 40px;
    height: 40px;
    position: absolute;
    bottom: 30px;
    right: 30px;
    cursor: pointer;
    margin: auto;

    text-align: center;
    font-weight: bold;
    border-radius: 50%;
    font-size: 35px;
    color: white;
`;

interface AddButtonProps {
    /**
     * Button label
     */
    label: string;
    /**
     * Optinal click handler
     */
    onClick?: () => void;
}

export const RoundButton = (props: AddButtonProps) => {
    return <Button onClick={props.onClick}>{props.label}</Button>;
};
