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

namespace First_Game
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
		//need matrices
		//is this working?
		Matrix projection;
		Player p;

		ReferencePoint rP;

		public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
			IsMouseVisible = true;
			p = new First_Game.Player(Vector3.Zero);
			//has cleaner process, just so you know. Althought we have written in an unloader in the next functio
		}

		protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
			projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f), graphics.GraphicsDevice.Viewport.AspectRatio, .1f, 1000f); //how far you can see is last

			rP = new ReferencePoint(Content);

			p.loadContent(Content);
		}

        protected override void UnloadContent()
        {
			Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Escape) == true)
            {
				this.Exit();
            }

			p.update();

			base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

			rP.draw(projection, p.view);
			p.draw(projection);

            base.Draw(gameTime);
        }
    }
}
