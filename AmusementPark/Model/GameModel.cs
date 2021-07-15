using AmusementPark.Model.Entities;
using AmusementPark.Persistence;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AmusementPark.Model
{
    public enum SelectedEntity { Field, Water, Road, Restaurant, WaterGame, Tree, Bush, Grass, FuseBox, GiantWheel, Buffet, Ringlispil }

    public class GameModel
    {

        private IGameDataAccess _dataAccess;
        private DateTime _gameTime;
        private readonly Timer _timer;
        private GameTable _table;
        private Player _player;
        private HashSet<Tuple<Facility, int, int>> _destination;
        private List<Guest> _listOfGuests;
        private List<Facility> _buildinInProgress;
        private List<(int, int)> _roads;
        private int[,] _matrix;
        private Dijkstra _dijkstra;
        private Random _random;

        #region Events
        //public event EventHandler<GameEventArgs> GameAdvanced;
        public event EventHandler<GameEventArgs> Refresh; // event to refresh the table
        public event EventHandler<EntityStatusChangedEventArgs> EntityStatusChanged; // event to indicate the change of an entity

        #endregion

        #region Properites

        public SelectedEntity SelectedToBuild { get; set; }

        public GameTable Table { get { return _table; } }

        public Player Player { get { return _player; } }

        public DateTime Time { get { return _gameTime; } }

        public bool IsPaused { get { return !_timer.Enabled; } }

        public List<Guest> Guests { get { return _listOfGuests; } }
        public List<(int, int)> Roads { get { return _roads; } }
        public bool IsOpened { get; set; }

        #endregion

        #region Constructor

        public GameModel(IGameDataAccess dataAccess)
        { // the constructor of the GameModel class
            _dataAccess = dataAccess;
            _table = new GameTable(13, 24); //create GameTable
            _player = new Player(); // create player
            _player.Money = 10000; // add money to player

            IsOpened = false;
            _table.GetMainGate().IsClosed = true; // set the park's open status

            _random = new Random();
            _destination = new HashSet<Tuple<Facility, int, int>>();

            _buildinInProgress = new List<Facility>();

            _listOfGuests = new List<Guest>();
            //_gametime inicialiálás:
            _gameTime = new DateTime(1, 1, 1, 0, 0, 0);

            //timer létrehozása
            _timer = new Timer(); // create the timer
            _timer.Interval = 1000;
            _timer.Tick += Timer_Tick;
            GameStart(); // start the game

            _roads = new List<(int, int)>();
            _dijkstra = new Dijkstra();
        }

        #endregion

        #region Public methods

        public void GameStart() // this method starts the timer
        {
            _timer.Start();
        }

        public void StartStopTime() // method to manage the time (start or stop)
        {
            if (_timer.Enabled)
            {
                _timer.Stop();
            }
            else
            {
                _timer.Start();
            }
        }
        public void OpenPark() // method to open the park for guests
        {
            if (IsOpened == false)
            {
                IsOpened = true;
                _table.GetMainGate().IsClosed = false;
                Tuple<int, int> coordinates = GetCoordinates(_table.GetMainGate());
                EntityStatusChanged(this, new EntityStatusChangedEventArgs(coordinates.Item1, coordinates.Item2));
            }
        }
        public bool PlaceGame(int x, int y) // method to place gane on the game table with x and y positions
        {
            Game game = new Game();
            if (_table.GetValue(x, y) is Field)
            {
                Field field = (Field)_table.GetValue(x, y);

                if (field.FieldType == Field.FieldTypes.Field) // checking what is the type of the game (watergame or game)
                {
                    if(SelectedToBuild == SelectedEntity.GiantWheel)
                    {
                        GiantWheel giantWheel = new GiantWheel();
                        game = giantWheel;
                    }
                    else if(SelectedToBuild == SelectedEntity.Ringlispil)
                    {
                        Ringlispil ringlispil = new Ringlispil();
                        game = ringlispil;
                    }
                    
                }
                else
                {
                    WaterGame waterGame = new WaterGame();
                    game = waterGame;
                    game.BuildingPrice = 30;
                    game.BuildingTimeInSec = 40;
                    game.PriceToKeepOpen = 10;
                    game.HasElectricity = false;
                }
                if (_player.Money >= game.BuildingPrice) //finish building method
                { // check if the money is enough to build
                    game.State = Facility.States.Building;
                    game.PlacingTime = DateTime.Now;

                    _table.SetValue(x, y, game);

                    SetIfFacilityIsNextToConnectedRoad(x, y);

                    _player.Money -= game.BuildingPrice;
                    _buildinInProgress.Add(game);
                    return true;
                }
            }

            return false;
        }
        public bool PlaceRestaurant(int x, int y) // method to place restaurant on the game table with x and y positions
        {
            Restaurant res = new Restaurant();
            if (_table.GetValue(x, y) is Field)
            {
                Field field = (Field)_table.GetValue(x, y);

                if (field.FieldType == Field.FieldTypes.Field)
                {
                    if(SelectedToBuild != SelectedEntity.Restaurant)
                    {
                        Buffet buffet = new Buffet();
                        res = buffet;
                    }
                    
                }

                if (_player.Money >= res.BuildingPrice) //finish building method
                { // check if the money is enough to build
                    res.State = Facility.States.Building;
                    res.PlacingTime = DateTime.Now;

                    _table.SetValue(x, y, res);

                    SetIfFacilityIsNextToConnectedRoad(x, y);

                    _player.Money -= res.BuildingPrice;
                    _buildinInProgress.Add(res);
                    return true;
                }

            }
            return false;
        }
        public bool PlaceRoad(int x, int y) // method to place road on the game table with x and y positions
        {
            if (_table.GetValue(x, y) is Field)
            {
                Field field = (Field)_table.GetValue(x, y);

                Road road = new Road();

                if (field.FieldType == Field.FieldTypes.Field) // checking what is the type of the roaad (road on water or simple road)
                {
                    road.BuildingPrice = 10;
                }
                else
                {
                    road.BuildingPrice = 20;
                }

                road.FieldType = field.FieldType;

                if (_player.Money >= road.BuildingPrice) // check if the money is enough to build
                {
                    _table.SetValue(x, y, road);
                    if (IsNextToConnected(x, y, road))
                    {
                        CheckConnections(x, y);
                        SetIfRoadConnectedToFacility(x, y);
                    }

                    _player.Money -= road.BuildingPrice;

                    SetMaxtrixDataByRoads();
                    SetRoadsImage();
                    return true;
                }
            }
            return false;
        }

        public bool PlaceWater(int x, int y) // method to place water on the game table with x and y positions
        {
            if (_table.GetValue(x, y) is Field)
            {
                Field field = (Field)_table.GetValue(x, y);
                if (field.FieldType == Field.FieldTypes.Water)
                {
                    return false;
                }

                field.FieldType = Field.FieldTypes.Water;

                if (_player.Money >= field.BuildingPriceWater)
                { // check if the money is enough to build
                    _player.Money -= field.BuildingWaterPrice;
                }
                return true;
            }
            return false;
        }

        public bool IsNextToConnected(int x, int y, Road road)
        { // method to check if the roads are connected
            if (x - 1 > 0 && _table.GetValue(x - 1, y) is Road && ((Road)_table.GetValue(x - 1, y)).IsConnectedToGate && !road.IsConnectedToGate)
            {
                road.IsConnectedToGate = true;
                SetIfRoadConnectedToFacility(x, y);
                return true;
            }
            if (x + 1 < _table.RowSize && _table.GetValue(x + 1, y) is Road && ((Road)_table.GetValue(x + 1, y)).IsConnectedToGate && !road.IsConnectedToGate)
            {
                road.IsConnectedToGate = true;
                SetIfRoadConnectedToFacility(x, y);
                return true;
            }
            if (y - 1 > 0 && _table.GetValue(x, y - 1) is Road && ((Road)_table.GetValue(x, y - 1)).IsConnectedToGate && !road.IsConnectedToGate)
            {
                road.IsConnectedToGate = true;
                SetIfRoadConnectedToFacility(x, y);
                return true;
            }
            if (y + 1 < _table.ColumnSize && _table.GetValue(x, y + 1) is Road && ((Road)_table.GetValue(x, y + 1)).IsConnectedToGate && !road.IsConnectedToGate)
            {
                road.IsConnectedToGate = true;
                SetIfRoadConnectedToFacility(x, y);
                return true;
            }

            return false;
        }
        private void CheckConnections(int x, int y)
        {
            if (x - 1 > 0 && _table.GetValue(x - 1, y) is Road && !((Road)_table.GetValue(x - 1, y)).IsConnectedToGate)
            {
                IsNextToConnected(x - 1, y, (Road)_table.GetValue(x - 1, y));
                CheckConnections(x - 1, y);
            }
            if (x + 1 < _table.RowSize && _table.GetValue(x + 1, y) is Road && !((Road)_table.GetValue(x + 1, y)).IsConnectedToGate)
            {
                IsNextToConnected(x + 1, y, (Road)_table.GetValue(x + 1, y));
                CheckConnections(x + 1, y);
            }
            if (y - 1 > 0 && _table.GetValue(x, y - 1) is Road && !((Road)_table.GetValue(x, y - 1)).IsConnectedToGate)
            {
                IsNextToConnected(x, y - 1, (Road)_table.GetValue(x, y - 1));
                CheckConnections(x, y - 1);
            }
            if (y + 1 < _table.ColumnSize && _table.GetValue(x, y + 1) is Road && !((Road)_table.GetValue(x, y + 1)).IsConnectedToGate)
            {
                IsNextToConnected(x, y + 1, (Road)_table.GetValue(x, y + 1));
                CheckConnections(x, y + 1);
            }
        }

        private Tuple<Facility, int, int> GetRandomDestination() // returns a random destination
        {
            int i = _random.Next(0, _destination.Count);
            Console.WriteLine("Add new random destination: " + i);
            foreach (var member in _destination)
            {
                if (i == 0)
                    return member;
                i--;
            }

            return null;

        }

        private void MoveTheGuest(Guest guest)
        {
            int x = guest.CurrentPosition.Item1;
            int y = guest.CurrentPosition.Item2;

            if (x == 12 && y == 12 && guest.IsWantGoHome)
            {
                _listOfGuests.Remove(guest);
                return;
            }


            if (guest.Path.Count == 0)
            {
                return;
            }

            if (guest.Path[0] == (x, y))
            {
                guest.Path.RemoveAt(0);
                if (guest.Path.Count == 0)
                {
                    return;
                }
            }
            if (guest.Path[0] != (x, y))
            {
                guest.CurrentPosition = (guest.Path[0].Item1, guest.Path[0].Item2);
            }
        }

        private void StartLongJourney(Guest guest)
        {

            if (guest.Target == null && _destination.Count > 0)
            {
                guest.Target = GetRandomDestination().Item1;
            }

            if (GetCoordinates(guest.Target) == null)
            {
                guest.Target = GetRandomDestination().Item1;
            }

            if (guest.Target != null)
            {
                int x = guest.CurrentPosition.Item1;
                int y = guest.CurrentPosition.Item2;

                Entity entity = guest.Target;

                Tuple<int, int> xy = GetCoordinates(entity);
                if (xy == null)
                {
                    return;
                }

                int item1 = xy.Item1;
                int item2 = xy.Item2;

                if (guest.IsWantGoHome && guest.Path.Count != 1)
                {
                    guest.Target = _table.GetValue(12, 12);
                    GetPathToDestination(guest);
                    MoveTheGuest(guest);
                    return;
                }

                if (x == item1 && y == item2)
                {
                    return;
                }

                if (guest.Path.Count == 0)
                {
                    if (CheckIfTheFacilityIsNextToGuest((Facility)guest.Target, guest.CurrentPosition.Item1, guest.CurrentPosition.Item2))
                    {
                        UseFacility(guest);

                        return;
                    }
                    else
                    {
                        GetPathToDestination(guest);
                    }
                }

                MoveTheGuest(guest);
            }
        }

        private bool CheckIfTheFacilityIsNextToGuest(Facility facility, int x, int y)
        {
            //fel
            if (x - 1 >= 0 && _table.GetValue(x - 1, y) is Facility && ((Facility)_table.GetValue(x - 1, y)).Equals(facility))
            {
                return true;
            }
            //le
            else if (x + 1 < _table.RowSize && _table.GetValue(x + 1, y) is Facility && ((Facility)_table.GetValue(x + 1, y)).Equals(facility))
            {
                return true;
            }
            //balra
            else if (y - 1 >= 0 && _table.GetValue(x, y - 1) is Facility && ((Facility)_table.GetValue(x, y - 1)).Equals(facility))
            {
                return true;
            }
            //jobbra
            else if (y + 1 < _table.ColumnSize && _table.GetValue(x, y + 1) is Facility && ((Facility)_table.GetValue(x, y + 1)).Equals(facility))
            {
                return true;
            }

            return false;
        }

        public void UseFacility(Guest guest)
        {
            int x = guest.CurrentPosition.Item1;
            int y = guest.CurrentPosition.Item2;
            Facility facility = (Facility)guest.Target;
            int item1 = GetCoordinates(facility).Item1;
            int item2 = GetCoordinates(facility).Item2;

            int numberOfGuest = GetGuestsAt(item1, item2).Count;

            if (x == item1 && y == item2)
            {
                return;
            }

            if (!guest.IsWilling(facility.PriceToUse))
            {
                guest.Target = null;
                return;
            }
            //fel
            if (x - 1 >= 0 && _table.GetValue(x - 1, y) is Facility && ((Facility)_table.GetValue(x - 1, y)).Equals(facility))
            {
                if (facility.Capacity > numberOfGuest && facility.State == Facility.States.Working)
                {
                    guest.CurrentPosition = (x - 1, y);
                }
                else if(facility.Capacity <= numberOfGuest)
                {
                    guest.ChangeHappiness(-1);
                }
            }
            //le
            else if (x + 1 < _table.RowSize && _table.GetValue(x + 1, y) is Facility && ((Facility)_table.GetValue(x + 1, y)).Equals(facility))
            {
                if (facility.Capacity > numberOfGuest && facility.State == Facility.States.Working)
                {
                    guest.CurrentPosition = (x + 1, y);

                }
                else if (facility.Capacity <= numberOfGuest)
                {
                    guest.ChangeHappiness(-1);
                }
            }
            //balra
            else if (y - 1 >= 0 && _table.GetValue(x, y - 1) is Facility && ((Facility)_table.GetValue(x, y - 1)).Equals(facility))
            {
                if (facility.Capacity > numberOfGuest && facility.State == Facility.States.Working)
                {
                    guest.CurrentPosition = (x, y - 1);
                }
                else if (facility.Capacity <= numberOfGuest)
                {
                    guest.ChangeHappiness(-1);
                }
            }
            //jobbra
            else if (y + 1 < _table.ColumnSize && _table.GetValue(x, y + 1) is Facility && ((Facility)_table.GetValue(x, y + 1)).Equals(facility))
            {
                if (facility.Capacity > numberOfGuest && facility.State == Facility.States.Working)
                {
                    guest.CurrentPosition = (x, y + 1);
                }
                else if (facility.Capacity <= numberOfGuest)
                {
                    guest.ChangeHappiness(-1);
                }
            }

            if (facility is Restaurant)
            {
                ((Restaurant)facility).GuestsInRestaurant = GetGuestsAt(item1, item2).Count;

            }
            else if (facility is Game)
            {
                ((Game)facility).GuestsInGame = GetGuestsAt(item1, item2).Count;
            }
        }

        public void PlaceAdditionalEntity(Entity entity, int x, int y)
        {
            if (_table.GetValue(x, y) is Field)
            {
                if (entity is Plant)
                {
                    ((Plant)entity).HappinessBoost = 10;
                    ((Plant)entity).Range = 3;
                }
                else if (entity is Electricity)
                {
                    entity.BuildingPrice = 25;
                    ((Electricity)entity).Range = 3;
                }

                if (_player.Money >= entity.BuildingPrice)
                {
                    _table.SetValue(x, y, entity);

                    _player.Money -= entity.BuildingPrice;
                }
            }
        }

        public void SetIfRoadConnectedToFacility(int x, int y)
        {
            if (!((Road)_table.GetValue(x, y)).IsConnectedToGate)
            {
                return;
            }
            if (x - 1 >= 0 && _table.GetValue(x - 1, y) is Facility)
            {
                ((Facility)_table.GetValue(x - 1, y)).IsNextToConnectedRoad = true;
                _destination.Add(new Tuple<Facility, int, int>((Facility)_table.GetValue(x - 1, y), x - 1, y));
                if (((Facility)_table.GetValue(x - 1, y)).State != Facility.States.Building)
                {
                    ((Facility)_table.GetValue(x - 1, y)).State = Facility.States.Working;
                }
                EntityStatusChanged(this, new EntityStatusChangedEventArgs(x - 1, y));
            }

            if (x + 1 < _table.RowSize && _table.GetValue(x + 1, y) is Facility)
            {
                ((Facility)_table.GetValue(x + 1, y)).IsNextToConnectedRoad = true;

                _destination.Add(new Tuple<Facility, int, int>((Facility)_table.GetValue(x + 1, y), x + 1, y));
                if (((Facility)_table.GetValue(x + 1, y)).State != Facility.States.Building)
                {
                    ((Facility)_table.GetValue(x + 1, y)).State = Facility.States.Working;
                }
                EntityStatusChanged(this, new EntityStatusChangedEventArgs(x + 1, y));
            }

            if (y - 1 >= 0 && _table.GetValue(x, y - 1) is Facility)
            {
                ((Facility)_table.GetValue(x, y - 1)).IsNextToConnectedRoad = true;
                _destination.Add(new Tuple<Facility, int, int>((Facility)_table.GetValue(x, y - 1), x, y - 1));
                if (((Facility)_table.GetValue(x, y - 1)).State != Facility.States.Building)
                {
                    ((Facility)_table.GetValue(x, y - 1)).State = Facility.States.Working;
                }
                EntityStatusChanged(this, new EntityStatusChangedEventArgs(x, y - 1));
            }

            if (y + 1 < _table.ColumnSize && _table.GetValue(x, y + 1) is Facility)
            {
                ((Facility)_table.GetValue(x, y + 1)).IsNextToConnectedRoad = true;
                _destination.Add(new Tuple<Facility, int, int>((Facility)_table.GetValue(x, y + 1), x, y + 1));
                if (((Facility)_table.GetValue(x, y + 1)).State != Facility.States.Building)
                {
                    ((Facility)_table.GetValue(x, y + 1)).State = Facility.States.Working;
                }
                EntityStatusChanged(this, new EntityStatusChangedEventArgs(x, y + 1));
            }
        }
        public void SetIfFacilityIsNextToConnectedRoad(int x, int y)
        {
            Facility facility = (Facility)(_table.GetValue(x, y));
            facility.IsNextToConnectedRoad = false;

            if (x - 1 >= 0 && _table.GetValue(x - 1, y) is Road && ((Road)_table.GetValue(x - 1, y)).IsConnectedToGate)
            {
                facility.IsNextToConnectedRoad = true;
                _destination.Add(new Tuple<Facility, int, int>(facility, x, y));
            }
            else if (x + 1 < _table.RowSize && _table.GetValue(x + 1, y) is Road && ((Road)_table.GetValue(x + 1, y)).IsConnectedToGate)
            {
                facility.IsNextToConnectedRoad = true;
                _destination.Add(new Tuple<Facility, int, int>(facility, x, y));
            }
            else if (y - 1 >= 0 && _table.GetValue(x, y - 1) is Road && ((Road)_table.GetValue(x, y - 1)).IsConnectedToGate)
            {
                facility.IsNextToConnectedRoad = true;
                _destination.Add(new Tuple<Facility, int, int>(facility, x, y));
            }
            else if (y + 1 < _table.ColumnSize && _table.GetValue(x, y + 1) is Road && ((Road)_table.GetValue(x, y + 1)).IsConnectedToGate)
            {
                facility.IsNextToConnectedRoad = true;
                _destination.Add(new Tuple<Facility, int, int>(facility, x, y));
            }
        }

        private (int, int) GetDestinationNextToTarget(Guest guest)
        {
            (int, int) target = (GetCoordinates(guest.Target).Item1, GetCoordinates(guest.Target).Item2);

            if (target.Item1 - 1 >= 0 && Table.GetValue(target.Item1 - 1, target.Item2) is Road)
            {
                return (target.Item1 - 1, target.Item2);
            }
            else if (target.Item1 + 1 < Table.RowSize && Table.GetValue(target.Item1 + 1, target.Item2) is Road)
            {
                return (target.Item1 + 1, target.Item2);
            }
            else if (target.Item2 - 1 >= 0 && Table.GetValue(target.Item1, target.Item2 - 1) is Road)
            {
                return (target.Item1, target.Item2 - 1);
            }
            else if (target.Item2 + 1 < Table.ColumnSize && Table.GetValue(target.Item1, target.Item2 + 1) is Road)
            {
                return (target.Item1, target.Item2 + 1);
            }

            return (-1, -1);
        }

        private void GuestLeaveFacility(Guest guest)
        {
            int x = guest.CurrentPosition.Item1;
            int y = guest.CurrentPosition.Item2;

            if (!(_table.GetValue(x, y) is Facility))
            {
                return;
            }

            if (x - 1 >= 0 && _table.GetValue(x - 1, y) is Road && ((Road)_table.GetValue(x - 1, y)).IsConnectedToGate)
            {
                guest.CurrentPosition = (x - 1, y);
            }
            else if (x + 1 < Table.RowSize && _table.GetValue(x + 1, y) is Road && ((Road)_table.GetValue(x + 1, y)).IsConnectedToGate)
            {
                guest.CurrentPosition = (x + 1, y);
            }
            else if (y - 1 >= 0 && _table.GetValue(x, y - 1) is Road && ((Road)_table.GetValue(x, y - 1)).IsConnectedToGate)
            {
                guest.CurrentPosition = (x, y - 1);

            }
            else if (y + 1 < _table.ColumnSize && _table.GetValue(x, y + 1) is Road && ((Road)_table.GetValue(x, y + 1)).IsConnectedToGate)
            {
                guest.CurrentPosition = (x, y + 1);
            }
        }

        private void GetPathToDestination(Guest guest)
        {
            (int, int) target = GetDestinationNextToTarget(guest);
            if (guest.IsWantGoHome)
            {
                target = (12, 12);
            }

            int start = GetRoadIndex(guest.CurrentPosition);
            if (start == -1)
            {
                GuestLeaveFacility(guest);
                start = GetRoadIndex(guest.CurrentPosition);
            }

            int destination = GetRoadIndex(target);
            _dijkstra.DijkstraAlgorithm(_matrix, start, destination, _roads, guest.Path);
        }

        // setting the matrix for Dijkstra
        private void SetMaxtrixDataByRoads()
        {
            for (int i = 0; i < Table.RowSize; i++)
            {
                for (int j = 0; j < Table.ColumnSize; j++)
                {
                    if (Table.GetValue(i, j) is Road)
                    {
                        if (!_roads.Contains((i, j)))
                        {
                            _roads.Add((i, j));
                        }
                    }
                }
            }

            _matrix = new int[_roads.Count, _roads.Count];

            _dijkstra.SetMatrix(_roads, _matrix);
        }

        private int GetRoadIndex((int, int) coordinate)
        {
            for (int i = 0; i < _roads.Count; i++)
            {
                if (_roads[i] == coordinate)
                {
                    return i;
                }
            }
            return -1;
        }

        private void SetRoadsImage()
        {
            foreach ((int, int) c in Roads)
            {

                int x = c.Item1;
                int y = c.Item2;
                if (!(_table.GetValue(x, y) is MainGate))
                {
                    bool up = false;
                    bool down = false;
                    bool right = false;
                    bool left = false;

                    if (x + 1 < _table.RowSize && _table.GetValue(x + 1, y) is Road)
                    {
                        down = true;
                    }

                    if (x - 1 >= 0 && _table.GetValue(x - 1, y) is Road)
                    {
                        up = true;
                    }

                    if (y - 1 >= 0 && _table.GetValue(x, y - 1) is Road)
                    {
                        left = true;
                    }

                    if (y + 1 < _table.ColumnSize && _table.GetValue(x, y + 1) is Road)
                    {
                        right = true;
                    }

                    ((Road)_table.GetValue(x, y)).SetImage(up, left, down, right);
                    EntityStatusChanged(this, new EntityStatusChangedEventArgs(x, y));
                }
            }
        }

        private Tuple<int, int> GetCoordinates(Entity ent)
        {
            for (int i = 0; i < _table.RowSize; i++)
            {
                for (int j = 0; j < _table.ColumnSize; j++)
                {
                    if (_table.GetValue(i, j).Equals(ent))
                    {
                        return (new Tuple<int, int>(i, j));
                    }
                }
            }


            Console.WriteLine("NULL");
            return null;
        }


        #region Reloads
        private void ReloadDestination()
        {
            for (int i = 0; i < _table.RowSize; i++)
            {
                for (int j = 0; j < _table.ColumnSize; j++)
                {
                    if (_table.GetValue(i, j) is Restaurant || _table.GetValue(i, j) is Game)
                    {
                        if (((Facility)_table.GetValue(i, j)).IsNextToConnectedRoad)
                        {
                            if (_table.GetValue(i, j) is Game)
                            {
                                _destination.Add(new Tuple<Facility, int, int>((Facility)_table.GetValue(i, j), i, j));
                            }
                            else
                            {
                                _destination.Add(new Tuple<Facility, int, int>((Facility)_table.GetValue(i, j), i, j));
                            }
                        }
                    }
                }
            }

            Console.WriteLine("RELOAD DESTINATION:");

            foreach (var item in _destination)
            {
                Console.WriteLine(item.Item1.GetType() + " x: " + item.Item2 + " y: " + item.Item3);
            }
        }

        private void ReloadRoads()
        {
            for (int i = 0; i < _table.RowSize; i++)
            {
                for (int j = 0; j < _table.ColumnSize; j++)
                {
                    if (_table.GetValue(i, j) is Road)
                    {
                        _roads.Add((i, j));
                    }
                }
            }
            SetRoadsImage();
            SetMaxtrixDataByRoads();
        }

        private void ReloadFacilityStatus()
        {
            for (int i = 0; i < _table.RowSize; i++)
            {
                for (int j = 0; j < _table.ColumnSize; j++)
                {
                    Entity ent = _table.GetValue(i, j);
                    if ((ent is Restaurant || ent is Game))
                    {
                        if (((Facility)ent).State == Facility.States.Building)
                        {
                            ((Facility)ent).State = Facility.States.Building;
                            EntityStatusChanged(this, new EntityStatusChangedEventArgs(i, j));
                            _buildinInProgress.Add((Facility)_table.GetValue(i, j));
                        }
                        else if (((Facility)ent).State == Facility.States.Stopped)
                        {
                            ((Facility)ent).State = Facility.States.Stopped;
                            EntityStatusChanged(this, new EntityStatusChangedEventArgs(i, j));
                        }
                    }
                }
            }
        }

        private void ReloadGuestsTarget()
        {
            foreach (var guest in _listOfGuests)
            {
                if (guest.Target != null)
                {
                    guest.Target = _table.GetValue(guest.TargetXY.Item1, guest.TargetXY.Item2);
                }
            }
        }

        private void SaveGuestsTarget()
        {
            foreach (var guest in _listOfGuests)
            {
                if (guest.Target != null)
                {
                    var xy = GetCoordinates(guest.Target);
                    guest.TargetXY = (xy.Item1, xy.Item2);
                }
            }
        }

        #endregion
        public void GuestPlantEffect(Guest guest)
        {
            int x = guest.CurrentPosition.Item1;
            int y = guest.CurrentPosition.Item2;

            int plantEffect = 0;

            for (int i = x - 3; i < x + 4; i++)
            {
                for (int j = y - 3; j < y + 4; j++)
                {
                    if (i >= 0 && i < _table.RowSize && j >= 0 && j < _table.ColumnSize && _table.GetValue(i, j) is Plant)
                    {
                        if (((Plant)_table.GetValue(i, j)).Range < i || ((Plant)_table.GetValue(i, j)).Range < j)
                        {
                            plantEffect += ((Plant)_table.GetValue(i, j)).HappinessBoost;
                        }
                    }
                }
            }

            guest.ChangeHappiness(plantEffect);
        }

        public void OnGameAdvanced()
        {
            for (int i = 0; i < _buildinInProgress.Count; i++)
            {
                Facility facility = _buildinInProgress[i];

                if (facility.PlacingTime.AddSeconds(facility.BuildingTimeInSec) < DateTime.Now)
                {
                    if (!facility.HasElectricity)
                    {
                        facility.State = Facility.States.Stopped;
                    }
                    else
                    {
                        facility.State = Facility.States.Working;
                    }
                    _buildinInProgress.Remove(facility);

                    Tuple<int, int> coordinates = GetCoordinates(facility);
                    if (coordinates != null)
                    {
                        EntityStatusChanged(this, new EntityStatusChangedEventArgs(coordinates.Item1, coordinates.Item2));
                    }
                }
            }

            foreach (Guest guest in _listOfGuests)
            {
                GuestPlantEffect(guest);    //ez a levonást is megoldja a boldogság alapján
                guest.ChangeHungriness(-1);
            }

            Refresh(this, new GameEventArgs());
        }

        public void CreateGuest()
        {
            Guest guest = new Guest();

            guest.CurrentPosition = (12, 12);
            int fee = ((MainGate)_table.GetValue(12, 12)).EnterPrice;
            if (!guest.IsWilling(fee, true))
            {
                return;
            }
            guest.Pay(fee);
            _player.Money += fee;

            _listOfGuests.Add(guest);
        }

        public void GuestsVisitPark()
        {
            if (!IsOpened || _destination.Count == 0)
            {
                return;
            }


            int fee = ((MainGate)_table.GetValue(12, 12)).EnterPrice;


            int additionalChance = 0;
            if (fee <= 50)
            {
                additionalChance = 20;
            }
            else if (fee > 50 && fee <= 100)
            {
                additionalChance = 10;
            }
            else if (fee > 100 && fee <= 200)
            {
                additionalChance = 5;
            }
            else if (fee > 200 && fee <= 400)
            {
                additionalChance = 2;
            }


            int chance = _random.Next(0, 101);

            int newGuests = 0;
            if (chance < 20 + additionalChance) //50%
            {
                newGuests = 1;
            }
            if (chance < 10 + additionalChance)  //40%
            {
                newGuests = 2;
            }
            if (chance < 5 + additionalChance)   //35%
            {
                newGuests = 3;
            }


            for (int i = 0; i < newGuests; i++)
            {
                CreateGuest();
            }
        }

        public void CheckIfHasElectricity(Facility facility, int x, int y)
        {
            for (int i = x - 3; i < x + 4; i++)
            {
                for (int j = y - 3; j < y + 4; j++)
                {
                    if (i >= 0 && i < _table.RowSize && j >= 0 && j < _table.ColumnSize && _table.GetValue(i, j) is Electricity)
                    {
                        facility.HasElectricity = true;
                        if (facility.State == Facility.States.Stopped)
                        {
                            facility.State = Facility.States.Working;

                            Tuple<int, int> coordinates = GetCoordinates(facility);
                            if (coordinates != null)
                            {
                                EntityStatusChanged(this, new EntityStatusChangedEventArgs(coordinates.Item1, coordinates.Item2));
                            }
                        }
                        return;
                    }
                }
            }
        }

        private void CheckIfGameRoundFinished(Game game, int x, int y)
        {
            if (game.IsRoundFinished)
            {
                foreach (Guest guest in GetGuestsAt(x, y))
                {
                    guest.Happiness = guest.Happiness + 50;
                    guest.Target = null;
                }
                game.IsRoundFinished = false;
                game.GuestsInGame = 0;
                game.State = Facility.States.Working;
                EntityStatusChanged(this, new EntityStatusChangedEventArgs(x, y));
            }
        }

        private void CheckIfRestaurantServiceFinished(Restaurant restaurant, int x, int y)
        {
            if (restaurant.IsServiceFinished)
            {
                foreach (Guest guest in GetGuestsAt(x, y))
                {
                    guest.Hungriness = 100;
                    guest.Target = null;
                }
                restaurant.IsServiceFinished = false;
                restaurant.GuestsInRestaurant = 0;
                restaurant.State = Facility.States.Working;
                EntityStatusChanged(this, new EntityStatusChangedEventArgs(x, y));
            }
        }


        public List<Guest> GetGuestsAt(int x, int y)
        {
            (int, int) pos;
            pos.Item1 = x;
            pos.Item2 = y;

            List<Guest> guestAt = new List<Guest>();

            foreach (Guest guest in _listOfGuests)
            {
                if (guest.CurrentPosition == pos)
                {
                    guestAt.Add(guest);
                }
            }

            return guestAt;
        }

        public async Task SaveGameAsync(String s) //task for persistence save
        {
            SaveGuestsTarget();
            await _dataAccess.SaveAsync(s, _table, _listOfGuests, _player, _gameTime);
        }
        public async Task LoadGameAsync(String s) //task for presistence load
        {
            _timer.Stop();
            (GameTable, List<Guest>, Player, DateTime) load = await _dataAccess.LoadAsync(s);
            _table = load.Item1;
            _listOfGuests = load.Item2;
            _player = load.Item3;
            _buildinInProgress.Clear();
            _destination.Clear();
            _roads.Clear();
            ReloadRoads();
            ReloadGuestsTarget();
            ReloadDestination();
            ReloadFacilityStatus();
            _timer.Start();
        }

        #endregion

        #region EventHandlers
        private void Timer_Tick(object sender, EventArgs e) //timer eventhandler
        {
            _gameTime = _gameTime.AddSeconds(1);
            OnGameAdvanced();

            GuestsVisitPark();

            if (_listOfGuests.Count > 0)
            {
                for (int i = 0; i < _listOfGuests.Count; i++)
                {
                    StartLongJourney(_listOfGuests[i]);
                }
            }

            foreach (var item in _destination)
            {
                if (!item.Item1.HasElectricity)
                {
                    CheckIfHasElectricity(item.Item1, item.Item2, item.Item3);
                }
                else
                {
                    _player.Money -= item.Item1.PriceToKeepOpen;
                    if (item.Item1 is Game) //change building status
                    {
                        Game game = (Game)item.Item1;
                        if (game.GuestsInGame == game.Capacity && game.State != Facility.States.InUse)
                        {
                            game.State = Facility.States.InUse;
                            game.StartUse();
                            foreach (Guest guest in GetGuestsAt(item.Item2, item.Item3))
                            {
                                guest.Pay(game.PriceToUse);
                                _player.Money += game.PriceToUse;

                            }
                            _player.Money = _player.Money - game.PriceToKeepOpen;
                            EntityStatusChanged(this, new EntityStatusChangedEventArgs(item.Item2, item.Item3));
                        }

                        CheckIfGameRoundFinished(game, item.Item2, item.Item3);

                        game.AdvanceTime();
                    }
                    else if (item.Item1 is Restaurant) //change building status
                    {
                        Restaurant restaurant = (Restaurant)item.Item1;
                        if (restaurant.GuestsInRestaurant == restaurant.Capacity && restaurant.State != Facility.States.InUse)
                        {
                            restaurant.State = Facility.States.InUse;
                            restaurant.StartUse();
                            foreach (Guest guest in GetGuestsAt(item.Item2, item.Item3))
                            {
                                guest.Money = guest.Money - restaurant.PriceToUse;
                                _player.Money += restaurant.PriceToUse;
                            }
                            _player.Money = _player.Money - restaurant.PriceToKeepOpen;

                            EntityStatusChanged(this, new EntityStatusChangedEventArgs(item.Item2, item.Item3));
                        }

                        CheckIfRestaurantServiceFinished(restaurant, item.Item2, item.Item3);

                        restaurant.AdvanceTime();
                    }
                }
            }
        }
        #endregion
    }
}
