import { AddButton } from 'components/add-button';
import { Modal, ModalTitle } from 'components/modal';
import { useState } from 'react';
import { GbfRaidBoss } from 'types';

const MainPage = () => {
    const [isOpen, setIsOpen] = useState(false);
    const [bossList, setBossList] = useState<GbfRaidBoss[]>([]);

    const getBossList = async () => {
        const response = await fetch('gbf-raid-bosses.json');
        const list: GbfRaidBoss[] = await response.json();
        setBossList(list);
    };

    const test = async () => {
        await getBossList();
        setIsOpen(true);
    };

    return (
        <>
            <Modal isOpen={isOpen} onClose={() => setIsOpen(false)}>
                <ModalTitle showCloseButton={true} />
                {bossList.map((boss, index) => (
                    <div key={index}>{boss.japName}</div>
                ))}
            </Modal>
            <AddButton onClick={() => test()}></AddButton>
        </>
    );
};

export default MainPage;
