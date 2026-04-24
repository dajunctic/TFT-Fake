using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace ShaderCrew.TheToonShader
{
    public static class ReplaceAndSyncUtils
    {
        public static void AddTheToonShaderInstancePrefix(Material materialInstance)
        {
            string name = materialInstance.name;
            name = name.Replace(" (Instance)", "");
            if (!name.Contains(TheToonShaderConstants.THETOONSHADER_INSTANCE_PREFIX))
            {
                materialInstance.name = name + " (" + TheToonShaderConstants.THETOONSHADER_INSTANCE_PREFIX + ")";
            }
        }

        private static Material getFirstInstancedMaterial(Transform[] transforms)
        {
            if (transforms != null && transforms.Length > 0)
            {
                foreach (Transform transform in transforms)
                {
                    if (transform != null)
                    {
                        Renderer renderer = transform.GetComponent<Renderer>();
                        if (renderer != null)
                        {
                            Material[] materials = renderer.sharedMaterials;
                            if (materials != null && materials.Length > 0)
                            {
                                foreach (Material material in materials)
                                {
                                    if (IsTheToonShaderMaterial(material))
                                    {
                                        return material;
                                    }
                                }
                            }

                        }
                    }
                }
            }
            return null;
        }


        /// <summary>
        /// Gets all materials, including the ones in various LOD levels, which are associated with given transforms
        public static List<Material> GetAllTheToonShaderMaterialsFromTransforms(Transform[] transforms, Material referenceMaterial)
        {
            List<Material> materialList = new List<Material>();

            foreach (Transform transform in transforms)
            {
                //Renderer rendererNonLOD = transform.GetComponent<Renderer>();
                Renderer[] rendererNonLODArray = transform.GetComponentsInChildren<Renderer>();
                for (int i = 0; i < rendererNonLODArray.Length; i++)
                {
                    if (transform.GetComponent<LODGroup>() != null)
                    {
                        foreach (LOD lod in transform.GetComponent<LODGroup>().GetLODs())
                        {
                            foreach (Renderer renderer in lod.renderers)
                            {
                                if (renderer == rendererNonLODArray[i])
                                {
                                    rendererNonLODArray[i] = null;
                                }
                                if (renderer != null && renderer.sharedMaterials.Length > 0)
                                {
                                    foreach (Material material in renderer.sharedMaterials)
                                    {
                                        if (material != null && IsTheToonShaderMaterial(material))
                                        {
                                            if (!materialList.Contains(material))
                                            {
                                                materialList.Add(material);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (rendererNonLODArray[i] != null)
                    {
                        Material[] materials = rendererNonLODArray[i].sharedMaterials;
                        if (materials.Length > 0)
                        {
                            foreach (Material material in materials)
                            {
                                if (material != null && IsTheToonShaderMaterial(material))
                                {
                                    if (!materialList.Contains(material))
                                    {
                                        materialList.Add(material);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return materialList;
        }

        private static bool IsTheToonShaderMaterial(Material material)
        {
            return (material != null && material.HasProperty(TheToonShaderConstants.THETOONSHADER_IDENTIFIER_PROPERTY));

        }


        public static void UpdateTheToonShaderMaterialPropertiesAndKeywords(List<Material> materials, Material referenceMaterial,
                                                                            Dictionary<string, bool> propertyNameToBoolDictionary,
                                                                            bool forceUpdateOfAllPropertiesAndKeywords = false)
        {
            if(forceUpdateOfAllPropertiesAndKeywords)
            {

                bool doKeywordSync = false; //a bit of a hack, make cleaner 1

                List<string> properties = new List<string>();
                foreach (string propertyName in ShaderUtils.ALL_PROPERITES)
                {
                    bool propertyInsideDictionary = propertyNameToBoolDictionary.TryGetValue(propertyName, out bool propertyToggled);
                    if (propertyInsideDictionary && propertyName != null)
                    {
                        if (propertyToggled)
                        {
                            properties.Add(propertyName);
                            //a bit of a hack, make cleaner 2
                            if(propertyName.Equals("_ShadingMode"))
                            {
                                doKeywordSync = true;
                            }
                        }
                    }
                }
                foreach (Material material in materials)
                {
                    if (properties.Count > 0)
                    {
                        updateMaterialProperties(material, referenceMaterial, properties);
                    }

                    if(doKeywordSync)//a bit of a hack, make cleaner 3
                    {
                        updateMaterialKeywords(material, referenceMaterial, ShaderUtils.THETOONSHADER_KEYWORDS_LIST);
                    }

                }

            }
            else
            {
                Material firstInstancedMaterial = null;
                foreach (Material material in materials)
                {
                    if (IsTheToonShaderMaterial(material))
                    {
                        firstInstancedMaterial = material;
                        break;
                    }
                }
                if (firstInstancedMaterial != null)
                {
                    List<string> namesOfChangedProperties = GetNamesOfAllChangedPropertyValues(firstInstancedMaterial, referenceMaterial, propertyNameToBoolDictionary);
                    List<string> namesOfChangedKeywords = GetNamesOfAllChangedKeywordValues(firstInstancedMaterial, referenceMaterial);
                    if ((namesOfChangedProperties != null && namesOfChangedProperties.Count > 0) || (namesOfChangedKeywords != null && namesOfChangedKeywords.Count > 0))
                    {
                        foreach (Material material in materials)
                        {
                            if (namesOfChangedProperties.Count > 0)
                            {
                                updateMaterialProperties(material, referenceMaterial, namesOfChangedProperties);
                            }
                            if (namesOfChangedKeywords.Count > 0)
                            {
                                if(namesOfChangedProperties.Contains("_ShadingMode")) //a bit of a hack, make cleaner 4
                                {
                                    updateMaterialKeywords(material, referenceMaterial, namesOfChangedKeywords);
                                }
                            }
                        }
                    }
                }
            }

        }

        public static void UpdateTheToonShaderMaterialPropertiesAndKeywords(Transform[] transforms, Material referenceMaterial,
                                                                            Dictionary<string, bool> propertyNameToBoolDictionary,
                                                                            bool forceUpdateOfAllPropertiesAndKeywords = false)
        {
            bool doKeywordSync = false; //a bit of a hack, make cleaner 1


            if (forceUpdateOfAllPropertiesAndKeywords)
            {
                //Debug.Log("UpdateTheToonShaderMaterialPropertiesAndKeywords forced");
                List<string> properties = new List<string>();
                foreach (string propertyName in ShaderUtils.ALL_PROPERITES)
                {
                    bool propertyInsideDictionary = propertyNameToBoolDictionary.TryGetValue(propertyName, out bool propertyToggled);
                    if (propertyInsideDictionary && propertyName != null)
                    {
                        if (propertyToggled)
                        {
                            properties.Add(propertyName);

                            if (propertyName.Equals("_ShadingMode")) //a bit of a hack, make cleaner 2
                            {
                                doKeywordSync = true;
                            }
                        }
                    }
                }
                List<Material> allTheToonShaderMaterials = GetAllTheToonShaderMaterialsFromTransforms(transforms, referenceMaterial);
                //Debug.Log("Toon Shader Count: " + allTheToonShaderMaterials.Count);
                if (allTheToonShaderMaterials != null && allTheToonShaderMaterials.Count > 0)
                {
                    foreach (Material material in allTheToonShaderMaterials)
                    {
                        //Debug.Log("Material name: " + material.name);
                        if (properties.Count > 0)
                        {
                            updateMaterialProperties(material, referenceMaterial, properties);
                        }
                        if (doKeywordSync)//a bit of a hack, make cleaner 3
                        {
                            updateMaterialKeywords(material, referenceMaterial, ShaderUtils.THETOONSHADER_KEYWORDS_LIST);
                        }

                    }
                }
            }
            else
            {
                //Debug.Log("UpdateTheToonShaderMaterialPropertiesAndKeywords");

                Material firstInstancedMaterial = getFirstInstancedMaterial(transforms);
                if (firstInstancedMaterial != null)
                {
                    List<string> namesOfChangedProperties = GetNamesOfAllChangedPropertyValues(firstInstancedMaterial, referenceMaterial, propertyNameToBoolDictionary);
                    List<string> namesOfChangedKeywords = GetNamesOfAllChangedKeywordValues(firstInstancedMaterial, referenceMaterial);
                    if ((namesOfChangedProperties != null && namesOfChangedProperties.Count > 0) || (namesOfChangedKeywords != null && namesOfChangedKeywords.Count > 0))
                    {
                        List<Material> allTheToonShaderMaterials = GetAllTheToonShaderMaterialsFromTransforms(transforms, referenceMaterial);
                        if(allTheToonShaderMaterials != null && allTheToonShaderMaterials.Count > 0)
                        {
                            foreach (Material material in allTheToonShaderMaterials)
                            {
                                if (namesOfChangedProperties.Count > 0)
                                {
                                    updateMaterialProperties(material, referenceMaterial, namesOfChangedProperties);
                                }
                                if (namesOfChangedKeywords.Count > 0)
                                {
                                    if (namesOfChangedProperties.Contains("_ShadingMode")) //a bit of a hack, make cleaner 4
                                    {
                                        updateMaterialKeywords(material, referenceMaterial, namesOfChangedKeywords);
                                    }
                                }
                            }
                        }
                    }
                }
            }



        }



        private static void updateMaterialProperties(Material instancedMaterial, Material referenceMaterial, List<string> namesOfChangedProperties)
        {
            if (instancedMaterial != null && referenceMaterial != null && namesOfChangedProperties != null && namesOfChangedProperties.Count > 0)
            {
                Shader shader = referenceMaterial.shader;

                foreach (string propertyName in namesOfChangedProperties)
                {
                    if (referenceMaterial.HasProperty(propertyName) && instancedMaterial.HasProperty(propertyName))
                    {
                        ShaderPropertyType shaderPropertyType = shader.GetPropertyType(shader.FindPropertyIndex(propertyName));


                        if (shaderPropertyType == ShaderPropertyType.Float || shaderPropertyType == ShaderPropertyType.Range)
                        {

                            float value = referenceMaterial.GetFloat(propertyName);
                            instancedMaterial.SetFloat(propertyName, value);
                        }
                        else if (shaderPropertyType == ShaderPropertyType.Color)
                        {
                            Color value = referenceMaterial.GetColor(propertyName);
                            instancedMaterial.SetColor(propertyName, value);
                        }
                        else if (shaderPropertyType == ShaderPropertyType.Texture)
                        {
                            Texture texture = referenceMaterial.GetTexture(propertyName);
                            instancedMaterial.SetTexture(propertyName, texture);
                            Vector2 offset = referenceMaterial.GetTextureOffset(propertyName);
                            instancedMaterial.SetTextureOffset(propertyName, offset);
                            Vector2 scale = referenceMaterial.GetTextureScale(propertyName);
                            instancedMaterial.SetTextureScale(propertyName, scale);
                        }
                        else if (shaderPropertyType == ShaderPropertyType.Vector)
                        {
                            Vector4 vector = referenceMaterial.GetVector(propertyName);
                            instancedMaterial.SetVector(propertyName, vector);
                        }
                    }
                }
            }
        }


        private static void updateMaterialKeywords(Material instancedMaterial, Material referenceMaterial, List<string> namesOfChangedKeywords)
        {
            foreach (string keyword in namesOfChangedKeywords)
            {
                bool isKeywordEnabled = referenceMaterial.IsKeywordEnabled(keyword);
                if (isKeywordEnabled)
                {
                    instancedMaterial.EnableKeyword(keyword);
                }
                else
                {
                    instancedMaterial.DisableKeyword(keyword);
                }
            }
        }

        private static List<string> GetNamesOfAllChangedPropertyValues(Material instancedMaterial,
                                                                        Material referenceMaterial,
                                                                        Dictionary<string, bool> propertyNameToBoolDictionary)
        {
            if (instancedMaterial != null && referenceMaterial != null)
            {
                if (IsTheToonShaderMaterial(instancedMaterial) && IsTheToonShaderMaterial(referenceMaterial))
                {
                    Shader shader = referenceMaterial.shader;
                    List<string> namesOfChangedProperties = new List<string>();
                    foreach (string propertyName in ShaderUtils.ALL_PROPERITES)
                    {
                        if (referenceMaterial.HasProperty(propertyName) && instancedMaterial.HasProperty(propertyName))
                        {
                            bool propertyInsideDictionary = propertyNameToBoolDictionary.TryGetValue(propertyName, out bool propertyToggled);
                            if (propertyInsideDictionary && propertyName != null)
                            {
                                if (propertyToggled)
                                {
                                    ShaderPropertyType shaderPropertyType = shader.GetPropertyType(referenceMaterial.shader.FindPropertyIndex(propertyName));

                                    if (shaderPropertyType == ShaderPropertyType.Float || shaderPropertyType == ShaderPropertyType.Range)
                                    {
                                        float instancedValue = instancedMaterial.GetFloat(propertyName);
                                        float referenceValue = referenceMaterial.GetFloat(propertyName);
                                        if ((instancedValue != referenceValue)) // || force)
                                        {
                                            namesOfChangedProperties.Add(propertyName);
                                        }
                                    }
                                    else if (shaderPropertyType == ShaderPropertyType.Color)
                                    {
                                        Color instancedColor = instancedMaterial.GetColor(propertyName);
                                        Color referenceColor = referenceMaterial.GetColor(propertyName);
                                        if (!instancedColor.Equals(referenceColor))
                                        {
                                            namesOfChangedProperties.Add(propertyName);
                                        }
                                    }
                                    else if (shaderPropertyType == ShaderPropertyType.Texture)
                                    {
                                        Texture instancedTexture = instancedMaterial.GetTexture(propertyName);
                                        Texture referenceTexture = referenceMaterial.GetTexture(propertyName);
                                        if (instancedTexture != null && referenceTexture != null)
                                        {
                                            if (!String.Equals(instancedTexture.name, referenceTexture.name))
                                            {
                                                namesOfChangedProperties.Add(propertyName);
                                            }
                                            else if (instancedMaterial.GetTextureOffset(propertyName) != referenceMaterial.GetTextureOffset(propertyName)
                                            || instancedMaterial.GetTextureScale(propertyName) != referenceMaterial.GetTextureScale(propertyName))
                                            {
                                                namesOfChangedProperties.Add(propertyName);
                                            }
                                        }
                                        else if ((instancedTexture != null) != (referenceTexture != null))
                                        {
                                            namesOfChangedProperties.Add(propertyName);
                                        }
                                    }
                                    else if (shaderPropertyType == ShaderPropertyType.Vector)
                                    {
                                        Vector4 instancedVector = instancedMaterial.GetVector(propertyName);
                                        Vector4 referenceVector = referenceMaterial.GetVector(propertyName);

                                        if (!instancedVector.Equals(referenceVector))
                                        {
                                            namesOfChangedProperties.Add(propertyName);
                                        }
                                    }
                                }
                            }
                        }

                    }
                    return namesOfChangedProperties;
                }
            }
            return null;
        }


        private static List<string> GetNamesOfAllChangedKeywordValues(Material instancedMaterial, Material referenceMaterial)
        {
            if (instancedMaterial != null && referenceMaterial != null)
            {
                if (IsTheToonShaderMaterial(instancedMaterial) && IsTheToonShaderMaterial(referenceMaterial))
                {
                    List<string> namesOfChangedProperties = new List<string>();
                    foreach (string keyword in ShaderUtils.THETOONSHADER_KEYWORDS_LIST)
                    {
                        bool instancedValue = instancedMaterial.IsKeywordEnabled(keyword);
                        bool referenceValue = referenceMaterial.IsKeywordEnabled(keyword);
                        if (instancedValue != referenceValue)
                        {
                            namesOfChangedProperties.Add(keyword);
                        }
                    }
                    return namesOfChangedProperties;
                }
            }
            return null;
        }

    }
}