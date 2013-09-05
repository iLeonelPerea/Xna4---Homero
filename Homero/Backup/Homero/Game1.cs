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
using System.IO;
using System.Xml.Serialization;

namespace Homero
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Texture2D springfield;
        private Texture2D homero;
        private Texture2D duff;
        private Viewport pantalla;
        //Escenas
        private EscenaJuego ej;
        private EscenaIntro ei;
        private EscenaInstrucciones eIns;
        private EscenaScore es;
        private EscenaSave esa;
        private Escena escenaActiva;
        //Fuentes
        private SpriteFont fuenteNormal, fuenteSeleccionada;
        ///Audio
        private AudioComponent audioComponent;
        //TECLADO
        KeyboardState oldKeyboardState;
        //Score
        private string nombreArchivo = "Homero.txt";
        public List<Score> listaScore = new List<Score>();


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 334;
            graphics.PreferredBackBufferWidth = 800;
            
            Content.RootDirectory = "Content";
            LeerArchivo();
        }


        public void LeerArchivo()
        {
            TextReader tr = new StreamReader(nombreArchivo);
            string linea = tr.ReadLine();
            listaScore.Sort(new ComparaScore());

            while (linea != null)
            {
                Console.WriteLine("LeerArchivo");
                string[] arr = linea.Split(':');
                listaScore.Add(new Score(arr[0], int.Parse(arr[1])));
                linea = tr.ReadLine();
            }
            tr.Close();
        }
        public void EscribeArchivo()
        {
            TextWriter tw = new StreamWriter(nombreArchivo);
            listaScore.Sort(new ComparaScore());
            for(int i=0; i< listaScore.Count; i++)
            {
                tw.WriteLine(((Score)listaScore[i]).Nombre +":"+((Score)listaScore[i]).Puntos);
            }
            tw.Close();
        }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //audioEngine = new AudioEngine("simpsons.xgs");
            //waveBank = new WaveBank(audioEngine,"Wave Bank.xwb");
            //soundBank = new SoundBank(audioEngine,"Sound Bank.xsb");
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof(SpriteBatch), spriteBatch);
            //pantalla
            pantalla = graphics.GraphicsDevice.Viewport;
            //Fuentes
            fuenteNormal = Content.Load<SpriteFont>("SimpsonNormal");
            fuenteSeleccionada = Content.Load<SpriteFont>("SimpsonSeleccionadaF");
            //Texturas
            springfield = Content.Load<Texture2D>("Springfield");
            homero = Content.Load<Texture2D>("homer");
            duff = Content.Load<Texture2D>("duffBeer");
            Texture2D imgFondo = Content.Load<Texture2D>("fondoIntro");
            Texture2D imgSimpsons = Content.Load<Texture2D>("losSimpsons");
            Texture2D imgInstrucciones = Content.Load<Texture2D>("instrucciones");
            Texture2D cloud1 = Content.Load<Texture2D>("cloud2");
            Texture2D cloud2 = Content.Load<Texture2D>("cloud2");
            //Audio
            audioComponent = new AudioComponent(this);
            Services.AddService(typeof(AudioComponent), audioComponent);
            audioComponent.Initialize();    //DUDA
            Components.Add(audioComponent);
            //Escenas
            ej = new EscenaJuego(this, homero, duff, springfield, fuenteNormal);
            ei = new EscenaIntro(this, ref fuenteNormal, ref fuenteSeleccionada, ref imgFondo, ref imgSimpsons,
                                    ref cloud1, ref cloud2);
            eIns= new EscenaInstrucciones(this, ref imgInstrucciones);
            es = new EscenaScore(this, fuenteNormal, ref imgFondo);
            esa = new EscenaSave(this, fuenteSeleccionada, imgFondo);
            //Agregar a componentes
            Components.Add(ej);
            Components.Add(ei);
            Components.Add(eIns);
            Components.Add(es);
            Components.Add(esa);
            //ej.Show();
            ei.Show();
            escenaActiva = ei;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            ManejarEscenas();
            //ej.Jugar();

            base.Update(gameTime);
        }

        public void ManejarEscenas()
        {
            if (escenaActiva == ej)
            {
                IsMouseVisible = false;
                if (ChecaTecla(Keys.Escape))
                {
                    MostrarEscena(ei);
                    audioComponent.PlayCue("ohhh");
                }
                else
                {
                    if (ChecaEnter())
                    {
                        ej.Pausado = !ej.Pausado;
                    }
                    ej.Jugar();
                    if (ej.Terminado)
                    {
                        esa.Score = ej.Score;
                        MostrarEscena(esa);
                    }
                }
            }
            if (escenaActiva == ei)
            {
                IsMouseVisible = false;
                ManejaEscenaIntro();
            }
            if (escenaActiva == eIns)
            {
                IsMouseVisible = false;
                if (ChecaEnter())
                {
                    MostrarEscena(ei);
                }
            }
            if (escenaActiva == es)
            {
                es.ListaScore = listaScore;
                IsMouseVisible = false;
                if (ChecaEnter())
                {
                    MostrarEscena(ei);
                }
            }
            if (escenaActiva == esa)
            {
                IsMouseVisible = false;
                ManejaEcenaSave();
            }
        }

        public void ManejaEscenaIntro()
        {
            if (ChecaEnter())
            {
                audioComponent.PlayCue("wooho");
                switch (ei.SelectedMenuIndex)
                {
                    case 0:
                        //Jugar
                        MostrarEscena(ej);
                        break;
                    case 1:
                        MostrarEscena(eIns);
                        break;
                    case 2:
                        Console.Write("es");
                        MostrarEscena(es);
                        break;
                    case 3:
                        Exit();
                        break;
                }
            }
        }

        public void ManejaEcenaSave()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            EscuchaTeclado();
            if (ChecaEnter())
            {
                listaScore.Add(new Score(esa.Nombre, esa.Score));
                EscribeArchivo();
                Console.WriteLine("Saving...");
                MostrarEscena(ei);
                Console.WriteLine("Saved...");
            }
            oldKeyboardState = keyboardState;
        }

        public void EscuchaTeclado()
        {
            //KeyboardState keyboardState = Keyboard.GetState();
            if (ChecaTecla(Keys.Up))
            {
                Console.Write("Up");
                esa.cambiarNombre(1);
            }
            if (ChecaTecla(Keys.Down))
            {
                Console.Write("down");
                esa.cambiarNombre(2);
            }
            if (ChecaTecla(Keys.Right))
            {
                Console.Write("right");
                esa.cambiarNombre(4);
            }
            if (ChecaTecla(Keys.Left))
            {
                Console.Write("left");
                esa.cambiarNombre(3);
            }
            //oldKeyboardState = keyboardState;
        }

        public bool ChecaEnter()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            bool resultado = (oldKeyboardState.IsKeyDown(Keys.Enter) && keyboardState.IsKeyUp(Keys.Enter));
            oldKeyboardState = keyboardState;
            return resultado;
        }

        public bool ChecaTecla(Keys key)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            bool resultado = (oldKeyboardState.IsKeyDown(key) && keyboardState.IsKeyUp(key));
            //oldKeyboardState = keyboardState;
            return resultado;
        }

        protected void MostrarEscena(Escena scene)
        {
            escenaActiva.Hide();
            escenaActiva = scene;
            escenaActiva.Show();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            base.Draw(gameTime);
            spriteBatch.End();
            //base.Draw(gameTime);
        }


        /*
         public void escuchaTeclado()
        {
            KeyboardState teclado = Keyboard.GetState();
            if (teclado.IsKeyDown(Keys.Left) && pos_homero.X > 0)
            {
                pos_homero.X -= velocidad;
            }
            if (teclado.IsKeyDown(Keys.Right) && pos_homero.X +homero.Width <pantalla.Width)
            {
                pos_homero.X += velocidad;
            }
        }

        private void agregarEsferas()
        {
            Random semilla = new Random();
            if (semilla.NextDouble() < aleatorio)
            {
                //agrego cheve
                int x = (int)(semilla.NextDouble() * (pantalla.Width - duff.Width));
                Vector2 v = new Vector2(x, -duff.Height);
                lista.Add(v);
            }
        }
        protected void actualizaEsfereas()
        {
            for (int i = 0; i < lista.Count; i++)
            {
                Vector2 v = lista[i];
                v.Y += velocidadEsferas;
                if (v.Y >= pantalla.Height - duff.Height)
                {
                    lista.RemoveAt(i);
                    i--;
                    score -= 5;
                    soundBank.PlayCue("doh");
                    continue;
                }
                lista[i] = v;
            }
        }
        protected void checaColision(){
            Rectangle rHomero= new Rectangle((int)pos_homero.X, (int)pos_homero.Y, homero.Width,homero.Height);
            for (int i = 0; i < lista.Count; i++)
            {
                Vector2 vCheve = lista[i];
                Rectangle rCheve = new Rectangle((int)vCheve.X, (int)vCheve.Y, duff.Width, duff.Height);
                if (rHomero.Intersects(rCheve))
                {
                    score += 10;
                    lista.RemoveAt(i);
                    i--;
                    soundBank.PlayCue("inh2o");
                }
            }
        }
         */
    }
}
