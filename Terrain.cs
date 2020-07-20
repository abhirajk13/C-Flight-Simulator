﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Glider_Simulation_V1
{
    class Terrain
    {
        #region Fields
        private VertexBuffer vertexBuffer;
        private IndexBuffer indexBuffer;

        private GraphicsDevice device;
        private Texture2D terrainTexture;
        private float textureScale;
        private float[,] heights;

        private Texture2D terrainTexture2;
        private Texture2D terrainTexture3;
        private float maxHeight;
        #endregion

        #region Constructor
        public Terrain(
            GraphicsDevice graphicsDevice,
            Texture2D heightMap,
            Texture2D terrainTexture,
            Texture2D terrainTexture2,
            Texture2D terrainTexture3,
            float textureScale,
            int terrainWidth,
            int terrainHeight,
            float heightScale)
        {
            device = graphicsDevice;
            this.terrainTexture = terrainTexture;
            this.terrainTexture2 = terrainTexture2;
            this.terrainTexture3 = terrainTexture3;
            maxHeight = heightScale;

            this.textureScale = textureScale;

            ReadHeightMap(
                heightMap,
                terrainWidth,
                terrainHeight,
                heightScale);

            BuildVertexBuffer(
                terrainWidth,
                terrainHeight,
                heightScale);

            BuildIndexBuffer(
                terrainWidth,
                terrainHeight);

            CalculateNormals();
        }
        #endregion

        #region Height Map
        private void ReadHeightMap(
            Texture2D heightMap,
            int terrainWidth,
            int terrainHeight,
            float heightScale)
        {
            float min = float.MaxValue;
            float max = float.MinValue;

            heights = new float[terrainWidth, terrainHeight];

            Color[] heightMapData = new Color[
                heightMap.Width * heightMap.Height];

            heightMap.GetData(heightMapData);
            for (int x = 0; x < terrainWidth; x++)
                for (int z = 0; z < terrainHeight; z++)
                {
                    int height = heightMapData[x + z * terrainWidth].R;
                    heights[x, z] = (float)height / 255f;

                    max = MathHelper.Max(max, heights[x, z]);
                    min = MathHelper.Min(min, heights[x, z]);
                }

            float range = (max - min);

            for (int x = 0; x < terrainWidth; x++)
                for (int z = 0; z < terrainHeight; z++)
                {
                    heights[x, z] =
                        ((heights[x, z] - min) / range) * heightScale;
                }
        }
        #endregion

        #region Vertex Buffer
        private void BuildVertexBuffer(
            int width,
            int height,
            float heightScale)
        {
            VertexPositionNormalTexture[] vertices =
                new VertexPositionNormalTexture[width * height];

            for (int x = 0; x < width; x++)
                for (int z = 0; z < height; z++)
                {
                    vertices[x + (z * width)].Position =
                      new Vector3(x, heights[x, z], z);
                    vertices[x + (z * width)].TextureCoordinate =
                        new Vector2((float)x / textureScale, (float)z / textureScale);
                }

            vertexBuffer = new VertexBuffer(
                device,
                typeof(VertexPositionNormalTexture),
                vertices.Length,
                BufferUsage.None);

            vertexBuffer.SetData(vertices);
        }
        #endregion

        #region Index Buffer
        private void BuildIndexBuffer(int width, int height)
        {
            int indexCount = (width - 1) * (height - 1) * 6;
            short[] indices = new short[indexCount];
            int counter = 0;

            for (short z = 0; z < height - 1; z++)
                for (short x = 0; x < height - 1; x++)
                {
                    short upperLeft = (short)(x + (z * width));
                    short upperRight = (short)(upperLeft + 1);
                    short lowerLeft = (short)(upperLeft + width);
                    short lowerRight = (short)(upperLeft + width + 1);

                    indices[counter++] = upperLeft;
                    indices[counter++] = lowerRight;
                    indices[counter++] = lowerLeft;
                    indices[counter++] = upperLeft;
                    indices[counter++] = upperRight;
                    indices[counter++] = lowerRight;
                }

            indexBuffer = new IndexBuffer(
                device,
                IndexElementSize.SixteenBits,
                indices.Length,
                BufferUsage.None);
            indexBuffer.SetData(indices);
        }
        #endregion

        #region Draw
        public void Draw(
            Camera camera,
            Effect effect)
        {
            effect.CurrentTechnique = effect.Techniques["Technique1"];
            effect.Parameters["LowTexture"].SetValue(terrainTexture);
            effect.Parameters["World"].SetValue(Matrix.Identity);
            effect.Parameters["View"].SetValue(camera.View);
            effect.Parameters["Projection"].SetValue(camera.Projection);

            Vector3 lightDirection = new Vector3(-1f, 1f, -1f);
            lightDirection.Normalize();
            effect.Parameters["DirectionOfLight"].SetValue(lightDirection);
            effect.Parameters["ColourOfLight"].SetValue(new Vector4(1, 1, 1, 1));
            effect.Parameters["Brightness"].SetValue(0.8f);

            effect.Parameters["ambienceLightLevel"].SetValue(0.15f);
            effect.Parameters["ambienceLightColour"].SetValue(
                new Vector4(1, 1, 1, 1));

            effect.Parameters["MiddleTexture"].SetValue(terrainTexture2);
            effect.Parameters["HighTexture"].SetValue(terrainTexture3);
            effect.Parameters["maximumElevation"].SetValue(maxHeight);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.SetVertexBuffer(vertexBuffer);
                device.Indices = indexBuffer;
                device.DrawIndexedPrimitives(
                    PrimitiveType.TriangleList,
                    0,
                    0,
                    vertexBuffer.VertexCount,
                    0,
                    indexBuffer.IndexCount / 3);
            }
        }
        #endregion

        #region Helper Methods
        public float GetHeight(float x, float z)
        {
            int xmin = (int)Math.Floor(x);
            int xmax = xmin + 1;
            int zmin = (int)Math.Floor(z);
            int zmax = zmin + 1;

            if (
                (xmin < 0) || (zmin < 0) ||
                (xmax > heights.GetUpperBound(0)) ||
                (zmax > heights.GetUpperBound(1)))
            {
                return 0;
            }

            Vector3 p1 = new Vector3(xmin, heights[xmin, zmax], zmax);
            Vector3 p2 = new Vector3(xmax, heights[xmax, zmin], zmin);
            Vector3 p3;

            if ((x - xmin) + (z - zmin) <= 1)
            {
                p3 = new Vector3(xmin, heights[xmin, zmin], zmin);
            }
            else
            {
                p3 = new Vector3(xmax, heights[xmax, zmax], zmax);
            }

            Plane plane = new Plane(p1, p2, p3);

            Ray ray = new Ray(new Vector3(x, 0, z), Vector3.Up);

            float? height = ray.Intersects(plane);

            return height.HasValue ? height.Value : 0f;
        }

        private void CalculateNormals()
        {
            VertexPositionNormalTexture[] vertices =
                new VertexPositionNormalTexture[vertexBuffer.VertexCount];
            short[] indices = new short[indexBuffer.IndexCount];

            vertexBuffer.GetData(vertices);
            indexBuffer.GetData(indices);
            int v1, v2, v3 = 0;

            for (int x = 0; x < vertices.Length; x++)
                vertices[x].Normal = Vector3.Zero;

            int triangleCount = indices.Length / 3;

            for (int x = 0; x < triangleCount; x++)
            {

                v1 = indices[x * 3];
                v2 = indices[(x * 3) + 1];
                v3 = indices[(x * 3) + 2];

                Vector3 firstSide =
                    vertices[v2].Position - vertices[v1].Position;
                Vector3 secondSide =
                    vertices[v1].Position - vertices[v3].Position;
                Vector3 triangleNormal =
                    Vector3.Cross(firstSide, secondSide);
                triangleNormal.Normalize();

                vertices[v1].Normal += triangleNormal;
                vertices[v2].Normal += triangleNormal;
                vertices[v3].Normal += triangleNormal;
            }

            for (int x = 0; x < vertices.Length; x++)
                vertices[x].Normal.Normalize();

            vertexBuffer.SetData(vertices);
        }

        #endregion

    }
}
