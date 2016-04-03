using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Sample
{
    public class SimpleModel : GameObject3D
    {
        public Model Model { get; set; }
        public Matrix[] BoneTransforms { get; set; }
       protected string _asset;

        public BoundingBox AABB { get; set; }

        public SimpleModel(string id, string asset, Vector3 position)
            : base(id, position)
        {
            _asset = asset;
        }

        public override void LoadContent()
        {
            if (!string.IsNullOrEmpty(_asset))
            {
                Model = GameUtilities.Content.Load<Model>("Models\\" + _asset);

                BoneTransforms = new Matrix[Model.Bones.Count];
                Model.CopyAbsoluteBoneTransformsTo(BoneTransforms);

                if (Model != null)
                {
                    Vector3[] vertices;
                    int[] indices;

                    ModelDataExtractor.GetVerticesAndIndicesFromModel(Model, out vertices, out indices);

                    AABB = BoundingBox.CreateFromPoints(vertices);
                    TransformBoundingBox(World);
                }
            }

            base.LoadContent();
        }

        public void TransformBoundingBox(Matrix matrix)
        {
            Vector3 origCorner1 = AABB.Min;
            Vector3 origCorner2 = AABB.Max;

            Vector3 transCorner1 = Vector3.Transform(origCorner1, matrix);
            Vector3 transCorner2 = Vector3.Transform(origCorner2, matrix);

            AABB = new BoundingBox(transCorner1, transCorner2);
        }

        public override void Update()
        {
            //if (AABB != null)
            //    DebugEngine.AddBoundingBox(AABB, Color.Red);

            base.Update();
        }
        public override void Draw(Camera camera)
        {
            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    if (part.Effect is BasicEffect)
                    {
                        (part.Effect as BasicEffect).View = camera.View;
                        (part.Effect as BasicEffect).Projection = camera.Projection;
                        (part.Effect as BasicEffect).World = BoneTransforms[mesh.ParentBone.Index] * World;
                    }

                    mesh.Draw();
                }

                base.Draw(camera);
            }
        }
    }
}


/*struct VectexShaderOutput
{
--------------------
    float UV : TEXCOORD0
};
*/