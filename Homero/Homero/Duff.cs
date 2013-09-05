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
    public class Duff : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Texture2D img;
        private Vector2 posicion;
        private Rectangle pantalla;
        private int velocidadY=2;
        private Random r = new Random();
        private SpriteBatch sBath;

        public Duff(Game game, ref Texture2D imagen)
            : base(game)
        {
            this.img = imagen;
            pantalla = new Rectangle(0, 0, game.Window.ClientBounds.Width, game.Window.ClientBounds.Height);
            Iniciar();
            sBath = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
        }
        public void Iniciar()
        {
            posicion.Y = -img.Height;
            posicion.X = (int)(r.NextDouble() * pantalla.Width - img.Width);
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

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            posicion.Y += velocidadY;
            base.Update(gameTime);
        }
        public bool checaColision(Rectangle r)
        {
            Rectangle rDuff = new Rectangle((int)posicion.X, (int)posicion.Y, img.Width, img.Height);
            return rDuff.Intersects(r);
        }
        public bool colisionaSuelo()
        {
            if (posicion.Y + img.Height >= pantalla.Height)
            {
                return true;
            }
            return false;
        }
        public override void Draw(GameTime gameTime)
        {
            sBath.Draw(img, posicion, Color.White);
            base.Draw(gameTime);
        }
    }
}