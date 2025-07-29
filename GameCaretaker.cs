
using System.Text.Json;

// GameCaretaker.cs
public class GameCaretaker
{
    public void SaveGame(GameState gameState, string filePath)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string jsonString = JsonSerializer.Serialize(gameState, options);
        File.WriteAllText(filePath, jsonString);
    }

    public GameState LoadGame(string filePath)
    {
        string jsonString = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<GameState>(jsonString);
    }
}
