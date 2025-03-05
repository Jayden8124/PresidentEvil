using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace PresidentEvil
{
    public class SKLT_AC : MonsterType
    {
        private int moveDirection = 1;
        public SKLT_AC(Texture2D texture) : base(texture)
        {

        }

         public SKLT_AC(Dictionary<string, Animation> animations) : base(animations)
        {
            
        }
        
         public override void Update(GameTime gameTime, List<GameObject> _gameObjects)
        {
             // ตรวจสอบว่ามอนสเตอร์ชนกับ hitblock หรือไม่
            bool collidedWithHitblock = false;

            if (Singleton.Instance.HitblockTiles != null)
            {
                foreach (var tile in Singleton.Instance.HitblockTiles)
                {
                    if (GameObject.CheckAABBCollision(this.Rectangle, tile))
                    {
                        collidedWithHitblock = true;
                        break;
                    }
                }
            }
            
            if (collidedWithHitblock)
            {
                moveDirection *= -1;
            }

            // ตัวอย่างการเคลื่อนที่:
            if (gameTime.TotalGameTime.TotalSeconds > 3)
            {
                // ถ้าชนกับ Player ให้เล่นแอนิเมชัน Attack
                if (GameObject.CheckAABBCollision(Singleton.Instance.player.Rectangle, this.Rectangle))
                {
                    AnimationManager.Play(Animations["Attack"]);
                }
                else
                {
                    float distanceX = Math.Abs(Position.X - Singleton.Instance.player.Position.X);
                    if (distanceX <= 150)
                    {
                        // เมื่อใกล้ Player ให้วิ่งเข้าหา
                        float runSpeed = 2f;
                        if (Singleton.Instance.player.Position.X < Position.X)
                        {
                            Position = new Vector2(Position.X - runSpeed, Position.Y);
                            AnimationManager.FacingRight = false;
                        }
                        else
                        {
                            Position = new Vector2(Position.X + runSpeed, Position.Y);
                            AnimationManager.FacingRight = true;
                        }
                        AnimationManager.Play(Animations["Idle"]);
                    }
                    else
                    {
                        // ในกรณีที่ไม่ได้ใกล้ Player ให้เดินตามทิศทางของ moveDirection
                        float walkSpeed = 1f;
                        Position = new Vector2(Position.X + walkSpeed * moveDirection, Position.Y);
                        AnimationManager.FacingRight = (moveDirection > 0);
                        AnimationManager.Play(Animations["Walk"]);
                    }
                }
            }
            AnimationManager.Update(gameTime);
            base.Update(gameTime, _gameObjects);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            AnimationManager.Position = Position;
            AnimationManager.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }

        public override void Reset()
        {
            base.Reset();
        }
    }
}