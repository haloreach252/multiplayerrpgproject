﻿//////////////////////////////////////////////////////
// MicroSplat
// Copyright (c) Jason Booth
//////////////////////////////////////////////////////


using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections.Generic;

namespace JBooth.MicroSplat
{
   [InitializeOnLoad]
   public class MicroSplatBaseFeatures : FeatureDescriptor
   {
      public override int DisplaySortOrder()
      {
         return -1000;
      }

      public override string ModuleName()
      {
         return "Core";
      }


      public enum DefineFeature
      {
         _MICROSPLAT = 0,
         _MAX3LAYER,
         _MAX2LAYER,
         _MAX4TEXTURES,
         _MAX8TEXTURES,
         _MAX12TEXTURES,
         _MAX16TEXTURES,
         _MAX20TEXTURES,
         _MAX24TEXTURES,
         _MAX28TEXTURES,
         _MAX32TEXTURES,
         _PERTEXTINT,
         _PERTEXBRIGHTNESS,
         _PERTEXCONTRAST,
         _PERTEXSATURATION,
         _PERTEXAOSTR,
         _PERTEXNORMSTR,
         _PERTEXSMOOTHSTR,
         _PERTEXMETALLIC,
         _PERTEXUVSCALEOFFSET,
         _PERTEXINTERPCONTRAST,
         _PERTEXHEIGHTOFFSET,
         _PERTEXHEIGHTCONTRAST,
         _PERTEXUVROTATION,
         _PERTEXFUZZYSHADE,
         _PERTEXSSS,
         _BDRF1,
         _BDRF2,
         _BDRF3,
         _BDRFLAMBERT,
         _SPECULARFROMMETALLIC,
         _USELODMIP,
         _USEGRADMIP,
         _DISABLEHEIGHTBLENDING,
         _WORLDUV,
         _USEEMISSIVEMETAL,
         _FORCEMODEL46,
         _FORCEMODEL50,
         _PACKINGHQ,
         _USESPECULARWORKFLOW,
         _PERPIXNORMAL,
         _NONOMALMAP,
         _MICROMESH,
         _MICROMESHTERRAIN,
         _DEBUG_OUTPUT_ALBEDO,
         _DEBUG_OUTPUT_HEIGHT,
         _DEBUG_OUTPUT_NORMAL,
         _DEBUG_OUTPUT_METAL,
         _DEBUG_OUTPUT_SMOOTHNESS,
         _DEBUG_OUTPUT_AO,
         _DEBUG_OUTPUT_EMISSION,
         _DEBUG_OUTPUT_SPECULAR,
         _DEBUG_OUTPUT_SPLAT0,
         _DEBUG_OUTPUT_SPLAT1,
         _DEBUG_OUTPUT_SPLAT2,
         _DEBUG_OUTPUT_SPLAT3,
         _DEBUG_OUTPUT_SPLAT4,
         _DEBUG_OUTPUT_SPLAT5,
         _DEBUG_OUTPUT_SPLAT6,
         _DEBUG_OUTPUT_SPLAT7,
         _DEBUG_OUTPUT_SPLAT0A,
         _DEBUG_OUTPUT_SPLAT1A,
         _DEBUG_OUTPUT_SPLAT2A,
         _DEBUG_OUTPUT_SPLAT3A,
         _DEBUG_OUTPUT_SPLAT4A,
         _DEBUG_OUTPUT_SPLAT5A,
         _DEBUG_OUTPUT_SPLAT6A,
         _DEBUG_OUTPUT_SPLAT7A,
         _CUSTOMSPLATTEXTURES,
         kNumFeatures,
      }
         

      public enum MaxTextureCount
      {
         Four = 4,
         Eight = 8,
         Twelve = 12,
         Sixteen = 16,
         Twenty = 20,
         TwentyFour = 24,
         TwentyEight = 28,
         ThirtyTwo = 32,
      }

      #if __MICROSPLAT_MESHTERRAIN__ || __MICROSPLAT_MESH__
      public enum Workflow
      {
         UnityTerrain,
      #if __MICROSPLAT_MESHTERRAIN__
         MeshTerrain,
      #endif
      #if __MICROSPLAT_MESH__
         Mesh
      #endif
      }
      #endif // __MICROSPLAT_MESHTERRAIN__


      public enum PerformanceMode
      {
         BestQuality,
         Balanced,
         Fastest
      }
         
      public enum UVMode
      {
         UV, 
         WorldSpace,
      }

      public enum LightingMode
      {
         Automatic = 0,
         StandardShader,
         Simplified,
         BlinnPhong,
         Lambert,
         StandardShaderNoSheen
      }


      public enum DebugOutput
      {
         None = 0,
         Albedo,
         Height,
         Normal,
         Metallic,
         Smoothness,
         AO,
         Emission,
         Specular,
#if __MICROSPLAT_PROCTEX__
         ProceduralSplatOutput0,
         ProceduralSplatOutput1,
         ProceduralSplatOutput2,
         ProceduralSplatOutput3,
         ProceduralSplatOutput4,
         ProceduralSplatOutput5,
         ProceduralSplatOutput6,
         ProceduralSplatOutput7,
         ProceduralSplatOutput0A,
         ProceduralSplatOutput1A,
         ProceduralSplatOutput2A,
         ProceduralSplatOutput3A,
         ProceduralSplatOutput4A,
         ProceduralSplatOutput5A,
         ProceduralSplatOutput6A,
         ProceduralSplatOutput7A,
#endif
      }

      public enum ShaderModel
      {
         Automatic,
         Force46,
         Force50
      }

      public enum SamplerMode
      {
         Default,
         LODSampler,
         GradientSampler
      }

      public bool useCustomSplatMaps = false;

      // state for the shader generation
      public PerformanceMode perfMode = PerformanceMode.BestQuality;
      public MaxTextureCount maxTextureCount = MaxTextureCount.Sixteen;
      public TextureArrayConfig.PackingMode packMode = TextureArrayConfig.PackingMode.Fastest;
      public TextureArrayConfig.PBRWorkflow pbrWorkflow = TextureArrayConfig.PBRWorkflow.Metallic;

      public bool disableNormals;
      public bool perTexTint;
      public bool perTexBrightness;
      public bool perTexContrast;
      public bool perTexSaturation;
      public bool perTexAOStr;
      public bool perTexNormStr;
      public bool perTexSmoothStr;
      public bool perTexMetallic;
      public bool perTexUVScale;
      public bool perTexUVRotation;
      public bool perTexInterpContrast;
      public bool perTexSSS;
      public bool disableHeightBlend;
      public bool perTexHeightOffset;
      public bool perTexHeightContrast;
      public bool perTexFuzzyShading;
      public bool emissiveArray = false;
      public UVMode uvMode = UVMode.UV;
      public bool perPixelNormal;

      public LightingMode lightingMode;
      public DebugOutput debugOutput = DebugOutput.None;
      public ShaderModel shaderModel = ShaderModel.Automatic;
      public SamplerMode samplerMode = SamplerMode.Default;

      #if __MICROSPLAT_MESHTERRAIN__ || __MICROSPLAT_MESH__
      public Workflow shaderType = Workflow.UnityTerrain;
      #endif

      // files to include
      static TextAsset properties_splat;
      static TextAsset cbuffer;

      #if __MICROSPLAT_MESHTERRAIN__ || __MICROSPLAT_MESH__
      GUIContent CWorkflow = new GUIContent ("Shader Type", "What type of object is this shader going to be used on");
      #endif
      GUIContent CInterpContrast = new GUIContent("Interpolation Contrast", "Controls how much hight map based blending is used");
      GUIContent CPackMode = new GUIContent("Packing Mode", "Mode in which the textures are packed (2 arrays for better speed, 3 arrays for better quality");
      GUIContent CPBRWorlkflow = new GUIContent ("PBR Workflow", "Metallic or Specular workflow");
      GUIContent CShaderPerfMode = new GUIContent("Blend Quality", "Can be used to reduce the number of textures blended per pixel to increase speed. May create blending artifacts when set too low");
      GUIContent CMaxTexCount = new GUIContent("Max Texture Count", "How many textures your terrain is allowed to use - if you are using less than 13 textures, this allows you to optimize our the work of sampling the extra control textures, and saves samplers");
      GUIContent CLightingMode = new GUIContent("Lighting Model", "Override Unity's automatic selection of a BDRF function to a fixed one. This will force the shader to render in forward rendering mode when not set to automatic");
      GUIContent CDisableHeightBlend = new GUIContent("Disable Height Blending", "Disables height based blending, which can be a minor speed boost on low end platforms");
      GUIContent CUVMode = new GUIContent("UV Mode", "Mode for Splat UV coordinates");
      GUIContent CForceShaderModel = new GUIContent("Shader Model", "Force a specific shader model to be used. By default, MicroSplat will use the minimum required shader model based on your shader settings");
      GUIContent CSamplerMode = new GUIContent("Sampler Mode", "Force usage of manual mip selection in the shader (fast) or gradient samplers (slow). Mostly only used when PerTexture UV Scale is used. See documentation for more info");
      GUIContent CEmissiveArray = new GUIContent("Emissive/Metallic Array", "Sample an emissive and metallic texture array");
      GUIContent CUseCustomSplatMaps = new GUIContent("Use Custom Splatmaps", "Use user provided splat maps instead of the ones unity generates");
      GUIContent CPerPixelNormal = new GUIContent("Per-Pixel Normal", "Allows you to generate and use a per-pixel normal. In 2018.3+ when using instancing, this is not necissary");
      GUIContent CDisableNormals = new GUIContent("Disable Normal Maps", "Disables sampling of normals data (and Smoothness/AO if fastest packing is used. Note, global normals, and other types of normal maps may still be applied.");
      GUIContent CSSSDistance = new GUIContent ("Distance", "Distance of Subsurface Scattering");
      GUIContent CSSSPower = new GUIContent ("Power", "Power of Subsurface Scattering");
      GUIContent CSSSScale = new GUIContent ("Scale", "Scale of Subsurface Scattering");
      // Can we template these somehow?
      static Dictionary<DefineFeature, string> sFeatureNames = new Dictionary<DefineFeature, string>();
      public static string GetFeatureName(DefineFeature feature)
      {
         string ret;
         if (sFeatureNames.TryGetValue(feature, out ret))
         {
            return ret;
         }
         string fn = System.Enum.GetName(typeof(DefineFeature), feature);
         sFeatureNames[feature] = fn;
         return fn;
      }

      public static bool HasFeature(string[] keywords, DefineFeature feature)
      {
         string f = GetFeatureName(feature);
         for (int i = 0; i < keywords.Length; ++i)
         {
            if (keywords[i] == f)
               return true;
         }
         return false;
      }

      public static bool HasFeature (string [] keywords, string f)
      {
         for (int i = 0; i < keywords.Length; ++i)
         {
            if (keywords [i] == f)
               return true;
         }
         return false;
      }

      public override string GetVersion()
      {
         return MicroSplatShaderGUI.MicroSplatVersion;
      }

      public override void WriteFunctions(System.Text.StringBuilder sb)
      {
         
      }



      public override void DrawFeatureGUI(MicroSplatKeywords keywords)
      {
         bool isSurfaceShader = keywords.IsKeywordEnabled("_MSRENDERLOOP_SURFACESHADER");
         #if __MICROSPLAT_MESHTERRAIN__ || __MICROSPLAT_MESH__
         shaderType = (Workflow)EditorGUILayout.EnumPopup(CWorkflow, shaderType);
         #endif
         pbrWorkflow = (TextureArrayConfig.PBRWorkflow)EditorGUILayout.EnumPopup(CPBRWorlkflow, pbrWorkflow);
         packMode = (TextureArrayConfig.PackingMode)EditorGUILayout.EnumPopup(CPackMode, packMode);
         perfMode = (PerformanceMode)EditorGUILayout.EnumPopup(CShaderPerfMode, perfMode);
         maxTextureCount = (MaxTextureCount)EditorGUILayout.EnumPopup(CMaxTexCount, maxTextureCount);
         if (isSurfaceShader)
         {
            lightingMode = (LightingMode)EditorGUILayout.EnumPopup(CLightingMode, lightingMode);
            if (lightingMode != LightingMode.Automatic && lightingMode != LightingMode.StandardShaderNoSheen)
            {
               EditorGUILayout.HelpBox ("Shader is forced to run in forward rendering due to lighting mode", MessageType.Info);
            }
         }
         uvMode = (UVMode)EditorGUILayout.EnumPopup(CUVMode, uvMode);
         shaderModel = (ShaderModel)EditorGUILayout.EnumPopup(CForceShaderModel, shaderModel);
         samplerMode = (SamplerMode)EditorGUILayout.EnumPopup(CSamplerMode, samplerMode);
         emissiveArray = EditorGUILayout.Toggle(CEmissiveArray, emissiveArray);
         perPixelNormal = EditorGUILayout.Toggle(CPerPixelNormal, perPixelNormal);
         disableHeightBlend = EditorGUILayout.Toggle(CDisableHeightBlend, disableHeightBlend);
         disableNormals = EditorGUILayout.Toggle(CDisableNormals, disableNormals);
         #if __MICROSPLAT_MESHTERRAIN__ || __MICROSPLAT_MESH__
         if (shaderType == Workflow.UnityTerrain)
         {
            useCustomSplatMaps = EditorGUILayout.Toggle(CUseCustomSplatMaps, useCustomSplatMaps);
         }
         #else
         useCustomSplatMaps = EditorGUILayout.Toggle(CUseCustomSplatMaps, useCustomSplatMaps);
         #endif

         debugOutput = (DebugOutput)EditorGUILayout.EnumPopup("Debug", debugOutput);
      }

      static GUIContent CAlbedoTex = new GUIContent("Albedo/Height Array", "Texture Array which contains albedo and height information");
      static GUIContent CNormalSpec = new GUIContent("Normal/Smooth/AO Array", "Texture Array with normal, smoothness, and ambient occlusion");
      static GUIContent CNormal = new GUIContent("Normal Array", "Texture Array with normals");
      static GUIContent CEmisMetal = new GUIContent("Emissive/Metal Array", "Texture Array with emissive and metalic values");
      static GUIContent CSmoothAO = new GUIContent("Smoothness/AO Array", "Texture Array with Smoothness and AO");
      static GUIContent CSpecular = new GUIContent ("Specular Array", "Specular Color array");

      public override void DrawShaderGUI(MicroSplatShaderGUI shaderGUI, MicroSplatKeywords keywords, Material mat, MaterialEditor materialEditor, MaterialProperty[] props)
      {
         if (!keywords.IsKeywordEnabled ("_DISABLESPLATMAPS"))
         {
            if (MicroSplatUtilities.DrawRollup ("Splats"))
            {

               var albedoMap = shaderGUI.FindProp ("_Diffuse", props);
               var normalMap = shaderGUI.FindProp ("_NormalSAO", props);
               materialEditor.TexturePropertySingleLine (CAlbedoTex, albedoMap);
               if (!disableNormals)
               {
                  if (packMode == TextureArrayConfig.PackingMode.Fastest)
                  {
                     materialEditor.TexturePropertySingleLine (CNormalSpec, normalMap);
                  }
                  else
                  {
                     materialEditor.TexturePropertySingleLine (CNormal, normalMap);
                  }
                  
               }
               if (pbrWorkflow == TextureArrayConfig.PBRWorkflow.Specular && mat.HasProperty ("_Specular"))
               {
                  var specMap = shaderGUI.FindProp ("_Specular", props);
                  materialEditor.TexturePropertySingleLine (CSpecular, specMap);
               }

               if (emissiveArray && mat.HasProperty ("_EmissiveMetal"))
               {
                  var emisMap = shaderGUI.FindProp ("_EmissiveMetal", props);
                  materialEditor.TexturePropertySingleLine (CEmisMetal, emisMap);
                  if (mat.HasProperty ("_EmissiveMult"))
                  {
                     var emisMult = shaderGUI.FindProp ("_EmissiveMult", props);
                     emisMult.floatValue = EditorGUILayout.Slider (CInterpContrast, emisMult.floatValue, 0.0f, 4.0f);
                  }
               }

               if (packMode == TextureArrayConfig.PackingMode.Quality && mat.HasProperty ("_SmoothAO"))
               {
                  var smoothAO = shaderGUI.FindProp ("_SmoothAO", props);
                  materialEditor.TexturePropertySingleLine (CSmoothAO, smoothAO);
               }

               if (!disableHeightBlend)
               {
                  var contrastProp = shaderGUI.FindProp ("_Contrast", props);
                  contrastProp.floatValue = EditorGUILayout.Slider (CInterpContrast, contrastProp.floatValue, 1.0f, 0.0001f);
               }


               if (!keywords.IsKeywordEnabled ("_TRIPLANAR"))
               {
                  EditorGUI.BeginChangeCheck ();
                  Vector4 uvScale = shaderGUI.FindProp ("_UVScale", props).vectorValue;
                  Vector2 scl = new Vector2 (uvScale.x, uvScale.y);
                  Vector2 offset = new Vector2 (uvScale.z, uvScale.w);
                  scl = EditorGUILayout.Vector2Field ("Global UV Scale", scl);
                  offset = EditorGUILayout.Vector2Field ("Global UV Offset", offset);
                  if (EditorGUI.EndChangeCheck ())
                  {
                     uvScale.x = scl.x;
                     uvScale.y = scl.y;
                     uvScale.z = offset.x;
                     uvScale.w = offset.y;
                     shaderGUI.FindProp ("_UVScale", props).vectorValue = uvScale;
                     EditorUtility.SetDirty (mat);
                  }
               }

               
            }
            
         }
         
         materialEditor.RenderQueueField();

         if (mat.HasProperty ("_SSSPower"))
         {
            if (MicroSplatUtilities.DrawRollup ("Subsurface Scattering"))
            {
               var distance = shaderGUI.FindProp ("_SSSDistance", props);
               var power = shaderGUI.FindProp ("_SSSPower", props);
               var scale = shaderGUI.FindProp ("_SSSScale", props);
               distance.floatValue = EditorGUILayout.FloatField (CSSSDistance, distance.floatValue);
               scale.floatValue = EditorGUILayout.FloatField (CSSSScale, scale.floatValue);
               power.floatValue = EditorGUILayout.FloatField (CSSSPower, power.floatValue);
            }
         }
      }

      public override string[] Pack()
      {
         List<string> features = new List<string>();
         features.Add(GetFeatureName(DefineFeature._MICROSPLAT));

         #if __MICROSPLAT_MESHTERRAIN__
         if (shaderType == Workflow.MeshTerrain)
         {
            features.Add(GetFeatureName(DefineFeature._MICROMESHTERRAIN));
         }
         #endif
         #if __MICROSPLAT_MESH__
         if (shaderType == Workflow.Mesh)
         {
            features.Add(GetFeatureName(DefineFeature._MICROMESH));
         }
         #endif
         if (perTexUVScale && samplerMode == SamplerMode.Default)
         {
            samplerMode = SamplerMode.GradientSampler;
         }
         if (pbrWorkflow == TextureArrayConfig.PBRWorkflow.Specular)
         {
            features.Add (GetFeatureName (DefineFeature._USESPECULARWORKFLOW));
         }

         if (useCustomSplatMaps)
         {
            features.Add(GetFeatureName(DefineFeature._CUSTOMSPLATTEXTURES));
         }

         if (disableNormals)
         {
            features.Add(GetFeatureName(DefineFeature._NONOMALMAP));
         }

         if (samplerMode == SamplerMode.LODSampler)
         {
            features.Add(GetFeatureName(DefineFeature._USELODMIP));
         }
         else if (samplerMode == SamplerMode.GradientSampler)
         {
            features.Add(GetFeatureName(DefineFeature._USEGRADMIP));
         }
         if (packMode == TextureArrayConfig.PackingMode.Quality)
         {
            features.Add(GetFeatureName(DefineFeature._PACKINGHQ));
         }
         
         if (emissiveArray)
         {
            features.Add(GetFeatureName(DefineFeature._USEEMISSIVEMETAL));
         }

         if (perfMode == PerformanceMode.Balanced)
         {
            features.Add(GetFeatureName(DefineFeature._MAX3LAYER));
         }
         else if (perfMode == PerformanceMode.Fastest)
         {
            features.Add(GetFeatureName(DefineFeature._MAX2LAYER));
         }
         if (disableHeightBlend)
         {
            features.Add(GetFeatureName(DefineFeature._DISABLEHEIGHTBLENDING));
         }
         if (maxTextureCount == MaxTextureCount.Four)
         {
            features.Add(GetFeatureName(DefineFeature._MAX4TEXTURES));
         }
         else if (maxTextureCount == MaxTextureCount.Eight)
         {
            features.Add(GetFeatureName(DefineFeature._MAX8TEXTURES));
         }
         else if (maxTextureCount == MaxTextureCount.Twelve)
         {
            features.Add(GetFeatureName(DefineFeature._MAX12TEXTURES));
         }
         else if (maxTextureCount == MaxTextureCount.Twenty)
         {
            features.Add(GetFeatureName(DefineFeature._MAX20TEXTURES));
         }
         else if (maxTextureCount == MaxTextureCount.TwentyFour)
         {
            features.Add(GetFeatureName(DefineFeature._MAX24TEXTURES));
         }
         else if (maxTextureCount == MaxTextureCount.TwentyEight)
         {
            features.Add(GetFeatureName(DefineFeature._MAX28TEXTURES));
         }
         else if (maxTextureCount == MaxTextureCount.ThirtyTwo)
         {
            features.Add(GetFeatureName(DefineFeature._MAX32TEXTURES));
         }

         if (perPixelNormal)
         {
            features.Add(GetFeatureName(DefineFeature._PERPIXNORMAL));
         }
         if (lightingMode == LightingMode.StandardShaderNoSheen)
         {
            features.Add (GetFeatureName (DefineFeature._SPECULARFROMMETALLIC));
         }
         else if (lightingMode == LightingMode.StandardShader)
         {
            features.Add(GetFeatureName(DefineFeature._BDRF1));
         }
         else if (lightingMode == LightingMode.Simplified)
         {
            features.Add(GetFeatureName(DefineFeature._BDRF2));
         }
         else if (lightingMode == LightingMode.BlinnPhong)
         {
            features.Add(GetFeatureName(DefineFeature._BDRF3));
         }
         else if (lightingMode == LightingMode.Lambert)
         {
            features.Add(GetFeatureName(DefineFeature._BDRFLAMBERT));
         }

         if (perTexUVScale)
         {
            features.Add(GetFeatureName(DefineFeature._PERTEXUVSCALEOFFSET));
         }
         if (perTexUVRotation)
         {
            features.Add(GetFeatureName (DefineFeature._PERTEXUVROTATION));
         }
         if (perTexFuzzyShading)
         {
            features.Add (GetFeatureName (DefineFeature._PERTEXFUZZYSHADE));
         }
         if (perTexSSS)
         {
            features.Add (GetFeatureName (DefineFeature._PERTEXSSS));
         }
         if (perTexHeightOffset)
         {
            features.Add(GetFeatureName(DefineFeature._PERTEXHEIGHTOFFSET));
         }
         if (perTexHeightContrast)
         {
            features.Add(GetFeatureName(DefineFeature._PERTEXHEIGHTCONTRAST));
         }

         if (uvMode == UVMode.WorldSpace)
         {
            features.Add(GetFeatureName(DefineFeature._WORLDUV));
         }

         if (perTexSaturation)
         {
            features.Add(GetFeatureName(DefineFeature._PERTEXSATURATION));
         }

         if (perTexInterpContrast)
         {
            features.Add(GetFeatureName(DefineFeature._PERTEXINTERPCONTRAST));
         }
         if (perTexTint)
         {
            features.Add(GetFeatureName(DefineFeature._PERTEXTINT));
         }
         if (perTexBrightness)
         {
            features.Add(GetFeatureName(DefineFeature._PERTEXBRIGHTNESS));
         }
         if (perTexContrast)
         {
            features.Add(GetFeatureName(DefineFeature._PERTEXCONTRAST));
         }
         if (perTexAOStr)
         {
            features.Add(GetFeatureName(DefineFeature._PERTEXAOSTR));
         }
         if (perTexNormStr)
         {
            features.Add(GetFeatureName(DefineFeature._PERTEXNORMSTR));
         }
         if (perTexSmoothStr)
         {
            features.Add(GetFeatureName(DefineFeature._PERTEXSMOOTHSTR));
         }
         if (perTexMetallic)
         {
            features.Add(GetFeatureName(DefineFeature._PERTEXMETALLIC));
         }
         if (shaderModel != ShaderModel.Automatic)
         {
            if (shaderModel == ShaderModel.Force46)
            {
               features.Add(GetFeatureName(DefineFeature._FORCEMODEL46));
            }
            else
            {
               features.Add(GetFeatureName(DefineFeature._FORCEMODEL50));
            }
         }

         if (debugOutput != DebugOutput.None)
         {
            if (debugOutput == DebugOutput.Albedo)
            {
               features.Add(GetFeatureName(DefineFeature._DEBUG_OUTPUT_ALBEDO));
            }
            else if (debugOutput == DebugOutput.Height)
            {
               features.Add(GetFeatureName(DefineFeature._DEBUG_OUTPUT_HEIGHT));
            }
            else if (debugOutput == DebugOutput.Normal)
            {
               features.Add(GetFeatureName(DefineFeature._DEBUG_OUTPUT_NORMAL));
            }
            else if (debugOutput == DebugOutput.Metallic)
            {
               features.Add(GetFeatureName(DefineFeature._DEBUG_OUTPUT_METAL));
            }
            else if (debugOutput == DebugOutput.Smoothness)
            {
               features.Add(GetFeatureName(DefineFeature._DEBUG_OUTPUT_SMOOTHNESS));
            }
            else if (debugOutput == DebugOutput.AO)
            {
               features.Add(GetFeatureName(DefineFeature._DEBUG_OUTPUT_AO));
            }
            else if (debugOutput == DebugOutput.Emission)
            {
               features.Add(GetFeatureName(DefineFeature._DEBUG_OUTPUT_EMISSION));
            }
            else if (debugOutput == DebugOutput.Specular)
            {
               features.Add (GetFeatureName (DefineFeature._DEBUG_OUTPUT_SPECULAR));
            }
#if __MICROSPLAT_PROCTEX__
            else if (debugOutput == DebugOutput.ProceduralSplatOutput0)
            {
               features.Add(GetFeatureName(DefineFeature._DEBUG_OUTPUT_SPLAT0));
            }
            else if (debugOutput == DebugOutput.ProceduralSplatOutput1)
            {
               features.Add(GetFeatureName(DefineFeature._DEBUG_OUTPUT_SPLAT1));
            }
            else if (debugOutput == DebugOutput.ProceduralSplatOutput2)
            {
               features.Add(GetFeatureName(DefineFeature._DEBUG_OUTPUT_SPLAT2));
            }
            else if (debugOutput == DebugOutput.ProceduralSplatOutput3)
            {
               features.Add(GetFeatureName(DefineFeature._DEBUG_OUTPUT_SPLAT3));
            }
            else if (debugOutput == DebugOutput.ProceduralSplatOutput4)
            {
               features.Add(GetFeatureName(DefineFeature._DEBUG_OUTPUT_SPLAT4));
            }
            else if (debugOutput == DebugOutput.ProceduralSplatOutput5)
            {
               features.Add(GetFeatureName(DefineFeature._DEBUG_OUTPUT_SPLAT5));
            }
            else if (debugOutput == DebugOutput.ProceduralSplatOutput6)
            {
               features.Add(GetFeatureName(DefineFeature._DEBUG_OUTPUT_SPLAT6));
            }
            else if (debugOutput == DebugOutput.ProceduralSplatOutput7)
            {
               features.Add(GetFeatureName(DefineFeature._DEBUG_OUTPUT_SPLAT7));
            }
            else if (debugOutput == DebugOutput.ProceduralSplatOutput0A)
            {
               features.Add(GetFeatureName(DefineFeature._DEBUG_OUTPUT_SPLAT0A));
            }
            else if (debugOutput == DebugOutput.ProceduralSplatOutput1A)
            {
               features.Add(GetFeatureName(DefineFeature._DEBUG_OUTPUT_SPLAT1A));
            }
            else if (debugOutput == DebugOutput.ProceduralSplatOutput2A)
            {
               features.Add(GetFeatureName(DefineFeature._DEBUG_OUTPUT_SPLAT2A));
            }
            else if (debugOutput == DebugOutput.ProceduralSplatOutput3A)
            {
               features.Add(GetFeatureName(DefineFeature._DEBUG_OUTPUT_SPLAT3A));
            }
            else if (debugOutput == DebugOutput.ProceduralSplatOutput4A)
            {
               features.Add(GetFeatureName(DefineFeature._DEBUG_OUTPUT_SPLAT4A));
            }
            else if (debugOutput == DebugOutput.ProceduralSplatOutput5A)
            {
               features.Add(GetFeatureName(DefineFeature._DEBUG_OUTPUT_SPLAT5A));
            }
            else if (debugOutput == DebugOutput.ProceduralSplatOutput6A)
            {
               features.Add(GetFeatureName(DefineFeature._DEBUG_OUTPUT_SPLAT6A));
            }
            else if (debugOutput == DebugOutput.ProceduralSplatOutput7A)
            {
               features.Add(GetFeatureName(DefineFeature._DEBUG_OUTPUT_SPLAT7A));
            }
#endif
         }
         return features.ToArray();
      }

      public override void Unpack(string[] keywords)
      {
         if (HasFeature(keywords, DefineFeature._MAX2LAYER))
         {
            perfMode = PerformanceMode.Fastest;
         }
         else if (HasFeature(keywords, DefineFeature._MAX3LAYER))
         {
            perfMode = PerformanceMode.Balanced;
         }
         else
         {
            perfMode = PerformanceMode.BestQuality;
         }

         useCustomSplatMaps = (HasFeature(keywords, DefineFeature._CUSTOMSPLATTEXTURES));

         #if __MICROSPLAT_MESH__ || __MICROSPLAT_MESHTERRAIN__
         shaderType = Workflow.UnityTerrain;
         #endif

         #if __MICROSPLAT_MESH__
         if (HasFeature(keywords, DefineFeature._MICROMESH))
         {
            shaderType = Workflow.Mesh;
            useCustomSplatMaps = false;
         }
         #endif
         #if __MICROSPLAT_MESHTERRAIN__
         if (HasFeature(keywords, DefineFeature._MICROMESHTERRAIN))
         {
            shaderType = Workflow.MeshTerrain;
            useCustomSplatMaps = false;
         }
         #endif

         disableNormals = (HasFeature(keywords, DefineFeature._NONOMALMAP));

         packMode = HasFeature(keywords, DefineFeature._PACKINGHQ) ? TextureArrayConfig.PackingMode.Quality : TextureArrayConfig.PackingMode.Fastest;
         if (HasFeature(keywords, DefineFeature._USESPECULARWORKFLOW))
         {
            pbrWorkflow = TextureArrayConfig.PBRWorkflow.Specular;
         }
         else
         {
            pbrWorkflow = TextureArrayConfig.PBRWorkflow.Metallic;
         }

         emissiveArray = HasFeature(keywords, DefineFeature._USEEMISSIVEMETAL);
         samplerMode = SamplerMode.Default;
         if (HasFeature(keywords, DefineFeature._USELODMIP))
         {
            samplerMode = SamplerMode.LODSampler;
         }
         else if (HasFeature(keywords, DefineFeature._USEGRADMIP))
         {
            samplerMode = SamplerMode.GradientSampler;
         }
         // force gradient sampling for stochastic mode
         if (samplerMode == SamplerMode.Default && System.Array.Exists(keywords, e => e == "_STOCHASTIC"))
         {
            samplerMode = SamplerMode.GradientSampler;
         }

         perPixelNormal = HasFeature(keywords, DefineFeature._PERPIXNORMAL);
         uvMode = HasFeature(keywords, DefineFeature._WORLDUV) ? UVMode.WorldSpace : UVMode.UV;

         perTexHeightOffset = HasFeature(keywords, DefineFeature._PERTEXHEIGHTOFFSET);
         perTexHeightContrast = HasFeature(keywords, DefineFeature._PERTEXHEIGHTCONTRAST);

         if (HasFeature(keywords, DefineFeature._MAX4TEXTURES))
         {
            maxTextureCount = MaxTextureCount.Four;
         }
         else if (HasFeature(keywords, DefineFeature._MAX8TEXTURES))
         {
            maxTextureCount = MaxTextureCount.Eight;
         }
         else if (HasFeature(keywords, DefineFeature._MAX12TEXTURES))
         {
            maxTextureCount = MaxTextureCount.Twelve;
         }
         else if (HasFeature(keywords, DefineFeature._MAX20TEXTURES))
         {
            maxTextureCount = MaxTextureCount.Twenty;
         }
         else if (HasFeature(keywords, DefineFeature._MAX24TEXTURES))
         {
            maxTextureCount = MaxTextureCount.TwentyFour;
         }
         else if (HasFeature(keywords, DefineFeature._MAX28TEXTURES))
         {
            maxTextureCount = MaxTextureCount.TwentyEight;
         }
         else if (HasFeature(keywords, DefineFeature._MAX32TEXTURES))
         {
            maxTextureCount = MaxTextureCount.ThirtyTwo;
         }
         else
         {
            maxTextureCount = MaxTextureCount.Sixteen;
         }

         disableHeightBlend = HasFeature(keywords, DefineFeature._DISABLEHEIGHTBLENDING);

         lightingMode = LightingMode.Automatic;
         if (HasFeature (keywords, DefineFeature._SPECULARFROMMETALLIC))
         {
            lightingMode = LightingMode.StandardShaderNoSheen;
         }
         else if (HasFeature(keywords, DefineFeature._BDRF1))
         {
            lightingMode = LightingMode.StandardShader;
         }
         else if (HasFeature(keywords, DefineFeature._BDRF2))
         {
            lightingMode = LightingMode.Simplified;
         }
         else if (HasFeature(keywords, DefineFeature._BDRF3))
         {
            lightingMode = LightingMode.BlinnPhong;
         }
         else if (HasFeature(keywords, DefineFeature._BDRFLAMBERT))
         {
            lightingMode = LightingMode.Lambert;
         }

         perTexUVScale = (HasFeature(keywords, DefineFeature._PERTEXUVSCALEOFFSET));
         perTexUVRotation = (HasFeature(keywords, DefineFeature._PERTEXUVROTATION));
         perTexInterpContrast = HasFeature(keywords, DefineFeature._PERTEXINTERPCONTRAST);
         perTexBrightness = HasFeature(keywords, DefineFeature._PERTEXBRIGHTNESS);
         perTexContrast = HasFeature(keywords, DefineFeature._PERTEXCONTRAST);
         perTexSaturation = HasFeature(keywords, DefineFeature._PERTEXSATURATION);
         perTexAOStr = (HasFeature(keywords, DefineFeature._PERTEXAOSTR));
         perTexMetallic = (HasFeature(keywords, DefineFeature._PERTEXMETALLIC));
         perTexNormStr = (HasFeature(keywords, DefineFeature._PERTEXNORMSTR));
         perTexSmoothStr = (HasFeature(keywords, DefineFeature._PERTEXSMOOTHSTR));
         perTexTint = (HasFeature(keywords, DefineFeature._PERTEXTINT));
         perTexFuzzyShading = (HasFeature (keywords, DefineFeature._PERTEXFUZZYSHADE));
         perTexSSS = (HasFeature (keywords, DefineFeature._PERTEXSSS));

         shaderModel = ShaderModel.Automatic;
         if (HasFeature(keywords, DefineFeature._FORCEMODEL46))
         {
            shaderModel = ShaderModel.Force46;
         }
         if (HasFeature(keywords, DefineFeature._FORCEMODEL50))
         {
            shaderModel = ShaderModel.Force50;
         }

         debugOutput = DebugOutput.None;
         if (HasFeature(keywords, DefineFeature._DEBUG_OUTPUT_ALBEDO))
         {
            debugOutput = DebugOutput.Albedo;
         }
         else if (HasFeature(keywords, DefineFeature._DEBUG_OUTPUT_HEIGHT))
         {
            debugOutput = DebugOutput.Height;
         }
         else if (HasFeature(keywords, DefineFeature._DEBUG_OUTPUT_NORMAL))
         {
            debugOutput = DebugOutput.Normal;
         }
         else if (HasFeature(keywords, DefineFeature._DEBUG_OUTPUT_SMOOTHNESS))
         {
            debugOutput = DebugOutput.Smoothness;
         }
         else if (HasFeature(keywords, DefineFeature._DEBUG_OUTPUT_METAL))
         {
            debugOutput = DebugOutput.Metallic;
         }
         else if (HasFeature(keywords, DefineFeature._DEBUG_OUTPUT_AO))
         {
            debugOutput = DebugOutput.AO;
         }
         else if (HasFeature(keywords, DefineFeature._DEBUG_OUTPUT_EMISSION))
         {
            debugOutput = DebugOutput.Emission;
         }
         else if (HasFeature(keywords, DefineFeature._DEBUG_OUTPUT_SPECULAR))
         {
            debugOutput = DebugOutput.Specular;
         }
#if __MICROSPLAT_PROCTEX__
         else if (HasFeature(keywords, DefineFeature._DEBUG_OUTPUT_SPLAT0))
         {
            debugOutput = DebugOutput.ProceduralSplatOutput0;
         }
         else if (HasFeature(keywords, DefineFeature._DEBUG_OUTPUT_SPLAT1))
         {
            debugOutput = DebugOutput.ProceduralSplatOutput1;
         }
         else if (HasFeature(keywords, DefineFeature._DEBUG_OUTPUT_SPLAT2))
         {
            debugOutput = DebugOutput.ProceduralSplatOutput2;
         }
         else if (HasFeature(keywords, DefineFeature._DEBUG_OUTPUT_SPLAT3))
         {
            debugOutput = DebugOutput.ProceduralSplatOutput3;
         }
         else if (HasFeature(keywords, DefineFeature._DEBUG_OUTPUT_SPLAT4))
         {
            debugOutput = DebugOutput.ProceduralSplatOutput4;
         }
         else if (HasFeature(keywords, DefineFeature._DEBUG_OUTPUT_SPLAT5))
         {
            debugOutput = DebugOutput.ProceduralSplatOutput5;
         }
         else if (HasFeature(keywords, DefineFeature._DEBUG_OUTPUT_SPLAT6))
         {
            debugOutput = DebugOutput.ProceduralSplatOutput6;
         }
         else if (HasFeature(keywords, DefineFeature._DEBUG_OUTPUT_SPLAT7))
         {
            debugOutput = DebugOutput.ProceduralSplatOutput7;
         }
         else if (HasFeature(keywords, DefineFeature._DEBUG_OUTPUT_SPLAT0A))
         {
            debugOutput = DebugOutput.ProceduralSplatOutput0A;
         }
         else if (HasFeature(keywords, DefineFeature._DEBUG_OUTPUT_SPLAT1A))
         {
            debugOutput = DebugOutput.ProceduralSplatOutput1A;
         }
         else if (HasFeature(keywords, DefineFeature._DEBUG_OUTPUT_SPLAT2A))
         {
            debugOutput = DebugOutput.ProceduralSplatOutput2A;
         }
         else if (HasFeature(keywords, DefineFeature._DEBUG_OUTPUT_SPLAT3A))
         {
            debugOutput = DebugOutput.ProceduralSplatOutput3A;
         }
         else if (HasFeature(keywords, DefineFeature._DEBUG_OUTPUT_SPLAT4A))
         {
            debugOutput = DebugOutput.ProceduralSplatOutput4A;
         }
         else if (HasFeature(keywords, DefineFeature._DEBUG_OUTPUT_SPLAT5A))
         {
            debugOutput = DebugOutput.ProceduralSplatOutput5A;
         }
         else if (HasFeature(keywords, DefineFeature._DEBUG_OUTPUT_SPLAT6A))
         {
            debugOutput = DebugOutput.ProceduralSplatOutput6A;
         }
         else if (HasFeature(keywords, DefineFeature._DEBUG_OUTPUT_SPLAT7A))
         {
            debugOutput = DebugOutput.ProceduralSplatOutput7A;
         }
#endif
      }

      public override void InitCompiler(string[] paths)
      {
         for (int i = 0; i < paths.Length; ++i)
         {
            string p = paths[i];
            if (p.EndsWith("microsplat_properties_splat.txt"))
            {
               properties_splat = AssetDatabase.LoadAssetAtPath<TextAsset>(p);
            }
            if (p.EndsWith ("microsplat_core_cbuffer.txt"))
            {
               cbuffer = AssetDatabase.LoadAssetAtPath<TextAsset> (p);
            }
         }

      }

      public override void WritePerMaterialCBuffer (string[] features, System.Text.StringBuilder sb)
      {
         sb.AppendLine (cbuffer.text);
         if (perTexSSS || HasFeature(features, "_MESHCOMBINEDUSESSS") || (HasFeature(features, "_SNOWSSS") && (HasFeature(features, "_SNOW"))))
         {
            sb.AppendLine ("      half _SSSScale;");
            sb.AppendLine ("      half _SSSPower;");
            sb.AppendLine ("      half _SSSDistance;");
         }
      }

      public override void WriteProperties(string[] features, System.Text.StringBuilder sb)
      {
         sb.AppendLine(properties_splat.text);
         if (emissiveArray)
         {
            sb.AppendLine("      [NoScaleOffset]_EmissiveMetal (\"Emissive Array\", 2DArray) = \"black\" {}");
         }
         if (packMode != TextureArrayConfig.PackingMode.Fastest)
         {
            sb.AppendLine("      [NoScaleOffset]_SmoothAO (\"Smooth AO Array\", 2DArray) = \"black\" {}");
         }
         if (pbrWorkflow == TextureArrayConfig.PBRWorkflow.Specular)
         {
            sb.AppendLine ("      [NoScaleOffset]_Specular (\"Specular Array\", 2DArray) = \"black\" {}");
         }
         if (perTexSSS || HasFeature (features, "_MESHCOMBINEDUSESSS") || (HasFeature (features, "_SNOWSSS") && (HasFeature (features, "_SNOW"))))
         {
            sb.AppendLine ("      _SSSDistance(\"SSS Distance\", Float) = 1");
            sb.AppendLine ("      _SSSScale(\"SSS Scale\", Float) = 4");
            sb.AppendLine ("      _SSSPower(\"SSS Power\", Float) = 4");

         }
      }

      public override void ComputeSampleCounts(string[] features, ref int arraySampleCount, ref int textureSampleCount, ref int maxSamples, ref int tessellationSamples, ref int depTexReadLevel)
      {
         textureSampleCount += ((int)maxTextureCount) / 4; // control textures


         if (perfMode == PerformanceMode.BestQuality)
         {
            arraySampleCount += disableNormals ? 4 : 8;
            if (emissiveArray)
            {
               arraySampleCount += 4;
            }
            if (packMode != TextureArrayConfig.PackingMode.Fastest)
            {
               arraySampleCount += 4;
            }
            if (pbrWorkflow == TextureArrayConfig.PBRWorkflow.Specular)
            {
               arraySampleCount += 4;
            }
         }
         else if (perfMode == PerformanceMode.Balanced)
         {
            arraySampleCount += disableNormals ? 3 : 6;
            if (emissiveArray)
            {
               arraySampleCount += 3;
            }
            if (packMode != TextureArrayConfig.PackingMode.Fastest)
            {
               arraySampleCount += 3;
            }
            if (pbrWorkflow == TextureArrayConfig.PBRWorkflow.Specular)
            {
               arraySampleCount += 3;
            }
         }
         else if (perfMode == PerformanceMode.Fastest)
         {
            arraySampleCount += disableNormals ? 2 : 4;
            if (emissiveArray)
            {
               arraySampleCount += 2;
            }
            if (packMode != TextureArrayConfig.PackingMode.Fastest)
            {
               arraySampleCount += 2;
            }
            if (pbrWorkflow == TextureArrayConfig.PBRWorkflow.Specular)
            {
               arraySampleCount += 2;
            }
         }
         if (perPixelNormal)
         {
            textureSampleCount++;
         }
      }

      static GUIContent CPerTexUV = new GUIContent("UV Scale", "UV Scale for the texture. You may need to change your sampler settings if this is enabled.");
      static GUIContent CPerTexUVOffset = new GUIContent("UV Offset", "UV Offset for each texture");
      static GUIContent CPerTexUVRotation = new GUIContent ("UV Rotation", "UV Rotation for each texture");
      static GUIContent CPerTexInterp = new GUIContent("Interpolation Contrast", "Control blend of sharpness vs other textures");
      static GUIContent CPerTexTint = new GUIContent("Tint", "Tint color for albedo");
      static GUIContent CPerTexNormStr = new GUIContent("Normal Strength", "Increase or decrease strength of normal mapping");
      static GUIContent CPerTexAOStr = new GUIContent("AO Strength", "Increase or decrease strength of the AO map");
      static GUIContent CPerTexSmoothStr = new GUIContent("Smoothness Strength", "Increase or decrease strength of the smoothness");
      static GUIContent CPerTexMetallic = new GUIContent("Metallic", "Set the metallic value of the texture");
      static GUIContent CPerTexBrightness = new GUIContent("Brightness", "Brightness of texture");
      static GUIContent CPerTexContrast = new GUIContent("Contrast", "Contrast of texture");
      static GUIContent CPerTexSaturation = new GUIContent("Saturation", "Saturation of the texture");
      static GUIContent CPerTexHeightOffset = new GUIContent("Height Offset", "Allows you to adjust the heightfield of each texture up or down");
      static GUIContent CPerTexHeightContrast = new GUIContent("Height Contrast", "Allows you to adjust the contrast of the height map");
      static GUIContent CPerTexInnerDarken = new GUIContent ("Fuzzy Direct Darken", "Darken or lighten areas facing camera");
      static GUIContent CPerTexEdgeLighten = new GUIContent ("Fuzzy Edge Lighten", "Darken or lighten areas edge on to the camera");
      static GUIContent CPerTexFuzzyPower = new GUIContent ("Fuzzy Power", "Controls fresnel width of fuzzy shading");
      static GUIContent CPerTexSSSTint = new GUIContent ("SSS Tint", "Subsurface Scattering Tint");
      static GUIContent CPerTexSSSThickness = new GUIContent ("SSS Thickness", "Subsurface Scattering Thickness");

      public override void DrawPerTextureGUI(int index, MicroSplatKeywords keywords, Material mat, MicroSplatPropData propData)
      {

         InitPropData (0, propData, new Color (1.0f, 1.0f, 0.0f, 0.0f)); // uvscale2, uvOffset
         InitPropData (1, propData, new Color (1.0f, 1.0f, 1.0f, 0.0f)); // tint, interp contrast
         InitPropData (2, propData, new Color (1.0f, 0.0f, 1.0f, 0.0f)); // norm str, smooth str, ao str, metal values
         InitPropData (3, propData, new Color (0.0f, 1.0f, 0.4f, 1.0f)); // brightness, contrast, porosity, foam
         InitPropData (9, propData, new Color (1, 1, 1, 1));
         InitPropData (10, propData, new Color (1, 1, 1, 1));
         InitPropData (16, propData, new Color (0, 0, 0, 0));
         InitPropData (17, propData, new Color (0, 0, 1, 0));
         InitPropData (18, propData, new Color (1, 1, 1, 1));

         perTexUVScale = DrawPerTexVector2Vector2 (index, 0, GetFeatureName (DefineFeature._PERTEXUVSCALEOFFSET), 
            keywords, propData, CPerTexUV, CPerTexUVOffset);

         if (keywords.IsKeywordEnabled("_TRIPLANAR"))
         {
            perTexUVRotation = DrawPerTexFloatSlider (index, 16, GetFeatureName (DefineFeature._PERTEXUVROTATION),
            keywords, propData, Channel.R, CPerTexUVRotation, -3.15f, 3.15f);
            DrawPerTexFloatSlider (index, 16, GetFeatureName (DefineFeature._PERTEXUVROTATION),
            keywords, propData, Channel.G, CPerTexUVRotation, -3.15f, 3.15f, false);
            DrawPerTexFloatSlider (index, 16, GetFeatureName (DefineFeature._PERTEXUVROTATION),
            keywords, propData, Channel.B, CPerTexUVRotation, -3.15f, 3.15f, false);
         }
         else
         {
            perTexUVRotation = DrawPerTexFloatSlider (index, 16, GetFeatureName (DefineFeature._PERTEXUVROTATION),
            keywords, propData, Channel.R, CPerTexUVRotation, -3.15f, 3.15f);
         }
         



         if (!disableHeightBlend)
         {
            perTexInterpContrast = DrawPerTexFloatSlider (index, 1, GetFeatureName (DefineFeature._PERTEXINTERPCONTRAST),
               keywords, propData, Channel.A, CPerTexInterp, -1.0f, 1.0f);
         }

         perTexTint = DrawPerTexColor (index, 1, GetFeatureName (DefineFeature._PERTEXTINT),
            keywords, propData, CPerTexTint, false);

         perTexBrightness = DrawPerTexFloatSlider (index, 3, GetFeatureName (DefineFeature._PERTEXBRIGHTNESS),
            keywords, propData, Channel.R, CPerTexBrightness, -1.0f, 1.0f);

         perTexContrast = DrawPerTexFloatSlider (index, 3, GetFeatureName (DefineFeature._PERTEXCONTRAST),
            keywords, propData, Channel.G, CPerTexContrast, 0.1f, 3.0f);

         perTexSaturation = DrawPerTexFloatSlider (index, 9, GetFeatureName (DefineFeature._PERTEXSATURATION),
            keywords, propData, Channel.A, CPerTexSaturation, 0.0f, 2.0f);

         perTexNormStr = DrawPerTexFloatSlider (index, 2, GetFeatureName (DefineFeature._PERTEXNORMSTR),
            keywords, propData, Channel.R, CPerTexNormStr, 0.0f, 3.0f);
      
         perTexSmoothStr = DrawPerTexFloatSlider (index, 2, GetFeatureName (DefineFeature._PERTEXSMOOTHSTR),
            keywords, propData, Channel.G, CPerTexSmoothStr, -1.0f, 1.0f);

         perTexAOStr = DrawPerTexFloatSlider (index, 2, GetFeatureName (DefineFeature._PERTEXAOSTR),
            keywords, propData, Channel.B, CPerTexAOStr, 0.5f, 3.0f);

         perTexMetallic = DrawPerTexFloatSlider (index, 2, GetFeatureName (DefineFeature._PERTEXMETALLIC),
            keywords, propData, Channel.A, CPerTexMetallic, 0, 1);

         perTexHeightOffset = DrawPerTexFloatSlider (index, 10, GetFeatureName (DefineFeature._PERTEXHEIGHTOFFSET),
            keywords, propData, Channel.B, CPerTexHeightOffset, 0, 2);

         perTexHeightContrast = DrawPerTexFloatSlider (index, 10, GetFeatureName (DefineFeature._PERTEXHEIGHTCONTRAST),
            keywords, propData, Channel.A, CPerTexHeightContrast, 0.2f, 4);

         perTexSSS = DrawPerTexFloatSlider(index, 18, GetFeatureName(DefineFeature._PERTEXSSS),
            keywords, propData, Channel.A, CPerTexSSSThickness, 0.0f, 1.0f);
         bool old = GUI.enabled;
         GUI.enabled = perTexSSS;
			DrawPerTexColorNoToggle(index, 18, propData, CPerTexSSSTint);
         GUI.enabled = old;


         perTexFuzzyShading = DrawPerTexFloatSlider (index, 17, GetFeatureName (DefineFeature._PERTEXFUZZYSHADE),
            keywords, propData, Channel.R, CPerTexInnerDarken, 0.0f, 1.0f);
         DrawPerTexFloatSliderNoToggle (index, 17, GetFeatureName (DefineFeature._PERTEXFUZZYSHADE),
            keywords, propData, Channel.G, CPerTexEdgeLighten, 0.0f, 1.0f);
         DrawPerTexFloatSliderNoToggle (index, 17, GetFeatureName (DefineFeature._PERTEXFUZZYSHADE),
            keywords, propData, Channel.B, CPerTexFuzzyPower, 0.2f, 16.0f);


      }
   }   
}

