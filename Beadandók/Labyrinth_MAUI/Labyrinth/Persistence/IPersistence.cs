using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Labyrinth.Model;

namespace Labyrinth.Persistence
{
    public interface IPersistence : IDisposable
    {
        //Task SaveGame(string path, int time, int playerX, int playerY, int size, int[,]? gameTable, List<Wall>? walls);
        //Task<(int, int, int, int, int[,], List<Wall>)> LoadGame(string path);
        Task<int[,]?> ReadMap(int size, Stream stream);
    }
}
