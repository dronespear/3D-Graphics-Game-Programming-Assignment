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
    public class DebugEngine
	{
		class DebugShape
		{
			public VertexPositionColor[] Vertices;
			public int LineCount;
			public float Lifetime;
		}
        
		private static readonly List<DebugShape> cachedShapes = new List<DebugShape>();
		private static readonly List<DebugShape> activeShapes = new List<DebugShape>();

        private static VertexPositionColor[] verts = new VertexPositionColor[64];

		private static BasicEffect effect;

		private static Vector3[] corners = new Vector3[8];

        private const int sphereResolution = 30;
        private const int sphereLineCount = (sphereResolution + 1) * 3;
        private static Vector3[] unitSphere;

        public DebugEngine()
        {
        }

        public void Initialize()
        {
            effect = new BasicEffect(GameUtilities.GraphicsDevice);
            effect.VertexColorEnabled = true;
            effect.TextureEnabled = false;
            effect.DiffuseColor = Vector3.One;
            effect.World = Matrix.Identity;

            InitializeSphere();
        }

        public static void Clear()
        {
            activeShapes.Clear();
            cachedShapes.Clear();
        }

		public static void AddLine(Vector3 a, Vector3 b, Color color)
		{
			AddLine(a, b, color, 0f);
		}

		public static void AddLine(Vector3 a, Vector3 b, Color color, float life)
        {
			DebugShape shape = GetShapeForLines(1, life);

			shape.Vertices[0] = new VertexPositionColor(a, color);
			shape.Vertices[1] = new VertexPositionColor(b, color);
		}

		public static void AddTriangle(Vector3 a, Vector3 b, Vector3 c, Color color)
		{
			AddTriangle(a, b, c, color, 0f);
		}

		public static void AddTriangle(Vector3 a, Vector3 b, Vector3 c, Color color, float life)
        {
            DebugShape shape = GetShapeForLines(3, life);

			shape.Vertices[0] = new VertexPositionColor(a, color);
			shape.Vertices[1] = new VertexPositionColor(b, color);
			shape.Vertices[2] = new VertexPositionColor(b, color);
			shape.Vertices[3] = new VertexPositionColor(c, color);
			shape.Vertices[4] = new VertexPositionColor(c, color);
			shape.Vertices[5] = new VertexPositionColor(a, color);
		}

		public static void AddBoundingFrustum(BoundingFrustum frustum, Color color)
		{
			AddBoundingFrustum(frustum, color, 0f);
		}

		public static void AddBoundingFrustum(BoundingFrustum frustum, Color color, float life)
        {
            DebugShape shape = GetShapeForLines(12, life);

			frustum.GetCorners(corners);

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
		}

		public static void AddBoundingBox(BoundingBox box, Color color)
		{
			AddBoundingBox(box, color, 0f);
		}

		public static void AddBoundingBox(BoundingBox box, Color color, float life)
		{
			DebugShape shape = GetShapeForLines(12, life);

			box.GetCorners(corners);

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
		}

        public static void AddBoundingSphere(BoundingSphere sphere, Color color)
        {
            AddBoundingSphere(sphere, color, 0f);
        }

        public static void AddBoundingSphere(BoundingSphere sphere, Color color, float life)
        {
            DebugShape shape = GetShapeForLines(sphereLineCount, life);

            for (int i = 0; i < unitSphere.Length; i++)
            {
                Vector3 vertPos = unitSphere[i] * sphere.Radius + sphere.Center;

                shape.Vertices[i] = new VertexPositionColor(vertPos, color);
            }
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


                bool resort = false;
                for (int i = activeShapes.Count - 1; i >= 0; i--)
                {
                    DebugShape s = activeShapes[i];

                    if (s.Lifetime <= 0)
                    {
                        cachedShapes.Add(s);
                        activeShapes.RemoveAt(i);
                        resort = true;
                    }
                }

                if (resort)
                    cachedShapes.Sort(CachedShapesSort);
            }
        }
        
        private static void InitializeSphere()
        {
            // We need two vertices per line, so we can allocate our vertices
            unitSphere = new Vector3[sphereLineCount * 2];

            // Compute our step around each circle
            float step = MathHelper.TwoPi / sphereResolution;

            // Used to track the index into our vertex array
            int index = 0;

            // Create the loop on the XY plane first
            for (float a = 0f; a < MathHelper.TwoPi; a += step)
            {
                unitSphere[index++] = new Vector3((float)Math.Cos(a), (float)Math.Sin(a), 0f);
                unitSphere[index++] = new Vector3((float)Math.Cos(a + step), (float)Math.Sin(a + step), 0f);
            }

            // Next on the XZ plane
            for (float a = 0f; a < MathHelper.TwoPi; a += step)
            {
                unitSphere[index++] = new Vector3((float)Math.Cos(a), 0f, (float)Math.Sin(a));
                unitSphere[index++] = new Vector3((float)Math.Cos(a + step), 0f, (float)Math.Sin(a + step));
            }

            // Finally on the YZ plane
            for (float a = 0f; a < MathHelper.TwoPi; a += step)
            {
                unitSphere[index++] = new Vector3(0f, (float)Math.Cos(a), (float)Math.Sin(a));
                unitSphere[index++] = new Vector3(0f, (float)Math.Cos(a + step), (float)Math.Sin(a + step));
            }
        }

        private static int CachedShapesSort(DebugShape s1, DebugShape s2)
        {
            return s1.Vertices.Length.CompareTo(s2.Vertices.Length);
        }

        private static DebugShape GetShapeForLines(int lineCount, float life)
        {
            DebugShape shape = null;

            // We go through our cached list trying to find a shape that contains
            // a large enough array to hold our desired line count. If we find such
            // a shape, we move it from our cached list to our active list and break
            // out of the loop.
            int vertCount = lineCount * 2;
            for (int i = 0; i < cachedShapes.Count; i++)
            {
                if (cachedShapes[i].Vertices.Length >= vertCount)
                {
                    shape = cachedShapes[i];
                    cachedShapes.RemoveAt(i);
                    activeShapes.Add(shape);
                    break;
                }
            }

            // If we didn't find a shape in our cache, we create a new shape and add it
            // to the active list.
            if (shape == null)
            {
                shape = new DebugShape { Vertices = new VertexPositionColor[vertCount] };
                activeShapes.Add(shape);
            }

            // Set the line count and lifetime of the shape based on our parameters.
            shape.LineCount = lineCount;
            shape.Lifetime = life;

            return shape;
        }
	}
}
