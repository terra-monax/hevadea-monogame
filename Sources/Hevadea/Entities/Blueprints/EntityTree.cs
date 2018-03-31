﻿using Hevadea.Entities.Components;
using Hevadea.Framework.Graphic.SpriteAtlas;
using Hevadea.Items;
using Hevadea.Registry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hevadea.Entities.Blueprints
{
    public class EntityTree : Entity
    {
        private readonly Sprite treeSprite;

        public EntityTree()
        {
            treeSprite = new Sprite(Ressources.TileEntities, new Point(6, 4), new Point(2, 6) );

            AddComponent(new Dropable { Items = { new Drop(ITEMS.WOOD_LOG, 1f, 1, 5), new Drop(ITEMS.PINE_CONE, 1f, 0, 3) } });
            AddComponent(new Health(5));
            AddComponent(new Colider(new Rectangle(-2, -2, 4, 4)));
            AddComponent(new Burnable(0.5f));
        }

        public override void OnDraw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            var offx = -16;
            var offy = -24;
            treeSprite.Draw(spriteBatch, new Vector2(X + offx, Y + offy - 64), Color.White);
        }
    }
}