import { StatusBar } from 'expo-status-bar';
import { useState } from 'react';
import { Button, FlatList, StyleSheet, Text, TextInput, View } from 'react-native';

export default function App() {
  const [newCountry, setNewCountry] = useState("");
  const [countryList, setCountryList] = useState([]);

  const addCountry = () => {
    if (newCountry.trim() === "") return; // הוספה רק אם לא ריק
    setCountryList((currentList) => [
      ...currentList,
      { id: Date.now().toString(), cName: newCountry }
    ]);
    setNewCountry("");
  };

  return (
    <View style={styles.container}>
      <Text style={styles.title}>Country List</Text>

      <View style={styles.inputContainer}>
        <TextInput
          style={styles.input}
          placeholder="Enter a new country"
          value={newCountry}
          onChangeText={text => setNewCountry(text)}
        />
        <Button title="Add" onPress={addCountry} />
      </View>

      <FlatList
        style={{ width: '100%' }}
        contentContainerStyle={{ alignItems: 'center' }}
        data={countryList}
        keyExtractor={item => item.id}
        renderItem={({ item }) => (
          <View style={styles.countryItem}>
            <Text style={styles.countryText}>{item.cName}</Text> {/* Apply the new style */}
          </View>
        )}
      />
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#fff',
    paddingTop: 60,
    alignItems: 'center',
  },
  title: {
    fontSize: 24,
    fontWeight: 'bold',
    marginBottom: 20,
  },
  inputContainer: {
    marginBottom: 20,
    alignItems: 'center',
  },
  input: {
    height: 40,
    borderColor: 'gray',
    borderWidth: 1,
    borderRadius: 5,
    paddingHorizontal: 10,
    marginBottom: 10,
    width: 200,
  },
  countryItem: {
    padding: 10,
    backgroundColor: '#f1f1f1',
    borderRadius: 5,
    marginVertical: 5,
    width: '90%',
    alignItems: 'center',
  },
});
