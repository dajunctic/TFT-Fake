
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using static ShaderCrew.TheToonShader.ShaderUtils;

namespace ShaderCrew.TheToonShader
{
    public class TheToonShaderGenerator
    {

        public TheToonShaderGenerator() { }

        //public bool _USE_OPTIMIZATION_DEFINES;
        public bool _ENABLE_TOON_SHADING;

        public int _SHADING_FUNCTION;
        public bool _SHADING_TERMINATORPOSITION;
        public bool _ENABLE_SHADOWS;
        public bool _ENABLE_CAST_SHADOWS;

        public bool _ENABLE_SPECULAR;
        public int _SPECULAR_BLENDING = -1;

        public bool _ENABLE_RIM;
        public int _RIM_BLENDING = -1;

        public bool _SUM_LIGHTS_BEFORE_POSTERIZATION;
        public bool _SHADING_USE_LIGHT_COLORS;
        public bool _SPECULAR_USE_LIGHT_COLORS;
        public bool _STYLING_SPECULAR_USE_LIGHT_COLORS;      


        public bool _ENABLE_STYLING;

        public int _STYLING_CASTSHADOWS_SYNC_WITH_OTHER_STYLING = -1;

        public bool _ENABLE_STYLING_DISTANCEFADE;

        public bool _ENABLE_SHADING_STYLING;
        public bool _SHADING_STYLING_TERMINATORPOSITION;
        public int _SHADING_STYLING_BLENDING = -1;
        public int _SHADING_STYLING_DRAWSPACE = -1;
        public int _SHADING_STYLING_COORDINATESYSTEM = -1;
        public int _SHADING_STYLE = -1;
        public int _SHADING_STYLING_NUMBER_OF_CELLS_HATCHING = -1;
        public bool _SHADING_STYLING_RANDOMIZER;
        public bool _SHADING_STYLING_RANDOMIZER_PERLIN;
        public bool _SHADING_STYLING_RANDOMIZER_PERLIN_FLOORED;
        public bool _SHADING_STYLING_RANDOMIZER_WHITE;
        public bool _SHADING_STYLING_RANDOMIZER_WHITE_FLOORED;


        public bool _ENABLE_CASTSHADOWS_STYLING;
        public int _CASTSHADOWS_STYLING_BLENDING = -1;
        public int _CASTSHADOWS_STYLING_DRAWSPACE = -1;
        public int _CASTSHADOWS_STYLING_COORDINATESYSTEM = -1;
        public int _CASTSHADOWS_STYLE = -1;
        public int _CASTSHADOWS_STYLING_NUMBER_OF_CELLS_HATCHING = -1;
        public bool _CASTSHADOWS_STYLING_RANDOMIZER;
        public bool _CASTSHADOWS_STYLING_RANDOMIZER_PERLIN;
        public bool _CASTSHADOWS_STYLING_RANDOMIZER_PERLIN_FLOORED;
        public bool _CASTSHADOWS_STYLING_RANDOMIZER_WHITE;
        public bool _CASTSHADOWS_STYLING_RANDOMIZER_WHITE_FLOORED;


        public bool _ENABLE_SPECULAR_STYLING;
        public int _SPECULAR_STYLING_BLENDING = -1;
        public int _SPECULAR_STYLING_DRAWSPACE = -1;
        public int _SPECULAR_STYLING_COORDINATESYSTEM = -1;
        public int _SPECULAR_STYLE = -1;
        public bool _SPECULAR_STYLING_RANDOMIZER;
        public bool _SPECULAR_STYLING_RANDOMIZER_PERLIN;
        public bool _SPECULAR_STYLING_RANDOMIZER_PERLIN_FLOORED;
        public bool _SPECULAR_STYLING_RANDOMIZER_WHITE;
        public bool _SPECULAR_STYLING_RANDOMIZER_WHITE_FLOORED;


        public bool _ENABLE_RIM_STYLING;
        public int _RIM_STYLING_BLENDING = -1;
        public int _RIM_STYLING_DRAWSPACE = -1;
        public int _RIM_STYLING_COORDINATESYSTEM = -1;
        public int _RIM_STYLE = -1;
        public bool _RIM_STYLING_RANDOMIZER;
        public bool _RIM_STYLING_RANDOMIZER_PERLIN;
        public bool _RIM_STYLING_RANDOMIZER_PERLIN_FLOORED;
        public bool _RIM_STYLING_RANDOMIZER_WHITE;
        public bool _RIM_STYLING_RANDOMIZER_WHITE_FLOORED;


#if USING_URP2D
        public int _MAX_LIGHT_COUNT = -1;
        public int _CELL_METHOD = -1;
        public bool _ENABLE_MAINTEX_POSTERIZATION;

        public bool _EMISSION;
        public bool _NORMALMAP;

        public bool _USE_MAINTEX;
        public bool _CONVERT_NORMAL_TO_ALBEDO;

        public bool _USE_CORE_SHADOW_COLOR;
        public bool _ENABLE_LIGHT_PARTITIONING;
        public int _ROUND_METHOD = -1;

#endif


        public bool _ENABLE_OUTLINE;


        public static bool CheckIfShaderIsAlreadyOptimized(Material material)
        {
            if (material != null && material.shader != null)
            {
                string dataPathWithoutAssets = Application.dataPath;
                if (dataPathWithoutAssets.EndsWith("/Assets"))
                {
                    dataPathWithoutAssets = dataPathWithoutAssets.Substring(0, dataPathWithoutAssets.LastIndexOf("/Assets"));
                }
                string realPath = null;

                string shaderAssetPath = UnityEditor.AssetDatabase.GetAssetPath(material.shader);
                string fileName = Path.GetFileName(shaderAssetPath);
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(shaderAssetPath);
                string extension = Path.GetExtension(shaderAssetPath);

                if (fileName != null && !fileName.Equals("") && extension != null && extension.Equals(".shader"))
                {
                    realPath = dataPathWithoutAssets + "/" + shaderAssetPath;
                }

                //if (material.shader.name == "Dev/TheToonShaderDev")
                //{
                //    realPath = dataPathWithoutAssets + "/Packages/com.shadercrew.the-toon-shader.core/Scripts/Shaders/Native/URP/TheToonShader_URP2020.shader";
                //}
                //else // remove in final
                //{
                //    Debug.Log("Remove me in final");
                //    return false;
                //}


                //Debug.Log(realPath);

                string fileContent = File.ReadAllText(realPath);
                //Debug.Log(fileContent);

                //string pattern = @"\b#define\s+_USE_OPTIMIZATION_DEFINES\s+1\b";
                string pattern = @"#\bdefine\s+_USE_OPTIMIZATION_DEFINES\s+1\b";
                Regex regex = new Regex(pattern);

                bool lineFound = regex.IsMatch(fileContent);


                return lineFound;
            }

            return false;
        }


        public static bool CheckIfShaderIsAlreadyOptimizedPropertyBased(Material material)
        {
            if (material != null && material.shader != null)
            {
                return material.HasProperty("_IsOptimized");
            }

            return false;
        }

        public Shader GenerateOptimizedShaderFile(Material material) // return generated shader
        {
            if(material != null && material.shader != null)
            {

                RenderPipelineOptions rp = getCurrentRenderPipeline();


                string shaderAssetPath = UnityEditor.AssetDatabase.GetAssetPath(material.shader);
                string fileName = Path.GetFileName(shaderAssetPath);
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(shaderAssetPath);
                string extension = Path.GetExtension(shaderAssetPath);


                string dataPathWithoutAssets = Application.dataPath;
                if (dataPathWithoutAssets.EndsWith("/Assets"))
                {
                    dataPathWithoutAssets = dataPathWithoutAssets.Substring(0, dataPathWithoutAssets.LastIndexOf("/Assets"));
                }

                string realPath = null;


                if (fileName != null && !fileName.Equals("") && extension != null && (extension.Equals(".shader") || extension.Equals(".shadergraph")))
                {
                    realPath = dataPathWithoutAssets + "/" + shaderAssetPath;
                }


                ////cause debug:

                ////realPath = dataPathWithoutAssets + "/Packages/com.shadercrew.the-toon-shader.core/Scripts/Shaders/Native/URP/TheToonShader_URP2020.shader";
                //if(material.shader.name == "Dev/TheToonShaderDev")
                //{
                //    realPath = dataPathWithoutAssets + "/Packages/com.shadercrew.the-toon-shader.core/Scripts/Shaders/Native/URP/TheToonShader_URP2020.shader";
                //}




                if (realPath != null && !realPath.Equals(""))
                {
                    string content = null;

                    if (rp == RenderPipelineOptions.URP2D)
                    {
                        content = ShaderGraphToShaderExporterUtil.GenerateShaderCode(shaderAssetPath, fileNameWithoutExtension);
                    } 
                    else
                    {
                        content = File.ReadAllText(realPath);
                    }
                    string optimizedShaderName = "";

                    //string patternShaderName = @"Shader\s*""([^""]+)""";
                    //string patternShaderName = @"(\r\n|\r|\n)\s*Shader\s*""([^""]+)""";
                    //Regex regex = new Regex(patternShaderName);
                    Regex regex = new Regex(@"^\s*Shader\s*""([^""]+)""", RegexOptions.Multiline);

                    Match matchShaderName = regex.Match(content);
                    if (matchShaderName.Success)
                    {
                        string shaderName = matchShaderName.Groups[1].Value;
                        string modifiedShaderName = shaderName + "_" + material.name + "_Optimized";

                        optimizedShaderName = modifiedShaderName;
                        //Debug.Log("optimizedShaderName: " + optimizedShaderName);
                        List<string> parts = optimizedShaderName.Split('/').ToList();
                        parts.Insert(1, "Optimized");
                        optimizedShaderName = string.Join("/", parts);

                        content = regex.Replace(content, matchShaderName.Value.Replace(shaderName, optimizedShaderName));

                    }

                    string patternProperties = @"Properties\s*\{\s*";
                    Match matchProperties = Regex.Match(content, patternProperties);

                    if (matchProperties.Success)
                    {
                        int insertPosition = matchProperties.Index + matchProperties.Length;

                        content = content.Insert(insertPosition, "[HideInInspector] _IsOptimized(\"_IsOptimized\", Float) = 1.0" + "\n");
                    }

                    string pattern = @"SubShader\s*\{\s*";

                    Match match = Regex.Match(content, pattern);

                    if (match.Success)
                    {
                        int insertPosition = match.Index + match.Length;

                        StringBuilder defineDefinitions = new StringBuilder(2000);

                        if (rp == RenderPipelineOptions.BiRP)
                        {
                            defineDefinitions.AppendLine("\tCGINCLUDE");
                        } 
                        else
                        {
                            defineDefinitions.AppendLine("\tHLSLINCLUDE");
                        }

                        defineDefinitions.AppendLine("\t\t#define _USE_OPTIMIZATION_DEFINES 1");

                        Type type = this.GetType();
                        FieldInfo[] properties = type.GetFields();

                        foreach (FieldInfo property in properties)
                        {
                            //if(!property.Name.Equals("_ENABLE_OUTLINE"))
                            //{

                            if (property.GetValue(this).GetType() == typeof(bool))
                            {
                                //Debug.Log("name: " + property.Name + " - " + (bool)property.GetValue(this));
                                if ((bool)property.GetValue(this) == true)
                                {
                                        defineDefinitions.AppendLine("\t\t#define " + property.Name + " 1");
                                }
                            }
                            else if (property.GetValue(this).GetType() == typeof(int))
                            {
                                    if ((int)property.GetValue(this) != -1)
                                    {
                                        defineDefinitions.AppendLine("\t\t#define " + property.Name + " " + (int)property.GetValue(this));
                                    }
                                //}
                            }

                        }

                        if (rp == RenderPipelineOptions.BiRP)
                        {
                            defineDefinitions.AppendLine("\tENDCG");
                        }
                        else
                        {
                            defineDefinitions.AppendLine("\tENDHLSL");
                        }

                        string modifiedContent = content.Insert(insertPosition, "\n" + defineDefinitions.ToString() + "\n");


                        //if (_ENABLE_OUTLINE == false)
                        //{
                        //    string outlinePassPattern = @"Pass\s*\{\s*Name\s*""Outline""[\s\S]*?ENDCG[\s\S]*?\}";
                        //    modifiedContent = Regex.Replace(modifiedContent, outlinePassPattern, "", RegexOptions.Singleline);
                        //}



                        //string filePathModfied = Application.dataPath + "/ToonShaderBuilder/ShaderCreatorTest/ShaderCreatorPlain_URP2020Optimized.shader";

                        string filePathModfied;

                        if (rp == RenderPipelineOptions.URP2D && material.HasProperty(TheToonShaderConstants.THETOONSHADER2D_IDENTIFIER_PROPERTY))
                        {
                            filePathModfied = dataPathWithoutAssets + "/Packages/com.shadercrew.the-toon-shader.2d/Scripts/Resources/OptimizedShaders/";
                        }
                        else
                        {
                            filePathModfied = dataPathWithoutAssets + "/Packages/com.shadercrew.the-toon-shader.3d/Scripts/Resources/OptimizedShaders/";
                        }

                        if (!Directory.Exists(filePathModfied))
                        {
                            Directory.CreateDirectory(filePathModfied);
                        }

                        filePathModfied += fileNameWithoutExtension + "_" + material.name +  "_Optimized";
                        filePathModfied += ".shader";

                        File.WriteAllText(filePathModfied, modifiedContent);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                        Shader optimizedShader = Shader.Find(optimizedShaderName);
                        //Debug.Log(optimizedShaderName);
                        return optimizedShader;
                    }


                }
            }

            return null;

        }
    }
}