﻿using Hevadea.Game;
using Hevadea.Game.Entities.Creatures;
using Hevadea.Game.Registry;
using Hevadea.WorldGenerator;
using Maker.Rise;
using Maker.Rise.Components;
using Maker.Rise.UI.Widgets;
using Maker.Rise.UI.Widgets.Containers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Threading;

namespace Hevadea.Scenes
{
    public class WorldGenScene : Scene
    {
        public Thread GeneratorThread;
        public Generator worldgen;
        private SpriteBatch _sb;
        private Label _progressLabel;
        private ProgressBar _progressBar;
        public WorldGenScene()
        {
            _sb = Engine.Graphic.CreateSpriteBatch();
            GeneratorThread = new Thread(() =>
            {
                Thread.Sleep(1000);
                GC.AddMemoryPressure(600 * 1024 * 1024);
                worldgen = GENERATOR.DEFAULT;
                worldgen.Seed = new Random().Next();
                var world = worldgen.Generate();
                var player = (PlayerEntity)ENTITIES.PLAYER.Build();
                Engine.Scene.Switch(new GameScene(new GameManager(world, player)));
            });

            _progressLabel = new Label { Text = "Generating world...", Anchor = Anchor.Center, Origine = Anchor.Center, Font = Ressources.FontRomulus, Offset = new Point(0, -24) };
            _progressBar = new ProgressBar { Bound = new Rectangle(0, 0, 320, 8), Anchor = Anchor.Center, Origine = Anchor.Center};
            
            Container = new AnchoredContainer
            {
                Childrens = {_progressLabel, _progressBar}
            };
        }
        
        public override void OnDraw(GameTime gameTime)
        {
 
        }

        public override void Load()
        {
            GeneratorThread.Start();
        }

        public override void Unload()
        {
        }

        public override void OnUpdate(GameTime gameTime)
        {
            if (worldgen?.CurrentLevel?.CurrentFeature != null)
            {
                _progressLabel.Text = $"{worldgen.CurrentLevel.LevelName}: {worldgen.CurrentLevel.CurrentFeature.GetName()}";
                _progressBar.Value = worldgen.CurrentLevel.CurrentFeature.GetProgress();
            }
        }
    }
}