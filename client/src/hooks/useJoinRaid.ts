import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { useEffect, useState } from 'react';
import { GbfRaidBoss, GbfRaidCode, Raid } from 'types';

const useJoinRaid = () => {
    const maxRaidCodeCount = 7;
    const [connection, setConnection] = useState<HubConnection | null>(null);
    const [raids, setRaids] = useState<Record<string, Raid>>({});

    useEffect(() => {
        const connection = new HubConnectionBuilder()
            .withUrl('http://localhost:5220/raids')
            .withAutomaticReconnect()
            .build();

        setConnection(connection);
    }, []);

    useEffect(() => {
        const run = async () => {
            if (connection) {
                await connection.start().catch((e) => console.log('Connection failed: ', e));

                connection.on(
                    'ReceiveRaidCode',
                    (perceptualHash: string, raidCode: GbfRaidCode) => {
                        setRaids((prev) => {
                            const tmpRaids = { ...prev };
                            const raid = tmpRaids[perceptualHash];
                            if (raid === undefined) {
                                return tmpRaids;
                            }

                            const raidCodes = raid.raidCode.slice();

                            if (raidCodes.length >= maxRaidCodeCount) {
                                raidCodes.pop();
                            }
                            raidCodes.unshift(raidCode);

                            tmpRaids[perceptualHash] = { ...raid, raidCode: raidCodes };
                            return tmpRaids;
                        });
                    },
                );
            }
        };
        run();
    }, [connection]);

    const joinRaid = async (boss: GbfRaidBoss) => {
        if (!connection || raids[boss.perceptualHash] !== undefined) {
            return;
        }

        await connection.invoke('JoinRaid', boss.perceptualHash);
        const raid: Raid = {
            perceptualHash: boss.perceptualHash,
            engName: boss.engName,
            japName: boss.japName,
            level: boss.level,
            raidCode: [],
        };
        setRaids((prev) => {
            const tmpRaids = { ...prev };
            tmpRaids[boss.perceptualHash] = raid;
            return tmpRaids;
        });
    };

    const quitRaid = async (perceptualHash: string) => {
        if (!connection) {
            return;
        }

        await connection.invoke('LeaveRaid', perceptualHash);
        setRaids((prev) => {
            const tmpRaids = { ...prev };
            delete tmpRaids[perceptualHash];
            return tmpRaids;
        });
    };

    return { raids, joinRaid, quitRaid };
};

export default useJoinRaid;
