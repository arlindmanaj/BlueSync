import React, { useEffect, useState } from "react";
import { View, Text, FlatList, Button } from "react-native";
import { DeviceGroupService } from "../services/DeviceGroupService";
import { DeviceGroup } from "../models/DeviceGroup";

const DeviceGroupComponent: React.FC = () => {
  const [deviceGroups, setDeviceGroups] = useState<DeviceGroup[]>([]);

  useEffect(() => {
    fetchDeviceGroups();
  }, []);

  const fetchDeviceGroups = async () => {
    try {
      const groups = await DeviceGroupService.getAllDeviceGroups();
      setDeviceGroups(groups);
    } catch (error) {
      console.error("Error fetching device groups:", error);
    }
  };

  const handleCreateGroup = async () => {
    try {
      await DeviceGroupService.createDeviceGroup("New Group", []);
      fetchDeviceGroups(); // Refresh list
    } catch (error) {
      console.error("Error creating device group:", error);
    }
  };

  return (
    <View>
      <Text style={{ fontSize: 20, fontWeight: "bold", marginBottom: 10 }}>Device Groups</Text>
      <FlatList
        data={deviceGroups}
        keyExtractor={(item) => item.id.toString()}
        renderItem={({ item }) => (
          <View style={{ padding: 10, borderBottomWidth: 1 }}>
            <Text>Name: {item.name}</Text>
            <Text>Devices: {item.deviceIds?.length || 0}</Text>
          </View>
        )}
      />
      <Button title="Create New Group" onPress={handleCreateGroup} />
    </View>
  );
};

export default DeviceGroupComponent;
