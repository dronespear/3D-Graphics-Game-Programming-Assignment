using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace s00130744Graphics.Graphics
{
    /// <summary>
    /// saved data already on model
    /// </summary>
    public class MeshTag
    {
        //public Color Color { get; set; }
        public Vector3 Color { get; set; }

        //Texture that applied to model by model processor
        public Texture2D Texture { get; set; }
        //SpecularPower, stength of shine on a model
        public float SpecularPower { get; set; }
        //Saved feect that can be used later
        public Effect CahcedEffect { get; set; }

        public MeshTag() { }
        public MeshTag (Vector3 color, Texture2D texture, float specularPower)
        {
            Color = color;
            Texture = texture;
            SpecularPower = specularPower;
        }
    }
}
