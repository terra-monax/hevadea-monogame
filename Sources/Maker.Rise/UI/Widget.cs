﻿using Maker.Rise.Extension;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Maker.Rise.UI.Widgets
{
    public enum Anchor
    {
        TopLeft,
        Top,
        TopRight,
        Left,
        Center,
        Right,
        BottomLeft,
        Bottom,
        BottomRight
    }
    
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
        public Rectangle Bound { get; set; } = new Rectangle(0, 0, 64, 64);
        public Rectangle Host  { get { return Padding.Apply(Bound); } }
        public Point Offset { get; set; } = Point.Zero;
        public Padding Padding { get; set; } = new Padding(0);
        
        public Anchor Anchor  { get; set; } = Anchor.TopLeft;
        public Anchor Origine { get; set; } = Anchor.TopLeft;

        public delegate void WidgetEventHandler(Widget sender);
        public event WidgetEventHandler MouseClick;
        public Dock Dock { get; set; } = Dock.None;
        public MouseState MouseState { get; set; } = MouseState.None;

        public virtual void RefreshLayout()
        {
            
        }
        
        public void HandleInput()
        {
            if (Bound.Contains(Engine.Input.MousePosition))
            {
                MouseState = MouseState.Over;
                if (Engine.Input.MouseLeft)
                {
                    MouseState = MouseState.Down;
                }
                
                if (Engine.Input.MouseLeftClick)
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
            if (Engine.Configuration.DebugUi)
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