using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace PresidentEvil
{
    public static class AnimationHealth
    {
        public static Dictionary<string, Animation> LoadAnimations(Texture2D texture)
        {
            return new Dictionary<string, Animation>
            {
                { "HP100", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(0, 2, 86, 24)
                    }, 0.0f, false)
                },

                { "HP75", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(96, 1, 86, 24)
                    }, 0.0f, false)
                },

                { "HP50", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(192, 1, 86, 24)
                    }, 0.0f, false)
                },

                { "HP25", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(288, 1, 86, 24)
                    }, 0.0f, false)
                },

                { "HP0", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(400, 0, 86, 24)
                    }, 0.0f, false)
                },
            };
        }
    }

     public static class AnimationUltimateCharge
    {
        public static Dictionary<string, Animation> LoadAnimations(Texture2D texture)
        {
            return new Dictionary<string, Animation>
            {
                { "ULT1", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(1, 1, 41, 43)
                    }, 0.0f, false)
                },

                { "ULT2", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(58, 2, 41, 43)
                    }, 0.0f, false)
                },

                { "ULT3", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(1, 56, 41, 43)
                    }, 0.0f, false)
                },

                { "ULT4", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(60, 56, 41, 43)
                    }, 0.0f, false)
                },
            };
        }
    }
}