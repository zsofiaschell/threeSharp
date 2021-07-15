using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using AmusementPark.Properties;

namespace AmusementPark.Model.Entities
{
    [Serializable]
    public class Road : Entity
    {
        #region Properties
        public bool IsConnectedToGate { get; set; } //check property

        #endregion

        #region Constructor

        public Road() // constructor of the road class
        {
            IsConnectedToGate = false;
            Bitmap = new Bitmap(Resources.road_straith, ScreenHeight, ScreenWidth); // set the pricture of the road on field
            WaterBitmap = new Bitmap(Resources.road_water_straith, ScreenHeight, ScreenWidth); // set the pricture of the road on water
        }

        #endregion

        #region SetView

        private Bitmap RotateImage(float rotationAngle, Bitmap img) // rotate the roads in the right direction
        {
            Bitmap bmp = new Bitmap(img.Width, img.Height);

            Graphics g = Graphics.FromImage(bmp);
            g.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);
            g.RotateTransform(rotationAngle);
            g.TranslateTransform(-(float)bmp.Width / 2, -(float)bmp.Height / 2);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(img, new Point(0, 0));
            g.Dispose();

            return bmp;
        }

        public void SetImage(bool up, bool left, bool down, bool right) // set the road picture while rotating
        {
            if (up && left && down && right) //4 node
            {
                Bitmap = new Bitmap(Resources.road_4cross, ScreenHeight, ScreenWidth);
                WaterBitmap = new Bitmap(Resources.road_water_4cross, ScreenHeight, ScreenWidth);
            }
            else if (up && left && down) // 3 node 1
            {
                Bitmap = new Bitmap(RotateImage((float)180, Resources.road_3cross), ScreenHeight, ScreenWidth);
                WaterBitmap = new Bitmap(RotateImage((float)180, Resources.road_water_3cross), ScreenHeight, ScreenWidth);
            }
            else if (up && left && right) // 3 node 2
            {
                Bitmap = new Bitmap(RotateImage((float)270, Resources.road_3cross), ScreenHeight, ScreenWidth);
                WaterBitmap = new Bitmap(RotateImage((float)270, Resources.road_water_3cross), ScreenHeight, ScreenWidth);
            }
            else if (up && down && right) // 3 node 3
            {
                Bitmap = new Bitmap(Resources.road_3cross, ScreenHeight, ScreenWidth);
                WaterBitmap = new Bitmap(Resources.road_water_3cross, ScreenHeight, ScreenWidth);
            }
            else if (down && left && right) // 3 node 4
            {
                Bitmap = new Bitmap(RotateImage((float)90, Resources.road_3cross), ScreenHeight, ScreenWidth);
                WaterBitmap = new Bitmap(RotateImage((float)90, Resources.road_water_3cross), ScreenHeight, ScreenWidth);
            }
            else if (up && right) //corner 1 //ez jó
            {
                Bitmap = new Bitmap(Resources.road_corner, ScreenHeight, ScreenWidth);
                WaterBitmap = new Bitmap(Resources.road_water_corner, ScreenHeight, ScreenWidth);
            }
            else if (up && left) //corner 2
            {
                Bitmap = new Bitmap(RotateImage((float)270, Resources.road_corner), ScreenHeight, ScreenWidth);
                WaterBitmap = new Bitmap(RotateImage((float)270, Resources.road_water_corner), ScreenHeight, ScreenWidth);
            }
            else if (down && right) //corner 3
            {
                Bitmap = new Bitmap(RotateImage((float)90, Resources.road_corner), ScreenHeight, ScreenWidth);
                WaterBitmap = new Bitmap(RotateImage((float)90, Resources.road_water_corner), ScreenHeight, ScreenWidth);
            }
            else if (down && left) //corner 4
            {
                Bitmap = new Bitmap(RotateImage((float)180, Resources.road_corner), ScreenHeight, ScreenWidth);
                WaterBitmap = new Bitmap(RotateImage((float)180, Resources.road_water_corner), ScreenHeight, ScreenWidth);
            }
            else if (up && down) //vertical
            {
                Bitmap = new Bitmap(Resources.road_straith, ScreenHeight, ScreenWidth);
                WaterBitmap = new Bitmap(Resources.road_water_straith, ScreenHeight, ScreenWidth);

            }
            else if (right && left) //horizontal
            {
                Bitmap = new Bitmap(RotateImage((float)90, Resources.road_straith), ScreenHeight, ScreenWidth);
                WaterBitmap = new Bitmap(RotateImage((float)90, Resources.road_water_straith), ScreenHeight, ScreenWidth);
            }
            else if (right || left)
            {
                Bitmap = new Bitmap(RotateImage((float)90, Resources.road_straith), ScreenHeight, ScreenWidth);
                WaterBitmap = new Bitmap(RotateImage((float)90, Resources.road_water_straith), ScreenHeight, ScreenWidth);
            }
            else
            {
                Bitmap = new Bitmap(Resources.road_straith, ScreenHeight, ScreenWidth);
                WaterBitmap = new Bitmap(Resources.road_water_straith, ScreenHeight, ScreenWidth);
            }
        }

        #endregion
    }
}
