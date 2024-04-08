using Labyrinth_WPF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labyrinth_WPF.Persistence
{
    public interface IPersistence : IDisposable
    {
        void SaveGame(string path, int time, int playerX, int playerY, int size, int[,]? gameTable, List<Wall>? walls);
        void LoadGame(string path, out int time, out int playerX, out int playerY, out int size, out int[,]? gameTable, out List<Wall>? walls);
        int[,]? ReadMap(string path, int size);
    }
}
