﻿namespace Maker.Hevadea.Game.Entities.Component.Interaction
{
    public class Breakable : EntityComponent
    {
        public void Break()
        {
            Owner.Components.Get<Dropable>()?.Drop();
            Owner.Remove();
        }
    }
}
