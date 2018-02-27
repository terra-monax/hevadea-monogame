﻿using System.Collections.Generic;
using System.Linq;
using Hevadea.Game.Entities.Creatures;
using Hevadea.Game.Registry;
using Hevadea.Game.Storage;
using Maker.Rise;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hevadea.Game.Worlds
{
    public class World
    {
        public GameManager Game;
        public List<Level> Levels = new List<Level>();
        public DayNightCycle DayNightCycle { get; }

        private readonly BlendState _lightBlend = new BlendState
        {
            ColorBlendFunction = BlendFunction.Add,
            ColorSourceBlend = Blend.DestinationColor,
            ColorDestinationBlend = Blend.Zero
        };

        public string PlayerSpawnLevel = "overworld";

        private readonly SpriteBatch spriteBatch;

        public World()
        {
            spriteBatch = Engine.Graphic.CreateSpriteBatch();
            DayNightCycle = new DayNightCycle(
                new DayStage("Dawn", 30, new Color(217, 151, 179)),
                new DayStage("Day", 5 * 60, Color.White),
                new DayStage("Dusk", 30, new Color(217, 151, 179)),
                new DayStage("Night", 5 * 30, Color.Blue * 0.1f)
                )
            { Transition = 30 };
        }

        public void SpawnPlayer(PlayerEntity player)
        {
            var level = GetLevel(PlayerSpawnLevel);
            level.SpawnEntity(player, level.Width / 2, level.Height / 2);
        }

        public Level GetLevel(string name)
        {
            return Levels.FirstOrDefault(l => l.Name == name);
        }

        public Level GetLevel(int id)
        {
            return Levels.FirstOrDefault(l => l.Id == id);
        }

        public void AddLevel(Level level)
        {
            if (GetLevel(level.Id) == null) Levels.Add(level);
        }


        public void Draw(GameTime gameTime, Camera camera)
        {
            var level = camera.FocusEntity.Level;
            var state = level.GetRenderState(camera);
            var cameraTransform = camera.GetTransform();
            
            Engine.Graphic.SetRenderTarget(Engine.Graphic.RenderTarget[1]);

            
            
            Engine.Graphic.Begin(spriteBatch, false, cameraTransform);
            level.DrawTerrain(state, spriteBatch, gameTime);
            level.DrawEntities(state, spriteBatch, gameTime);
            level.DrawEntitiesOverlay(state, spriteBatch, gameTime);
            spriteBatch.End();

            Engine.Graphic.SetRenderTarget(Engine.Graphic.RenderTarget[0]);

            if (level.Properties.AffectedByDayNightCycle)
            {
                Engine.Graphic.GetGraphicsDevice().Clear(DayNightCycle.GetAmbiantLight());
            }
            else
            {
                Engine.Graphic.GetGraphicsDevice().Clear(level.Properties.AmbiantLight);
            }

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.LinearClamp, transformMatrix: cameraTransform);
            level.DrawLightMap(state, spriteBatch, gameTime);
            spriteBatch.End();

            Engine.Graphic.SetDefaultRenderTarget();

            Engine.Graphic.Begin(spriteBatch);

            spriteBatch.Draw(Engine.Graphic.RenderTarget[1], Engine.Graphic.GetResolutionRect(), Color.White);

            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Immediate, _lightBlend);

            spriteBatch.Draw(Engine.Graphic.RenderTarget[0], Engine.Graphic.GetResolutionRect(), Color.White);

            spriteBatch.End();
        }

        public void Initialize(GameManager game)
        {
            Game = game;
            foreach (var l in Levels) l.Initialize(this, game);
        }

        public WorldStorage Save()
        {
            var w = new WorldStorage();
            foreach (var l in Levels)
            {
                w.Levels.Add(l.Save());
            }

            
            w.Time = DayNightCycle.Time;
            w.PlayerSpawnLevel = PlayerSpawnLevel;
            return w;
        }

        public void Load(WorldStorage store)
        {
            DayNightCycle.Time = store.Time;
            PlayerSpawnLevel = store.PlayerSpawnLevel;
            
            foreach (var levelData in store.Levels)
            {
                var level = new Level(LEVELS.GetProperties(levelData.Type), levelData.Width, levelData.Height);
                level.Load(levelData);
                Levels.Add(level);
            }
        }
    }
}