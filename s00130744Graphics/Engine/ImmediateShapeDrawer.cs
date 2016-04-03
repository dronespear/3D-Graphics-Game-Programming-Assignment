#region File Description
//-----------------------------------------------------------------------------
// DebugShapeRenderer.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Sample
{
    public class ImmediateShapeDrawer
	{
		class DebugShape
		{
			public VertexPositionColor[] Vertices;
			public int LineCount;
			public float Lifetime;
		}
        
		private static readonly List<DebugShape> activeShapes = new List<DebugShape>();

        private static VertexPositionColor[] verts = new VertexPositionColor[64];

		private static BasicEffect effect;

		private static Vector3[] corners = new Vector3[8];

        private const int sphereResolution = 30;
        private const int sphereLineCount = (sphereResolution + 1) * 3;
        private static Vector3[] unitSphere;

        public ImmediateShapeDrawer()
        {
        }

        public void Initialize()
        {
            effect = new BasicEffect(GameUtilities.GraphicsDevice);
            effect.VertexColorEnabled = true;
            effect.TextureEnabled = false;
            effect.DiffuseColor = Vector3.One;
            effect.World = Matrix.Identity;
        }

        public static void Clear()
        {
            activeShapes.Clear();
        }

		public void DrawBoundingBox(BoundingBox box, Camera camera)
		{
			DebugShape shape = GetShapeForLines(12, 0);

			box.GetCorners(corners);

            Color color = Color.Red;

			shape.Vertices[0] = new VertexPositionColor(corners[0], color);
			shape.Vertices[1] = new VertexPositionColor(corners[1], color);
			shape.Vertices[2] = new VertexPositionColor(corners[1], color);
			shape.Vertices[3] = new VertexPositionColor(corners[2], color);
			shape.Vertices[4] = new VertexPositionColor(corners[2], color);
			shape.Vertices[5] = new VertexPositionColor(corners[3], color);
			shape.Vertices[6] = new VertexPositionColor(corners[3], color);
			shape.Vertices[7] = new VertexPositionColor(corners[0], color);

			shape.Vertices[8] = new VertexPositionColor(corners[4], color);
			shape.Vertices[9] = new VertexPositionColor(corners[5], color);
			shape.Vertices[10] = new VertexPositionColor(corners[5], color);
			shape.Vertices[11] = new VertexPositionColor(corners[6], color);
			shape.Vertices[12] = new VertexPositionColor(corners[6], color);
			shape.Vertices[13] = new VertexPositionColor(corners[7], color);
			shape.Vertices[14] = new VertexPositionColor(corners[7], color);
			shape.Vertices[15] = new VertexPositionColor(corners[4], color);

			shape.Vertices[16] = new VertexPositionColor(corners[0], color);
			shape.Vertices[17] = new VertexPositionColor(corners[4], color);
			shape.Vertices[18] = new VertexPositionColor(corners[1], color);
			shape.Vertices[19] = new VertexPositionColor(corners[5], color);
			shape.Vertices[20] = new VertexPositionColor(corners[2], color);
			shape.Vertices[21] = new VertexPositionColor(corners[6], color);
			shape.Vertices[22] = new VertexPositionColor(corners[3], color);
			shape.Vertices[23] = new VertexPositionColor(corners[7], color);

            Draw(camera);
            Clear();
		}

        public void DrawBoundingSphere(BoundingSphere sphere, Camera camera)
        {
            DebugShape shape = GetShapeForLines(sphereLineCount, 0);

            for (int i = 0; i < unitSphere.Length; i++)
            {
                Vector3 vertPos = unitSphere[i] * sphere.Radius + sphere.Center;

                shape.Vertices[i] = new VertexPositionColor(vertPos, Color.Red);
            }

            Draw(camera);
            Clear();
        }

		public void Draw(Camera _camera)
		{
            if (_camera != null)
            {
                effect.View = _camera.View;
                effect.Projection = _camera.Projection;

                int vertexCount = 0;
                foreach (var shape in activeShapes)
                    vertexCount += shape.LineCount * 2;

                if (vertexCount > 0)
                {
                    if (verts.Length < vertexCount)
                    {
                        verts = new VertexPositionColor[vertexCount * 2];
                    }

                    int lineCount = 0;
                    int vertIndex = 0;
                    foreach (DebugShape shape in activeShapes)
                    {
                        lineCount += shape.LineCount;
                        int shapeVerts = shape.LineCount * 2;
                        for (int i = 0; i < shapeVerts; i++)
                            verts[vertIndex++] = shape.Vertices[i];
                    }

                    effect.CurrentTechnique.Passes[0].Apply();

                    int vertexOffset = 0;
                    while (lineCount > 0)
                    {
                        int linesToDraw = Math.Min(lineCount, 65535);

                        GameUtilities.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;

                        GameUtilities.GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, verts, vertexOffset, linesToDraw);

                        vertexOffset += linesToDraw * 2;

                        lineCount -= linesToDraw;
                    }
                }
            }
        }
        
        private static DebugShape GetShapeForLines(int lineCount, float life)
        {
            DebugShape shape = null;


            int vertCount = lineCount * 2;

            if (shape == null)
            {
                shape = new DebugShape { Vertices = new VertexPositionColor[vertCount] };
                activeShapes.Add(shape);
            }

            shape.LineCount = lineCount;
            shape.Lifetime = life;

            return shape;
        }
	}
}
