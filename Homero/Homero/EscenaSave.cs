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
    public class EscenaSave : Escena
    {
        protected SpriteFont fuente;
        protected SpriteBatch sBatch;
        protected int score;
        protected string[] nombre = { "A", "A", "A" };
        private Texture2D fondo;
        private int currCar;
        private Vector2 pantalla;

        public EscenaSave(Game game, SpriteFont funte, Texture2D fondo)
            : base(game)
        {
            this.fuente = funte;
            this.fondo = fondo;
            pantalla = new Vector2(game.Window.ClientBounds.Width, game.Window.ClientBounds.Height);
        }

        public string Nombre{
            get{return nombre[0]+nombre[1]+nombre[2];}
        }

        public int Score{
            get{return score;}
            set{score=value;}
        }
        public override void Show(){
            currCar=0;
            base.Show();
        }

        public void cambiarNombre(int cambio){
            switch(cambio){
                case 1:
                    nombre[currCar]= "" + (char)(((int)(char)nombre[currCar][0]) +1);
                    break;
                case 2:
                    nombre[currCar]= "" + (char)(((int)(char)nombre[currCar][0]) -1);
                    break;
                case 3:
                    currCar-=1;
                    if(currCar<0)
                        currCar=2;
                    break;
                case 4:
                    currCar=(currCar +1)%3;
                    break;
            }
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
            SpriteBatch sBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            sBatch.Draw(fondo, new Vector2(0, 0), Color.White);

            string strScore = "Score: "+score;
            Vector2 posScore = fuente.MeasureString(strScore);
            posScore.Y = 100;
            posScore.X = pantalla.X/2-posScore.X/2;
            
            string strIniciales = "Iniciales ";
            Vector2 medIniciales = fuente.MeasureString(strIniciales);
            Vector2 posIniciales = Vector2.Zero;
            posIniciales.Y = 200;
            posIniciales.X = pantalla.X/2-medIniciales.X;
            float x = medIniciales.X/2 + pantalla.X/2;

            sBatch.DrawString(fuente,strScore, posScore, Color.Yellow);
            sBatch.DrawString(fuente,strIniciales, posIniciales, Color.Yellow);
            for (int i = 0; i < 3; i++)
            {
                Color color = Color.Yellow;
                if (currCar == i)
                    color = Color.Red;
                sBatch.DrawString(fuente, nombre[i] , new Vector2(x+50*i, 200), color);
                if(i<2)
                    sBatch.DrawString(fuente, "-" , new Vector2(x+50*i+25, 200), Color.Yellow);
            }
             
            base.Draw(gameTime);
        }

        //Vector2 size = fuenteNormal.MeasureString(item);//Se usa fuenteSeleccionada
        //height += fuenteSeleccionada.LineSpacing;

    }
}