using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace s00130744Graphics.Graphics
{
    public class AssignmentDirectionalLight : AssignmentCustomEffectModel
    {
        public class AssignmentLambertDirectionalLightMaterial : Material
        {
            public Vector3 AmbientColor { get; set; }
            public Vector3 LightDirection { get; set; }
            public Vector3 LightColor { get; set; }
            public Texture2D Texture { get; set; }
            public bool TextureEnabled { get; set; }
            public Vector3 DiffuseColor { get; set; }

            public AssignmentLambertDirectionalLightMaterial()
            {
                //Texture = GameUtilities.Content.Load<Texture2D>("Textures\\sand");
                //Texture = GameUtilities.Content.Load<Texture2D>("Textures\\White");
                AmbientColor = Color.Green.ToVector3();
                LightDirection = new Vector3(1, 1, 0);
                LightColor = Color.White.ToVector3();
                DiffuseColor = Color.Purple.ToVector3();
                //TextureEnabled = true;
            }

            public override void SetEffectParameters(Effect effect)
            {
                if (effect.Parameters["AmbientColor"] != null)
                    effect.Parameters["AmbientColor"].SetValue(AmbientColor);

                if (effect.Parameters["LightDirection"] != null)
                    effect.Parameters["LightDirection"].SetValue(LightDirection);

                if (effect.Parameters["LightColor"] != null)
                    effect.Parameters["LightColor"].SetValue(LightColor);

                //if (effect.Parameters["Texture"] != null)
                //    effect.Parameters["Texture"].SetValue(Texture);

                //if (effect.Parameters["TextureEnabled"] != null)
                //    effect.Parameters["TextureEnabled"].SetValue(TextureEnabled);

                //if (effect.Parameters["DiffuseColor"] != null)
                //    effect.Parameters["DiffuseColor"].SetValue(DiffuseColor);
            }
        }

        public AssignmentDirectionalLight(string id, string asset, Vector3 position) : base(id, asset, position)
        {
        }

        public override void LoadContent()
        {
            Material = new AssignmentLambertDirectionalLightMaterial();
            //{
            //    Texture = GameUtilities.Content.Load<Texture2D>("Textures\\sand"),
            //    //LightColor = Color.White.ToVector3(),
            //    //LightDirection = new Vector3(1, 1, 0),
            //    //AmbientColor = Color.Green.ToVector3()
            //};

            AssignmentCustomEffect = GameUtilities.Content.Load<Effect>("Effects\\DirectionLight");

            base.LoadContent();
        }

        public override void Update()
        {
            //not work even if it has LightAttenuation variable in class so removed LightAttenuationvariable
            //if (InputEngine.IsKeyHeld(Microsoft.Xna.Framework.Input.Keys.Q))
            //{
            //    LightAttenuation += 1;
            //}

            //if (InputEngine.IsKeyHeld(Microsoft.Xna.Framework.Input.Keys.E))
            //{
            //    LightAttenuation -= 1;
            //}

            base.Update();
        }
    }
}
