using AmusementPark.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace AmusementPark.Persistence
{
    public class GameDataAccess : IGameDataAccess
    {
        public async Task<(GameTable, List<Guest>, Player, DateTime)> LoadAsync(string path)
        {
            byte[] result;
            using (FileStream fStream = File.Open(path, FileMode.Open))
            {
                fStream.Seek(0, SeekOrigin.Begin);
                result = new byte[fStream.Length];
                await fStream.ReadAsync(result, 0, (int)fStream.Length);
                fStream.Close();
            }
            GameTable table;
            List<Guest> listOfGuests;
            Player player;

            BinaryFormatter binFormatter = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(result))
            {
                ms.Seek(0, SeekOrigin.Begin);
                table = binFormatter.Deserialize(ms) as GameTable;
                listOfGuests = binFormatter.Deserialize(ms) as List<Guest>;
                player = binFormatter.Deserialize(ms) as Player;
                //time = binFormatter.Deserialize(ms) as long;
            }

            return (table, listOfGuests, player, new DateTime(1, 1, 1, 0, 0, 0));
        }

        public async Task SaveAsync(string path, GameTable table, List<Guest> listOfGuests, Player player, DateTime time)
        {
            var binFormatter = new BinaryFormatter();
            var mStream = new MemoryStream();

            binFormatter.Serialize(mStream, table);
            binFormatter.Serialize(mStream, listOfGuests);
            binFormatter.Serialize(mStream, player);
            binFormatter.Serialize(mStream, time.ToBinary());

            using (FileStream fStream = File.Open(path, FileMode.Create))
            {
                fStream.Seek(0, SeekOrigin.Begin);
                await fStream.WriteAsync(mStream.ToArray(), 0, mStream.ToArray().Length);
                fStream.Close();
            }
        }
    }
}
