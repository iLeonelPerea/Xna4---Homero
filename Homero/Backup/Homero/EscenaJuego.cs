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
    public class EscenaJuego : Escena
    {
        //Atributos
        private Texture2D imgF;
        //private ImagenFondo imgF;
        private Homero homer;
        private Random generador= new Random();
        float aleatorio=.01f;
        private Texture2D imgDuff;
        private SpriteFont fuente;
        private int score=0, caidos =0;
        private bool pausado;
        protected AudioComponent audioComponent;

        public EscenaJuego(Game game, Texture2D imgHomero, Texture2D imgDuff,Texture2D imgFondo, SpriteFont fuente)
            : base(game)
        {
            homer = new Homero(game, imgHomero, 5);
            //imgF = new ImagenFondo(game, imgFondo);
            imgF = imgFondo;
            this.imgDuff = imgDuff;
            this.fuente = fuente;
            //Componentes.Add(imgF);
            Componentes.Add(homer);

            audioComponent = (AudioComponent)game.Services.GetService(typeof(AudioComponent));
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

        public int Score
        {
            get { return score; }
        }
        public bool Terminado
        {
            get { return caidos >= 5;}
        }
        public bool Pausado
        {
            get { return pausado; }
            set { 
                pausado = value;
                if (pausado)
                {
                    HabilitaCheves(false);
                    homer.Enabled = false;
                }
                else
                {
                    HabilitaCheves(true);
                    homer.Enabled = true;
                }
                
            }
        }

        public override void Show()
        {
            score = 0;
            caidos=0;
            pausado = false;
            base.Show();
        }

        public void generaCheve()
        {
            if (generador.NextDouble() < aleatorio)
            {
                Duff e = new Duff(Game, ref imgDuff);
                Componentes.Add(e);
            }
        }

        public void HabilitaCheves(bool estado)
        {
            for (int i = 0; i < Componentes.Count; i++)
            {
                if (Componentes[i] is Duff)
                {
                    Duff e = (Duff)Componentes[i];
                    e.Enabled = estado;
                }
            }
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
            //el spritebatch se obtine de la clase principal
            SpriteBatch sBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            //sBatch.DrawString
            if (pausado)
            {
                sBatch.Draw(imgF, new Vector2(0, 0), Color.Red);
                sBatch.DrawString(fuente, "P A U S A", new Vector2(100, 100), Color.Yellow);
            }
            else
            {
                sBatch.Draw(imgF, new Vector2(0, 0), Color.White);
                sBatch.DrawString(fuente, "Score: "+ score, new Vector2(10, 10), Color.Yellow);
                sBatch.DrawString(fuente, "Caidos: " + caidos, new Vector2(200, 10), Color.Yellow);
            }
            base.Draw(gameTime);
        }


        public void Jugar()
        {
            if (!pausado)
            {
                generaCheve();
                ChecaColision();
            }
            //verificar cambio de nivel
            //verificar sim termino juego
        }
        public void ChecaColision()
        {
            //Cheves con el piso
            for (int i = 0; i < Componentes.Count; i++)
            {
                if (Componentes[i] is Duff)
                {
                    Duff e = Componentes[i] as Duff;
                    if (e.colisionaSuelo())
                    {
                        Componentes.RemoveAt(i);
                        i--;
                        audioComponent.PlayCue("doh");
                        score -= 5;
                        caidos += 1;
                        //Actualizar score y combiar de nivel
                    }
                }
            }
            //cheves con homeros
            for (int i = 0; i < Componentes.Count; i++)
            {
                if (Componentes[i] is Duff)
                {
                    Duff e = Componentes[i] as Duff;
                    if (e.checaColision(homer.GetArea()))
                    {
                        Componentes.RemoveAt(i);
                        i--;
                        audioComponent.PlayCue("wooho");
                        score += 10;
                        //Actualizar score y combiar de nivel
                    }
                }
            }
        }
    }
}