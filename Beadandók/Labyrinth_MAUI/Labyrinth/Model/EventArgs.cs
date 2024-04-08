

namespace Labyrinth.Model
{
    public class RefreshEventArgs : EventArgs
    {
        public RefreshEventArgs(int size, int[,]? gameTable, List<Wall>? walls)
        {
            TableSize = size;
            GameTable = gameTable;
            Walls = walls;
        }

        public int TableSize { get; }
        public int[,]? GameTable { get; }
        public List<Wall>? Walls { get; }
    }
    public class RefreshCellEventArgs : EventArgs
    {
        public RefreshCellEventArgs(int x, int y, CType t)
        {
            CellX = x;
            CellY = y;
            CellCType = t;
        }

        public int CellX { get; }
        public int CellY { get; }
        public CType CellCType { get; }
    }
    public class GameEndEventArgs : EventArgs
    {
        public GameEndEventArgs(string msg)
        {
            EndGameMessage = msg;
        }
        public string EndGameMessage { get; private set; }
    }
    public class RefreshTextEventArgs : EventArgs
    {
        public RefreshTextEventArgs(int elapsedTime)
        {
            ElapsedTime = elapsedTime;
        }
        public int ElapsedTime { get; }
    }
    public class SaveEventArgs : EventArgs
    {
        public SaveEventArgs(string msg)
        {
            SaveFailedMessage = msg;
        }
        public string SaveFailedMessage { get; }
    }
}
