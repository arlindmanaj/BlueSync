import { DeviceGroup } from "./DeviceGroup";

export interface AudioSession {
  id: number;
  groupId: number;
  group: DeviceGroup;
  audioSource?: string;
  isPlaying: boolean;
}
