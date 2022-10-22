import { BossBoard } from 'components/boss-board';
import { FlexBox } from 'components/flex-box';
import { Modal, ModalTitle } from 'components/modal';
import { RoundButton } from 'components/round-button';
import useGetBossList from 'hooks/useGetBossList';
import useJoinRaid from 'hooks/useJoinRaid';
import { useState } from 'react';
import { GbfRaidBoss } from 'types';

const MainPage = () => {
    const [isOpen, setIsOpen] = useState(false);
    const [lang, setLang] = useState<'eng' | 'jap'>('jap');
    const { bossList, getLatestBossList } = useGetBossList();
    const { raids, joinRaid, quitRaid } = useJoinRaid();

    const openBossList = async (language: string) => {
        await getLatestBossList(language);
        setIsOpen(true);
    };

    const selectRaid = async (boss: GbfRaidBoss) => {
        await joinRaid(boss);
        setIsOpen(false);
    };

    const copyCode = (code: string) => navigator.clipboard.writeText(code);

    return (
        <>
            <Modal isOpen={isOpen} onClose={() => setIsOpen(false)}>
                <ModalTitle showCloseButton={true}>Raid Bosses</ModalTitle>
                {bossList.map((boss, index) => (
                    <div key={index} onClick={async () => await selectRaid(boss)}>
                        {boss.japName}
                    </div>
                ))}
            </Modal>

            <FlexBox>
                {Object.keys(raids).map((key) => (
                    <BossBoard
                        key={raids[key].perceptualHash}
                        bossName={raids[key].japName}
                        raidCodes={raids[key].raidCode}
                        onClose={async () => await quitRaid(raids[key].perceptualHash)}
                        copyCode={copyCode}
                    />
                ))}
            </FlexBox>
            {!isOpen && <RoundButton label="+" onClick={() => openBossList(lang)}></RoundButton>}
        </>
    );
};

export default MainPage;
