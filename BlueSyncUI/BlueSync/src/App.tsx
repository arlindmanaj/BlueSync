import React from "react";
import { SafeAreaView } from "react-native";
import DeviceList from "./components/DeviceList";

const App: React.FC = () => {
    return (
        <SafeAreaView>
            <DeviceList />
        </SafeAreaView>
    );
};

export default App;
