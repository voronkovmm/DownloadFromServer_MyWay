using System.IO;
using UnityEditor;
using UnityEngine;

namespace Core.Scripts.Editor
{
    public class ToolsEditor
    {
        [MenuItem("Tools/BuildNewBundle")]
        public static void BuildAssetBundles()
        {
            string assetBundleDirectory = Path.Combine(Application.streamingAssetsPath, "Bundles");

            if (!Directory.Exists(assetBundleDirectory))
            {
                Directory.CreateDirectory(assetBundleDirectory);
            }

            BuildPipeline.BuildAssetBundles(
                assetBundleDirectory,
                BuildAssetBundleOptions.None,
                BuildTarget.StandaloneWindows64
            );

            Debug.Log("<color=green>AssetBundles built successfully!</color>");
        }
    }
}