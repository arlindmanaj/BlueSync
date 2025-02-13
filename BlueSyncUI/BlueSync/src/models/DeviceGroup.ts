export interface DeviceGroup {
    id: number;
    name?: string;
    deviceIds: number[]; // Store device IDs instead of full objects
  }
  