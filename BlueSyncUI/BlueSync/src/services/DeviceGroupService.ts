import axios from "axios";
import { DeviceGroup } from "../models/DeviceGroup";

const API_BASE_URL = "http://localhost:7131/api/device-groups"; // Replace with your actual backend URL

export const getAllDeviceGroups = async (): Promise<DeviceGroup[]> => {
  const response = await axios.get<DeviceGroup[]>(`${API_BASE_URL}`);
  return response.data;
};

export const getDeviceGroupById = async (id: number): Promise<DeviceGroup> => {
  const response = await axios.get<DeviceGroup>(`${API_BASE_URL}/${id}`);
  return response.data;
};

export const createDeviceGroup = async (name: string, deviceIds: number[]): Promise<void> => {
  await axios.post(`${API_BASE_URL}`, { name, deviceIds });
};

export const updateDeviceGroup = async (id: number, name: string, deviceIds: number[]): Promise<void> => {
  await axios.put(`${API_BASE_URL}/${id}`, { name, deviceIds });
};

export const deleteDeviceGroup = async (id: number): Promise<void> => {
  await axios.delete(`${API_BASE_URL}/${id}`);
};

export const assignDeviceToGroup = async (groupId: number, deviceId: number): Promise<void> => {
  await axios.post(`${API_BASE_URL}/${groupId}/assign-device/${deviceId}`);
};
