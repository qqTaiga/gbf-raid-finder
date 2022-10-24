import { Theme } from 'components/theme';

declare module 'styled-components' {
    //eslint-disable-next-line
    interface DefaultTheme extends Theme {}
}
