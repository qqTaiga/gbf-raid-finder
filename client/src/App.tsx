import { BossBoard } from 'components/boss-board';
import { GlobalTheme, darkTheme, lightTheme } from 'components/theme';
import { ThemeButton } from 'components/theme-button';
import MainPage from 'pages/MainPage';
import { useState } from 'react';
import { ThemeProvider } from 'styled-components';
import { GbfRaidCode } from 'types';

const App = () => {
    const [theme, setTheme] = useState<'light' | 'dark'>('light');
    const toggleTheme = () => {
        theme == 'light' ? setTheme('dark') : setTheme('light');
    };

    const test: GbfRaidCode = { code: 'ADJEIFJ', createdAt: '2022-04-10T17:35:11.000Z' };
    const test2: GbfRaidCode = { code: 'ADJEIFJ', createdAt: '2022-04-10T17:36:11.000Z' };

    return (
        <ThemeProvider theme={theme == 'light' ? lightTheme : darkTheme}>
            <>
                <GlobalTheme />
                <MainPage />
                <BossBoard bossName={'Lvl 200 Lindwurm'} raidCodes={[test, test2]} />
                <ThemeButton toggleTheme={() => toggleTheme()}></ThemeButton>
            </>
        </ThemeProvider>
    );
};

export default App;
