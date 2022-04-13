import styled from 'styled-components';
import { GbfRaidCode } from 'types';

const Board = styled.div`
    width: 480px;
    height: 720px;
    border: 2px solid purple;
`;

const Title = styled.div`
    text-align: center;
    font-size: 20px;
`;

const CloseButton = styled.span`
    float: right;
`;

interface BossBoardProps {
    bossName: string;
    raidCodes: GbfRaidCode[];
    onClose: () => void;
}

export const BossBoard = (props: BossBoardProps) => {
    const calculateTimeDiff = (time: string) => {
        const currentTime = new Date();
        const prevTime = new Date(time);
        const diff = (currentTime.getTime() - prevTime.getTime()) / 1000;

        const sec = Math.abs(diff);
        if (sec < 60) return sec.toFixed(0) + 's';

        const min = sec / 60;
        if (min < 60) return min.toFixed(0) + 'm';

        const hour = min / 60;
        if (hour < 24) return hour.toFixed(0) + 'h';

        const day = hour / 24;
        return day.toFixed(0) + 'd';
    };

    return (
        <Board>
            <Title>
                {props.bossName}
                <CloseButton onClick={props.onClose}>X</CloseButton>
            </Title>
            {props.raidCodes.map((rc, index) => (
                <div key={props.bossName + index}>
                    {rc.code} --- {calculateTimeDiff(rc.createdAt)}
                </div>
            ))}
        </Board>
    );
};
