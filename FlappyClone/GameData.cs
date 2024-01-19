using System.Text.Json;

namespace FlappyClone
{
    [Serializable]
    public class GameData
    {
        public int Score { get; set; } = 0;
        public int HardScore { get; set; } = 0;

        public static GameData? current;
        public static void Load(string _filePath) {
            if (current != null) return;

            try {
                string _content = File.ReadAllText(_filePath);
                current = JsonSerializer.Deserialize<GameData>(_content);
            } catch (Exception _e) {
                Console.WriteLine("Could not load GameData: " + _e.Message);
                current = new GameData();
            }
        }

        public static void Save(string _filePath) {
            try {
                string _content = JsonSerializer.Serialize(current);
                File.WriteAllText(_filePath, _content);
            } catch (Exception _e) {
                Console.WriteLine("Could not Save GameData: " + _e.Message);
            }

        }

    }
}
