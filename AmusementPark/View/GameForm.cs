using AmusementPark.Model;
using AmusementPark.Model.Entities;
using AmusementPark.Persistence;
using AmusementPark.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AmusementPark
{
    public partial class GameForm : Form
    {
        private GameModel _model;
        private Button[,] _buttonGrid;
        private IGameDataAccess _dataAccess;
        private bool _isSelecting;
        private Button _buttonPopUp;
        private ToolTip _buttonToolTip = new ToolTip();
        Form form;
        FacilityDataStruct facilityDataStruct = new FacilityDataStruct(0, 0, 0, 0);
        private int x;
        private int y;

        private struct FacilityDataStruct //struct for saving facility data
        {
            public int Price { get; set; }
            public int Capacity { get; set; }
            public int Time { get; set; }
            public int MinimumCapacity { get; set; } //game only

            public FacilityDataStruct(int price, int cap, int time, int min)
            {
                Price = price;
                Capacity = cap;
                Time = time;
                MinimumCapacity = min;
            }
        };

        #region Constructor

        public GameForm() //gameform constructor
        {
            InitializeComponent();
        }

        #endregion

        #region Private methods

        private void GenerateTable() //generate the player's table
        {
            int height = Screen.PrimaryScreen.Bounds.Height;
            int width = Screen.PrimaryScreen.Bounds.Width;

            //this.BackgroundImage = Resources.grass;

            _buttonGrid = new Button[_model.Table.RowSize, _model.Table.ColumnSize];
            for (int i = 0; i < _model.Table.RowSize; i++)
                for (int j = 0; j < _model.Table.ColumnSize; j++)
                { //set values and add buttons to the table
                    _buttonGrid[i, j] = new Button();
                    _buttonGrid[i, j].FlatStyle = FlatStyle.Flat;
                    _buttonGrid[i, j].BackColor = Color.Transparent;
                    _buttonGrid[i, j].FlatAppearance.BorderSize = 0;
                    _buttonGrid[i, j].Location = new Point(5 + ((height / _model.Table.RowSize) - 5) * j, 25 +
                        ((width / _model.Table.ColumnSize) - 5) * i); // elhelyezkedés
                    _buttonGrid[i, j].Size = new Size((height / _model.Table.RowSize) - 5, (width / _model.Table.ColumnSize) - 5); // méret
                    _buttonGrid[i, j].TabIndex = i * _model.Table.ColumnSize + j; // a gomb számát a TabIndex-ben tároljuk
                    _buttonGrid[i, j].MouseHover += new EventHandler(Buttons_mousehover);
                    _buttonGrid[i, j].Click += new EventHandler(Buttons_click);
                    Controls.Add(_buttonGrid[i, j]);
                }
            InitializeTable();
        }

        private void InitializeTable()
        { //initialize the game table
            for (int i = 0; i < _model.Table.RowSize; i++)
            {
                for (int j = 0; j < _model.Table.ColumnSize; j++)
                {
                    //_buttonGrid[i, j].BackgroundImage = _model.Table.GetValue(i, j).Bitmap;
                    if (_model.Table.GetValue(i, j).FieldType != Field.FieldTypes.Water)
                    {
                        _buttonGrid[i, j].BackgroundImage = _model.Table.GetValue(i, j).Bitmap;
                    }
                    else
                    {
                        _buttonGrid[i, j].BackgroundImage = _model.Table.GetValue(i, j).WaterBitmap;
                    }
                }
            }
        }

        private void RefreshTable()
        { // refresh the game table and view
            moneyStatusLabel.Text = "| Money: " + _model.Player.Money.ToString() + " |";
            timeStatusLabel.Text = "| Time: " + _model.Time.ToString("HH : mm : ss");


            foreach ((int, int) road in _model.Roads)
            {
                _buttonGrid[road.Item1, road.Item2].Image = null;
            }

            List<(int, int)> guests = new List<(int, int)>();

            foreach (Guest guest in _model.Guests)
            {
                guests.Add((guest.CurrentPosition.Item1, guest.CurrentPosition.Item2));

                int numOfGuests = guests.FindAll(x => (x.Item1 == guest.CurrentPosition.Item1 && x.Item2 == guest.CurrentPosition.Item2)).Count;
                if (numOfGuests == 1)
                {
                    _buttonGrid[guest.CurrentPosition.Item1, guest.CurrentPosition.Item2].Image = Resources.guest01;
                }
                else if (numOfGuests == 2)
                {
                    _buttonGrid[guest.CurrentPosition.Item1, guest.CurrentPosition.Item2].Image = Resources.guest02;
                }
                else if (numOfGuests == 3)
                {
                    _buttonGrid[guest.CurrentPosition.Item1, guest.CurrentPosition.Item2].Image = Resources.guest03;
                }
                else
                {
                    _buttonGrid[guest.CurrentPosition.Item1, guest.CurrentPosition.Item2].Image = Resources.guest03;
                }
            }

            guests.Clear();
        }

        private void SetFacilityData() //set the data of facilities
        { //set the properties for the actual entity
            if (_model.SelectedToBuild == SelectedEntity.GiantWheel || _model.SelectedToBuild == SelectedEntity.Ringlispil || _model.SelectedToBuild == SelectedEntity.WaterGame)
            {
                Field.FieldTypes fieldtype = Field.FieldTypes.Field;
                if (_model.SelectedToBuild == SelectedEntity.WaterGame)
                {
                    fieldtype = Field.FieldTypes.Water;
                }

                if (_model.PlaceGame(x, y))
                {
                    Game game = (Game)_model.Table.GetValue(x, y);
                    game.PriceToUse = facilityDataStruct.Price;
                    game.RoundTime = facilityDataStruct.Time;
                    game.MinimumCapacity = facilityDataStruct.MinimumCapacity;
                    game.Capacity = facilityDataStruct.Capacity;
                    game.FieldType = fieldtype;
                }

            }
            else if (_model.SelectedToBuild == SelectedEntity.Restaurant || _model.SelectedToBuild == SelectedEntity.Buffet)
            {
                if (_model.PlaceRestaurant(x, y))
                {
                    Restaurant restaurant = (Restaurant)_model.Table.GetValue(x, y);
                    restaurant.PriceToUse = facilityDataStruct.Price;
                    restaurant.Capacity = facilityDataStruct.Capacity;
                    restaurant.ServiceTime = facilityDataStruct.Time;
                }
            }
        }

        private void ReadSettingsData(Form form)
        { //read data for the given facility
            NumericUpDown priceBox = (NumericUpDown)form.Controls[1];
            NumericUpDown capBox = (NumericUpDown)form.Controls[4];
            NumericUpDown timeBox = (NumericUpDown)form.Controls[7];

            if (_model.SelectedToBuild == SelectedEntity.Restaurant || _model.SelectedToBuild == SelectedEntity.Buffet)
            {
                facilityDataStruct.Price = int.Parse(priceBox.Text);
                facilityDataStruct.Capacity = int.Parse(capBox.Text);
                facilityDataStruct.Time = int.Parse(timeBox.Text);
            }
            else if (_model.SelectedToBuild == SelectedEntity.Ringlispil || _model.SelectedToBuild == SelectedEntity.WaterGame || _model.SelectedToBuild == SelectedEntity.GiantWheel)
            {
                NumericUpDown minBox = (NumericUpDown)form.Controls[9];

                facilityDataStruct.Price = int.Parse(priceBox.Text);
                facilityDataStruct.Capacity = int.Parse(capBox.Text);
                facilityDataStruct.Time = int.Parse(timeBox.Text);
                facilityDataStruct.MinimumCapacity = int.Parse(minBox.Text);
            }
        }
        private void ReadSettingsDataAgain(Form form)
        {
            NumericUpDown priceBox = (NumericUpDown)form.Controls[1];

            if (_model.Table.GetValue(x, y) is Restaurant)
            {
                NumericUpDown capBox = (NumericUpDown)form.Controls[4];
                NumericUpDown timeBox = (NumericUpDown)form.Controls[7];

                ((Restaurant)(_model.Table.GetValue(x, y))).PriceToUse = int.Parse(priceBox.Text);
                ((Restaurant)(_model.Table.GetValue(x, y))).Capacity = int.Parse(capBox.Text);
                ((Restaurant)(_model.Table.GetValue(x, y))).ServiceTime = int.Parse(timeBox.Text);
            }
            else if (_model.Table.GetValue(x, y) is Game)
            {
                NumericUpDown capBox = (NumericUpDown)form.Controls[4];
                NumericUpDown timeBox = (NumericUpDown)form.Controls[7];
                NumericUpDown minBox = (NumericUpDown)form.Controls[9];

                ((Game)(_model.Table.GetValue(x, y))).PriceToUse = int.Parse(priceBox.Text);
                ((Game)(_model.Table.GetValue(x, y))).Capacity = int.Parse(capBox.Text);
                ((Game)(_model.Table.GetValue(x, y))).RoundTime = int.Parse(timeBox.Text);
                ((Game)(_model.Table.GetValue(x, y))).MinimumCapacity = int.Parse(minBox.Text);
            }
            else if (_model.Table.GetValue(x, y) is MainGate)
            {
                ((MainGate)(_model.Table.GetValue(x, y))).EnterPrice = int.Parse(priceBox.Text);
            }
        }
        private void SettingsWindow() //pop up settings window for facilities
        {

            if (_model.SelectedToBuild == SelectedEntity.Ringlispil || _model.SelectedToBuild == SelectedEntity.GiantWheel || _model.SelectedToBuild == SelectedEntity.Buffet || _model.SelectedToBuild == SelectedEntity.Restaurant || _model.SelectedToBuild == SelectedEntity.WaterGame)
            {
                form = new Form();
                form.Text = "Settings";
                form.Size = new Size(350, 400);


                Label price = new Label();
                price.AutoSize = true;
                price.Location = new Point(0, 0);
                price.Text = "Price: ";
                form.Controls.Add(price);

                NumericUpDown priceBox = new NumericUpDown();
                priceBox.Location = new Point(120, 0);
                priceBox.Text = "200";
                priceBox.Minimum = 0;
                form.Controls.Add(priceBox);

                Button okButton = new Button();
                okButton.Location = new Point(120, 220);
                okButton.Size = new Size(70, 50);
                okButton.Text = "OK";
                okButton.Name = "SettingsOK";
                okButton.Click += SettingsOK_Click;
                form.Controls.Add(okButton);

                Label cap = new Label();
                cap.AutoSize = true;
                cap.Location = new Point(0, 80);
                cap.Text = "Capacity: ";
                form.Controls.Add(cap);

                NumericUpDown capBox = new NumericUpDown();
                capBox.Location = new Point(120, 80);
                capBox.Text = "3";
                capBox.Minimum = 0;
                form.Controls.Add(capBox);

                Label timeSec = new Label();
                timeSec.AutoSize = true;
                timeSec.Location = new Point(250, 45);
                timeSec.Text = "sec";
                form.Controls.Add(timeSec);

                if (_model.SelectedToBuild == SelectedEntity.Ringlispil || _model.SelectedToBuild == SelectedEntity.WaterGame || _model.SelectedToBuild == SelectedEntity.GiantWheel)
                {

                    Label time = new Label();
                    time.AutoSize = true;
                    time.Location = new Point(0, 40);
                    time.Text = "Round time: ";
                    form.Controls.Add(time);

                    NumericUpDown timeBox = new NumericUpDown();
                    timeBox.Location = new Point(120, 40);
                    timeBox.Text = "15";
                    timeBox.Minimum = 0;
                    form.Controls.Add(timeBox);

                    Label min = new Label();
                    min.AutoSize = true;
                    min.Location = new Point(0, 120);
                    min.Text = "Minimum capacity: ";
                    form.Controls.Add(min);

                    NumericUpDown minBox = new NumericUpDown();
                    minBox.Location = new Point(170, 120);
                    minBox.Text = "30";
                    minBox.Minimum = 0;
                    minBox.Increment = 10;
                    form.Controls.Add(minBox);

                    Label percentages = new Label();
                    percentages.AutoSize = true;
                    percentages.Location = new Point(220, 125);
                    percentages.Text = "%";
                    form.Controls.Add(percentages);
                }
                else if (_model.SelectedToBuild == SelectedEntity.Restaurant || _model.SelectedToBuild == SelectedEntity.Buffet)
                {

                    Label time = new Label();
                    time.AutoSize = true;
                    time.Location = new Point(0, 40);
                    time.Text = "Service time: ";
                    form.Controls.Add(time);

                    NumericUpDown timeBox = new NumericUpDown();
                    timeBox.Location = new Point(120, 40);
                    timeBox.Text = "15";
                    form.Controls.Add(timeBox);
                }
            }
            form.Show();
        }

        private void SetAgainWindow() //pop up settings window for facilities for setting them again during the game
        {
            form = new Form();
            form.Text = "Settings";
            form.Size = new Size(350, 400);

            Label price = new Label();
            price.AutoSize = true;
            price.Location = new Point(0, 0);
            price.Text = "Price: ";
            form.Controls.Add(price);

            NumericUpDown priceBox = new NumericUpDown();
            priceBox.Location = new Point(120, 0);
            priceBox.Text = "200";
            priceBox.Minimum = 0;
            priceBox.Maximum = 1000;
            form.Controls.Add(priceBox);

            Button okButton = new Button();
            okButton.Location = new Point(120, 220);
            okButton.Size = new Size(70, 50);
            okButton.Text = "OK";
            okButton.Name = "SettingsOK";
            okButton.Click += SettingsOKAgain_Click;
            form.Controls.Add(okButton);

            if (_model.Table.GetValue(x, y) is Game)
            {

                Label cap = new Label();
                cap.AutoSize = true;
                cap.Location = new Point(0, 80);
                cap.Text = "Capacity: ";
                form.Controls.Add(cap);

                NumericUpDown capBox = new NumericUpDown();
                capBox.Location = new Point(120, 80);
                capBox.Text = ((Game)_model.Table.GetValue(x, y)).Capacity.ToString();
                capBox.Minimum = 0;
                form.Controls.Add(capBox);

                Label timeSec = new Label();
                timeSec.AutoSize = true;
                timeSec.Location = new Point(250, 45);
                timeSec.Text = "sec";
                form.Controls.Add(timeSec);

                Label time = new Label();
                time.AutoSize = true;
                time.Location = new Point(0, 40);
                time.Text = "Round time: ";
                form.Controls.Add(time);

                NumericUpDown timeBox = new NumericUpDown();
                timeBox.Location = new Point(120, 40);
                timeBox.Text = "15";
                timeBox.Text = (((Game)_model.Table.GetValue(x, y)).RoundTime).ToString();
                timeBox.Minimum = 0;
                form.Controls.Add(timeBox);

                Label min = new Label();
                min.AutoSize = true;
                min.Location = new Point(0, 120);
                min.Text = "Minimum capacity: ";
                form.Controls.Add(min);

                NumericUpDown minBox = new NumericUpDown();
                minBox.Location = new Point(170, 120);
                minBox.Text = "30";
                minBox.Text = (((Game)_model.Table.GetValue(x, y)).MinimumCapacity).ToString();
                minBox.Minimum = 0;
                minBox.Increment = 10;
                form.Controls.Add(minBox);

                Label percentages = new Label();
                percentages.AutoSize = true;
                percentages.Location = new Point(220, 125);
                percentages.Text = "%";
                form.Controls.Add(percentages);
            }
            else if (_model.Table.GetValue(x, y) is Restaurant)
            {

                Label cap = new Label();
                cap.AutoSize = true;
                cap.Location = new Point(0, 80);
                cap.Text = "Capacity: ";
                form.Controls.Add(cap);

                NumericUpDown capBox = new NumericUpDown();
                capBox.Location = new Point(120, 80);
                capBox.Text = "30";
                capBox.Text = (((Restaurant)_model.Table.GetValue(x, y)).Capacity).ToString();
                capBox.Minimum = 0;
                form.Controls.Add(capBox);

                Label timeSec = new Label();
                timeSec.AutoSize = true;
                timeSec.Location = new Point(250, 45);
                timeSec.Text = "sec";
                form.Controls.Add(timeSec);

                Label time = new Label();
                time.AutoSize = true;
                time.Location = new Point(0, 40);
                time.Text = "Service time: ";
                form.Controls.Add(time);

                NumericUpDown timeBox = new NumericUpDown();
                timeBox.Location = new Point(120, 40);
                timeBox.Text = "30";
                timeBox.Text = (((Restaurant)_model.Table.GetValue(x, y)).ServiceTime).ToString(); ;
                form.Controls.Add(timeBox);
            }
            else if (_model.Table.GetValue(x, y) is Road)
            {
                if (_model.Table.GetValue(x, y) is MainGate)
                {

                }
            }
            form.Show();
        }
        #endregion

        #region EventHandlers
        private void SettingsOK_Click(object sender, EventArgs e) // facility save
        {

            ReadSettingsData(form);
            SetFacilityData();
            form.Close();
            RefreshTable();

            if (_model.Table.GetValue(x, y).FieldType == Field.FieldTypes.Field)
            {
                _buttonGrid[x, y].BackgroundImage = _model.Table.GetValue(x, y).Bitmap;
            }
            else
            {
                _buttonGrid[x, y].BackgroundImage = _model.Table.GetValue(x, y).WaterBitmap;
            }
        }

        private void SettingsOKAgain_Click(object sender, EventArgs e) // facility save change
        {
            ReadSettingsDataAgain(form);
            SetFacilityData();
            form.Close();
            RefreshTable();
        }

        private void Key_Pressed(Object sender, KeyEventArgs e) // eventhandler if a key is pressed, set keycodes for menu items
        {

            if (e.KeyCode == Keys.R && e.Control)
            {
                _isSelecting = true;
                _model.SelectedToBuild = SelectedEntity.Road;
            }
            if (e.KeyCode == Keys.E && e.Control)
            {
                _isSelecting = true;
                _model.SelectedToBuild = SelectedEntity.Restaurant;
            }
            if (e.KeyCode == Keys.A && e.Control)
            {
                _isSelecting = true;
                _model.SelectedToBuild = SelectedEntity.Tree;
            }
            if (e.KeyCode == Keys.W && e.Control)
            {
                _isSelecting = true;
                _model.SelectedToBuild = SelectedEntity.Water;
            }
            if (e.KeyCode == Keys.G && e.Control)
            {
                _isSelecting = true;
                _model.SelectedToBuild = SelectedEntity.GiantWheel;
            }
            if (e.KeyCode == Keys.F && e.Control)
            {
                _isSelecting = true;
                _model.SelectedToBuild = SelectedEntity.FuseBox;
            }
            if (e.KeyCode == Keys.Q && e.Control)
            {
                _isSelecting = true;
                _model.SelectedToBuild = SelectedEntity.WaterGame;
            }
            if (e.KeyCode == Keys.B && e.Control)
            {
                _isSelecting = true;
                _model.SelectedToBuild = SelectedEntity.Bush;
            }
            if (e.KeyCode == Keys.X && e.Control)
            {
                _isSelecting = true;
                _model.SelectedToBuild = SelectedEntity.Grass;
            }
            if (e.KeyCode == Keys.Y && e.Control)
            {
                _isSelecting = true;
                _model.SelectedToBuild = SelectedEntity.Ringlispil;
            }
            if (e.KeyCode == Keys.D && e.Control)
            {
                _isSelecting = true;
                _model.SelectedToBuild = SelectedEntity.Buffet;
            }
            if (e.KeyCode == Keys.S && e.Control) //save game
            {
                SaveGame();
            }
            if (e.KeyCode == Keys.L && e.Control) //load game
            {
                LoadGame();
            }

        }


        private void GameForm_Load(object sender, EventArgs e)
        {

            this.WindowState = FormWindowState.Maximized;

            // modell létrehozása és az eseménykezelők társítása
            _dataAccess = new GameDataAccess();
            _model = new GameModel(_dataAccess);

            // játéktábla és menük inicializálása
            GenerateTable();

            // új játék indítása
            _model.GameStart();
            // model események
            _model.Refresh += new EventHandler<GameEventArgs>(Model_Refresh);
            _model.EntityStatusChanged += new EventHandler<EntityStatusChangedEventArgs>(Model_EntityStatusChanged);
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(this.Key_Pressed);
            this.BackgroundImage = Resources.grass;

            //az összes build menüitm click eventhez hozzárendeljük a build_click esemenyt
            foreach (ToolStripMenuItem item in this.buildMenuItem.DropDownItems)
            {
                item.Click += new EventHandler(Build_clicked);
            }

            this.saveGameToolStripMenuItem.Click += new EventHandler(SaveGameMenuItem_Clicked);
            this.loadGameToolStripMenuItem.Click += new EventHandler(LoadGameMenuItem_Clicked);

            startStopTimeMenuItem.Click += new EventHandler(StartStopTime_Clikced);
            _isSelecting = false;
        }

        private void Model_Refresh(object sender, GameEventArgs e)
        {
            RefreshTable();
        }


        private void MenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void StartStopTime_Clikced(object sender, EventArgs e)
        {
            _model.StartStopTime();

            if (_model.IsPaused)
            {
                startStopTimeMenuItem.Text = "Start Time";
            }
            else
            {
                startStopTimeMenuItem.Text = "Stop Time";
            }
        }

        private void Build_clicked(object sender, EventArgs e)
        {
            //jelezzük, hogy a kiválasztás folyamatban van.
            _isSelecting = true;

            //a sender alapján kiválasztjuk a megfelelő facilityt amit építeni akarunk majd
            if (sender == this.roadMenuItem)
            {
                //a modellben beállítjuk, hogy mit szeretnénk építeni.
                _model.SelectedToBuild = SelectedEntity.Road;
            }
            else if (sender == this.gameMenuItem)
            {
                _model.SelectedToBuild = SelectedEntity.GiantWheel;
            }
            else if (sender == this.gameMenuItem2)
            {
                _model.SelectedToBuild = SelectedEntity.Ringlispil;
            }
            else if (sender == this.restaurantMenuItem)
            {
                _model.SelectedToBuild = SelectedEntity.Restaurant;
            }
            else if (sender == this.restaurantMenuItem2)
            {
                _model.SelectedToBuild = SelectedEntity.Buffet;
            }
            else if (sender == this.treeMenuItem)
            {
                _model.SelectedToBuild = SelectedEntity.Tree;
            }
            else if (sender == this.bushMenuItem)
            {
                _model.SelectedToBuild = SelectedEntity.Bush;
            }
            else if (sender == this.grassMenuItem)
            {
                _model.SelectedToBuild = SelectedEntity.Grass;
            }
            else if (sender == this.fuseBoxMenuItem)
            {
                _model.SelectedToBuild = SelectedEntity.FuseBox;
            }
            else if (sender == this.waterToolStripMenuItem)
            {
                _model.SelectedToBuild = SelectedEntity.Water;
            }
            else if (sender == this.waterGameToolStripMenuItem)
            {
                _model.SelectedToBuild = SelectedEntity.WaterGame;
            }
        }

        private void Buttons_click(object sender, EventArgs e)
        {
            x = ((sender as Button).TabIndex) / _model.Table.ColumnSize;
            y = ((sender as Button).TabIndex) % _model.Table.ColumnSize;

            if (_isSelecting)
            {
                if (!(_model.Table.GetValue(x, y) is Field))
                {
                    _buttonGrid[x, y].BackgroundImage = _model.Table.GetValue(x, y).Bitmap;
                    return;
                }

                if (_model.SelectedToBuild == SelectedEntity.Road)
                {
                    _model.PlaceRoad(x, y);

                    if (_model.Table.GetValue(x, y).FieldType == Field.FieldTypes.Field)
                    {
                        _buttonGrid[x, y].BackgroundImage = _model.Table.GetValue(x, y).Bitmap;
                    }
                    else
                    {
                        _buttonGrid[x, y].BackgroundImage = _model.Table.GetValue(x, y).WaterBitmap;
                    }
                }
                else if (_model.SelectedToBuild == SelectedEntity.WaterGame)
                {
                    if (_model.Table.GetValue(x, y).FieldType != Field.FieldTypes.Water)
                    {
                        return;
                    }

                    SettingsWindow();
                }
                else if (_model.SelectedToBuild == SelectedEntity.Ringlispil)
                {
                    if (_model.Table.GetValue(x, y).FieldType != Field.FieldTypes.Field)
                    {
                        return;
                    }
                    SettingsWindow();
                }
                else if (_model.SelectedToBuild == SelectedEntity.GiantWheel)
                {
                    if (_model.Table.GetValue(x, y).FieldType != Field.FieldTypes.Field)
                    {
                        return;
                    }
                    SettingsWindow();
                }
                else if (_model.SelectedToBuild == SelectedEntity.Restaurant)
                {
                    if (_model.Table.GetValue(x, y).FieldType != Field.FieldTypes.Field)
                    {
                        return;
                    }
                    SettingsWindow();
                }
                else if (_model.SelectedToBuild == SelectedEntity.Buffet)
                {
                    if (_model.Table.GetValue(x, y).FieldType != Field.FieldTypes.Field)
                    {
                        return;
                    }
                    SettingsWindow();
                }
                else if (_model.SelectedToBuild == SelectedEntity.Bush)
                {
                    if (_model.Table.GetValue(x, y).FieldType != Field.FieldTypes.Field)
                    {
                        return;
                    }

                    Bush bush = new Bush();
                    _model.PlaceAdditionalEntity(bush, x, y);

                    _buttonGrid[x, y].BackgroundImage = _model.Table.GetValue(x, y).Bitmap;
                }
                else if (_model.SelectedToBuild == SelectedEntity.Tree)
                {
                    if (_model.Table.GetValue(x, y).FieldType != Field.FieldTypes.Field)
                    {
                        return;
                    }
                    Tree tree = new Tree();
                    _model.PlaceAdditionalEntity(tree, x, y);

                    _buttonGrid[x, y].BackgroundImage = _model.Table.GetValue(x, y).Bitmap;

                }
                else if (_model.SelectedToBuild == SelectedEntity.Grass)
                {
                    if (_model.Table.GetValue(x, y).FieldType != Field.FieldTypes.Field)
                    {
                        return;
                    }
                    Grass grass = new Grass();
                    _model.PlaceAdditionalEntity(grass, x, y);

                    _buttonGrid[x, y].BackgroundImage = _model.Table.GetValue(x, y).Bitmap;


                }
                else if (_model.SelectedToBuild == SelectedEntity.FuseBox)
                {
                    if (_model.Table.GetValue(x, y).FieldType != Field.FieldTypes.Field)
                    {
                        return;
                    }

                    Electricity electricity = new Electricity();
                    _model.PlaceAdditionalEntity(electricity, x, y);

                    _buttonGrid[x, y].BackgroundImage = _model.Table.GetValue(x, y).Bitmap;
                }
                else if (_model.SelectedToBuild == SelectedEntity.Water)
                {
                    if (_model.PlaceWater(x, y))
                    {
                        _buttonGrid[x, y].BackgroundImage = _model.Table.GetValue(x, y).WaterBitmap;
                    }
                }
            }
            else
            {
                if (_model.Table.GetValue(x, y) is Game || _model.Table.GetValue(x, y) is Restaurant || _model.Table.GetValue(x, y) is MainGate)
                {
                    SetAgainWindow();
                }

            }

            if (_buttonPopUp != null)
            {
                _buttonPopUp.Image = null;
            }

            _isSelecting = false;

        }


        //kijelöléskor majd amelyik gomb felett tartjuk az egeret az adott épület "blueprintje" meg fog jelenni.
        private void Buttons_mousehover(object sender, EventArgs e)
        {
            //törli az előzőről a képet.
            //később majd beállítja az adott mező facilitijének a képére.
            RefreshTable();

            if (_buttonPopUp != null)
            {
                _buttonPopUp.Image = null;
            }

            _buttonPopUp = (sender as Button);

            int x = _buttonPopUp.TabIndex / _model.Table.ColumnSize;
            int y = _buttonPopUp.TabIndex % _model.Table.ColumnSize;

            Button button = (Button)sender;

            Console.WriteLine("______");
            Console.WriteLine((_model.Table.GetValue(x, y)).GetType());
            Console.WriteLine((_model.Table.GetValue(x, y)).FieldType);
            Console.WriteLine("______");

            if (_isSelecting)
            {
                if (_model.Table.GetValue(x, y) is Field)
                {
                    if (_model.Table.GetValue(x, y).FieldType == Field.FieldTypes.Field)
                    {
                        if (_model.SelectedToBuild == SelectedEntity.Road)
                        {
                            button.Image = Resources.road_blueprint;
                        }
                        else if (_model.SelectedToBuild == SelectedEntity.Ringlispil)
                        {
                            button.Image = Resources.ringlispil;
                        }
                        else if (_model.SelectedToBuild == SelectedEntity.GiantWheel)
                        {
                            button.Image = Resources.giantwheel;
                        }
                        else if (_model.SelectedToBuild == SelectedEntity.Buffet)
                        {
                            button.Image = Resources.buffet;
                        }
                        else if (_model.SelectedToBuild == SelectedEntity.Restaurant)
                        {
                            button.Image = Resources.restaurant;
                        }
                        else if (_model.SelectedToBuild == SelectedEntity.FuseBox)
                        {
                            button.Image = Resources.fusebox;
                        }
                        else if (_model.SelectedToBuild == SelectedEntity.Tree)
                        {
                            button.Image = Resources.tree;
                        }
                        else if (_model.SelectedToBuild == SelectedEntity.Bush)
                        {
                            button.Image = Resources.bush;
                        }
                        else if (_model.SelectedToBuild == SelectedEntity.Grass)
                        {
                            button.Image = Resources.grass;
                        }
                        else if (_model.SelectedToBuild == SelectedEntity.Water)
                        {
                            button.Image = Resources.water;
                        }
                        else if (_model.SelectedToBuild == SelectedEntity.WaterGame)
                        {
                            button.Image = Resources.disabled;
                        }
                    }
                    else
                    {
                        if (_model.SelectedToBuild == SelectedEntity.Road)
                        {
                            button.Image = Resources.road_water_straith;
                        }
                        else if (_model.SelectedToBuild == SelectedEntity.WaterGame)
                        {
                            button.Image = Resources.water_game;
                        }
                        else
                        {
                            button.Image = Resources.disabled;
                        }
                    }
                }
                else
                {
                    button.Image = Resources.disabled;
                }
            }
            else
            {
                _buttonToolTip.UseFading = true;
                _buttonToolTip.UseAnimation = true;
                _buttonToolTip.IsBalloon = true;
                _buttonToolTip.ShowAlways = true;
                _buttonToolTip.AutoPopDelay = 5000;
                _buttonToolTip.InitialDelay = 1000;
                _buttonToolTip.ReshowDelay = 500;

                string[] checkList = (_model.Table.GetValue(x, y).GetType().ToString()).Split('.');
                _buttonToolTip.ToolTipTitle = checkList[3];

                StringBuilder guestsDetails = new StringBuilder();
                string guestDetail;

                foreach (Guest guest in _model.GetGuestsAt(x, y))
                {
                    guestsDetails.AppendLine();
                    guestDetail = "Money :" + guest.Money + "| Happiness: " + guest.Happiness + "| Hungriness: " + guest.Hungriness;

                    guestsDetails.Append(guestDetail);
                    guestsDetails.AppendLine();
                }


                if (_model.Table.GetValue(x, y) is Road)
                {
                    if (_model.Table.GetValue(x, y) is MainGate) //mennyi a jegy, kattintani lehessen
                    {
                        _buttonToolTip.SetToolTip(_buttonGrid[x, y], "Entrance fee: " + ((MainGate)(_model.Table.GetValue(x, y))).EnterPrice + "\nClick to change entrance fee");
                    }
                    else
                    {
                        _buttonToolTip.SetToolTip(_buttonGrid[x, y], "People: " + _model.GetGuestsAt(x, y).Count + guestsDetails);
                    }
                }
                else if (_model.Table.GetValue(x, y) is Restaurant) //allapot, hany ember
                {
                    _buttonToolTip.SetToolTip(_buttonGrid[x, y], "People: " + _model.GetGuestsAt(x, y).Count + guestsDetails +
                        "\nService time: " +((Restaurant)(_model.Table.GetValue(x,y))).ServiceTime+ "\nPrice: "+ 
                        ((Restaurant)(_model.Table.GetValue(x, y))).PriceToUse + "\nCapacity: " + 
                        ((Restaurant)(_model.Table.GetValue(x, y))).Capacity+ "\nClick to change settings");
                }
                else if (_model.Table.GetValue(x, y) is Game) //allapot, hany ember
                {
                    _buttonToolTip.SetToolTip(_buttonGrid[x, y], "People: " + _model.GetGuestsAt(x, y).Count + guestsDetails +
                        "\nRound time: "+ ((Game)(_model.Table.GetValue(x, y))).RoundTime + "\nPrice: "+
                        ((Game)(_model.Table.GetValue(x, y))).PriceToUse + "\nCapacity: " + ((Game)(_model.Table.GetValue(x, y))).Capacity
                        + "\nMinimum capacity: "+ ((Game)(_model.Table.GetValue(x, y))).MinimumCapacity +
                        "\nClick to change settings");
                }
                RefreshTable();
            }
        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            _model.StartStopTime();
            if (MessageBox.Show("Are you sure you want to exit the game?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.None) == DialogResult.Yes) { this.Close(); }
            _model.StartStopTime();
        }

        public void Model_EntityStatusChanged(object sender, EntityStatusChangedEventArgs e)
        {
            if (_model.Table.GetValue(e.X, e.Y).FieldType == Field.FieldTypes.Field)
            {
                _buttonGrid[e.X, e.Y].BackgroundImage = _model.Table.GetValue(e.X, e.Y).Bitmap;
            }
            else
            {
                _buttonGrid[e.X, e.Y].BackgroundImage = _model.Table.GetValue(e.X, e.Y).WaterBitmap;
            }
        }

        #endregion

        private void OpenParkToolStripMenuItem1_Click(object sender, EventArgs e) //eventhandler for open park menu item
        {
            _model.OpenPark();
            openParkToolStripMenuItem1.Text = "OPENED"; //change the text
            openParkToolStripMenuItem1.Enabled = false;
        }

        private async void SaveGame() 
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.InitialDirectory = "c:\\";
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.Filter = "Amusement game files (*.amg)|*.amg|All files (*.*)|*.*";
            saveFileDialog.Title = "Save the state of your amusement park.";

            _model.StartStopTime();

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string path = saveFileDialog.FileName;
                if (path != "")
                {
                    await _model.SaveGameAsync(path);
                }
            }

            _model.StartStopTime();
        }
        private void SaveGameMenuItem_Clicked(object sender, EventArgs e) //eventhandler for save game menu item
        {
            SaveGame();
        }
        private async void LoadGame()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Filter = "Amusement game files (*.amg)|*.amg|All files (*.*)|*.*";
            openFileDialog.Title = "Load your previous saves.";

            _model.StartStopTime();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog.FileName;
                if (path != "")
                {
                    await _model.LoadGameAsync(path);
                }
            }

            InitializeTable();


        }
        private void LoadGameMenuItem_Clicked(object sender, EventArgs e) //eventhandler for load game menu item
        {
            LoadGame();
        }
    }
}
