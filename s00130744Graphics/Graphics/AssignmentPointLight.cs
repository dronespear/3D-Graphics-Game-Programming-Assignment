using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace s00130744Graphics.Graphics
{
    public class AssignmentPointLight : AssignmentCustomEffectModel
    {
        public class AssignmentLambertPointLightMaterial : Material
        {
            public Vector3 AmbientColor { get; set; }
            public Vector3 LightPosition { get; set; }
            public Vector3 LightColor { get; set; }

            public float LightAttenuation { get; set; }
            public float LightFallOff { get; set; }

            public Vector3 DiffuseColor { get; set; }

            public AssignmentLambertPointLightMaterial()
            {
                AmbientColor = Color.Gray.ToVector3();
                LightPosition = new Vector3(1, 10, 20);
                LightColor = Color.White.ToVector3();
                DiffuseColor = Color.White.ToVector3();
                LightAttenuation = 54;
                LightFallOff = 2;
            }

            public override void Update()
            {

                //if (InputEngine.IsKeyHeld(Microsoft.Xna.Framework.Input.Keys.Left))
                //{
                //    LightPosition += new Vector3(-1, 0, 0);
                //}

                //if (InputEngine.IsKeyHeld(Microsoft.Xna.Framework.Input.Keys.Right))
                //{
                //    LightPosition += new Vector3(1, 0, 0);
                //}

                //if (InputEngine.IsKeyHeld(Microsoft.Xna.Framework.Input.Keys.Up))
                //{
                //    LightPosition += new Vector3(0, 1, 0);
                //}

                //if (InputEngine.IsKeyHeld(Microsoft.Xna.Framework.Input.Keys.Down))
                //{
                //    LightPosition += new Vector3(0, -1, 0);
                //}

                //if (InputEngine.IsKeyHeld(Microsoft.Xna.Framework.Input.Keys.K))
                //{
                //    LightPosition += new Vector3(0, 0, 1);
                //}

                //if (InputEngine.IsKeyHeld(Microsoft.Xna.Framework.Input.Keys.L))
                //{
                //    LightPosition += new Vector3(0, 0, -1);
                //}

                ////

                if (InputEngine.IsKeyHeld(Microsoft.Xna.Framework.Input.Keys.Left))
                {
                    LightPosition += Vector3.Left * 6;
                }

                if (InputEngine.IsKeyHeld(Microsoft.Xna.Framework.Input.Keys.Right))
                {
                    LightPosition += Vector3.Right * 6;
                }

                if (InputEngine.IsKeyHeld(Microsoft.Xna.Framework.Input.Keys.Up))
                {
                    LightPosition += Vector3.Up * 6;
                }

                if (InputEngine.IsKeyHeld(Microsoft.Xna.Framework.Input.Keys.Down))
                {
                    LightPosition += Vector3.Down * 6;
                }

                if (InputEngine.IsKeyHeld(Microsoft.Xna.Framework.Input.Keys.K))
                {
                    LightPosition += Vector3.UnitZ * 6;
                }

                if (InputEngine.IsKeyHeld(Microsoft.Xna.Framework.Input.Keys.L))
                {
                    LightPosition -= Vector3.UnitZ * 6;
                }

                if (InputEngine.IsKeyHeld(Microsoft.Xna.Framework.Input.Keys.Z))
                {
                    LightAttenuation += 1;
                }

                if (InputEngine.IsKeyHeld(Microsoft.Xna.Framework.Input.Keys.X))
                {
                    LightAttenuation -= 1;
                }

                DebugEngine.AddBoundingSphere(new BoundingSphere(LightPosition, 2), Color.Red);
                base.Update();
            }

            public override void SetEffectParameters(Effect effect)
            {
                if (effect.Parameters["AmbientColor"] != null)
                    effect.Parameters["AmbientColor"].SetValue(AmbientColor);

                if (effect.Parameters["LightPosition"] != null)
                    effect.Parameters["LightPosition"].SetValue(LightPosition);

                if (effect.Parameters["LightColor"] != null)
                    effect.Parameters["LightColor"].SetValue(LightColor);

                if (effect.Parameters["LightAttenuation"] != null)
                    effect.Parameters["LightAttenuation"].SetValue(LightAttenuation);

                if (effect.Parameters["LightFallOff"] != null)
                    effect.Parameters["LightFallOff"].SetValue(LightFallOff);

                if (effect.Parameters["DiffuseColor"] != null)
                    effect.Parameters["DiffuseColor"].SetValue(DiffuseColor);
            }
        }

        public AssignmentPointLight(string id, string asset, Vector3 position) : base(id, asset, position)
        {

        }

        public override void LoadContent()
        {
            Material = new AssignmentLambertPointLightMaterial();

            AssignmentCustomEffect = GameUtilities.Content.Load<Effect>("Effects\\PointLight");
                        
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
