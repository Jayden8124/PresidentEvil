using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace TheKnightAwakening
{
    public class SKLT_AC : MonsterType
    {
        public Bullet Bullet;

        public SKLT_AC(Texture2D texture) : base(texture)
        {

        }

        public SKLT_AC(Dictionary<string, Animation> animations) : base(animations)
        {

        }

        public override void Update(GameTime gameTime, List<GameObject> _gameObjects)
        {
            // รีเซ็ต flag ของ hitblock ในแต่ละเฟรม
            collidedWithHitblock = false;

            // สร้าง front rectangle เพื่อตรวจสอบว่ากำแพง (hitblock) อยู่ด้านหน้าหรือไม่
            int offset = 5; // ระยะที่ใช้ตรวจสอบด้านหน้า
            Rectangle frontRect;
            if (AnimationManager.FacingRight)
            {
                frontRect = new Rectangle(this.Rectangle.Right, this.Rectangle.Y, offset, this.Rectangle.Height);
            }
            else
            {
                frontRect = new Rectangle(this.Rectangle.X - offset, this.Rectangle.Y, offset, this.Rectangle.Height);
            }

            // ตรวจสอบ collision กับ hitblock เฉพาะส่วนด้านหน้า (เพื่อไม่ให้ตรวจจับพื้น)
            if (Singleton.Instance.HitblockTiles != null)
            {
                foreach (var tile in Singleton.Instance.HitblockTiles)
                {
                    if (frontRect.Intersects(tile))
                    {
                        collidedWithHitblock = true;
                        break;
                    }
                }
            }

            // ถ้าชนกับกำแพงที่ด้านหน้า ให้เปลี่ยนทิศทาง
            if (collidedWithHitblock)
            {
                moveDirection *= -1;
                AnimationManager.FacingRight = moveDirection > 0;
            }

            // ส่วนการเคลื่อนที่และโจมตี
            if (gameTime.TotalGameTime.TotalSeconds > 1)
            {
                attackTimer = 0f;
                if (DistanceMoved <= 200)
                {
                    float attackAnimDuration = Animations["Attack"].FrameSpeed * Animations["Attack"].FrameCount;
                    attackTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (attackTimer <= -0.4f) // adjust animation when collision for attack
                    {
                        Console.WriteLine("Attack");
                        // เมื่อ delay หมดแล้ว ให้เล่นแอนิเมชัน Attack และโจมตี Player แล้วรีเซ็ต timer
                        AnimationManager.Play(Animations["Attack"]);
                        attackTimer = attackDelay;
                        var newBullet = Bullet.Clone() as Bullet;
                        newBullet.Position = new Vector2(Rectangle.Width / 2 + Position.X - newBullet.Rectangle.Width / 2, Position.Y);
                        newBullet.Reset();
                        _gameObjects.Add(newBullet);
                    }
                }
                else
                {
                    attackTimer = 0f;
                    // เมื่อ Player ใกล้ (≤150) และอยู่ในแนวเดียวกัน ให้วิ่งเข้าหา
                    if (Singleton.Instance.player.Position.X < Position.X)
                    {
                        Position = new Vector2(Position.X - runSpeed, Position.Y);
                        moveDirection = -1;
                        AnimationManager.FacingRight = false;
                    }
                    else
                    {
                        Position = new Vector2(Position.X + runSpeed, Position.Y);
                        moveDirection = 1;
                        AnimationManager.FacingRight = true;
                    }
                    AnimationManager.Play(Animations["Walk"]);
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
            Health = 5;
            walkSpeed = 1f;
            runSpeed = 2f;
            moveDirection = -1;
            attackTimer = 0f;
            attackDelay = 3.0f;
            base.Reset();
        }
    }
}