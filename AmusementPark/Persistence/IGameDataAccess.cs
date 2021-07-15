using AmusementPark.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AmusementPark.Persistence
{
    public interface IGameDataAccess
    {
        Task SaveAsync(string path, GameTable table, List<Guest> listOfGuests, Player player, DateTime time);

        Task<(GameTable, List<Guest>, Player, DateTime)> LoadAsync(string path);
    }
}
