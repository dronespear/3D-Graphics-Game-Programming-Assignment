using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace s00130744Graphics.Graphics
{
    public class AssignmentCustomEffectModel : SimpleModel
    {
        public Effect AssignmentCustomEffect { get; set; }

        public AssignmentCustomEffectModel(string id, string asset, Vector3 position) : base(id, asset, position)
        {

        }

        public Material Material { get; set; }
        public override void LoadContent()
        {
            base.LoadContent();
            if (Model != null)
            {
                GenerateMeshTag();

                if (AssignmentCustomEffect != null)
                    foreach (var mesh in Model.Meshes)
                        foreach (var part in mesh.MeshParts)
                            part.Effect = AssignmentCustomEffect;
            }
        }

        public override void Draw(Camera camera)
        {
            if (AssignmentCustomEffect != null)
            {
                SetModelEffect(AssignmentCustomEffect, true);

                foreach (ModelMesh mesh in Model.Meshes)
                {
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        SetEffectParameter(part.Effect, "World", BoneTransforms[mesh.ParentBone.Index] * World);
                        SetEffectParameter(part.Effect, "View", camera.View);
                        SetEffectParameter(part.Effect, "Projection", camera.Projection);

                        if (Material != null)
                            Material.SetEffectParameters(part.Effect);
                    }
                    mesh.Draw();
                }
            }
            else
            {
                base.Draw(camera);
            }
        }

        public void GenerateMeshTag()
        {
            foreach (ModelMesh mesh in Model.Meshes)
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    MeshTag tag = new MeshTag();

                    if (part.Effect is BasicEffect)
                    {
                        tag.Color = (part.Effect as BasicEffect).DiffuseColor;
                        tag.Texture = (part.Effect as BasicEffect).Texture;
                        tag.SpecularPower = (part.Effect as BasicEffect).SpecularPower;

                        part.Tag = tag;
                    }
                }
        }

        public void CacheEffect()
        {
            foreach (ModelMesh mesh in Model.Meshes)
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    (part.Tag as MeshTag).CahcedEffect = part.Effect;
                }
        }

        public void RestoreEffect()
        {
            foreach (ModelMesh mesh in Model.Meshes)
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    if (part.Tag != null)
                    {
                        if ((part.Tag as MeshTag).CahcedEffect != null)
                            part.Effect = (part.Tag as MeshTag).CahcedEffect;
                    }
                }
        }

        public virtual void SetEffectParameter(Effect effect, string paramName, object value)
        {
            if (effect.Parameters[paramName] == null)
                return;

            if (value is Vector3)
            {
                effect.Parameters[paramName].SetValue((Vector3)value);
            }

            else if (value is Matrix)
            {
                effect.Parameters[paramName].SetValue((Matrix)value);
            }

            else if (value is bool)
            {
                effect.Parameters[paramName].SetValue((bool)value);
            }

            else if (value is Texture2D)
            {
                effect.Parameters[paramName].SetValue((Texture2D)value);
            }

            else if (value is float)
            {
                effect.Parameters[paramName].SetValue((float)value);
            }

            else if (value is int)
            {
                effect.Parameters[paramName].SetValue((int)value);
            }
        }

        public virtual void SetModelEffect(Effect effect, bool copyEffect)
        {
            foreach (ModelMesh mesh in Model.Meshes)
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    Effect toBeSet = effect;

                    if (copyEffect)
                    {
                        toBeSet = effect.Clone();
                    }

                    var tag = (part.Tag as MeshTag);

                    if (tag.Texture != null)
                    {
                        SetEffectParameter(toBeSet, "Texture", tag.Texture);
                        SetEffectParameter(toBeSet, "TextureEnabled", true);
                    }
                    else
                        SetEffectParameter(toBeSet, "TextureEnabled", false);

                    SetEffectParameter(toBeSet, "Color", tag.Color);
                    SetEffectParameter(toBeSet, "SpecularPower", tag.SpecularPower);

                    part.Effect = toBeSet;

                }
        }
    }
}
