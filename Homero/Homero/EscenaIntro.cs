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
    public class EscenaIntro : Escena
    {
        //Atributos
        private SpriteBatch sBatch;
        private Menu menu;
        private Texture2D fondo, imgSimpsons, cloud1, cloud2;
        private Rectangle rImagen, pantalla, rCloud1, rCloud2;
        protected AudioComponent audioComponent;

        public EscenaIntro(Game game, ref SpriteFont fn, ref SpriteFont fs, ref Texture2D fondo,
            ref Texture2D imgSimpsons, ref Texture2D cloud1, ref Texture2D cloud2)
            : base(game)
        {
            sBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));
            menu = new Menu(game, fn, fs);
            string[] menuItems = { "Iniciar", "Instrucciones", "Mejores puntajes", "Salir" };
            menu.SetMenuItems(menuItems);
            menu.Posicion = new Vector2(game.Window.ClientBounds.Width/2 - menu.Width/2, game.Window.ClientBounds.Height/2 - menu.Height/2);

            this.fondo= fondo;
            this.imgSimpsons=imgSimpsons;
            this.cloud1=cloud1;
            this.cloud2=cloud2;

            pantalla = new Rectangle(0, 0, game.Window.ClientBounds.Width, game.Window.ClientBounds.Height);
            audioComponent = (AudioComponent)game.Services.GetService(typeof(AudioComponent));
            //menu.Visible=true;
            //menu.Enabled = true;
            Componentes.Add(menu);

        }
        //Propiedad
        public int SelectedMenuIndex
        {
            get { return menu.SelectedIndex; }
        }

        public override void Show()
        {
            menu.Visible = false;
            menu.Enabled = false;
            rImagen = new Rectangle(pantalla.Width / 2 - 15, pantalla.Height / 2 - 6 -(50), 30, 12);
            rCloud1 = new Rectangle(pantalla.Width /5*2, pantalla.Height/2 - 6, 52,12);
            rCloud2 = new Rectangle(pantalla.Width - pantalla.Width/5*2, pantalla.Height / 2 - 7, 60, 14);
            audioComponent.PlayCue("simpsons");
            base.Show();
        }

        public void IncrementaImagen(int inc)
        {
            rImagen.Width = rImagen.Width + inc;
            rImagen.Height = rImagen.Height + inc/3;
            rImagen.X = pantalla.Width/2 - rImagen.Width/2;
            rImagen.Y = pantalla.Height / 2 - rImagen.Height / 2 - (50);
        }

        public void AcercarNubes()
        {
            rCloud1.Width = rCloud1.Width+3;
            rCloud1.Height = rCloud1.Height + 1;
            rCloud1.X = rCloud1.X - 3;
            //rCloud1.Y = pantalla.Height / 2 - rCloud1.Height / 2;
            rCloud1.Y = rCloud1.Y -1;

            rCloud2.Width = rCloud2.Width + 3;
            rCloud2.Height = rCloud2.Height + 1;
            //rCloud2.X = rCloud2.X;
            //rCloud2.Y = pantalla.Height / 2 - rCloud2.Height / 2;
            rCloud2.Y = pantalla.Height / 4*3 - rCloud2.Height / 2;
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
            if (!menu.Visible)
            {
                if(rImagen.Width <= 600)
                    IncrementaImagen(6);
                else if (rImagen.Width >= 600 && rImagen.Width <= 1200)
                    IncrementaImagen(9);
                else
                    IncrementaImagen(12);
                AcercarNubes();
                if (rImagen.Width >= 4200)//600
                {
                    menu.Visible = true;
                    menu.Enabled = true;
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            sBatch.Draw(fondo, new Vector2(0, 0), Color.White);
            sBatch.Draw(cloud1, rCloud1, Color.White);
            sBatch.Draw(cloud2, rCloud2, Color.White);
            if (rImagen.Width < 4200)
                sBatch.Draw(imgSimpsons, rImagen, Color.White);
            
            base.Draw(gameTime);
        }
    }
}