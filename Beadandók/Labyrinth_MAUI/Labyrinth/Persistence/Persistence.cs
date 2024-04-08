using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Labyrinth.Model;

namespace Labyrinth.Persistence
{
    public class Persistence : IPersistence
    {
        private string? _directory = string.Empty;

        public Persistence(string? directory = null)
        {
            _directory = directory;
        }


        public async Task<int[,]?> ReadMap(int size, Stream stream)
        {
            try
            {
                Stream fileStream = stream;
                using StreamReader reader = new StreamReader(fileStream);
                var result = new int[size, size];

                for (var i = 0; i < size; i++)
                {
                    var line = await reader.ReadLineAsync();
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
