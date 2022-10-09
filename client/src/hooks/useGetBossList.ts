import { useState } from 'react';
import { GbfRaidBoss } from 'types';

const useGetBossList = () => {
    const [bossList, setBossList] = useState<GbfRaidBoss[]>([]);

    const getLatestBossList = async (language: string) => {
        const response = await fetch('gbf-raid-bosses.json');
        const list: GbfRaidBoss[] = await response.json();
        sortBossList(list, language);
        setBossList(list);
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

    return { bossList, getLatestBossList };
};

export default useGetBossList;
