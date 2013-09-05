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
using Homero.Core;

namespace Homero
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class EscenaScore : Escena
    {
        //Atributos
        private SpriteBatch sBatch;
        private Texture2D fondo;
        private SpriteFont fuente;
        private List<Score> listaScores;

        public EscenaScore(Game game, SpriteFont fuente ,ref Texture2D fondo)
            : base(game)
        {
            sBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));
            this.fondo = fondo;
            this.fuente = fuente;
        }


        public List<Score> ListaScore
        {
            set { listaScores = value; }
            get { return listaScores; }
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
            // TODO: Add your update code here

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Console.WriteLine("DrawSocres");
            sBatch.Draw(fondo, new Vector2(0, 0), Color.White);
            int i=0;
            while ((i <= 10) && (i < listaScores.Count))
            {
                sBatch.DrawString(fuente, "" + (i+1)+": "+ ((Score) listaScores[i]).Puntos + " - " + ((Score)listaScores[i]).Nombre,new Vector2(10, i*30), Color.White );
                i++;
            }
            base.Draw(gameTime);
        }

    }
}