﻿// Copyright (c) 2014 Silicon Studio Corp. (http://siliconstudio.co.jp)
// This file is distributed under GPL v3. See LICENSE.md for details.

using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Graphics;

namespace SiliconStudio.Xenko.Particles.VertexLayouts
{
    /// <summary>
    /// A list of common <see cref="VertexElement"/> used in the particles' vertex layout
    /// </summary>
    public static class ParticleVertexElements
    {
        public static VertexElement Position = VertexElement.Position<Vector3>();

        public static VertexElement Color    = VertexElement.Color<Color>();

        public static VertexElement Lifetime = new VertexElement("BATCH_LIFETIME", PixelFormat.R32_Float);

        public static VertexElement RandSeed = new VertexElement("BATCH_RANDOMSEED", PixelFormat.R32_Float);

        public static VertexElement[] TexCoord = new VertexElement[]
        {
            new VertexElement("TEXCOORD",  0, VertexElement.ConvertTypeToFormat<Vector2>()),
            new VertexElement("TEXCOORD",  1, VertexElement.ConvertTypeToFormat<Vector2>()),
            new VertexElement("TEXCOORD",  2, VertexElement.ConvertTypeToFormat<Vector2>()),
            new VertexElement("TEXCOORD",  3, VertexElement.ConvertTypeToFormat<Vector2>()),
            new VertexElement("TEXCOORD",  4, VertexElement.ConvertTypeToFormat<Vector2>()),
            new VertexElement("TEXCOORD",  5, VertexElement.ConvertTypeToFormat<Vector2>()),
            new VertexElement("TEXCOORD",  6, VertexElement.ConvertTypeToFormat<Vector2>()),
            new VertexElement("TEXCOORD",  7, VertexElement.ConvertTypeToFormat<Vector2>()),
            new VertexElement("TEXCOORD",  8, VertexElement.ConvertTypeToFormat<Vector2>()),
            new VertexElement("TEXCOORD",  9, VertexElement.ConvertTypeToFormat<Vector2>()),
            new VertexElement("TEXCOORD", 10, VertexElement.ConvertTypeToFormat<Vector2>()),
            new VertexElement("TEXCOORD", 11, VertexElement.ConvertTypeToFormat<Vector2>()),
            new VertexElement("TEXCOORD", 12, VertexElement.ConvertTypeToFormat<Vector2>()),
            new VertexElement("TEXCOORD", 13, VertexElement.ConvertTypeToFormat<Vector2>()),
            new VertexElement("TEXCOORD", 14, VertexElement.ConvertTypeToFormat<Vector2>()),
            new VertexElement("TEXCOORD", 15, VertexElement.ConvertTypeToFormat<Vector2>()),
        };
    }
}
