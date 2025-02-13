import axios from "axios";
import { DeviceGroup } from "../models/DeviceGroup";

const API_BASE_URL = "http://localhost:7131/api/device-groups"; // Adjust URL if needed
const WS_URL = "wss://localhost:7131/ws"; // Adjust WebSocket URL if necessary

// Initialize WebSocket connection
const socket = new WebSocket(WS_URL);

// WebSocket event listeners
socket.onopen = () => {
    console.log("[WebSocket] Connected to server.");
};

socket.onmessage = (event) => {
    const message = JSON.parse(event.data);
    console.log("[WebSocket] Message received:", message);
    // Handle incoming WebSocket messages here (e.g., update state in React)
};

socket.onclose = () => {
    console.log("[WebSocket] Connection closed.");
};

// Service methods
export const DeviceGroupService = {
    getAllDeviceGroups: async (): Promise<DeviceGroup[]> => {
        const response = await axios.get<DeviceGroup[]>(`${API_BASE_URL}`);
        return response.data;
    },

    getDeviceGroupById: async (id: number): Promise<DeviceGroup> => {
        const response = await axios.get<DeviceGroup>(`${API_BASE_URL}/${id}`);
        return response.data;
    },

    createDeviceGroup: async (name: string, deviceIds: number[]): Promise<void> => {
        await axios.post(`${API_BASE_URL}`, { name, deviceIds }, {
            headers: { "Content-Type": "application/json" },
        });

        // Send WebSocket event to notify clients
        socket.send(JSON.stringify({ type: "group_created", payload: { name, deviceIds } }));
    },

    updateDeviceGroup: async (id: number, name: string, deviceIds: number[]): Promise<void> => {
        await axios.put(`${API_BASE_URL}/${id}`, { name, deviceIds }, {
            headers: { "Content-Type": "application/json" },
        });

        // Send WebSocket event to notify clients
        socket.send(JSON.stringify({ type: "group_updated", payload: { id, name, deviceIds } }));
    },

    deleteDeviceGroup: async (id: number): Promise<void> => {
        await axios.delete(`${API_BASE_URL}/${id}`);

        // Send WebSocket event to notify clients
        socket.send(JSON.stringify({ type: "group_deleted", payload: { id } }));
    },

    assignDeviceToGroup: async (groupId: number, deviceId: number): Promise<void> => {
        await axios.post(`${API_BASE_URL}/${groupId}/assign-device/${deviceId}`);

        // Send WebSocket event to notify clients
        socket.send(JSON.stringify({ type: "device_assigned", payload: { groupId, deviceId } }));
    },
};
