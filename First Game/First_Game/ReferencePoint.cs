using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace First_Game
{
	class ReferencePoint
	{
		Model model;
		Vector3 pos;
		
		//collisions
		public BoundingBox box;
		float size = 10;

		Matrix[] bonetransformations;


		public ReferencePoint(ContentManager c)
		{
			model = c.Load<Model>("Bullet");
			Console.WriteLine("Loaded");
			pos = new Vector3(20, 0, 20);
			box = new BoundingBox(new Vector3(pos.X - size / 2, pos.Y, pos.Z - size / 2), new Vector3(pos.X + size / 2, pos.Y + size, pos.Z + size / 2));//two corners, essentially
		}

		public void draw(Matrix projection, Matrix view)
		{
			bonetransformations = new Matrix[model.Bones.Count];
			model.CopyAbsoluteBoneTransformsTo(bonetransformations);

			foreach (ModelMesh mesh in model.Meshes)
			{
				foreach (BasicEffect effect in mesh.Effects)
				{
					effect.World = bonetransformations[mesh.ParentBone.Index] * Matrix.CreateScale(5f) * Matrix.CreateTranslation(pos);
					effect.View = view;
					effect.Projection = projection;
					effect.EnableDefaultLighting();
				}
				mesh.Draw();
			}

		}

	}
}
