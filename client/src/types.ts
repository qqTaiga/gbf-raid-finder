export interface GbfRaidCode {
    createdAt: string;
    code: string;
}

export interface ModalProps {
    children: React.ReactNode;
    isOpen: boolean;
    onClose: () => void;
}

export interface RaidModalProps {
    isOpen: boolean;
}
