using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace PresidentEvil
{
    public static class CollisionManager
    {
        // เมธอดสำหรับแก้ไขตำแหน่งเมื่อเกิดการชนกับ tile map
        public static void ResolveCollision(GameObject obj, List<Rectangle> collisionTiles)
        {
            Rectangle objRect = obj.Rectangle;
            foreach (var tile in collisionTiles)
            {
                if (objRect.Intersects(tile))
                {
                    // คำนวณพื้นที่ทับซ้อน
                    Rectangle intersection = Rectangle.Intersect(objRect, tile);
                    
                    // แก้ไขการชนในแนวที่มีการทับซ้อนน้อยกว่า
                    if (intersection.Width < intersection.Height)
                    {
                        // แก้ไขในแนวแกน X
                        if (objRect.Center.X < tile.Center.X)
                            obj.Position = new Vector2(obj.Position.X - intersection.Width, obj.Position.Y);
                        else
                            obj.Position = new Vector2(obj.Position.X + intersection.Width, obj.Position.Y);
                    }
                    else
                    {
                        // แก้ไขในแนวแกน Y
                        if (objRect.Center.Y < tile.Center.Y)
                            obj.Position = new Vector2(obj.Position.X, obj.Position.Y - intersection.Height);
                        else
                            obj.Position = new Vector2(obj.Position.X, obj.Position.Y + intersection.Height);
                    }
                    // อัพเดท objRect หลังจากแก้ไขตำแหน่ง
                    objRect = obj.Rectangle;
                }
            }
        }

        // เมธอดสำหรับอัปเดทสถานะ OnGround
        public static void UpdateOnGround(GameObject obj, List<Rectangle> collisionTiles)
        {
            Rectangle objRect = obj.Rectangle;
            // สร้าง rectangle เล็ก ๆ ที่ด้านล่างของวัตถุ (ความสูง 5 พิกเซล)
            Rectangle footRect = new Rectangle(objRect.X, objRect.Bottom, objRect.Width, 5);
            bool onGround = false;
            foreach (var tile in collisionTiles)
            {
                if (footRect.Intersects(tile))
                {
                    onGround = true;
                    break;
                }
            }

            // สำหรับ Player ให้ตรวจสอบเฉพาะเมื่อกำลังตกลงมา (VerticalVelocity ≥ 0)
            if (obj is Player player)
            {
                if (player.VerticalVelocity >= 0)
                    player.OnGround = onGround;
            }
            else if (obj is MonsterType monster)
            {
                // หาก Monster มีการใช้ Velocity.Y ในการเคลื่อนที่
                if (monster.Velocity.Y >= 0)
                    monster.OnGround = onGround;
            }
        }
    }
}
