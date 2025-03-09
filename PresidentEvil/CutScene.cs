using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace PresidentEvil
{
    public class CutScene
    {
        private Texture2D background;
        private List<string> messages;
        private int currentMessageIndex = 0;
        private string displayedText = "";
        private float textSpeed = 0.05f;
        private float textTimer = 0f;
        private int charIndex = 0;
        private GraphicsDevice _graphicsDevice;
        private SpriteFont _font;
        private Texture2D _bg;
        private Texture2D _frame;
        public enum CutsceneType
        {
            StartGame,
            BossRoom,
            BossDefeated
        }
        private CutsceneType currentCutscene;

        public CutScene(GraphicsDevice graphicsDevice)
        {
            this._graphicsDevice = graphicsDevice;
        }

        public void LoadContent(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            _font = Content.Load<SpriteFont>("game_font");
            _bg = Content.Load<Texture2D>("bg");
            currentCutscene = CutsceneType.StartGame;
            LoadSceneData(_bg);
        }

        private void LoadSceneData(Texture2D bg)
        {
            switch (currentCutscene)
            {
                case CutsceneType.StartGame:
                    background = bg;
                    messages = new List<string>
            {
                "King: My knight, Medusa's curse has spread across our kingdom...",
                "King: You must vanquish her and end this catastrophe!",
                "Knight: Your Majesty, I shall obey your command..."
            };

                    break;

                case CutsceneType.BossRoom:
                    background = bg;
                    messages = new List<string>
            {
                "Medusa: Did he send you to 'free' the kingdom from my curse?",
                "Medusa: This shall be your end!",
                "Knight: Even if it costs me my life, I will stop you!"
            };
                    break;

                case CutsceneType.BossDefeated:
                    background = bg;
                    messages = new List<string>
            {
                "Medusa: I... I only wanted him to feel my pain...",
                "Knight: Who do you mean?",
                "Medusa: Your king! He betrayed me..."
            };
                    break;
            }
        }
        public void Update(GameTime gameTime)
        {
            textTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (textTimer >= textSpeed && charIndex < messages[currentMessageIndex].Length)
            {
                displayedText += messages[currentMessageIndex][charIndex];
                charIndex++;
                textTimer = 0f;
            }
            else if (charIndex >= messages[currentMessageIndex].Length &&
                     Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                if (currentMessageIndex < messages.Count - 1)
                {
                    currentMessageIndex++;
                    displayedText = "";
                    charIndex = 0;
                }
                else
                {
                    Singleton.Instance.CurrentGameState = Singleton.GameState.GamePlaying;
                }
            }

            Singleton.Instance.PreviousKey = Singleton.Instance.CurrentKey;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            // spriteBatch.Draw(background, new Rectangle(0, 0, Singleton.SCREENWIDTH, Singleton.SCREENHEIGHT), Color.White);
            spriteBatch.DrawString(_font, displayedText, new Vector2(Singleton.SCREENWIDTH / 4, Singleton.SCREENHEIGHT / 2), Color.Black);
            spriteBatch.End();
        }
    }
}
