import { AddButton } from 'components/add-button';
import { BossBoard } from 'components/boss-board';
import { FlexBox } from 'components/flex-box';
import { Modal, ModalTitle } from 'components/modal';
import { useState } from 'react';
import { GbfRaidBoss, GbfRaidCode } from 'types';

const MainPage = () => {
    const [isOpen, setIsOpen] = useState(false);
    const [bossList, setBossList] = useState<GbfRaidBoss[]>([]);
    const [joinedRaid, setJoinedRaid] = useState<GbfRaidBoss[]>([]);

    const getBossList = async () => {
        const response = await fetch('gbf-raid-bosses.json');
        const list: GbfRaidBoss[] = await response.json();
        setBossList(list);
    };

    const openBossList = async () => {
        await getBossList();
        setIsOpen(true);
    };

    const joinBoss = (boss: GbfRaidBoss) => {
        const tmp = joinedRaid.slice();
        tmp.push(boss);
        setJoinedRaid(tmp);
        setIsOpen(false);
        console.log(joinedRaid);
    };

    const exitRaid = (perceptualHash: string) => {
        const filtered = joinedRaid.filter((raid) => raid.perceptualHash != perceptualHash);
        setJoinedRaid(filtered);
    };
    const test1: GbfRaidCode = { code: 'ADJEIFJ', createdAt: '2022-04-10T17:35:11.000Z' };
    const test2: GbfRaidCode = { code: 'ADJEIFJ', createdAt: '2022-04-10T17:35:11.000Z' };

    return (
        <>
            <Modal isOpen={isOpen} onClose={() => setIsOpen(false)}>
                <ModalTitle showCloseButton={true}>Raid Bosses</ModalTitle>
                {bossList.map((boss, index) => (
                    <div key={index} onClick={() => joinBoss(boss)}>
                        {boss.japName}
                    </div>
                ))}
            </Modal>

            <FlexBox>
                {joinedRaid.map((raid) => (
                    <BossBoard
                        key={raid.perceptualHash}
                        bossName={raid.japName}
                        raidCodes={[test1, test2]}
                        onClose={() => exitRaid(raid.perceptualHash)}
                    />
                ))}
            </FlexBox>
            {!isOpen ? <AddButton onClick={() => openBossList()}></AddButton> : ''}
        </>
    );
};

export default MainPage;
