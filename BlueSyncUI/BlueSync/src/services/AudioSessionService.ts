import axios from "axios";
import { AudioSession } from "../models/AudioSession";

const API_BASE_URL = "http://localhost:7131/api/audio-sessions"; // Adjust URL if needed
const WS_URL = "wss://localhost:7131/ws"; // WebSocket URL

// Initialize WebSocket connection
const socket = new WebSocket(WS_URL);

socket.onopen = () => console.log("[WebSocket] Connected to server.");
socket.onmessage = (event) => console.log("[WebSocket] Message received:", JSON.parse(event.data));
socket.onclose = () => console.log("[WebSocket] Connection closed.");

export const AudioSessionService = {
    getAllAudioSessions: async (): Promise<AudioSession[]> => {
        const response = await axios.get<AudioSession[]>(`${API_BASE_URL}`);
        return response.data;
    },

    getAudioSessionById: async (id: number): Promise<AudioSession> => {
        const response = await axios.get<AudioSession>(`${API_BASE_URL}/${id}`);
        return response.data;
    },

    startAudioSession: async (groupId: number, audioSource: string): Promise<void> => {
        await axios.post(`${API_BASE_URL}/start/${groupId}`, audioSource, {
            headers: { "Content-Type": "application/json" },
        });

        // Notify WebSocket clients
        socket.send(JSON.stringify({ type: "session_started", payload: { groupId, audioSource } }));
    },

    stopAudioSession: async (sessionId: number): Promise<void> => {
        await axios.delete(`${API_BASE_URL}/stop/${sessionId}`);

        // Notify WebSocket clients
        socket.send(JSON.stringify({ type: "session_stopped", payload: { sessionId } }));
    },

    togglePlayback: async (sessionId: number): Promise<void> => {
        await axios.post(`${API_BASE_URL}/toggle-playback/${sessionId}`);

        // Notify WebSocket clients
        socket.send(JSON.stringify({ type: "playback_toggled", payload: { sessionId } }));
    },

    addToQueue: async (sessionId: number, audioSource: string): Promise<string[]> => {
        const response = await axios.post<string[]>(`${API_BASE_URL}/queue/add/${sessionId}`, audioSource, {
            headers: { "Content-Type": "application/json" },
        });

        // Notify WebSocket clients
        socket.send(JSON.stringify({ type: "queue_updated", payload: { sessionId, queue: response.data } }));

        return response.data;
    },

    changeQueue: async (sessionId: number, newQueue: string[]): Promise<void> => {
        await axios.put(`${API_BASE_URL}/queue/change/${sessionId}`, newQueue, {
            headers: { "Content-Type": "application/json" },
        });

        // Notify WebSocket clients
        socket.send(JSON.stringify({ type: "queue_changed", payload: { sessionId, queue: newQueue } }));
    },

    getQueue: async (sessionId: number): Promise<string[]> => {
        const response = await axios.get<string[]>(`${API_BASE_URL}/queue/${sessionId}`);
        return response.data;
    },
};
