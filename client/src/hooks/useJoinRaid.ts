import { useState } from 'react';
import { GbfRaidBoss, Raid } from 'types';

const useJoinRaid = () => {
    const [raids, setRaids] = useState<Map<string, Raid>>(new Map());

    const joinRaid = (boss: GbfRaidBoss) => {
        const raid: Raid = {
            perceptualHash: boss.perceptualHash,
            engName: boss.engName,
            japName: boss.japName,
            level: boss.level,
            raidCode: [
                { createdAt: new Date().toString(), code: 'ba' },
                { createdAt: '123', code: 'ba' },
            ],
        };
        raids.set(boss.perceptualHash, raid);
        setRaids(new Map(raids));
    };

    const quitRaid = (perceptualHash: string) => {
        raids.delete(perceptualHash);
        setRaids(new Map(raids));
    };

    return { raids, joinRaid, quitRaid };
};

export default useJoinRaid;
