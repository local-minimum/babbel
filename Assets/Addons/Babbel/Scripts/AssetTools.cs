using UnityEngine;
using UnityEditor;
using System.IO;

namespace Babbel
{
    public static class AssetTools
    {


        /// <summary>
        //	This makes it easy to create, name and place unique new ScriptableObject asset files.
        /// </summary>
        public static T CreateAsset<T>(string path="") where T : ScriptableObject
        {
            T asset = ScriptableObject.CreateInstance<T>();

            if (string.IsNullOrEmpty(path))
            {
                path = AssetDatabase.GetAssetPath(Selection.activeObject);
            }

            if (string.IsNullOrEmpty(path))
            {
                path = "Assets";
            }
            else if (Path.GetExtension(path) != "")
            {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }

            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New " + typeof(T).ToString() + ".asset");

            AssetDatabase.CreateAsset(asset, assetPathAndName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return asset;
        }

        public static T LoadByQuery<T>(string query) where T : UnityEngine.Object
        {
            foreach (string guid in AssetDatabase.FindAssets(query)){
                return AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid));
            }
            return null;
        }

        public static void EnsureFolder(string path)
        {
            string parent = null;
            foreach (string child in path.Split('/'))
            {
                if (string.IsNullOrEmpty(parent))
                {
                    parent = child;
                    continue;
                }

                string curPath = string.Join("/", new string[] { parent, child });

                if (AssetDatabase.IsValidFolder(curPath))
                {
                    parent = curPath;
                }
                else
                {
                    string guid = AssetDatabase.CreateFolder(parent, child);
                    parent = AssetDatabase.GUIDToAssetPath(guid);
                }
            }
        }
    }

}