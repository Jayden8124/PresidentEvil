using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TheKnightAwakening
{
    public static class AnimationPlayer
    {
        public static Dictionary<string, Animation> LoadAnimations(Texture2D player)
        {
            return new Dictionary<string, Animation>
            {
                { "Idle", new Animation(player, new List<Rectangle>
                    {
                        new Rectangle(5,   0, 43, 64),
                        new Rectangle(72,  0, 43, 64),
                        new Rectangle(139, 0, 43, 64),
                        new Rectangle(206, 0, 43, 64)
                    }, 0.15f, true)
                },

                { "Walk", new Animation(player, new List<Rectangle>
                    {
                        new Rectangle(8,   80, 40, 64),
                        new Rectangle(74,  81, 43, 63),
                        new Rectangle(143, 81, 48, 63),
                        new Rectangle(217, 80, 47, 64),
                        new Rectangle(290, 80, 46, 64),
                        new Rectangle(342, 81, 43, 63),
                        new Rectangle(411, 80, 43, 64),
                        new Rectangle(480, 80, 44, 64)
                    }, 0.10f, true)
                },

                { "Run", new Animation(player, new List<Rectangle>
                    {
                        new Rectangle(2,   163, 48, 61),
                        new Rectangle(69,  165, 54, 59),
                        new Rectangle(142, 162, 49, 62),
                        new Rectangle(210, 161, 51, 63),
                        new Rectangle(280, 164, 48, 60),
                        new Rectangle(347, 165, 54, 59),
                        new Rectangle(420, 162, 48, 62)
                    }, 0.10f, true)
                },

                { "Jump", new Animation(player, new List<Rectangle>
                    {
                        new Rectangle(2,   247, 49, 57),
                        new Rectangle(81,  240, 49, 64),
                        new Rectangle(153, 240, 46, 64),
                        new Rectangle(229, 239, 69, 65),
                        new Rectangle(305, 246, 64, 58),
                        new Rectangle(385, 247, 61, 57)
                    }, 0.12f, false)
                },

                { "Attack1", new Animation(player, new List<Rectangle>
                    {
                        new Rectangle(4,   320, 43, 64),
                        new Rectangle(70,  320, 59, 64),
                        new Rectangle(152, 320, 64, 64),
                        new Rectangle(251, 320, 33, 64),
                        new Rectangle(319, 310, 85, 64)
                    }, 0.10f, false)
                },

                { "Attack2", new Animation(player, new List<Rectangle>
                    {
                        new Rectangle(6,   405, 42, 75),
                        new Rectangle(101, 409, 43, 71),
                        new Rectangle(198, 412, 88, 68),
                        new Rectangle(299, 416, 66, 64)
                    }, 0.10f, false)
                },

                { "Attack3", new Animation(player, new List<Rectangle>
                    {
                        new Rectangle(9,   496, 48, 64),
                        new Rectangle(113, 496, 49, 64),
                        new Rectangle(217, 498, 85, 62),
                        new Rectangle(315, 501, 80, 59)
                    }, 0.10f, false)
                },


                { "Dead", new Animation(player, new List<Rectangle>
                    {
                        new Rectangle(5,   583, 43, 58),
                        new Rectangle(78,  596, 47, 45),
                        new Rectangle(160, 607, 45, 34),
                        new Rectangle(239, 608, 50, 33),
                        new Rectangle(316, 608, 54, 33),                            
                        new Rectangle(392, 608, 54, 33)
                    }, 0.10f, false)
                },


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