import axios from "axios";
import { Device } from "../models/Device";

const API_BASE_URL = "http://localhost:7131/api/devices"; // Adjust URL if needed
const WS_URL = "wss://localhost:7131/ws"; // WebSocket URL

// Initialize WebSocket connection
const socket = new WebSocket(WS_URL);

socket.onopen = () => console.log("[WebSocket] Connected to server.");
socket.onmessage = (event) => console.log("[WebSocket] Message received:", JSON.parse(event.data));
socket.onclose = () => console.log("[WebSocket] Connection closed.");

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
        
        // Notify WebSocket clients
        socket.send(JSON.stringify({ type: "device_added", payload: response.data }));

        return response.data;
    },

    updateDevice: async (id: number, device: Partial<Device>): Promise<Device> => {
        const response = await axios.put<Device>(`${API_BASE_URL}/${id}`, device);
        
        // Notify WebSocket clients
        socket.send(JSON.stringify({ type: "device_updated", payload: response.data }));

        return response.data;
    },

    deleteDevice: async (id: number): Promise<void> => {
        await axios.delete(`${API_BASE_URL}/${id}`);

        // Notify WebSocket clients
        socket.send(JSON.stringify({ type: "device_deleted", payload: { id } }));
    },

    setVolume: async (id: number, level: number): Promise<void> => {
        await axios.put(`${API_BASE_URL}/${id}/volume/${level}`);

        // Notify WebSocket clients
        socket.send(JSON.stringify({ type: "volume_updated", payload: { id, level } }));
    },
};
