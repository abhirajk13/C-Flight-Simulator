using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Glider_Simulation_V1

{
    class Glider
    {

        private Model model;
        private GraphicsDevice device;

        private Vector3 position;

        private Matrix[] boneTransforms;



        public Vector3 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }



        #region Constructor
        public Glider(GraphicsDevice device, Model model, Vector3 position)
        {
            this.device = device;
            this.model = model;
            Position = position;
            boneTransforms = new Matrix[model.Bones.Count];


        }
        #endregion

        #region Draw
        public void Draw(Camera camera)
        {
            model.Root.Transform = Matrix.CreateRotationY(Game1.angle2) * Matrix.Identity *
                Matrix.CreateScale(0.002f) *
                Matrix.CreateTranslation(Position);   // * Matrix.CreateRotationZ(MathHelper.ToRadians(30)); // change the roll angle

            model.CopyAbsoluteBoneTransformsTo(boneTransforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect basicEffect in mesh.Effects)
                {
                    basicEffect.World = boneTransforms[mesh.ParentBone.Index];
                    basicEffect.View = camera.View;
                    basicEffect.Projection = camera.Projection;

                    basicEffect.EnableDefaultLighting();
                }

                mesh.Draw();
            }
        }
        #endregion

    }
}
