import styled from 'styled-components';
import { GbfRaidCode } from 'types';
import { calculateTimeDiff } from 'utils/DateTimeUtils';

const Board = styled.div`
    width: 480px;
    height: 720px;
    border: 2px solid purple;
`;

const Title = styled.h1`
    text-align: center;
    font-size: 20px;
`;

const CloseButton = styled.span`
    float: right;
`;

const RaidCode = styled.div`
    text-align: center;
    border: 1px solid black;
    padding: 10px;
`;

interface BossBoardProps {
    bossName: string;
    raidCodes: GbfRaidCode[];
    onClose: () => void;
    copyCode: (code: string) => void;
}

export const BossBoard = (props: BossBoardProps) => {
    return (
        <Board>
            <Title>
                {props.bossName}
                <CloseButton onClick={props.onClose}>X</CloseButton>
            </Title>
            {props.raidCodes &&
                props.raidCodes.map((rc, index) => (
                    <RaidCode key={props.bossName + index} onClick={() => props.copyCode(rc.code)}>
                        {rc.code} --- {calculateTimeDiff(rc.createdAt)}
                    </RaidCode>
                ))}
        </Board>
    );
};
