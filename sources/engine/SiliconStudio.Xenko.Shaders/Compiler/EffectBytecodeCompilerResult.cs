﻿// Copyright (c) 2014 Silicon Studio Corp. (http://siliconstudio.co.jp)
// This file is distributed under GPL v3. See LICENSE.md for details.
using SiliconStudio.Core.Diagnostics;

namespace SiliconStudio.Xenko.Shaders.Compiler
{
    /// <summary>
    /// Result of an effect bytecode compilation.
    /// </summary>
    public struct EffectBytecodeCompilerResult
    {
        private static readonly LoggerResult emptyLoggerResult = new LoggerResult();

        /// <summary>
        /// The effect bytecode. Might be null.
        /// </summary>
        public readonly EffectBytecode Bytecode;

        /// <summary>
        /// The compilation log.
        /// </summary>
        public readonly LoggerResult CompilationLog;

        public EffectBytecodeCompilerResult(EffectBytecode bytecode) : this()
        {
            Bytecode = bytecode;
            CompilationLog = emptyLoggerResult;
        }

        public EffectBytecodeCompilerResult(EffectBytecode bytecode, LoggerResult compilationLog)
        {
            Bytecode = bytecode;
            CompilationLog = compilationLog;
        }
    }
}