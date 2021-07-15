using System;

namespace AmusementPark.Model
{
    public class GameEventArgs : EventArgs
    {
        private int _gameTime;
        public int GameTime { get { return _gameTime; } }

        public GameEventArgs() { }
        public GameEventArgs(int gameTime)
        {
            _gameTime = gameTime;
        }
    }
}
