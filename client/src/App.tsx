import { GlobalTheme, darkTheme, lightTheme } from 'components/theme';
import { ThemeButton } from 'components/theme-button';
import MainPage from 'pages/MainPage';
import { useState } from 'react';
import { ThemeProvider } from 'styled-components';

const App = () => {
    const [theme, setTheme] = useState<'light' | 'dark'>('light');
    const toggleTheme = () => {
        theme == 'light' ? setTheme('dark') : setTheme('light');
    };

    return (
        <ThemeProvider theme={theme == 'light' ? lightTheme : darkTheme}>
            <>
                <GlobalTheme />
                <MainPage />
                <ThemeButton toggleTheme={() => toggleTheme()}></ThemeButton>
            </>
        </ThemeProvider>
    );
};

export default App;
