export interface GbfRaidBoss {
    perceptualHash: string;
    engName: string;
    japName: string;
    level: number;
}

export interface GbfRaidCode {
    createdAt: string;
    code: string;
}

export interface Raid {
    perceptualHash: string;
    engName: string;
    japName: string;
    level: number;
    raidCode: GbfRaidCode[];
}
