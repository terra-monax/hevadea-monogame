﻿using Hevadea.Framework.Graphic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hevadea.Framework.UI
{
    public enum Dock
    {
        Top,
        Right,
        Bottom,
        Left,
        Fill,
        None
    }

    public enum MouseState
    {
        Over, Down, None
    }

    public class Padding
    {
        public int Up { get; set; }
        public int Down { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }

        public Padding(int up, int down, int left, int right)
        {
            Up = up;
            Down = down;
            Left = left;
            Right = right;
        }

        public Padding(int all)
        {
            Up = Down = Left = Right = all;
        }

        public Padding(int horizontal, int vertical)
        {
            Up = Down = vertical;
            Left = Right = horizontal;
        }

        public Rectangle Apply(Rectangle rect)
        {
            return new Rectangle(rect.X + Left, rect.Y + Up, rect.Width - Left - Right, rect.Height - Up - Down);
        }
    }

    public class Widget
    {

        public int Scale(int val) =>  (int)(val * Rise.Ui.ScaleFactor);

        public Rectangle UnitBound { get; set; } = new Rectangle(0, 0, 64, 64);
        public Rectangle UnitHost { get { return Padding.Apply(UnitBound); } }
        public Point UnitOffset { get; set; } = Point.Zero;
        public Padding Padding { get; set; } = new Padding(0);

        public Anchor Anchor  { get; set; } = Anchor.TopLeft;
        public Anchor Origine { get; set; } = Anchor.TopLeft;

        public delegate void WidgetEventHandler(Widget sender);
        public event WidgetEventHandler MouseClick;
        public event WidgetEventHandler MouseHold;
        public Dock Dock { get; set; } = Dock.None;
        public MouseState MouseState { get; set; } = MouseState.None;

        protected Rectangle Bound => new Rectangle((int)(UnitBound.X * Rise.Ui.ScaleFactor), (int)(UnitBound.Y * Rise.Ui.ScaleFactor), (int)(UnitBound.Width * Rise.Ui.ScaleFactor), (int)(UnitBound.Height * Rise.Ui.ScaleFactor));
        protected Rectangle Host => new Rectangle((int)(UnitHost.X * Rise.Ui.ScaleFactor), (int)(UnitHost.Y * Rise.Ui.ScaleFactor), (int)(UnitHost.Width * Rise.Ui.ScaleFactor), (int)(UnitHost.Height * Rise.Ui.ScaleFactor));
        protected Point Offset => new Point((int)(UnitOffset.X * Rise.Ui.ScaleFactor), (int)(UnitOffset.Y * Rise.Ui.ScaleFactor));
        
        public virtual void RefreshLayout()
        {
            
        }

        public void HandleInput()
        {
            if (Rise.Pointing.AreaOver(Bound))
            {
                MouseState = MouseState.Over;
                if (Rise.Pointing.AreaDown(Bound))
                {
                    MouseState = MouseState.Down;
                    MouseHold?.Invoke(this);
                }
                
                if (Rise.Pointing.AreaClick(Bound))
                {
                    MouseClick?.Invoke(this);
                }
            }
            else
            {
                MouseState = MouseState.None;
            }
        }
        
        public void UpdateInternal(GameTime gameTime)
        {
            HandleInput();
            Update(gameTime);
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public void DrawIternal(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Draw(spriteBatch, gameTime);
            if (Rise.DebugUI)
            {
                spriteBatch.DrawRectangle(Host, Color.Cyan);
                spriteBatch.DrawRectangle(Bound, Color.Black);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

        }

        public Widget RegisterMouseClickEvent(WidgetEventHandler func)
        {
            MouseClick += func;
            return this;
        }
    }
}