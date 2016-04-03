using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization;

namespace Sample
{
    public delegate void ObjectStringIDHandler(string id);

    public static class GameUtilities
    {
        public static GraphicsDevice GraphicsDevice { get; set; }
        public static GameTime Time { get; set; }
        public static ContentManager Content { get; set; }
        public static Random Random { get; set; }
        public static SpriteFont DebugFont { get; set; }

        public static void SetGraphicsDeviceFor3D()
        {
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
        }
    }

}
