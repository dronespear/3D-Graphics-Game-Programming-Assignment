using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using s00130744Graphics.Graphics;
using Sample;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace s00130744Graphics
{
    public class Game1 : Game
    {
        #region variables List

        GraphicsDeviceManager graphics;

        InputEngine input;
        DebugEngine debug;
        ImmediateShapeDrawer shapeDrawer;

        List<AssignmentCustomEffectModel> gameObjects = new List<AssignmentCustomEffectModel>();
        Camera mainCamera;

        SpriteBatch spriteBatch;
        SpriteFont sfont;

        Effect captureDepthAndNormalsEffect;
        Effect depthNormalEffect;
        RenderTarget2D depthTarget;
        RenderTarget2D normalsTarget;

        RenderTarget2D lightTarget;
        List<Material> Lights = new List<Material>();
        Model PointLightMesh;
        Effect lightMapEffect;

        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 768;
            ////////graphics.GraphicsProfile = GraphicsProfile.HiDef;
            graphics.ApplyChanges();

            input = new InputEngine(this);
            debug = new DebugEngine();
            shapeDrawer = new ImmediateShapeDrawer();

            IsMouseVisible = true;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            GameUtilities.Content = Content;
            GameUtilities.GraphicsDevice = GraphicsDevice;

            debug.Initialize();
            shapeDrawer.Initialize();

            mainCamera = new Camera("cam",new Vector3(0, 5, 10), new Vector3(0,0,-1));
            mainCamera.Initialize();

            base.Initialize();
        }

        RenderTarget2D renderTarget;
        protected override void LoadContent()
        {
            
            //////////renderTarget = new RenderTarget2D(GraphicsDevice, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            ////////////or
            //////////renderTarget = new RenderTarget2D(GraphicsDevice, graphics.PreferredBackBufferWidth = 800, graphics.PreferredBackBufferHeight = 640);
            ////////////or
            //////////renderTarget = new RenderTarget2D(GraphicsDevice, graphics.PreferredBackBufferWidth*8, graphics.PreferredBackBufferHeight*6);

            sfont = Content.Load<SpriteFont>("debug");
            spriteBatch = new SpriteBatch(GraphicsDevice);


            //House Model used here
            //gameObjects.Add(new AssignmentDirectionalLight("ball0", "house", new Vector3(0, 0, 0)));
            //gameObjects.Add(new AssignmentPointLight("ball0", "house", new Vector3(0, 0, 0)));
            //gameObjects.Add(new AssignmentPointLightUpdateClass("ball0", "house", new Vector3(0, 0, 0)));

            //Assignmentscene(Quick Model Created) Model used here
            //gameObjects.Add(new AssignmentDirectionalLight("ball1", "Assignmentscene", new Vector3(0, 0, 0)));
            gameObjects.Add(new AssignmentPointLight("ball1", "Assignmentscene", new Vector3(0, 0, 0)));
            //gameObjects.Add(new AssignmentPointLightUpdateClass("ball1", "Assignmentscene", new Vector3(0, 0, 0)));

            depthNormalEffect = Content.Load<Effect>("Effects\\CaptureDepthAndNormals");

            lightMapEffect = Content.Load<Effect>("Effects\\LightMap");

            PointLightMesh = Content.Load<Model>("Effects\\LightMesh");
            PointLightMesh.Meshes[0].MeshParts[0].Effect = lightMapEffect;

            

            gameObjects.ForEach(go => go.LoadContent());


            
            

            //depth and normal
            captureDepthAndNormalsEffect = Content.Load<Effect>("Effect\\captureDepthAndNormals");

            normalsTarget = new RenderTarget2D(
                GraphicsDevice,
                GraphicsDevice.PresentationParameters.BackBufferWidth,
                GraphicsDevice.PresentationParameters.BackBufferHeight,
                false,
                SurfaceFormat.Color,
                DepthFormat.Depth24);

            depthTarget = new RenderTarget2D(
               GraphicsDevice,
               GraphicsDevice.PresentationParameters.BackBufferWidth,
               GraphicsDevice.PresentationParameters.BackBufferHeight,
               false,
               SurfaceFormat.Single,
               DepthFormat.Depth24);

            lightTarget = new RenderTarget2D(GraphicsDevice,
                GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height,
                false,
                SurfaceFormat.Color,
                DepthFormat.Depth24);

            #region Lights Creation and Update
            Lights.Add(new AssignmentPointLight.AssignmentLambertPointLightMaterial()
            {
                AmbientColor = Color.DimGray.ToVector3(),
                LightPosition = new Vector3(70, 10, -50),
                LightColor = Color.Aquamarine.ToVector3(),
                LightAttenuation = 56,
                LightFallOff = 2,
            });

            Lights.ForEach(i => i.Update());

            foreach (var i in Lights.OfType<AssignmentPointLight.AssignmentLambertPointLightMaterial>())
            {
                DebugEngine.AddBoundingSphere(
                    new BoundingSphere(
                        i.LightPosition,
                        0.5f),
                    Color.White);
            }
            #endregion

        }

        public void DrawDepthAndNormalMaps(Camera camera)
        {
            GraphicsDevice.SetRenderTargets(normalsTarget, depthTarget);
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;

            GraphicsDevice.Clear(Color.White);

            foreach (var model in gameObjects)
            {
                model.CacheEffect();

                model.SetModelEffect(captureDepthAndNormalsEffect, true);
                model.Draw(camera);

                model.RestoreEffect();
            }
            GraphicsDevice.SetRenderTarget(null);
        }


        public void DrawDepthAndNormalMaps(Camera camera, AssignmentPointLight model)
        {
            GraphicsDevice.SetRenderTargets(normalsTarget, depthTarget);
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;

            GraphicsDevice.Clear(Color.White);

            foreach (var mod in gameObjects)
            {
                mod.CacheEffect();

                mod.SetModelEffect(depthNormalEffect, true);
                mod.Draw(camera);

                mod.RestoreEffect();
            }
            GraphicsDevice.SetRenderTarget(null);
        }

        public void DrawLightMap(Camera _camera)
        {
            lightMapEffect.Parameters["DepthTexture"].SetValue(depthTarget);
            lightMapEffect.Parameters["NormalTexture"].SetValue(normalsTarget);

            var viewProjection = _camera.View * _camera.Projection;
            var inverseViewProjection = Matrix.Invert(viewProjection);
            lightMapEffect.Parameters["InverseViewProjection"].SetValue(inverseViewProjection);

            GraphicsDevice.SetRenderTarget(lightTarget);

            GraphicsDevice.Clear(Color.Black);
            GraphicsDevice.BlendState = BlendState.Additive;
            GraphicsDevice.DepthStencilState = DepthStencilState.None;

            foreach(var pointLight in Lights.OfType<AssignmentPointLight.AssignmentLambertPointLightMaterial>())
            {
                pointLight.setEffectParameters(lightMapEffect);

                var wvp = (Matrix.CreateScale(pointLight.LightAttenuation)
                    * Matrix.CreateTranslation(pointLight.LightPosition)) * viewProjection;

                lightMapEffect.Parameters["WorldViewProjection"].SetValue(wvp);

                float dist = Vector3.Distance(_camera.World.Translation, pointLight.LightPosition);

                if (dist < pointLight.LightAttenuation)
                    GraphicsDevice.RasterizerState = RasterizerState.CullClockwise;

                PointLightMesh.Meshes[0].Draw();

                GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            }

            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SetRenderTarget(null);

        }

        public void PrepareMainPass(Camera _camera)
        {
            foreach (var model in gameObjects)
            {
                if (model.Model != null)
                {
                    foreach (var mesh in model.Model.Meshes)
                    {
                        foreach (var part in mesh.MeshParts)
                        {
                            model.SetEffectParameter(part.Effect, "LightTexture", lightTarget);
                            model.SetEffectParameter(part.Effect, "ViewportWidth", (float)GraphicsDevice.Viewport.Width);
                            model.SetEffectParameter(part.Effect, "ViewportHeight", (float)GraphicsDevice.Viewport.Height);
                        }
                    }
                }
            }
        }

        protected override void UnloadContent()
        {

        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            ////////if (InputEngine.IsKeyPressed(Keys.P))
            ////////{
            ////////    SaveScreenShot();
            ////////}
 

            GameUtilities.Time = gameTime;

            if (InputEngine.IsKeyHeld(Keys.Escape))
                Exit();

            mainCamera.Update();

            gameObjects.ForEach(go => go.Update());

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            DrawDepthAndNormalMaps(mainCamera);
            DrawLightMap(mainCamera);
            PrepareMainPass(mainCamera);

            GraphicsDevice.Clear(Color.CornflowerBlue);

            foreach(var go in gameObjects)
            {
                if(mainCamera.Frustum.Contains(go.AABB) != ContainmentType.Disjoint)
                {
                    go.Draw(mainCamera);
                }
            }
            
            debug.Draw(mainCamera);

            spriteBatch.Begin();
            spriteBatch.Draw(normalsTarget, new Rectangle(10, 10, 400, 200), Color.White);
            spriteBatch.Draw(depthTarget, new Rectangle(435, 10, 400, 200), Color.White);
            spriteBatch.Draw(lightTarget, new Rectangle(860, 10, 400, 200), Color.White);
            spriteBatch.End();

            GameUtilities.SetGraphicsDeviceFor3D();

            base.Draw(gameTime);
        }


        ////////private void SaveScreenShot(string path, RenderTarget2D target)
        ////////{
        ////////    using (var stream = File.Create(path + "png"))
        ////////    {
        ////////        target.SaveAsPing(stream, target.width,);
        ////////    }
        ////////}


        //public bool DoesFrustumContain(SimpleModel model)
        //{
        //    if (mainCamera.Frustum.Contains(model.AABB) != ContainmentType.Disjoint)
        //        return true;
        //    else return false;
        //}

        //public bool IsOccluded(SimpleModel model)
        //{
        //    //timer.Begin();
        //    occQuery.Begin();
        //    shapeDrawer.DrawBoundingBox(model.AABB, mainCamera);
        //    occQuery.End();
        //    //timer.End();
        //    //timer += millseconds

        //    while (!occQuery.IsComplete) { }

        //    if (occQuery.IsComplete && occQuery.PixelCount > 0)
        //        return true;
        //    else return false;
        //}

        //stage 1 save data

        //public void DrawDepthAndNormalMaps(Camera camera, CustomEffectModel model)
        //{
        //    //set the render target
        //    GraphicsDevice.SetRenderTargets(normalTarget, depthTarget);
        //    GraphicsDevice.RasterizerState.CullMode = CullMode.None;
        //    //clear the previous data
        //    GraphicsDevice.Clear(Color.White);
        //    //set the effect on the model
        //    foreach (var model in gameObjects)
        //    {
        //        model.CacheEffect();

        //        model.SetModelEffect(captureDepthAndNormalsEffect, true);
        //        //draw the target
        //        model.Draw(camera);
        //        //retore effect
        //        model.RestoreEffect();
        //    }
        //    //set to draw  to the back buffer
        //    GraphicsDevice.SetRenderTarget(null);          

        //}
    }
}
