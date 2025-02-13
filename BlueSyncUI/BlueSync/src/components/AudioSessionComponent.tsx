import React, { useEffect, useState } from "react";
import { View, Text, FlatList, Button, TextInput } from "react-native";
import { AudioSessionService } from "../services/AudioSessionService";
import { AudioSession } from "../models/AudioSession";

const AudioSessionComponent: React.FC = () => {
  const [audioSessions, setAudioSessions] = useState<AudioSession[]>([]);
  const [audioSource, setAudioSource] = useState("");
  const [selectedSessionId, setSelectedSessionId] = useState<number | null>(null);

  useEffect(() => {
    fetchAudioSessions();
  }, []);

  const fetchAudioSessions = async () => {
    try {
      const sessions = await AudioSessionService.getAllAudioSessions();
      setAudioSessions(sessions);
    } catch (error) {
      console.error("Error fetching audio sessions:", error);
    }
  };

  const handleStartSession = async () => {
    if (audioSource.trim() === "") return;
    try {
      await AudioSessionService.startAudioSession(1, audioSource); // Default to Group 1 for now
      setAudioSource("");
      fetchAudioSessions();
    } catch (error) {
      console.error("Error starting session:", error);
    }
  };

  const handleStopSession = async (sessionId: number) => {
    try {
      await AudioSessionService.stopAudioSession(sessionId);
      fetchAudioSessions();
    } catch (error) {
      console.error("Error stopping session:", error);
    }
  };

  const handleTogglePlayback = async (sessionId: number) => {
    try {
      await AudioSessionService.togglePlayback(sessionId);
      fetchAudioSessions();
    } catch (error) {
      console.error("Error toggling playback:", error);
    }
  };

  const handleAddToQueue = async () => {
    if (!selectedSessionId || audioSource.trim() === "") return;
    try {
      await AudioSessionService.addToQueue(selectedSessionId, audioSource);
      setAudioSource("");
    } catch (error) {
      console.error("Error adding to queue:", error);
    }
  };

  return (
    <View>
      <Text style={{ fontSize: 20, fontWeight: "bold", marginBottom: 10 }}>Audio Sessions</Text>
      <FlatList
        data={audioSessions}
        keyExtractor={(item) => item.id.toString()}
        renderItem={({ item }) => (
          <View style={{ padding: 10, borderBottomWidth: 1 }}>
            <Text>Session ID: {item.id}</Text>
            <Text>Playing: {item.isPlaying ? "Yes" : "No"}</Text>
            <Button title="Toggle Playback" onPress={() => handleTogglePlayback(item.id)} />
            <Button title="Stop Session" onPress={() => handleStopSession(item.id)} />
          </View>
        )}
      />
      <TextInput
        placeholder="Enter Audio Source"
        value={audioSource}
        onChangeText={setAudioSource}
        style={{ borderWidth: 1, padding: 5, marginBottom: 10 }}
      />
      <Button title="Start New Session" onPress={handleStartSession} />
      <Button title="Add to Queue" onPress={handleAddToQueue} />
    </View>
  );
};

export default AudioSessionComponent;
