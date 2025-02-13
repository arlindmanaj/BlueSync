import React, { useEffect, useState } from "react";
import { View, Text, FlatList, StyleSheet } from "react-native";
import { DeviceService } from "../services/DeviceService";
import { Device } from "../models/Device";

const DeviceList: React.FC = () => {
    const [devices, setDevices] = useState<Device[]>([]);

    useEffect(() => {
        fetchDevices();
    }, []);

    const fetchDevices = async () => {
        try {
            const data = await DeviceService.getAllDevices();
            setDevices(data);
        } catch (error) {
            console.error("Error fetching devices:", error);
        }
    };

    return (
        <View style={styles.container}>
            <Text style={styles.title}>Connected Devices</Text>
            <FlatList
                data={devices}
                keyExtractor={(item) => item.id.toString()}
                renderItem={({ item }) => (
                    <View style={styles.deviceItem}>
                        <Text style={styles.deviceName}>{item.name}</Text>
                        <Text>{item.isConnected ? "🟢 Connected" : "🔴 Disconnected"}</Text>
                    </View>
                )}
            />
        </View>
    );
};

const styles = StyleSheet.create({
    container: {
        flex: 1,
        padding: 20,
    },
    title: {
        fontSize: 18,
        fontWeight: "bold",
        marginBottom: 10,
    },
    deviceItem: {
        padding: 10,
        borderBottomWidth: 1,
        borderColor: "#ccc",
    },
    deviceName: {
        fontSize: 16,
        fontWeight: "bold",
    },
});

export default DeviceList;
