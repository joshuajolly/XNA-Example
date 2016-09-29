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
		Matrix view, projection;
		Model model;
		Matrix[] boneTransformations;

		Vector3 rotation;

		public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
			IsMouseVisible = true;
			
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
			model = Content.Load<Model>("Car");
			rotation = Vector3.Zero;
			view = Matrix.CreateLookAt(new Vector3(10, 10, 10), Vector3.Zero, Vector3.Up);//position of camera, where object is, looking at 0,0, vector3.up is face straight up
			projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f), graphics.GraphicsDevice.Viewport.AspectRatio, .1f, 1000f); //how far you can see is last
			//has cleaner process, just so you know.
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
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

			rotation.Y += .1f;

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

			boneTransformations = new Matrix[model.Bones.Count];
			model.CopyAbsoluteBoneTransformsTo(boneTransformations);


            foreach (ModelMesh mesh in model.Meshes)
			{
				foreach (BasicEffect effect in mesh.Effects)
				{
					effect.World = boneTransformations[mesh.ParentBone.Index] * Matrix.CreateScale(2f) * Matrix.CreateRotationX(rotation.X) * Matrix.CreateRotationY(rotation.Y) * Matrix.CreateRotationZ(rotation.Z); //where transformations are going on //can also do matrix.createtranslation to move it around
					effect.View = view;
					effect.Projection = projection;
					effect.EnableDefaultLighting();
				}
				mesh.Draw();
			}

            base.Draw(gameTime);
        }
    }
}
