import { Theme } from 'components/theme/Themes';
import { createGlobalStyle } from 'styled-components';

export const GlobalTheme = createGlobalStyle<{ theme: Theme }>`
    body {
        color: ${({ theme }) => theme.color};
        background-color : ${({ theme }) => theme.backgroundColor};
        transition: all 0.50s linear;
    }
`;
