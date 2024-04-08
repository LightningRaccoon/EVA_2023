using Labyrinth_game.Model;

namespace Labyrinth_game.Persistence
{
    public interface IPersistence : IDisposable
    {
        void SaveGame(string path, int time, int playerX, int playerY, int size, int[,]? gameTable, List<Wall>? walls);
        void LoadGame(string path, out int time, out int playerX, out int playerY, out int size, out int[,]? gameTable, out List<Wall>? walls);
        int[,]? ReadMap(string path, int size);
    }
}

