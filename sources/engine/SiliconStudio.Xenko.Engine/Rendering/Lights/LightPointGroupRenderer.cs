// Copyright (c) 2014 Silicon Studio Corp. (http://siliconstudio.co.jp)
// This file is distributed under GPL v3. See LICENSE.md for details.

using System;
using SiliconStudio.Core.Collections;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Graphics;
using SiliconStudio.Xenko.Rendering.Shadows;
using SiliconStudio.Xenko.Shaders;

namespace SiliconStudio.Xenko.Rendering.Lights
{
    public struct PointLightData
    {
        public Vector3 PositionWS;
        public float InvSquareRadius;
        public Color3 Color;
        private float padding0;
    }

    /// <summary>
    /// Light renderer for <see cref="LightPoint"/>.
    /// </summary>
    public class LightPointGroupRenderer : LightGroupRendererDynamic
    {
        public override void Initialize(RenderContext context)
        {
        }

        public override LightShaderGroupDynamic CreateLightShaderGroup(RenderDrawContext context, ILightShadowMapShaderGroupData shadowGroup)
        {
            return new PointLightShaderGroup(shadowGroup);
        }

        class PointLightShaderGroup : LightShaderGroupDynamic
        {
            private ValueParameterKey<int> countKey;
            private ValueParameterKey<PointLightData> lightsKey;
            private FastListStruct<PointLightData> lightsData = new FastListStruct<PointLightData>(8);

            public PointLightShaderGroup(ILightShadowMapShaderGroupData shadowGroupData)
                : base(shadowGroupData)
            {
            }

            public override void UpdateLayout(string compositionName)
            {
                base.UpdateLayout(compositionName);

                countKey = DirectLightGroupPerDrawKeys.LightCount.ComposeWith(compositionName);
                lightsKey = LightPointGroupKeys.Lights.ComposeWith(compositionName);
            }

            protected override void UpdateLightCount()
            {
                base.UpdateLightCount();

                var mixin = new ShaderMixinSource();
                mixin.Mixins.Add(new ShaderClassSource("LightPointGroup", LightCurrentCount));
                // Old fixed path kept in case we need it again later
                //mixin.Mixins.Add(new ShaderClassSource("LightPointGroup", LightCurrentCount));
                //mixin.Mixins.Add(new ShaderClassSource("DirectLightGroupFixed", LightCurrentCount));
                ShadowGroup?.ApplyShader(mixin);

                ShaderSource = mixin;
            }

            /// <inheritdoc/>
            protected override int ComputeLightCount(int lightCount)
            {
                // TODO: Some way to override this
                return 8;
            }

            /// <inheritdoc/>
            public override int AddView(int viewIndex, RenderView renderView, int lightCount)
            {
                base.AddView(viewIndex, renderView, lightCount);

                // We allow more lights than LightCurrentCount (they will be culled)
                return lightCount;
            }

            public override void ApplyDrawParameters(RenderDrawContext context, int viewIndex, ParameterCollection parameters, ref BoundingBoxExt boundingBox)
            {
                CurrentLights.Clear();
                var lightRange = LightRanges[viewIndex];
                for (int i = lightRange.Start; i < lightRange.End; ++i)
                    CurrentLights.Add(Lights[i]);

                base.ApplyDrawParameters(context, viewIndex, parameters, ref boundingBox);

                // TODO: Since we cull per object, we could maintain a higher number of allowed light than the shader support (i.e. 4 lights active per object even though the scene has many more of them)
                // TODO: Octree structure to select best lights quicker
                var boundingBox2 = (BoundingBox)boundingBox;
                foreach (var lightEntry in CurrentLights)
                {
                    var light = lightEntry.Light;

                    if (light.BoundingBox.Intersects(ref boundingBox2))
                    {
                        var pointLight = (LightPoint)light.Type;
                        lightsData.Add(new PointLightData
                        {
                            PositionWS = light.Position,
                            InvSquareRadius = pointLight.InvSquareRadius,
                            Color = light.Color,
                        });

                        // Did we reach max number of simultaneous lights?
                        // TODO: Still collect everything but sort by importance and remove the rest?
                        if (lightsData.Count >= LightCurrentCount)
                            break;
                    }
                }

                parameters.Set(countKey, lightsData.Count);
                parameters.Set(lightsKey, lightsData.Count, ref lightsData.Items[0]);
                lightsData.Clear();
            }
        }
    }
}