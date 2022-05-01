
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace Editor.EditorUtils
{
    public static class AddressableExtensions
    {
        
        public static string GetAddressableAssetPath(this Object source)
        {
            if (source == null || !AssetDatabase.Contains(source))
                return string.Empty;

            var entry = source.GetAddressableAssetEntry();
            return entry != null ? entry.address : string.Empty;
        }

        private static AddressableAssetEntry GetAddressableAssetEntry(this Object source)
        {
            if (source == null || !AssetDatabase.Contains(source))
                return null;
            
            var addressableSettings = AddressableAssetSettingsDefaultObject.Settings;
            var sourcePath = AssetDatabase.GetAssetPath(source);
            var sourceGuid = AssetDatabase.AssetPathToGUID(sourcePath);

            if (addressableSettings != null)
                return addressableSettings.FindAssetEntry(sourceGuid);


            return null;
        }
    }
}