using Microsoft.VisualStudio.TestTools.UnitTesting;
using AmusementPark.Model.Entities;
using AmusementPark.Model;
using AmusementPark.Persistence;
using System.Threading.Tasks;
using AmusementPark;
using System;

namespace AmusementParkUnitTests
{
    [TestClass]
    public class UnitTests
    {
        private IGameDataAccess _dataAccess;
        private GameModel _model;
        private string checkField;
        private GameForm form;

        [TestMethod]
        public void Initialize()
        {
            _model = new GameModel(_dataAccess = new GameDataAccess());
            //form = new GameForm();
            //_model.EntityStatusChanged += new EventHandler<EntityStatusChangedEventArgs>(form.Model_EntityStatusChanged);
        }

        [TestMethod]
        public void TableField()
        {
            _model = new GameModel(_dataAccess);
            Assert.AreEqual(13, _model.Table.RowSize);
            Assert.AreEqual(24, _model.Table.ColumnSize);

            for (int i = 0; i < _model.Table.RowSize; i++)
            {
                for (int j = 0; j < _model.Table.ColumnSize; j++)
                {

                    checkField = ((Field)_model.Table.GetValue(1, 1)).ToString();
                    string[] checkList = checkField.Split('.');
                    Assert.AreEqual((Field.FieldTypes.Field).ToString(), checkList[3]);
                }
            }
        }

        [TestMethod]
        public void PlaceFieldItemGame()
        {
            Game game = new Game();
            //check settings manually
            game.RoundTime = 1;
            game.PriceToPayWhenUsed = 300;
            game.MinimumCapacity = 40;
            game.BuildingPrice = 30;
            game.BuildingTimeInSec = 20;
            game.PriceToKeepOpen = 55;
            game.Capacity = 33;
            game.HasElectricity = true;
            game.BuildingPriceWater = 100;
            Assert.AreEqual(1, game.RoundTime);
            Assert.AreEqual(300, game.PriceToPayWhenUsed);
            Assert.AreEqual(40, game.MinimumCapacity);
            Assert.AreEqual(30, game.BuildingPrice);
            Assert.AreEqual(20, game.BuildingTimeInSec);
            Assert.AreEqual(55, game.PriceToKeepOpen);
            Assert.AreEqual(33, game.Capacity);
            Assert.AreEqual(true, game.HasElectricity);
            Assert.AreEqual(100, game.BuildingPriceWater);

            Initialize();

            checkField = ((Field)_model.Table.GetValue(1, 1)).ToString();
            string[] checkList = checkField.Split('.');
            Assert.AreEqual((Field.FieldTypes.Field).ToString(), checkList[3]);

            _model.PlaceGame(1, 1);
            checkField = ((Game)_model.Table.GetValue(1, 1)).ToString();
            checkList = checkField.Split('.');
            Assert.AreEqual("Game", checkList[3]);

        }
        [TestMethod]
        public void PlaceFieldItemRestaurant()
        {
            Restaurant restaurant = new Restaurant();
            //check settings manually
            restaurant.ServiceTime = 10;
            restaurant.BuildingPrice = 20;
            restaurant.BuildingTimeInSec = 25;
            restaurant.PriceToKeepOpen = 52;
            restaurant.Capacity = 33;
            restaurant.HasElectricity = true;
            restaurant.BuildingPriceWater = 100;
            Assert.AreEqual(10, restaurant.ServiceTime);
            Assert.AreEqual(20, restaurant.BuildingPrice);
            Assert.AreEqual(25, restaurant.BuildingTimeInSec);
            Assert.AreEqual(52, restaurant.PriceToKeepOpen);
            Assert.AreEqual(33, restaurant.Capacity);
            Assert.AreEqual(true, restaurant.HasElectricity);
            Assert.AreEqual(100, restaurant.BuildingPriceWater);

            Initialize();
            checkField = ((Field)_model.Table.GetValue(2, 2)).ToString();
            string[] checkList = checkField.Split('.');
            Assert.AreEqual((Field.FieldTypes.Field).ToString(), checkList[3]);

            _model.PlaceRestaurant(2, 2);
            checkField = ((Restaurant)_model.Table.GetValue(2, 2)).ToString();
            checkList = checkField.Split('.');
            Assert.AreEqual("Buffet", checkList[3]);

        }
        [TestMethod]
        public void PlaceFieldItemFuseBox()
        {
            Electricity electricity = new Electricity();
            //check settings manually
            electricity.Range = 22;
            Assert.AreEqual(22, electricity.Range);

            Initialize();
            checkField = ((Field)_model.Table.GetValue(3, 3)).ToString();
            string[] checkList = checkField.Split('.');
            Assert.AreEqual((Field.FieldTypes.Field).ToString(), checkList[3]);

            _model.PlaceAdditionalEntity(electricity, 3, 3);
            checkField = ((Electricity)_model.Table.GetValue(3, 3)).ToString();
            checkList = checkField.Split('.');
            Assert.AreEqual("Electricity", checkList[3]);

        }
        [TestMethod]
        public void PlaceFieldItemPlant()
        {
            Plant plant = new Plant();
            //check settings manually
            plant.Range = 22;
            plant.HappinessBoost = 100;
            Assert.AreEqual(22, plant.Range);
            Assert.AreEqual(100, plant.HappinessBoost);

            Initialize();
            checkField = ((Field)_model.Table.GetValue(4, 4)).ToString();
            string[] checkList = checkField.Split('.');
            Assert.AreEqual((Field.FieldTypes.Field).ToString(), checkList[3]);

            _model.PlaceAdditionalEntity(plant, 4, 4);
            checkField = ((Plant)_model.Table.GetValue(4, 4)).ToString();
            checkList = checkField.Split('.');
            Assert.AreEqual("Plant", checkList[3]);

        }

        public bool PlaceRoad(GameModel model, int x, int y)
        {
            if (model.Table.GetValue(x, y) is Field)
            {
                Field field = (Field)model.Table.GetValue(x, y);

                Road road = new Road();

                if (field.FieldType == Field.FieldTypes.Field)
                {
                    road.BuildingPrice = 10;
                }
                else
                {
                    road.BuildingPrice = 20;
                }

                road.FieldType = field.FieldType;

                if (model.Player.Money >= road.BuildingPrice)
                {
                    model.Table.SetValue(x, y, road);
                    if (model.IsNextToConnected(x, y, road))
                    {
                        model.SetIfRoadConnectedToFacility(x, y);
                    }

                    model.Player.Money -= road.BuildingPrice;
                    return true;
                }
            }
            return false;
        }

        [TestMethod]
        public void PlaceFieldItemRoad()
        {
            Initialize();
            checkField = ((Field)_model.Table.GetValue(5, 5)).ToString();
            string[] checkList = checkField.Split('.');
            Assert.AreEqual((Field.FieldTypes.Field).ToString(), checkList[3]);


            PlaceRoad(_model, 5, 5);
            checkField = ((Road)_model.Table.GetValue(5, 5)).ToString();
            checkList = checkField.Split('.');
            System.Console.WriteLine(checkList[3]);
            Assert.AreEqual("Road", checkList[3]);
        }
        [TestMethod]
        public void PlaceFieldItemWater()
        {
            Initialize();

            checkField = ((Field.FieldTypes)_model.Table.GetValue(6, 7).FieldType).ToString();
            System.Console.WriteLine(checkField);
            Assert.AreEqual("Field", checkField); //field

            _model.PlaceWater(6, 7);
            checkField = ((Field.FieldTypes)_model.Table.GetValue(6, 7).FieldType).ToString();
            System.Console.WriteLine(checkField);
            Assert.AreEqual("Water", checkField);
        }
        [TestMethod]
        public void PlaceWaterItemGame()
        {
            Initialize();

            _model.PlaceWater(10, 10);
            _model.PlaceGame(10, 10);

            checkField = ((Game)_model.Table.GetValue(10, 10)).ToString();
            Assert.AreEqual("AmusementPark.Model.Entities.WaterGame", checkField);

        } //check table before and after
        [TestMethod]
        public void PlaceWaterItemRoad()
        {
            Initialize();
            _model.PlaceWater(6, 6);
            PlaceRoad(_model, 6, 6);
            checkField = ((Road)_model.Table.GetValue(6, 6)).ToString();
            string[] checkList = checkField.Split('.');
            Assert.AreEqual("Road", checkList[3]);
        } //check table before and after
        [TestMethod]
        public void CheckElectricityTest()
        {
            Initialize();

            Electricity electricity = new Electricity();

            _model.PlaceGame(9, 12);
            Assert.AreEqual(false, ((Game)_model.Table.GetValue(9, 12)).HasElectricity);

            _model.PlaceAdditionalEntity(electricity, 6, 12);

            _model.CheckIfHasElectricity(((Game)_model.Table.GetValue(9, 12)), 9, 12);

            Assert.AreEqual(true, ((Game)_model.Table.GetValue(9, 12)).HasElectricity);
        }
        //check if the item has electricity
        [TestMethod]
        public void IsConnectedToTheMainGate()
        {
            Initialize();

            PlaceRoad(_model, 11, 12);
            PlaceRoad(_model, 10, 12);
            PlaceRoad(_model, 9, 12);
            Road road = (Road)_model.Table.GetValue(11, 12);
            Road road2 = (Road)_model.Table.GetValue(10, 12);
            Road road3 = (Road)_model.Table.GetValue(9, 12);

            Assert.AreEqual(true, road.IsConnectedToGate);
            Assert.AreEqual(true, road2.IsConnectedToGate);
            Assert.AreEqual(true, road3.IsConnectedToGate);

        } //check if the roads are connected or not connected to each otther and to the main gate
        //[TestMethod]
        //public void IsConnectedToRoad()
        //{
        //    Initialize();

        //    PlaceRoad(_model, 11, 12);
        //    PlaceRoad(_model, 10, 12);

        //    _model.PlaceGame(8, 12);
        //    Game game = (Game)_model.Table.GetValue(8, 12);

        //    Assert.AreEqual(false, game.IsNextToConnectedRoad);
        //    PlaceRoad(_model, 9, 12);

        //    Assert.AreEqual(true, game.IsNextToConnectedRoad);

        //}//check which facility is connected to road (which is available to the guests)
        [TestMethod]
        public void CheckPlantTest()
        {
            Initialize();
            //guest
            Guest guest = new Guest();
            guest.CurrentPosition=(12, 12);
            guest.Happiness = 20;

            Assert.AreEqual(guest.Happiness, 20);

            //after plants

            Tree tree = new Tree();
            _model.PlaceAdditionalEntity(tree, 11,12);
            _model.GuestPlantEffect(guest);
            Assert.AreEqual(guest.Happiness, 29);


            Bush bush = new Bush();
            _model.PlaceAdditionalEntity(bush, 10, 12);
            _model.GuestPlantEffect(guest);
            Assert.AreEqual(guest.Happiness, 48);

        } //check plants
    }
}
