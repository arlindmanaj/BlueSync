import React from "react";
import { SafeAreaView } from "react-native";
import DeviceList from "./components/DeviceList";
import AudioSessionComponent from "./components/AudioSessionComponent";
import DeviceGroupComponent from "./components/DeviceGroupComponent";

const App = () => {
    return (
      <SafeAreaView>
        <DeviceList />
        <AudioSessionComponent />
        <DeviceGroupComponent />
      </SafeAreaView>
    );
  };

export default App;
