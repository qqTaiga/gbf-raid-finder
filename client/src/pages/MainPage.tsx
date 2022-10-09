import { AddButton } from 'components/add-button';
import { BossBoard } from 'components/boss-board';
import { FlexBox } from 'components/flex-box';
import { Modal, ModalTitle } from 'components/modal';
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

    const selectRaid = (boss: GbfRaidBoss) => {
        joinRaid(boss);
        setIsOpen(false);
    };

    const copyCode = (code: string) => navigator.clipboard.writeText(code);

    return (
        <>
            <Modal isOpen={isOpen} onClose={() => setIsOpen(false)}>
                <ModalTitle showCloseButton={true}>Raid Bosses</ModalTitle>
                {bossList.map((boss, index) => (
                    <div key={index} onClick={() => selectRaid(boss)}>
                        {boss.japName}
                    </div>
                ))}
            </Modal>

            <FlexBox>
                {Array.from(raids.values()).map((raid) => (
                    <BossBoard
                        key={raid.perceptualHash}
                        bossName={raid.japName}
                        raidCodes={raid.raidCode}
                        onClose={() => quitRaid(raid.perceptualHash)}
                        copyCode={copyCode}
                    />
                ))}
            </FlexBox>
            {!isOpen ? <AddButton onClick={() => openBossList(lang)}></AddButton> : ''}
        </>
    );
};

export default MainPage;
