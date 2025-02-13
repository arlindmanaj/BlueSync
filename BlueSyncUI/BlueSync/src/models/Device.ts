export interface Device {
    id: number;
    name: string;
    macAddress: string;
    isConnected: boolean;
    volume: number;
    isMuted: boolean;
    previousVolume?: number;
}
