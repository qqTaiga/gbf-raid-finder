import { useEffect, useState } from 'react';
import styled from 'styled-components';
import { GbfRaidCode } from 'types';

const Board = styled.div`
    width: 480px;
    height: 720px;
    border: 2px solid purple;
`;

interface BossBoardProps {
    bossName: string;
    raidCodes: GbfRaidCode[];
}
export const BossBoard = (props: BossBoardProps) => {
    const [refresh, setRefresh] = useState<number>(0);

    const calculateTimeDiff = (time: string) => {
        const currentTime = new Date();
        const prevTime = new Date(time);
        const diff = (currentTime.getTime() - prevTime.getTime()) / 1000;
        return Math.abs(diff);
    };

    useEffect(() => {
        const updateTime = setTimeout(() => setRefresh(refresh + 1), 5000);
        return () => clearTimeout(updateTime);
    }, [refresh]);

    return (
        <Board>
            {props.bossName}
            {props.raidCodes.map((rc, index) => (
                <div key={props.bossName + index}>
                    {rc.code} --- {calculateTimeDiff(rc.createdAt)}
                </div>
            ))}
        </Board>
    );
};
