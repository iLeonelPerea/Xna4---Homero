using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;


namespace Homero
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Homero : Microsoft.Xna.Framework.DrawableGameComponent
    {
        protected Texture2D imagen;
        protected int velocidad;
        protected Vector2 posicion;
        protected Rectangle pantalla;

        public Homero(Game game, Texture2D img, int vel)
            : base(game)
        {
            imagen = img;
            velocidad = vel;
            pantalla=new Rectangle(0,0,game.Window.ClientBounds.Width, game.Window.ClientBounds.Height);
            iniciar();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        public void iniciar()
        {
            posicion.X = pantalla.Width / 2 - imagen.Width / 2;
            posicion.Y = pantalla.Height - imagen.Height;
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            manejaTeclado();
            checaColision();

            base.Update(gameTime);
        }
        public void manejaTeclado()
        {
            KeyboardState teclado = Keyboard.GetState();
            if (teclado.IsKeyDown(Keys.Left))
            {
                posicion.X -= velocidad;
            }
            if (teclado.IsKeyDown(Keys.Right))
            {
                posicion.X += velocidad;
            }
        }
        public void checaColision()
        {
            if (posicion.X < 0)
            {
                posicion.X = 0;
            }
            if (posicion.X + imagen.Width > pantalla.Width)
            {
                posicion.X = pantalla.Width - imagen.Width;
            }
        }
        public Rectangle GetArea()
        {
            return new Rectangle((int)posicion.X, (int)posicion.Y, imagen.Width, imagen.Height);
        }

        public override void Draw(GameTime gameTime)
        {
            //el sprotebatch se obtine de la clase principal
            SpriteBatch sBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            sBatch.Draw(imagen, posicion, Color.White);
            base.Draw(gameTime);
        }
    }
}