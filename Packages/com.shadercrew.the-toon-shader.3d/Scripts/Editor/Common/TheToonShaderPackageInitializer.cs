#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

namespace ShaderCrew.TheToonShader
{
    [InitializeOnLoad]
    public static class TheToonShaderPackageInitializer
    {
        //private const string CurrentPackageVersion = "1.3.1";
        private const string VersionKey = "IsTheToonShaderPackageInitialized";

        static TheToonShaderPackageInitializer()
        {
            EditorApplication.delayCall += RunInitialization;
            Events.registeredPackages += OnPackagesRegistered;
        }

        private static void RunInitialization()
        {
            string storedVersion = EditorPrefs.GetString(VersionKey + Application.productName, "");
            if (storedVersion != TheToonShaderConstants.THETOONSHADER_VERSION_3D)
            {
#if !__SHADERCREW_DEV_ENV__
                EditorPrefs.SetString(VersionKey + Application.productName, TheToonShaderConstants.THETOONSHADER_VERSION_3D);
                DoTildeShaderFolderAdjustments();
#endif
            }




        }
        private static void OnPackagesRegistered(PackageRegistrationEventArgs args)
        {
            //if(args.added != null)
            //{
            //    foreach (UnityEditor.PackageManager.PackageInfo item in args.added)
            //    {
            //        Debug.Log(item.name);
            //    }
            //}
            if (args.added != null && args.added.Any(pkg => pkg.name == "com.shadercrew.the-toon-shader.3d") ||
                args.changedFrom != null && args.changedFrom.Any(pkg => pkg.name == "com.shadercrew.the-toon-shader.3d") ||
                args.added != null && args.added.Any(pkg => pkg.name == "com.shadercrew.seethroughshader.3d") ||
                args.removed != null && args.removed.Any(pkg => pkg.name == "com.shadercrew.seethroughshader.3d"))
            {
#if !__SHADERCREW_DEV_ENV__
                DoTildeShaderFolderAdjustments();
#endif
            }
        }
        private static void DoTildeShaderFolderAdjustments()
        {

            string shaderPackageFolder = "Packages/com.shadercrew.the-toon-shader.3d/";

            string nativeShaderFolderURP = shaderPackageFolder + "Scripts/Shaders/Native/URP";
            string nativeShaderFolderURPTilde = nativeShaderFolderURP + "~";


#if USING_URP
        RenameFolder(nativeShaderFolderURPTilde, nativeShaderFolderURP);

        string nativeShaderFolderURP2020 = nativeShaderFolderURP + "/2020";
        string nativeShaderFolderURP2020Tilde = nativeShaderFolderURP2020 + "~";

        string nativeShaderFolderURP2021 = nativeShaderFolderURP + "/2021";
        string nativeShaderFolderURP2021Tilde = nativeShaderFolderURP2021 + "~";

        string nativeShaderFolderURP2022 = nativeShaderFolderURP + "/2022";
        string nativeShaderFolderURP2022Tilde = nativeShaderFolderURP2022 + "~";

        string nativeShaderFolderURPUnity6 = nativeShaderFolderURP + "/Unity6";
        string nativeShaderFolderURPUnity6Tilde = nativeShaderFolderURPUnity6 + "~";


#if UNITY_2020_1_OR_NEWER && !UNITY_2021_1_OR_NEWER
                RenameFolder(nativeShaderFolderURP2020Tilde, nativeShaderFolderURP2020);
#else
                RenameFolder(nativeShaderFolderURP2020, nativeShaderFolderURP2020Tilde);
#endif

#if UNITY_2021_1_OR_NEWER && !UNITY_2022_1_OR_NEWER
                RenameFolder(nativeShaderFolderURP2021Tilde, nativeShaderFolderURP2021);
#else
                RenameFolder(nativeShaderFolderURP2021, nativeShaderFolderURP2021Tilde);
#endif

#if UNITY_2022_1_OR_NEWER && !UNITY_2023_1_OR_NEWER
                RenameFolder(nativeShaderFolderURP2022Tilde, nativeShaderFolderURP2022);
#else
                RenameFolder(nativeShaderFolderURP2022, nativeShaderFolderURP2022Tilde);
#endif

#if UNITY_2023_1_OR_NEWER
                RenameFolder(nativeShaderFolderURPUnity6Tilde, nativeShaderFolderURPUnity6);
#else
                RenameFolder(nativeShaderFolderURPUnity6, nativeShaderFolderURPUnity6Tilde);
#endif
#else
            RenameFolder(nativeShaderFolderURP, nativeShaderFolderURPTilde);
#endif





            string stsShaderFolder = shaderPackageFolder + "Scripts/Shaders/STS";
            string stsShaderFolderTilde = stsShaderFolder + "~";

            string stsShaderFolderURP = shaderPackageFolder + "Scripts/Shaders/STS/URP";
            string stsShaderFolderURPTilde = stsShaderFolderURP + "~";

#if USING_SEE_THROUGH_SHADER
            RenameFolder(stsShaderFolderTilde, stsShaderFolder);


#if USING_URP
        RenameFolder(stsShaderFolderURPTilde, stsShaderFolderURP);

        string stsShaderFolderURP2020 = stsShaderFolderURP + "/2020";
        string stsShaderFolderURP2020Tilde = stsShaderFolderURP2020 + "~";

        string stsShaderFolderURP2021 = stsShaderFolderURP + "/2021";
        string stsShaderFolderURP2021Tilde = stsShaderFolderURP2021 + "~";

        string stsShaderFolderURP2022 = stsShaderFolderURP + "/2022";
        string stsShaderFolderURP2022Tilde = stsShaderFolderURP2022 + "~";

        string stsShaderFolderURPUnity6 = stsShaderFolderURP + "/Unity6";
        string stsShaderFolderURPUnity6Tilde = stsShaderFolderURPUnity6 + "~";


#if UNITY_2020_1_OR_NEWER && !UNITY_2021_1_OR_NEWER
        RenameFolder(stsShaderFolderURP2020Tilde, stsShaderFolderURP2020);
#else
        RenameFolder(stsShaderFolderURP2020, stsShaderFolderURP2020Tilde);
#endif

#if UNITY_2021_1_OR_NEWER && !UNITY_2022_1_OR_NEWER
        RenameFolder(stsShaderFolderURP2021Tilde, stsShaderFolderURP2021);
#else
        RenameFolder(stsShaderFolderURP2021, stsShaderFolderURP2021Tilde);
#endif

#if UNITY_2022_1_OR_NEWER && !UNITY_2023_1_OR_NEWER
        RenameFolder(stsShaderFolderURP2022Tilde, stsShaderFolderURP2022);
#else
        RenameFolder(stsShaderFolderURP2022, stsShaderFolderURP2022Tilde);
#endif

#if UNITY_2023_1_OR_NEWER
        RenameFolder(stsShaderFolderURPUnity6Tilde, stsShaderFolderURPUnity6);
#else
        RenameFolder(stsShaderFolderURPUnity6, stsShaderFolderURPUnity6Tilde);
#endif

#else
            RenameFolder(stsShaderFolderURP, stsShaderFolderURPTilde);
#endif
#else
        RenameFolder(stsShaderFolder, stsShaderFolderTilde);
#endif
        }

        private static void RenameFolder(string oldName, string newName)
        {
            if (Directory.Exists(oldName))
            {
                Directory.Move(oldName, newName);
                string meta = oldName + ".meta";
                if (File.Exists(meta))
                {
                    File.Delete(meta);
                }
                AssetDatabase.Refresh();
            }
        }

    }
}
#endif
