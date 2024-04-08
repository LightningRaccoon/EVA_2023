using System;
using System.Collections.Generic;
using System.IO;
using Labyrinth_WPF.Model;

namespace Labyrinth_WPF.Persistence
{
    public class Persistence : IPersistence
    { 
        /*
         *************************************************
         *          Variables to save the game           *
         *************************************************
         *                                               *
         *  path        -> file path                     *
         *  time        -> elapse game time              *
         *  playerX     -> player x position             *
         *  playerY     -> player y position             *
         *  gameTable   -> table of walls and paths      *
         *  size        -> size of table                 *
         *  walls       -> list of walls                 *
         *                                               *
         *************************************************
         */

        public void SaveGame(string path, int time, int playerX, int playerY, int size, int[,]? gameTable, List<Wall>? walls)
        {
            try
            {
                using StreamWriter writer = new(path);
                writer.WriteLine(time);
                writer.WriteLine(playerX);
                writer.WriteLine(playerY);
                writer.WriteLine(size);

                // Table writing
                for (var i = 0; i < size; i++)
                {
                    for (var j = 0; j < size; j++)
                    {
                        if (gameTable != null) writer.WriteAsync(gameTable[i, j] + " ");
                    }
                    writer.WriteLineAsync();
                }

                // Walls writing
                if (walls == null) return;
                foreach (var wall in walls)
                {
                    writer.WriteLine(wall.X + " " + wall.Y);
                }
            }
            catch (Exception)
            {
                throw new Exception("Saving game has failed :(");
            }
        }

        /*
         *************************************************
         *          Variables to load the game           *
         *************************************************
         *                                               *
         *  path        -> file path                     *
         *  time        -> elapse game time              *
         *  playerX     -> player x position             *
         *  playerY     -> player y position             *
         *  size        -> size of table                 *
         *  gameTable   -> table of walls and paths      *
         *  walls       -> list of walls                 *
         *                                               *
         *                                               *
         * Save file contains different data in each     *
         * line so it can be parsed                      *
         *************************************************
         */

        public void LoadGame(string path, out int time, out int playerX, out int playerY, out int size, out int[,]? gameTable, out List<Wall>? walls)
        {
            try
            {
                using StreamReader reader = new(path);
                time = int.Parse(reader.ReadLine() ?? string.Empty);
                playerX = int.Parse(reader.ReadLine() ?? string.Empty);
                playerY = int.Parse(reader.ReadLine() ?? string.Empty);
                size = int.Parse(reader.ReadLine() ?? string.Empty);
                gameTable = new int[size, size];

                for (var i = 0; i < size; i++)
                {
                    var line = reader.ReadLine();
                    var lines = line?.Split(" ");
                    if (lines == null) continue;
                    for (var j = 0; j < lines.Length - 1; j++)
                    {
                        gameTable[i, j] = int.Parse(lines[j]);
                    }
                }

                walls = new List<Wall>();
                //walls.Clear();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var lines = line?.Split(" ");
                    var wx = 0;
                    var wy = 0;
                    if (lines != null)
                    {
                        wx = int.Parse(lines[0]);
                        wy = int.Parse(lines[1]);
                    }

                    var w = new Wall(wx, wy);
                    walls.Add(w);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Loading game has failed", e);
            }
        }

        public int[,]? ReadMap(string path, int size)
        {
            try
            {
                using StreamReader reader = new(path);
                var result = new int[size, size];

                for (var i = 0; i < size; i++)
                {
                    var line = reader.ReadLine();
                    var lines = line?.Split(" ");
                    if (lines == null) continue;
                    for (var j = 0; j < lines.Length; j++)
                    {
                        if (result != null)
                        {
                            var sd = int.Parse(lines[j]);
                            result[i, j] = sd;
                        }
                    }
                }

                return result;
            }
            catch (Exception e)
            {
                throw new Exception("Loading map has failed", e);
            }
        }

        public void Dispose()
        {
            //GC.SuppressFinalize(this);
            //GC.KeepAlive(this);
        }
    }
}
