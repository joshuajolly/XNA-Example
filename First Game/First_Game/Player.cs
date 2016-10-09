using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace First_Game
{
	class Player
	{//can do extends

		Model model;
		Vector3 pos;
		float wheelRotation;
		Vector3 rotation;
		Vector3 camPos;
		float velocity = 1;

		float DX;
		float DZ;

		float size = 10;

		public Matrix view;
		Matrix[] boneTransformations;

		BoundingBox box;

		public Player(Vector3 position)
		{
			camPos = new Vector3(position.X,position.Y + 7,position.Z-15);

			pos = position;

			//make a square so it's always the same box around it.  It'll work constantly then.
			box = new BoundingBox(new Vector3(pos.X - size / 2, pos.Y, pos.Z - size / 2), new Vector3(pos.X + size / 2, pos.Y + size, pos.Z + size / 2));//two corners, essentially

			rotation = Vector3.Zero;
		}

		public void loadContent(ContentManager c)
		{
			model = c.Load<Model>("Car");
		}

		public void update(BoundingBox referenceBox)
		{
			Console.WriteLine("POS X: " + pos.X + " POS Y " + pos.Y + " POS Z " + pos.Z + "\n");

			KeyboardState keyState = Keyboard.GetState();

			if (keyState.IsKeyDown(Keys.Left))
			{
				rotation.Y += .05f;
			}

			if (keyState.IsKeyDown(Keys.Right))
			{
				rotation.Y -= .05f;
			}

			if (keyState.IsKeyDown(Keys.Up))
			{
				DX += velocity * (float)Math.Sin(rotation.Y);
				DZ += velocity * (float)Math.Cos(rotation.Y);
				wheelRotation += .05f;
			}
			else if (keyState.IsKeyDown(Keys.Down))
			{
				DX = -velocity * (float)Math.Sin(rotation.Y);
				DZ = -velocity * (float)Math.Cos(rotation.Y);
				wheelRotation -= .05f;
			}
			else
			{
				DX = 0;
				DZ = 0;
			}

			bool collisionX = processCollisions(referenceBox, DX, 0);
			bool collisionZ = processCollisions(referenceBox, 0, DZ); //can alter outcome based on direction

			if (collisionX)
				DX = 0;
			if (collisionZ)
				DZ = 0;

			pos.X += DX;
			pos.Z += DZ;

			camPos.X = pos.X - 15 * (float)Math.Sin(rotation.Y);
			camPos.Z = pos.Z - 15 * (float)Math.Cos(rotation.Y);

			view = Matrix.CreateLookAt(camPos, pos, Vector3.Up);//position of camera, where object is, looking at 0,0, vector3.up is face straight up

			rotateWheels();
		}

		public void rotateWheels()
		{
			model.Bones[2].Transform = Matrix.CreateRotationX(wheelRotation) * Matrix.CreateTranslation(model.Bones[2].Transform.Translation);//translate before trotatie
			model.Bones[10].Transform = Matrix.CreateRotationX(wheelRotation) * Matrix.CreateTranslation(model.Bones[10].Transform.Translation);//translate before trotatie
			model.Bones[9].Transform = Matrix.CreateRotationX(wheelRotation) * Matrix.CreateTranslation(model.Bones[9].Transform.Translation);//translate before trotatie
			model.Bones[8].Transform = Matrix.CreateRotationX(wheelRotation) * Matrix.CreateTranslation(model.Bones[8].Transform.Translation);//translate before trotatie
		}

		public bool processCollisions(BoundingBox otherBox,float DX, float DZ)
		{
			box = new BoundingBox(new Vector3(pos.X - size / 2 + DX, pos.Y, pos.Z - size / 2 + DZ), new Vector3(pos.X + size / 2 + DX, pos.Y + size, pos.Z + size / 2 + DZ));//two corners, essentially

			if (box.Intersects(otherBox))
				return true;
			return false;
		}

		public void draw(Matrix projection)
		{
			boneTransformations = new Matrix[model.Bones.Count];
			model.CopyAbsoluteBoneTransformsTo(boneTransformations);


			foreach (ModelMesh mesh in model.Meshes)
			{
				foreach (BasicEffect effect in mesh.Effects)
				{
					effect.World = boneTransformations[mesh.ParentBone.Index] * Matrix.CreateScale(2f) * Matrix.CreateRotationX(rotation.X) * Matrix.CreateRotationY(rotation.Y) * Matrix.CreateRotationZ(rotation.Z) * Matrix.CreateTranslation(pos); //where transformations are going on //can also do matrix.createtranslation to move it around //set rotation.X to 0, etc.
					effect.View = view;
					effect.Projection = projection;
					effect.EnableDefaultLighting();
				}
				mesh.Draw();
			}
		}
	}
}
