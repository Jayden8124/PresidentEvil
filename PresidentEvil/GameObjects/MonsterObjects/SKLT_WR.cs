using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace PresidentEvil
{
    public class SKLT_WR : MonsterType
    {
        public SKLT_WR(Texture2D texture) : base(texture)
        {

        }

        public SKLT_WR(Dictionary<string, Animation> animations) : base(animations)
        {

        }

    //     public override void Update(GameTime gameTime, List<GameObject> _gameObjects)
    //     {
    //         if (gameTime.TotalGameTime.TotalSeconds > 3)
    // {
    //     // ตรวจสอบการชนระหว่าง Monster กับ Player
    //     if (GameObject.CheckAABBCollision(Singleton.Instance.player.Rectangle, this.Rectangle))
    //     {
    //         // เมื่อชนกันให้เล่น Attack อย่างต่อเนื่อง
    //         AnimationManager.Play(Animations["Attack"]);
    //     }
    //     else
    //     {
    //         // คำนวณระยะห่างในแกน X ระหว่าง Monster กับ Player
    //         float distanceX = Math.Abs(Position.X - Singleton.Instance.player.Position.X);
            
    //         // ถ้าอยู่ใกล้ (<= 150) ให้วิ่งเข้าหา (Run)
    //         if (distanceX <= 150)
    //         {
    //             float runSpeed = 2f; // กำหนดความเร็วในการวิ่ง
    //             if (Singleton.Instance.player.Position.X < Position.X)
    //             {
    //                 Position = new Vector2(Position.X - runSpeed, Position.Y);
    //                 AnimationManager.FacingRight = false;
    //             }
    //             else
    //             {
    //                 Position = new Vector2(Position.X + runSpeed, Position.Y);
    //                 AnimationManager.FacingRight = true;
    //             }
    //             AnimationManager.Play(Animations["Run"]);
    //         }
    //         // ถ้าระยะห่าง > 150 ให้เดินเข้าหา (Walk)
    //         else
    //         {
    //             float walkSpeed = 1f; // กำหนดความเร็วในการเดิน
    //             if (Singleton.Instance.player.Position.X < Position.X)
    //             {
    //                 Position = new Vector2(Position.X - walkSpeed, Position.Y);
    //                 AnimationManager.FacingRight = false;
    //             }
    //             else
    //             {
    //                 Position = new Vector2(Position.X + walkSpeed, Position.Y);
    //                 AnimationManager.FacingRight = true;
    //             }
    //             AnimationManager.Play(Animations["Walk"]);
    //         }
    //     }
    // }

    //         AnimationManager.FacingRight = false;

    //         AnimationManager.Update(gameTime);
    //         base.Update(gameTime, _gameObjects);
    //     }

    public override void Update(GameTime gameTime, List<GameObject> _gameObjects)
{
    // เพิ่มการอัพเดทแรงโน้มถ่วงสำหรับ Monster
    
    // ตัวอย่างการเคลื่อนที่ในแนว X เพื่อเข้าใกล้ Player
    if (gameTime.TotalGameTime.TotalSeconds > 3)
    {
        // หากชนกับ Player ให้เล่น Attack อย่างต่อเนื่อง
        if (GameObject.CheckAABBCollision(Singleton.Instance.player.Rectangle, this.Rectangle))
        {
            AnimationManager.Play(Animations["Attack"]);
        }
        else
        {
            float distanceX = Math.Abs(Position.X - Singleton.Instance.player.Position.X);
            if (distanceX <= 150)
            {
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
                AnimationManager.Play(Animations["Run"]);
            }
            else
            {
                float walkSpeed = 1f;
                if (Singleton.Instance.player.Position.X < Position.X)
                {
                    Position = new Vector2(Position.X - walkSpeed, Position.Y);
                    AnimationManager.FacingRight = false;
                }
                else
                {
                    Position = new Vector2(Position.X + walkSpeed, Position.Y);
                    AnimationManager.FacingRight = true;
                }
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
            DistanceMoved = 0;
            base.Reset();
        }
    }
}