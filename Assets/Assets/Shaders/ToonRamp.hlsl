void ToonShading_float(
    in float3 Normal, 
    in float ToonRampSmoothness, 
    in float3 ClipSpacePos, 
    in float3 WorldPos, 
    in float3 ToonRampTinting,
    in float ToonRampOffset, 
    in float ToonRampOffsetPoint, 
    in float Ambient, 
    out float3 ToonRampOutput, 
    out float3 Direction)
{
    // Set the shader graph node previews
    #ifdef SHADERGRAPH_PREVIEW
        ToonRampOutput = float3(0.5, 0.5, 0);
        Direction = float3(0.5, 0.5, 0);
    #else
        // Grab the shadow coordinates
        #if SHADOWS_SCREEN
            half4 shadowCoord = ComputeScreenPos(ClipSpacePos);
        #else
            half4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
        #endif 

        // Grab the main light
        #if _MAIN_LIGHT_SHADOWS_CASCADE || _MAIN_LIGHT_SHADOWS
            Light light = GetMainLight(shadowCoord);
        #else
            Light light = GetMainLight();
        #endif

        // Dot product for toon ramp
        half d = dot(Normal, light.direction) * 0.5 + 0.5;

        // Toon ramp in a smoothstep
        half toonRamp = smoothstep(ToonRampOffset, ToonRampOffset + ToonRampSmoothness, d);

        float3 extraLights = 0;
        // Get the number of point/spot lights
        int pixelLightCount = GetAdditionalLightsCount();

        // Loop over every additional light
        for (int j = 0; j < pixelLightCount; ++j) {
            // Grab the additional light
            Light aLight = GetAdditionalLight(j, WorldPos);
            
            // Compute light color with attenuation
            float3 attenuatedLightColor = aLight.color * (aLight.distanceAttenuation * aLight.shadowAttenuation);

            // Dot product for toon ramp
            half d = dot(Normal, aLight.direction) * 0.5 + 0.5;

            // Toon ramp in a smoothstep for additional lights
            half toonRampExtra = smoothstep(ToonRampOffsetPoint, ToonRampOffsetPoint + ToonRampSmoothness, d);

            // Accumulate extra light contribution
            extraLights += (attenuatedLightColor * toonRampExtra);
        }

        // Multiply with shadows
        toonRamp *= light.shadowAttenuation;

        // Add in light contributions and ambient tinting
        ToonRampOutput = light.color * (toonRamp + ToonRampTinting) + Ambient;

        // Add additional point/spot light contributions
        ToonRampOutput += extraLights;

        // Output light direction for rim lighting
        Direction = normalize(light.direction);
    #endif
}
