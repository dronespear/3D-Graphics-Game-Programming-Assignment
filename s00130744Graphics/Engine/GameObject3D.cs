using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Sample
{
    public class GameObject3D
    {
        public string ID { get; set; }
        public Matrix World { get; set; }

        public GameObject3D(string id)
        {
            ID = id;
        }

        public GameObject3D(string id, Vector3 position)
        {
            ID = id;

            World = Matrix.Identity * Matrix.CreateTranslation(position);
        }

        public virtual void Initialize() { }
        public virtual void LoadContent() { }
        public virtual void Update() { }
        public virtual void Draw(Camera camera) { }
    }
 
}
