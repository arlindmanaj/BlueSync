import axios from "axios"
import { Device } from "../models/Device";

const API_BASE_URL = "http://localhost:7131/api/devices"; // Adjust URL if necessary

export const DeviceService = {
    getAllDevices: async (): Promise<Device[]> => {
        const response = await axios.get<Device[]>(`${API_BASE_URL}`);
        return response.data;
    },

    getDeviceById: async (id: number): Promise<Device> => {
        const response = await axios.get<Device>(`${API_BASE_URL}/${id}`);
        return response.data;
    },

    addDevice: async (device: Partial<Device>): Promise<Device> => {
        const response = await axios.post<Device>(`${API_BASE_URL}`, device);
        return response.data;
    },

    updateDevice: async (id: number, device: Partial<Device>): Promise<Device> => {
        const response = await axios.put<Device>(`${API_BASE_URL}/${id}`, device);
        return response.data;
    },

    deleteDevice: async (id: number): Promise<void> => {
        await axios.delete(`${API_BASE_URL}/${id}`);
    },

    setVolume: async (id: number, level: number): Promise<void> => {
        await axios.put(`${API_BASE_URL}/${id}/volume/${level}`);
    },
};
