using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TheKnightAwakening
{
    public static class AnimationCoin
    {
        public static Dictionary<string, Animation> LoadAnimations(Texture2D player)
        {
            return new Dictionary<string, Animation>
            {
                { "Defend", new Animation(player, new List<Rectangle>
                    {
                        new Rectangle(7,   660, 43, 60),
                        new Rectangle(83,  659, 43, 61),
                        new Rectangle(159, 659, 43, 61),
                        new Rectangle(235, 659, 43, 61),
                        new Rectangle(311, 660, 43, 60)
                    }, 0.10f, false)
                }
            };
        }
    }
}