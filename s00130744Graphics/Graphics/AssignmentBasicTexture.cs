using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace s00130744Graphics.Graphics
{
    public class AssignmentBasicTexture : AssignmentCustomEffectModel
    {
        public class AssignmentBasicTextureMaterial : Material
        {
            public Vector3 Color { get; set; }
            public Texture2D Texture { get; set; }

            public override void SetEffectParameters(Effect effect)
            {
                if (effect.Parameters["Color"] != null)
                    effect.Parameters["Color"].SetValue(Color);

                if (effect.Parameters["Texture"] != null)
                    effect.Parameters["Texture"].SetValue(Texture);
            }
        }

        private Random random = new Random();
        public float elapsed = 0;


        public AssignmentBasicTexture(string id, string asset, Vector3 position) : base(id, asset, position)
        {

        }

        public override void LoadContent()
        {
            Material = new AssignmentBasicTextureMaterial()
            {
                Color = Color.Green.ToVector3(),
                Texture = GameUtilities.Content.Load<Texture2D>("Textures\\sand")
            };

            AssignmentCustomEffect = GameUtilities.Content.Load<Effect>("Effects\\BasicTextureEffect");

            base.LoadContent();
        }
    }
}
