using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace s00130744Graphics.Graphics
{
    public class AssignmentSpotLight : AssignmentCustomEffectModel
    {
        public class AssignmentLamberSpotLightMaterial : Material
        {
            public Vector3 AmbientLightColor { get; set; }
            public Vector3 LightPosition { get; set; }
            public Vector3 LightColor { get; set; }

            public Vector3 LightDirection { get; set; }

            public float ConeAngle { get; set; }
            public float LightFallOff { get; set; }
            public Texture2D Texture { get; set; }
            public Vector3 DiffuseColor { get; set; }

            public AssignmentSpotLight(string id, string asset, Vector3 position) : base(id, asset, position)
            {

            }

            public AssignmentSpotLight(Color ambientColor, Color lightColor, Vector3 lightPosition, float coneAngle, float fallOff)
            {

            }

            public override void SetEffectParameters(Effect effect)
            {
                if (effect.Parameters["AmbientLightColor"] != null)
                    effect.Parameters["AmbientLightColor"].SetValue(AmbientLightColor);

                if (effect.Parameters["LightPosition"] != null)
                    effect.Parameters["LightPosition"].SetValue(LightPosition);

                if (effect.Parameters["LightColor"] != null)
                    effect.Parameters["LightColor"].SetValue(LightColor);

                if (effect.Parameters["LightDirection"] != null)
                    effect.Parameters["LightDirection"].SetValue(LightDirection);

                if (effect.Parameters["ConeAngle"] != null)
                    effect.Parameters["ConeAngle"].SetValue(ConeAngle);

                if (effect.Parameters["LightFallOff"] != null)
                    effect.Parameters["LightFallOff"].SetValue(LightFallOff);

                if (effect.Parameters["Texture"] != null)
                    effect.Parameters["Texture"].SetValue(Texture);

                if (effect.Parameters["DiffuseColor"] != null)
                    effect.Parameters["DiffuseColor"].SetValue(DiffuseColor);
            }

            public override void LoadContent()
            {
                Material = new AssignmentLamberSpotLightMaterial();

                AssignmentCustomEffect = GameUtilities.Content.Load<Effect>("Effects\\SpotLight");

                base.LoadContent();
            }

            public override void Update()
            {
                Material.Update();

                //World += Matrix.CreateRotationX(0.1f);

                base.Update();
            }
        }
    }
}
