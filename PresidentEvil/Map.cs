using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PresidentEvil
{
    public class Map
    {
        private Texture2D textureMap;
        private Texture2D hitboxTexture;
        private Dictionary<Vector2, int> fg;
        private Dictionary<Vector2, int> collisions;
        public Rectangle hitbox;
        private GraphicsDevice _graphicsDevice;

        private int TILESIZE = 24; // size of the tile in the game
        private int num_tile_per_row = 32; // number of tiles per row in the tileset
        private int pixel_tilesize = 16;  // size of the tile in the tileset

        public Map(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
        }

        public void LoadContent(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            textureMap = Content.Load<Texture2D>("TilesetGround");
            hitboxTexture = Content.Load<Texture2D>("hitbox");
            fg = LoadMap("../../../TileMap/map_fg.csv");
            collisions = LoadMap("../../../TileMap/map_hitblock.csv");
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            foreach (var item in fg)
            {
                Rectangle drest = new(
                    (int)item.Key.X * TILESIZE,
                    (int)item.Key.Y * TILESIZE,
                    TILESIZE,
                    TILESIZE
                );

                int x = item.Value % num_tile_per_row;
                int y = item.Value / num_tile_per_row;

                Rectangle src = new(
                    x * pixel_tilesize,
                    y * pixel_tilesize,
                    pixel_tilesize,
                    pixel_tilesize
                );
                spriteBatch.Draw(textureMap, drest, src, Color.White);
            }
            foreach (var item in collisions)
            {
                Rectangle drest = new(
                    (int)item.Key.X * TILESIZE,
                    (int)item.Key.Y * TILESIZE,
                    TILESIZE,
                    TILESIZE
                );
                Rectangle hitbox = new Rectangle(
                    (int)item.Key.X * TILESIZE,
                    (int)item.Key.Y * TILESIZE,
                    TILESIZE,
                    TILESIZE
                );

                int x = item.Value % num_tile_per_row;
                int y = item.Value / num_tile_per_row;

                Rectangle src = new(
                    x * pixel_tilesize,
                    y * pixel_tilesize,
                    pixel_tilesize,
                    pixel_tilesize
                );
                this.hitbox = hitbox;
                // spriteBatch.Draw(hitboxTexture, drest, src, Color.White);
            }
        }

        private Dictionary<Vector2, int> LoadMap(string filepath)
        {
            Dictionary<Vector2, int> result = new();
            StreamReader reader = new(filepath);

            int y = 0;
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] items = line.Split(',');

                for (int x = 0; x < items.Length; x++)
                {
                    if (int.TryParse(items[x], out int value))
                    {
                        if (value > -1)
                        {
                            result[new Vector2(x, y)] = value;
                        }
                    }
                }
                y++;
            }
            return result;
        }
    }
}