import { GlobalTheme, darkTheme, lightTheme } from 'components/theme';
import { ThemeProvider } from 'styled-components';

export const parameters = {
    actions: { argTypesRegex: '^on[A-Z].*' },
    controls: {
        matchers: {
            color: /(background|color)$/i,
            date: /Date$/,
        },
    },
};

export const globalTypes = {
    theme: {
        name: 'Theme',
        description: 'Global theme for components',
        defaultValue: 'light',
        toolbar: {
            icon: 'circlehollow',
            // Array of plain string values or MenuItem shape (see below)
            items: ['light', 'dark'],
            // Property that specifies if the name of the item will be displayed
            showName: true,
            // Change title based on selected value
            dynamicTitle: true,
        },
    },
};

const withThemeProvider = (Story, context) => {
    const theme = context.globals.theme;
    return (
        <ThemeProvider theme={theme == 'light' ? lightTheme : darkTheme}>
            <>
                <GlobalTheme />
                <Story />
            </>
        </ThemeProvider>
    );
};
export const decorators = [withThemeProvider];
