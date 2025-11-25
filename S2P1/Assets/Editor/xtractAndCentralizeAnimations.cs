using UnityEngine;
using UnityEditor;
using System.IO;

public class ExtractAnimationClips : EditorWindow
{
    private string sourceFolder = "Assets/_Animation";
    private string destinationFolder = "Assets/_Animation/";

    [MenuItem("Tools/Extract AnimationClips")]
    public static void ShowWindow()
    {
        GetWindow<ExtractAnimationClips>("Extract Animations");
    }

    void OnGUI()
    {
        GUILayout.Label("Animation Extraction Tool", EditorStyles.boldLabel);
        sourceFolder = EditorGUILayout.TextField("Source Folder", sourceFolder);
        destinationFolder = EditorGUILayout.TextField("Destination Folder", destinationFolder);

        if (GUILayout.Button("Extract and Centralize"))
        {
            ExtractClips(sourceFolder, destinationFolder);
        }
    }

    static void ExtractClips(string sourcePath, string destPath)
    {
        if (!AssetDatabase.IsValidFolder(destPath))
        {
            Directory.CreateDirectory(destPath);
            AssetDatabase.Refresh();
        }

        string[] fbxFiles = Directory.GetFiles(sourcePath, "*.fbx", SearchOption.AllDirectories);

        foreach (string fbxFile in fbxFiles)
        {
            string assetPath = fbxFile.Replace("\\", "/");
            Object[] assets = AssetDatabase.LoadAllAssetsAtPath(assetPath);

            foreach (Object asset in assets)
            {
                if (asset is AnimationClip clip && !clip.name.StartsWith("__preview__"))
                {
                    string fbxName = Path.GetFileNameWithoutExtension(assetPath);
                    string newName = $"{fbxName}.anim";
                    string newPath = Path.Combine(destPath, newName).Replace("\\", "/");

                    AnimationClip newClip = Object.Instantiate(clip);
                    AssetDatabase.CreateAsset(newClip, newPath);
                }
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("✅ Tous les AnimationClips ont été extraits et centralisés.");
    }
}