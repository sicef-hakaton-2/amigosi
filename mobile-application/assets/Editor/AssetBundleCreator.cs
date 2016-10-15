using UnityEngine;
using UnityEditor;

public class CreateAssetBundles:Editor
{
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        BuildPipeline.BuildAssetBundles("AssetBundles");
    }
}
