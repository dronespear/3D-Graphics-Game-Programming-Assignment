using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Sample
{
    public class Camera : GameObject3D
    {
        public Vector3 CameraDirection { get; set; }

        protected Vector3 CurrentTarget;

        protected Matrix view;
        protected Matrix projection;
        private Vector3 UpVector;
        protected float NearPlane = 0.25f;
        protected float FarPlane = 10000;

        float speed = 1f;

        public BoundingFrustum Frustum { get { return new BoundingFrustum(view * projection); } }

        public Camera(string id, Vector3 position, Vector3 target)
            : base(id, position)
        {
            CameraDirection = target;
        }

        public override void Initialize()
        {
            NearPlane = 1.0f;
            FarPlane = 1000.0f;
            UpVector = Vector3.Up;
            CameraDirection.Normalize();

            UpdateView();

            Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.AspectRatio,
                NearPlane,
                FarPlane);

            base.Initialize();
        }

        public override void Update()
        {
            if (InputEngine.IsKeyHeld(Keys.A))
            {
                World *= Matrix.CreateTranslation(new Vector3(-speed, 0, 0));
            }
            else if (InputEngine.IsKeyHeld(Keys.D))
            {
                World *= Matrix.CreateTranslation(new Vector3(speed, 0, 0));
            }

            if (InputEngine.IsKeyHeld(Keys.W))
            {
                World *= Matrix.CreateTranslation(new Vector3(0, 0, -speed));
            }
            else if (InputEngine.IsKeyHeld(Keys.S))
            {
                World *= Matrix.CreateTranslation(new Vector3(0, 0, speed));
            }

            if (InputEngine.IsKeyHeld(Keys.E))
            {
                World *= Matrix.CreateTranslation(new Vector3(0, speed, 0));
            }
            else if (InputEngine.IsKeyHeld(Keys.Q))
            {
                World *= Matrix.CreateTranslation(new Vector3(0, -speed, 0));
            }

            UpdateView();
            base.Update();
        }

        private void UpdateView()
        {
            CurrentTarget = World.Translation + CameraDirection;

            View = Matrix.CreateLookAt(
               World.Translation,
                CurrentTarget,
                UpVector);
        }

        public Matrix View
        {
            get { return view; }
            set { view = value; }
        }

        public Matrix Projection
        {
            get { return projection; }
            set { projection = value; }
        }

    }
}
