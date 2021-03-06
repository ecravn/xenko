// Copyright (c) 2014 Silicon Studio Corp. (http://siliconstudio.co.jp)
// This file is distributed under GPL v3. See LICENSE.md for details.
using SiliconStudio.Xenko.Shaders.Parser.Ast;
using SiliconStudio.Shaders.Ast;

namespace SiliconStudio.Xenko.Shaders.Parser.Mixins
{
    internal class MethodDeclarationShaderCouple
    {
        public MethodDeclaration Method;
        public ShaderClassType Shader;

        public MethodDeclarationShaderCouple() : this(null, null){}

        public MethodDeclarationShaderCouple(MethodDeclaration method, ShaderClassType shader)
        {
            Method = method;
            Shader = shader;
        }
    }
}