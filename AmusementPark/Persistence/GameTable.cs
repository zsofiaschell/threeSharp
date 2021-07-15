using AmusementPark.Model.Entities;
using System;
using static AmusementPark.Model.Entities.Field;

namespace AmusementPark.Persistence
{
    [Serializable]
    public class GameTable
    {
        private Entity[,] _fieldValues;
        public int RowSize { get; set; }
        public int ColumnSize { get; set; }
        public int gameTime;

        private int mainX;
        private int mainY;
        public GameTable(int rows, int columns) //constructor of the GameTable class
        {
            _fieldValues = new Entity[rows, columns];
            RowSize = rows;
            ColumnSize = columns;
            Initialize(rows, columns);
        }

        public Entity GetValue(int x, int y) //return the value of the field
        {
            if (x < 0 || x >= _fieldValues.GetLength(0))
                throw new ArgumentOutOfRangeException("x", "The X coordinate is out of range.");
            if (y < 0 || y >= _fieldValues.GetLength(1))
                throw new ArgumentOutOfRangeException("y", "The Y coordinate is out of range.");

            return _fieldValues[x, y];
        }

        public void SetValue(int x, int y, Entity entity) //sets the value of the field
        {
            if (x < 0 || x >= _fieldValues.GetLength(0))
                throw new ArgumentOutOfRangeException("x", "The X coordinate is out of range.");
            if (y < 0 || y >= _fieldValues.GetLength(1))
                throw new ArgumentOutOfRangeException("y", "The Y coordinate is out of range.");

            _fieldValues[x, y] = entity;
        }

        public void Initialize(int rows, int columns)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Field field = new Field(FieldTypes.Field);
                    _fieldValues[i, j] = field;
                }
            }
            _fieldValues[rows - 1, columns / 2] = new MainGate();
            mainX = rows - 1;
            mainY = columns / 2;

        }

        public MainGate GetMainGate()
        { //returns the Main Gate
            return (MainGate)_fieldValues[mainX, mainY];
        }


    }
}
