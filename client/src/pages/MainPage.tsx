import { AddButton } from 'components/add-button';
import { BossBoard } from 'components/boss-board';
import { FlexBox } from 'components/flex-box';
import { Modal, ModalTitle } from 'components/modal';
import { useState } from 'react';
import { GbfRaidBoss, Raid } from 'types';

const MainPage = () => {
    const [isOpen, setIsOpen] = useState(false);
    const [lang, setLang] = useState<'eng' | 'jap'>('jap');
    const [bossList, setBossList] = useState<GbfRaidBoss[]>([]);
    const [raids, setRaids] = useState<Map<string, Raid>>(new Map());

    const getBossList = async (language: string) => {
        const response = await fetch('gbf-raid-bosses.json');
        const list: GbfRaidBoss[] = await response.json();
        sortBossList(list, language);
        setBossList(list);
    };

    const openBossList = async (language: string) => {
        await getBossList(language);
        setIsOpen(true);
    };

    const sortBossList = (list: GbfRaidBoss[], language: string) => {
        list.sort((a, b) => {
            if (a.level != b.level) return a.level - b.level;

            if (language == 'jap') {
                if (a.japName > b.japName) return 1;
                else return -1;
            } else {
                if (a.engName > b.engName) return 1;
                else return -1;
            }
        });
    };

    const joinRaid = (boss: GbfRaidBoss) => {
        const raid: Raid = {
            perceptualHash: boss.perceptualHash,
            engName: boss.engName,
            japName: boss.japName,
            level: boss.level,
            raidCode: [],
        };
        raids.set(boss.perceptualHash, raid);
        setRaids(new Map(raids));
        setIsOpen(false);
    };

    const quitRaid = (perceptualHash: string) => {
        raids.delete(perceptualHash);
        setRaids(new Map(raids));
    };

    const copyCode = (code: string) => navigator.clipboard.writeText(code);

    return (
        <>
            <Modal isOpen={isOpen} onClose={() => setIsOpen(false)}>
                <ModalTitle showCloseButton={true}>Raid Bosses</ModalTitle>
                {bossList.map((boss, index) => (
                    <div key={index} onClick={() => joinRaid(boss)}>
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
