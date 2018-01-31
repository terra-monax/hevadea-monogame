﻿using System.Collections.Generic;
using Maker.Hevadea.Game.Entities;
using Maker.Hevadea.Game.Items;
using Maker.Hevadea.Game.Items.Materials;
using Maker.Rise.Ressource;
using Microsoft.Xna.Framework;

namespace Maker.Hevadea.Game.Registry
{
    public static class ITEMS
    {
        public static List<Item> ById = new List<Item>();

        public static Material WoodMaterial;
        public static Material IronMaterial;
        public static Material GoldMaterial;

        public static RessourceItem WoodLog;
        public static RessourceItem WoodPlank;
        public static RessourceItem WoodStick;
        public static RessourceItem PineCone;
        public static RessourceItem Stone;
        public static RessourceItem Coal;
        
        public static PlacableItem<ChestEntity> ChestItem;
        public static PlacableItem<TorchEntity> TorchItem;
        public static PlacableItem<CraftingBenchEntity> CraftingbenchItem;

        public static void Initialize()
        {   
            WoodMaterial = new BaseMaterial(2f);
            IronMaterial = new BaseMaterial(4f);
            GoldMaterial = new BaseMaterial(8f);

            WoodLog   = new RessourceItem("Wood Log", new Sprite(Ressources.tile_items, 6));
            WoodPlank = new RessourceItem("Wood Plank", new Sprite(Ressources.tile_items, new Point(6,1)));
            WoodStick = new RessourceItem("Wood Stick", new Sprite(Ressources.tile_items, 5));
            PineCone  = new RessourceItem("Pine Cone", new Sprite(Ressources.tile_items, new Point(5,2)));
            Stone     = new RessourceItem("Stone", new Sprite(Ressources.tile_items, new Point(7, 0)));
            Coal      = new RessourceItem("Coal", new Sprite(Ressources.tile_items, new Point(6, 2)));
            
            ChestItem = new PlacableItem<ChestEntity>("Chest", new Sprite(Ressources.tile_entities, new Point(1, 1)));
            TorchItem = new PlacableItem<TorchEntity>("Torch", new Sprite(Ressources.tile_entities, new Point(1, 3)));
            CraftingbenchItem = new PlacableItem<CraftingBenchEntity>("Bench", new Sprite(Ressources.tile_entities, new Point(2, 2)));
        }
    }
}
