using Microsoft.Xna.Framework.Input;
using System;

namespace PresidentEvil
{
    public class Singleton
    {
        // Size of the screen
        public const int SCREENWIDTH = 1280;
        public const int SCREENHEIGHT = 720;

        // Utility variables
        public int Score;
        public long Timer;
        public Random Random;

        // Game state
        public enum GameState
        {
            Start,
            GamePlaying,
            GameOver,   
        }
        public GameState CurrentGameState;

        public KeyboardState PreviousKey, CurrentKey;
        public MouseState PreviousMouse, CurrentMouse;

        // Singleton instance
        private static Singleton instance;
        private Singleton() { }
        public static Singleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Singleton();
                }
                return instance;
            }
        }
    }
}