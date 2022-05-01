
// Encryption method directive (should be always the first line in this file):
//#define USE_AES

using System.Linq;

#if UNITY_5_3_OR_NEWER
using UnityEngine;
#endif

#if UNITY_WEBGL
// Plugin (WebGL):
using System.Runtime.InteropServices;
#endif

#if USE_AES
// AES Encryption (Not compatible with WindowsPhone):
using System.Security.Cryptography;
#endif

/*
 * File Management:
 * 
 * Easy control over files. All data is saved in binary so preventing data loss or corruption.
 * This asset was downloaded from the Unity AssetStore:
 * 
 * https://assetstore.unity.com/packages/tools/input-management/file-management-easy-way-to-save-and-read-files-67183
 * 
 * V 1.1 Features:
 * - You may chose between Xor or Aes encryption algorythms manually.
 * Define or comment the USE_AES directive to switch encryption methods.
 * (AES encryption is not available on Windows Phone platforms)
 * - Images can be easily encrypted.
 * 
 * V 1.2 Features:
 * - New feature: Unity types Vector2, Vector3, Vector4, Quaternion, Rect, Color and Color32 now supported for save and load.
 * - New feature: Load AudioClip from file (WebGL not supported yet).
 * - New feature: Increased WebGL save size (Now images can be saved in web browser too!!!).
 * - New feature: Save and load Arrays or Lists in a single line of code.
 * - New feature: Automated virtual StreamingAssets file/directory index. Now StreamingAssets can be accessed on Android and WebGL as real folders!!!
 * - New feature: Directory tools: Exists, Create, Delete and Empty content (StreamingAssets is read only).
 * - New feature: Get lists of files or directories from a given path.
 * - New feature: Get a List<byte[]> with the whole content of a given folder (matching the file name list ListFiles()).
 * - BugFix: File existance detection wasn't working on Android/StreamingAssets.
 * - BugFix: Optimized reading very big strings.
 * 
 * V 1.3 Features:
 * - New feature: Add raw data to an existing file with AddRawData().
 * - New feature: You can drag and resize the BrowserWindow.
 * - New feature: Added several configurations that can be combined to achieve different Browser behaviours.
 * - New feature: Added NormalizePath(), GetExtension() and GetFileExtension() interfaces (check documentation).
 * - New feature: Added CopyFile(), CopyDirectory(), Move() and Rename() (fully compatible with files and folders).
 * - New feature: Added Cut, Copy and Paste buttons to the FileBrowser.
 * - New feature: Added Delete, Rename and New folder buttons to the FileBrowser.
 * - New feature: Creates the requested path when writing to disk.
 * - New feature: WAV files import now supported in WebGL (uses a custom wrapper).
 * - BugFix: Important improvements in path interpretation (OSX bugfixes).
 * 
 * V 1.4 Features:
 * - New Feature: ReadList and ReadArray overflow allowing multiple separatos. (Useful for CSV parsing)
 * - New Feature: ReadAllLines from any platform using multiple line separators.
 * - New Feature: Navigate back and fwd in FileBrowser window.
 * - BugFix: Extended WAV files compatibility.
 * - BugFix: Importing audio in Android.
 * - BugFix: Not copying empty folders.
 * - BugFix: Retrieving wring files from StreamingAssets index.
 * 
 * V 1.5 Features:
 * - New Feature: Filters the ListFiles view by multiple file extensions.
 * - New Feature: File list filtering is integrated to the FileBrowser prefab.
 * - New Feature: Dropdown with the desired file extensions from the filter.
 * - BugFix: Thread safe.
 * 
 * v1.6 Features:
 * - New Feature: FileBrowser admits a default file to be selected or saved at start.
 * - New Feature: The OpenWavParser is integrated within FileManagement.
 * - New Feature: AudioClips can be saved into wav files using OpenWavParser.
 * - New Feature: FileBrowser includes a caption icon (customisable).
 * - New Feature: Caption text customizable in FileBrowser.
 * - New Feature: Added new GetFileNameWithoutExtension() method.
 * - New Feature: AudioClip name is set with the source file automatically (ImportAudio).
 * 
 * v1.7 Features:
 * - New Feature: Parses .ini files into a complete utility class (FM_IniFile).
 * - New Feature: Run .apk file installation in Android devices (User action required to grant permission).
 * - New Feature: Allows string encoding for all system supported formats.
 * - New Feature: The ListFiles method (with extension filter) now allows the scan of subfolders.
 * - BugFix: Parse error when reading an empty file in ReadList()
 * - BugFix: Corrected audio import for Unity 2017 and further.
 * 
 * v1.8 Feature:
 * - BugFix: System.Text.Encoding modes not supported on WSA 8.1 (Skipped to allow only "Fast" 8bit mode).
 * - BugFix: FileBrowser was throwing an error in Unity 2019 (UI issue).
 * - BugFix: Mono in Unity 2018 uses localization culture in float values (failing interpretation of dots and commas).
 * - BugFix: Not performing a folder cut/paste correctly.
 * - BugFix: WWW not available since Unity 2019.
 * - New Feature: AddLine method allows to add text lines to an existing file.
 * - New Feature: Reading StreamingAssets in Android is thread safe (not using WWW nor UnityWebRequest).
 * - New Feature: Adapted to work on non-Unity applications.
 * - New Feature: FileBrowser now is fully 3D/VR compatible.
 * 
 * v1.9 Feature:
 * - BugFix: The Delete() method crashed when tried to delete an open/read_only file instead of continue.
 * - BugFix: Compilation error under WebGL platform.
 * 
 * v2.0 Feature (unreleased):
 * - New feature: Allows default filter selection in file browser.
 * - BugFix: Setting the CultureInfo failing on WSA exports.
 */

/// <summary>String conversion available modes</summary>
public enum FM_StringMode
{
    UTF8,
    Fast,
    ASCII,
    BigEndianUnicode,
    Unicode,
    UTF32,
    UTF7
}

public static class FileManagement
{
    // For AES encryption, 'key' must be 16, 24 or 32 bytes length.
    // Xor encryption uses any length.

    // IMPORTANT: DON'T FORGET TO SET YOUR OWN KEY (Keys must never be written in plain text)
    private readonly static byte[] key = { 217, 134, 151, 168, 185, 202, 129, 135, 150, 130, 141, 201, 210, 167, 198, 169 };
    private static string[] blocks;     // This is the StreamingAssets index for Android and WebGL.
    // The current string conversion method:
    public static FM_StringMode stringConversion = FM_StringMode.UTF8;
    // Automatic paths (they are saved to prevent access error in a Thread):
    private static string persistentDataPath;
    private static string streamingAssetsPath;


#if UNITY_5_3_OR_NEWER
    // Android object used to access Compressed StreamingAssets folder:
    private static AndroidJavaObject aManager;
    /// <summary>Constructor to initialize the automatic paths for further use</summary>
    static FileManagement()
    {
        persistentDataPath = Application.persistentDataPath;
        streamingAssetsPath = Application.streamingAssetsPath;
    }
#else
    static FileManagement()
    {
        persistentDataPath = System.AppDomain.CurrentDomain.BaseDirectory;
        streamingAssetsPath = System.AppDomain.CurrentDomain.BaseDirectory;
    }
#endif


#if UNITY_WINRT
    /// <summary>Saves a new file (overwrites if exists an older one)</summary>
    public static void SaveRawFile(string name, byte[] content, bool enc = false, bool fullPath = false)
    {
        if (name != "")
        {
            if (!fullPath)
                name = Combine(persistentDataPath, name);
            if (content != null)
            {
                CreateDirectory(GetParentDirectory(name));
                if (enc)
                    UnityEngine.Windows.File.WriteAllBytes(name, Encrypt(content, key));
                else
                    UnityEngine.Windows.File.WriteAllBytes(name, content);
            }
            else
                WriteLine("[FileManagement.SaveRawFile] Exception: Trying to save null data.", true);
        }
        else
            WriteLine("[FileManagement.SaveRawFile] Can't save an unnamed file.", true);
    }

    /// <summary>Returns the byte[] content of a file</summary>
    public static byte[] ReadRawFile(string name, bool enc = false, bool checkSA = true, bool fullPath = false)
    {
        byte[] contentArray = { };
        string path = name;

        if (name != "")
        {
            // First checks if file exists no matter where:
            if (FileExists(path, checkSA, fullPath))
            {
                if (!fullPath)
                {
                    // Checks if file exists in PersistentDataPath only:
                    if (FileExists(name, false))
                        path = Combine(persistentDataPath, name);
                    else if (checkSA)    // Then checks StreamingAssets if desired.
                        path = Combine(streamingAssetsPath, name);
                }
                // Read content normally:
                contentArray = UnityEngine.Windows.File.ReadAllBytes(path);
            }
            else
                WriteLine("[FileManagement.ReadRawFile] File not found: " + path);

            // Decryption:
            if (contentArray.Length > 0 && enc)
                contentArray = Decrypt(contentArray, key);
        }
        else
            WriteLine("[FileManagement.ReadRawFile] Can't read an unnamed file.", true);
        return contentArray;
    }

    /// <summary>Deletes a file</summary>
    public static void DeleteFile(string name, bool fullPath = false)
    {
        if (FileExists(name, false, fullPath))
        {
            if (!fullPath)
                name = Combine(persistentDataPath, name);
            UnityEngine.Windows.File.Delete(name);
        }
        else
            WriteLine("[FileManagement.DeleteFile] File not found: " + name);
    }
#elif UNITY_WEBGL
    [DllImport("__Internal")] private static extern void SyncFiles();
    [DllImport("__Internal")] private static extern int ReadFileLen(string url);    // Downloads a file and returns the length.
    [DllImport("__Internal")] private static extern System.IntPtr ReadData();       // Pointer to the downloaded data.
    [DllImport("__Internal")] public static extern void ShowMessage(string msg);    // Shows messages in browser for debugging.

    // Legacy interfaces (for cookies):
    [DllImport("__Internal")] private static extern void WriteCookie(string name, string value, string expires = null, bool sec = false, string path = null, string domain = null);
    [DllImport("__Internal")] private static extern void DeleteCookie(string name, string path = null, string domain = null);
    [DllImport("__Internal")] private static extern int ReadCookieLen(string name);
    [DllImport("__Internal")] private static extern void DeleteAllCookies();

    /// <summary>Saves a new file (overwrites if exists an older one)</summary>
    public static void SaveRawFile(string name, byte[] content, bool enc = false, bool fullPath = false)
    {
        if (name != "")
        {
            if (!fullPath)
                name = Combine(persistentDataPath, name);

            if (content != null)
            {
                CreateDirectory(GetParentDirectory(name));
                if (enc)
                    System.IO.File.WriteAllBytes(name, Encrypt(content, key));
                else
                    System.IO.File.WriteAllBytes(name, content);

                if (Application.platform == RuntimePlatform.WebGLPlayer)
                    SyncFiles();
            }
            else
            {
                WriteLine("[FileManagement.SaveRawFile] Exception: Trying to save null data.", true);
            }
        }
        else
        {
            WriteLine("[FileManagement.SaveRawFile] Can't save an unnamed file.", true);
        }
    }

    /// <summary>Returns the byte[] content of a file</summary>
    public static byte[] ReadRawFile(string name, bool enc = false, bool checkSA = true, bool fullPath = false)
    {
        byte[] contentArray = { };
        string path = name;
        if (name != "")
        {
            // First checks if file exists no matter where:
            if (FileExists(path, checkSA, fullPath))
            {
                if (!fullPath)
                {
                    // Checks if file exists in PersistentDataPath only:
                    if (FileExists(name, false))
                        path = Combine(persistentDataPath, name);
                    else if (checkSA)    // Then checks StreamingAssets if desired.
                        path = Combine(streamingAssetsPath, name);
                }
                // Chose correct method to read:
                if (path.Contains("://") && Application.platform == RuntimePlatform.WebGLPlayer)
                {
                    // Read content from StreamingAssets:
                    int contentLen = ReadFileLen(path);     // Get file length.
                    if (contentLen > 0)
                    {
                        contentArray = new byte[contentLen];
                        Marshal.Copy(ReadData(), contentArray, 0, contentLen);
                    }
                    else
                    {
                        WriteLine("[FileManagement.ReadRawFile] File was deleted in server side: " + path);
                    }
                }
                else
                {
                    // Read content normally:
                    contentArray = System.IO.File.ReadAllBytes(path);
                }
            }
            else
            {
                WriteLine("[FileManagement.ReadRawFile] File not found: " + path);
            }

            // Decryption:
            if (contentArray.Length > 0 && enc)
                contentArray = Decrypt(contentArray, key);
        }
        else
        {
            WriteLine("[FileManagement.ReadRawFile] Can't read an unnamed file.", true);
        }
        return contentArray;
    }

    /// <summary>Deletes a file</summary>
    public static void DeleteFile(string name, bool fullPath = false)
    {
        if (FileExists(name, false, fullPath))
        {
            if (!fullPath)
                name = Combine(persistentDataPath, name);
            System.IO.File.Delete(name);

            if (Application.platform == RuntimePlatform.WebGLPlayer)
                SyncFiles();
        }
        else
        {
            WriteLine("[FileManagement.DeleteFile] File not found: " + name);
        }
    }

    /// <summary>Checks a name into the automatic StreamingAssets index file (WebGL)</summary>
    private static bool CheckNameOnIndex(string name, string type)
    {
        // First block is StreamingAssets, then there are every subfolders:
        if(blocks == null)
        {
            // Load blocks first time:
            string indexPath = streamingAssetsPath + "/FMSA_Index";
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                int contentLen = ReadFileLen(indexPath);     // Get file length.
                if (contentLen > 0)
                {
                    // Get index directly:
                    byte[] contentArray = new byte[contentLen];
                    Marshal.Copy(ReadData(), contentArray, 0, contentLen);
                    string content = ByteArrayToString(contentArray);
                    // Get blocks:
                    blocks = content.Split('|');
                }
                else
                {
                    blocks = new string[0];
                    WriteLine("[FileManagement.CheckNameOnIndex] Index file not found: " + indexPath);
                }
            }
            else
            {
                // To allow testing in editor:
                byte[] contentArray = { };
                if (System.IO.File.Exists(indexPath))
                    contentArray = System.IO.File.ReadAllBytes(indexPath);
                string content = ByteArrayToString(contentArray);
                // Get blocks:
                blocks = content.Split('|');
            }
        }
        // Checks name existance:
        name = name.Replace('\\', '/');     // Normalize path format with index.
        for (int b = 0; b < blocks.Length; b++)
        {
            // Search every folder and subfodler separatelly:
            string[] entries = blocks[b].Split(';');
            for (int e = 0; e < entries.Length; e++)
            {
                string[] data = entries[e].Split(',');  // [0] file path and name, [1] type ("F" or "D").
                data[0] = data[0].Replace('\\', '/');
                if (name == data[0] && data[1] == type)
                    return true;
            }
        }
        return false;
    }

    /// <summary>Checks a virtual path into the index and returns the names matching (WebGL)</summary>
    private static string[] GetNamesOnIndex(string name, string type)
    {
        if (blocks == null)
            CheckNameOnIndex("", "");
        System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
        name = name.Replace('\\', '/');     // Normalize path format with index.
        name = name.Replace("//", "/");            // Allow root folder (empty path).
        // Collect name occurrencies:
        bool exit = false;
        for (int b = 0; b < blocks.Length; b++)
        {
            // Search every folder and subfodler separatelly:
            string[] entries = blocks[b].Split(';');
            for (int e = 0; e < entries.Length; e++)
            {
                string[] data = entries[e].Split(',');  // [0] file path and name, [1] type ("F" or "D").
                data[0] = data[0].Replace('\\', '/');
                if (data[0].Contains(name) && type == data[1])
                {
                    list.Add(data[0]);
                    exit = true;
                }
            }
            if (exit)
                break;
        }
        return list.ToArray();
    }
#else
    /// <summary>Saves a new file (overwrites if exists an older one)</summary>
    public static void SaveRawFile(string name, byte[] content, bool enc = false, bool fullPath = false)
    {
        if (name != "")
        {
            if (!fullPath)
                name = Combine(persistentDataPath, name);

            if (content != null)
            {
                CreateDirectory(GetParentDirectory(name));
                if (content.Length > 0)
                {
                    if (enc)
                        System.IO.File.WriteAllBytes(name, Encrypt(content, key));
                    else
                        System.IO.File.WriteAllBytes(name, content);
                }
                else
                {
                    System.IO.File.Create(name).Dispose();                          // Create an empty file.
                }
            }
            else
                WriteLine("[FileManagement.SaveRawFile] Exception: Trying to save null data.", true);
        }
        else
            WriteLine("[FileManagemnt.SaveRawFile] Can't save an unnamed file.", true);
    }

    /// <summary>Returns the byte[] content of a file</summary>
    public static byte[] ReadRawFile(string name, bool enc = false, bool checkSA = true, bool fullPath = false)
    {
        byte[] contentArray = { };
        string path = name;

        if (path != "")
        {
            // First checks if file exists no matter where:
            if (FileExists(path, checkSA, fullPath))
            {
                if (!fullPath)
                {
                    if (FileExists(path, false))
                    {
                        path = Combine(persistentDataPath, path);                   // File exists in persistent data only, so get its full path.
                        contentArray = System.IO.File.ReadAllBytes(path);           // Read content.
                    }
                    else if (checkSA)
                    {
#if UNITY_5_3_OR_NEWER
                        if (Application.platform == RuntimePlatform.Android)
                        {
                            contentArray = GetFileFromStreamingAsets(name);         // Get file from StreamingAssets in Android.
                        }
                        else
                        {
                            path = Combine(streamingAssetsPath, path);              // Get the full path to StreamingAssets.
                            contentArray = System.IO.File.ReadAllBytes(path);       // Read content.
                        }
#else
                        path = Combine(streamingAssetsPath, path);              // Get the full path to StreamingAssets.
                        contentArray = System.IO.File.ReadAllBytes(path);       // Read content.
#endif
                    }
                }
            }
            else
            {
                WriteLine("[FileManagement.ReadRawFile] File not found: " + path);
            }
            // Decryption:
            if (contentArray.Length > 0 && enc)
                contentArray = Decrypt(contentArray, key);
        }
        else
        {
            WriteLine("[FileManagement.ReadRawFile] Can't read an unnamed file.", true);
        }
        return contentArray;
    }

    /// <summary>Deletes a file</summary>
    public static void DeleteFile(string name, bool fullPath = false)
    {
        if (FileExists(name, false, fullPath))
        {
            if (!fullPath)
                name = Combine(persistentDataPath, name);
            try
            {
                System.IO.File.Delete(name);
            }
            catch (System.Exception e)
            {
                WriteLine("[FileManagement.DeleteFile] File " + name + " couldn't be deleted: " + e.Message);
            }
        }
        else
            WriteLine("[FileManagement.DeleteFile] File not found: " + name);
    }

#endif

#if UNITY_5_3_OR_NEWER

#if UNITY_ANDROID
    /// <summary>Checks a name into the automatic StreamingAssets index file (Android)</summary>
    private static bool CheckNameOnIndex(string name, string type)
    {
        // First block is StreamingAssets, then there are every subfolders:
        if (blocks == null)
        {
            // Load blocks first time:
            string indexPath = "FMSA_Index";
            byte[] contentArray = { };
            if (Application.platform == RuntimePlatform.Android)
            {
                // Read StreamingAssets in Android:
                contentArray = GetFileFromStreamingAsets(indexPath);
            }
            else
            {
                // Read content normally (It allows testing in Editor for Android):
                indexPath = NormalizePath(Combine(streamingAssetsPath, indexPath));
                if (System.IO.File.Exists(indexPath))
                    contentArray = System.IO.File.ReadAllBytes(indexPath);
                else
                    contentArray = new byte[0];
            }
            // If file was successfully found:
            if (contentArray != null)
            {
                string content = ByteArrayToString(contentArray);
                blocks = content.Split('|');
            }
            else
            {
                WriteLine("[FileManagement.CheckNameOnIndex] Index file not found: " + indexPath);
            }
        }
        // Checks name existence:
        name = name.Replace('\\', '/');      // Normalize path format with index.
        for (int b = 0; b < blocks.Length; b++)
        {
            // Search every folder and subfodler separatelly:
            string[] entries = blocks[b].Split(';');
            for (int e = 0; e < entries.Length; e++)
            {
                string[] data = entries[e].Split(',');  // [0] file path and name, [1] type ("F" or "D").
                data[0] = data[0].Replace('\\', '/');
                if (name == data[0] && type == data[1])
                    return true;
            }
        }
        return false;
    }

    /// <summary>Checks a virtual path into the index and returns the names matching (Android)</summary>
    private static string[] GetNamesOnIndex(string name, string type)
    {
        if (blocks == null)
            CheckNameOnIndex("", "");   // Forces load the index (if not already loaded)
        System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
        name = name.Replace("//", "/");                 // Allow root folder (empty path).
        name = name.Replace('\\', '/');                 // Normalize path format with index.
        // Collect name occurrencies:
        bool exit = false;
        for (int b = 0; b < blocks.Length; b++)
        {
            // Search every folder and subfodler separatelly:
            string[] entries = blocks[b].Split(';');
            for (int e = 0; e < entries.Length; e++)
            {
                string[] data = entries[e].Split(',');  // [0] file path and name, [1] type ("F" or "D").
                data[0] = NormalizePath(data[0]);
                if (data[0].StartsWith(name) && type == data[1])
                {
                    list.Add(data[0]);
                    exit = true;
                }
            }
            if (exit)
                break;
        }
        return list.ToArray();
    }
#endif

    /// <summary>Read files from the installed APK in "assets" folder (Android only)</summary>
    private static byte[] GetFileFromStreamingAsets(string name)
    {
        // This function reads a file placed into the "assets" folder inside the compressed APK.
        // The method is not using any Unity class so it's completely thread safe.
        byte[] contentArray = { };
        // Read StreamingAssets in Android ("assets):
        if (aManager == null)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");
            aManager = context.Call<AndroidJavaObject>("getAssets");                                    // AssetManager
        }
        // Get content:
        AndroidJavaObject iStream = aManager.Call<AndroidJavaObject>("open", new object[] { name });    // InputStream
        contentArray = new byte[iStream.Call<int>("available")];
        for (int i = 0; i < contentArray.Length; i++)
        {
            contentArray[i] = (byte)iStream.Call<int>("read");
        }
        iStream.Call("close");
        return contentArray;
    }

    /// <summary>Loads a JPG/PNG image from file and returns a Texture2D</summary>
    public static Texture2D ImportTexture(string file, bool enc = false, bool checkSA = true, bool fullPath = false)
    {
        byte[] image = ReadRawFile(file, enc, checkSA, fullPath);
        Texture2D texture = new Texture2D(2, 2); // Assigns minimum size
        texture.LoadImage(image);
        texture.Apply();
        return texture;
    }

    /// <summary>Loads a JPG/PNG image from file and returns a Sprite</summary>
    public static Sprite ImportSprite(string file, bool enc = false, bool checkSA = true, bool fullPath = false)
    {
        Sprite icon = null;
        Texture2D texture = ImportTexture(file, enc, checkSA, fullPath);
        texture.Apply();
        if (texture.width >= 32 && texture.height >= 32)  // Minimum valid size
            icon = Sprite.Create(texture, new Rect(new Vector2(0f, 0f), new Vector2(texture.width, texture.height)), new Vector2(0f, 0f));
        return icon;
    }

    /// <summary>Saves a texture to file formated as JPG (StreamingAssets folder is read only)</summary>
    public static void SaveJpgTexture(string name, Texture texture, int quality = 75, bool enc = false, bool fullPath = false)
    {
        Texture2D image = (Texture2D)texture;
        SaveRawFile(name, image.EncodeToJPG(quality), enc, fullPath);
    }

    /// <summary>Saves a texture to file formated as PNG (StreamingAssets folder is read only)</summary>
    public static void SavePngTexture(string name, Texture texture, bool enc = false, bool fullPath = false)
    {
        Texture2D image = (Texture2D)texture;
        SaveRawFile(name, image.EncodeToPNG(), enc, fullPath);
    }

#endif

    /// <summary>Checks file existence (checks PersistentData, then StreamingAssets)</summary>
    public static bool FileExists(string name, bool checkSA = true, bool fullPath = false)
    {
        // Check existance:
        bool result = false;
        if (!fullPath)
        {
            // Check PersistentData path first:
            string path = Combine(persistentDataPath, name);
            result = System.IO.File.Exists(path);
            if (!result && checkSA)
            {
                // Then check StreamingAssets path:
#if UNITY_ANDROID || UNITY_WEBGL
                result = CheckNameOnIndex("StreamingAssets/" + name, "F");
#else
                path = Combine(streamingAssetsPath, name);
                result = System.IO.File.Exists(path);
#endif
            }
        }
        else
        {
            // Direct check:
            result = System.IO.File.Exists(name);
        }
        return result;
    }

    /// <summary>Save file with convertion (StreamingAssets folder is read only)</summary>
    public static void SaveFile<T>(string name, T content, bool enc = false, bool fullPath = false)
    {
        // Force invariant culture:
        System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en-US");
        System.Globalization.CultureInfo.CurrentCulture = ci;
        System.Globalization.CultureInfo.CurrentUICulture = ci;
        // Convert string to byte array:
        byte[] contentArray = StringToByteArray(content.ToString());
        SaveRawFile(name, contentArray, enc, fullPath);
    }

    /// <summary>Read file with conversion</summary>
    public static T ReadFile<T>(string name, bool enc = false, bool checkSA = true, bool fullPath = false)
    {
        byte[] contentArray = { };
        string content = "";
        T val = default(T);   // Return value.
        contentArray = ReadRawFile(name, enc, checkSA, fullPath);

        // Restore the string to parse:
        if (contentArray.Length > 0)
            content = ByteArrayToString(contentArray);
        else
            return default(T);  // There is no string, so return the apropriate null value.

        try
        {
            val = CustomParser<T>(content);
        }
        catch (System.FormatException)
        {
            WriteLine("[FileManagement.ReadFile] Exception: FormatException - Trying to read data in the wrong format. (" + typeof(T) + "): " + name, true);
        }

        return val;
    }

    /// <summary>Saves arrays or lists of one dimension</summary>
    public static void SaveArray<T>(string name, T[] content, char separator = (char)0x09, bool enc = false, bool fullPath = false)
    {
        string save = "";
        for (int i = 0; i < content.Length; i++)
        {
            save += content[i].ToString();
            if (i < (content.Length - 1))
                save += separator;
        }
        if (content.Length > 0)
            SaveFile(name, save, enc, fullPath);
        else
            WriteLine("[FileManagement.SaveArray] Trying to save empty array: " + name, true);
    }
    /// <summary>Saves arrays or lists of one dimension</summary>
    public static void SaveArray<T>(string name, System.Collections.Generic.List<T> content, char separator = (char)0x09, bool enc = false, bool fullPath = false)
    {
        SaveArray(name, content.ToArray(), separator, enc, fullPath);
    }

    /// <summary>Reads a one dimension Array from file</summary>
    public static T[] ReadArray<T>(string name, char separator = (char)0x09, bool enc = false, bool checkSA = true, bool fullPath = false)
    {
        return ReadList<T>(name, separator, enc, checkSA, fullPath).ToArray();
    }
    /// <summary>Reads a one dimension Array from file</summary>
    public static T[] ReadArray<T>(string name, string[] separator, bool enc = false, bool checkSA = true, bool fullPath = false)
    {
        return ReadList<T>(name, separator, enc, checkSA, fullPath).ToArray();
    }

    /// <summary>Reads a List from file</summary>
    public static System.Collections.Generic.List<T> ReadList<T>(string name, char separator = (char)0x09, bool enc = false, bool checkSA = true, bool fullPath = false)
    {
        System.Collections.Generic.List<T> list = new System.Collections.Generic.List<T>();
        string file = ReadFile<string>(name, enc, checkSA, fullPath);
        if (!string.IsNullOrEmpty(file))
        {
            string[] content = file.Split(separator);
            for (int i = 0; i < content.Length; i++)
            {
                list.Add(CustomParser<T>(content[i]));
            }
        }
        return list;
    }
    /// <summary>Reads a List from file</summary>
    public static System.Collections.Generic.List<T> ReadList<T>(string name, string[] separator, bool enc = false, bool checkSA = true, bool fullPath = false)
    {
        System.Collections.Generic.List<T> list = new System.Collections.Generic.List<T>();
        string file = ReadFile<string>(name, enc, checkSA, fullPath);
        if (!string.IsNullOrEmpty(file))
        {
            string[] content = file.Split(separator, System.StringSplitOptions.None);
            for (int i = 0; i < content.Length; i++)
            {
                list.Add(CustomParser<T>(content[i]));
            }
        }
        return list;
    }

    /// <summary>Reads all text lines from a file</summary>
    public static string[] ReadAllLines(string name, bool enc = false, bool checkSA = true, bool fullPath = false)
    {
        string[] eol = { "\r\n", "\n", "\r" };
        return ReadArray<string>(name, eol, enc, checkSA, fullPath);
    }

    /// <summary>Imports a .ini file into a Dictionary</summary>
    public static FM_IniFile ImportIniFile(string name, bool enc = false, bool checkSA = true, bool fullPath = false)
    {
        return new FM_IniFile(name, enc, checkSA, fullPath);
    }

    /// <summary>Adds text lines to an existing file (StreamingAssets folder is read only)</summary>
    public static void AddLogLine(string name, string content, bool deleteDate = false, bool enc = false, bool fullPath = false)
    {
        string savedContent = ReadFile<string>(name, enc, false, fullPath); // Files can't be saved dynamically in StreamingAssets.
        if (savedContent == null)
        {
            if (deleteDate)
                savedContent = content;
            else
                savedContent = System.DateTime.Now.ToString() + " - " + content;
        }
        else
        {
            savedContent += System.Environment.NewLine;
            if (deleteDate)
                savedContent += content;
            else
                savedContent += System.DateTime.Now.ToString() + " - " + content;
        }
        // Save new content:
        SaveFile(name, savedContent, enc, fullPath);
    }

    /// <summary>Adds text lines to an existing file (StreamingAssets folder is read only)</summary>
    public static void AddLine(string name, string content, bool enc = false, bool fullPath = false)
    {
        string savedContent = ReadFile<string>(name, enc, false, fullPath); // Files can't be saved dynamically in StreamingAssets.
        if (savedContent == null)
        {
            savedContent = content;
        }
        else
        {
            savedContent += System.Environment.NewLine;
            savedContent += content;
        }
        // Save new content (creates the file if it doesn't exists):
        SaveFile(name, savedContent, enc, fullPath);
    }

    /// <summary>Adds raw data to an existing file (StreamingAssets folder is read only)</summary>
    public static void AddRawData(string name, byte[] content, bool enc = false, bool fullPath = false)
    {
        byte[] file = ReadRawFile(name, enc, true, fullPath);   // Can read a default file and add raw data, then saves into PersistentData.
        byte[] data = new byte[file.Length + content.Length];
        System.Buffer.BlockCopy(file, 0, data, 0, file.Length); // Add file first.
        System.Buffer.BlockCopy(content, 0, data, file.Length, content.Length); // Add raw data.
        // If requested name doesn't exists, it's created:
        SaveRawFile(name, data, enc, fullPath);
    }

    /// <summary>Checks directory existence (checks PersistentData, then StreamingAssets)</summary>
    public static bool DirectoryExists(string folder, bool checkSA = true, bool fullPath = false)
    {
        // Check existance:
        bool result = false;
        if (!fullPath)
        {
            // Check PersistentData path first:
            string path = Combine(persistentDataPath, folder);
            result = System.IO.Directory.Exists(path);
            if (!result && checkSA)
            {
                // Then check StreamingAssets path:
#if UNITY_ANDROID || UNITY_WEBGL
                result = CheckNameOnIndex("StreamingAssets/" + folder, "D");
#else
                path = Combine(streamingAssetsPath, folder);
                result = System.IO.Directory.Exists(path);
#endif
            }
        }
        else
        {
            // Direct check:
            result = System.IO.Directory.Exists(folder);
        }
        return result;
    }

    /// <summary>Create directory (StreamingAssets folder is read only)</summary>
    public static void CreateDirectory(string name, bool fullPath = false)
    {
        if (!fullPath)
            name = Combine(persistentDataPath, name);
        System.IO.Directory.CreateDirectory(name);
#if UNITY_WEBGL
            if(Application.platform == RuntimePlatform.WebGLPlayer)
                SyncFiles();
#endif
    }

    /// <summary>Delete directory and its content (StreamingAssets is read only)</summary>
    public static void DeleteDirectory(string name, bool fullPath = false)
    {
        if (DirectoryExists(name, false, fullPath))
        {
            if (!fullPath)
                name = Combine(persistentDataPath, name);
            System.IO.Directory.Delete(name, true);
#if UNITY_WEBGL
            if (Application.platform == RuntimePlatform.WebGLPlayer)
                SyncFiles();
#endif
        }
        else
        {
            WriteLine("[FileManagement.DeleteDirectory] Can't delete, directory not found: " + name);
        }
    }

    /// <summary>Delete directory content (StreamingAssets is read only)</summary>
    public static void EmptyDirectory(string name = "", bool filesOnly = true, bool fullPath = false)
    {
        if (DirectoryExists(name, false, fullPath))
        {
            // Delete all files:
            string[] files = ListFiles(name, false, fullPath);
            for (int i = 0; i < files.Length; i++)
            {
                string path = Combine(name, files[i]);
                DeleteFile(path, fullPath);
            }
            // Delete all fodlers also (if requested):
            if (!filesOnly)
            {
                string[] folders = ListDirectories(name, false, fullPath);
                for (int i = 0; i < folders.Length; i++)
                {
                    string path = Combine(name, folders[i]);
                    DeleteDirectory(path, fullPath);
                }
            }

#if UNITY_WEBGL
            if (Application.platform == RuntimePlatform.WebGLPlayer)
                SyncFiles();
#endif
        }
        else
        {
            WriteLine("[FileManagement.EmptyDirectory] Can't delete folder content, folder not found: " + name);
        }
    }

    /// <summary>List directory content (files only)</summary>
    public static string[] ListFiles(string folder, bool checkSA = true, bool fullPath = false)
    {
        // Check existance:
        string[] result = null;
        try
        {
            if (!fullPath)
            {
                // Check PersistentData path first:
                string path = Combine(persistentDataPath, folder);
                path = NormalizePath(path);
                if (DirectoryExists(path, false, true))
                {
                    result = System.IO.Directory.GetFiles(path);
                    result = FilterPathNames(result);
                }
                // Then check StreamingAssets path:
                if (checkSA)
                {
#if UNITY_ANDROID || UNITY_WEBGL
                    if (DirectoryExists(folder))
                    {
                        if (result != null)
                        {
                            string[] temp = GetNamesOnIndex("StreamingAssets/" + folder + "/", "F");
                            temp = FilterPathNames(temp);
                            result = result.Union(temp).ToArray();
                        }
                        else
                        {
                            result = GetNamesOnIndex("StreamingAssets/" + folder + "/", "F");
                            result = FilterPathNames(result);
                        }
                    }
#else
                    path = Combine(streamingAssetsPath, folder);
                    path = NormalizePath(path);
                    if (DirectoryExists(path, false, true))
                    {
                        if (result != null)
                        {
                            string[] temp = System.IO.Directory.GetFiles(path);
                            temp = FilterPathNames(temp);
                            result = result.Union(temp).ToArray();

                        }
                        else
                        {
                            result = System.IO.Directory.GetFiles(path);
                            result = FilterPathNames(result);
                        }
                    }
#endif
                }
            }
            else
            {
                // Checks directly:
                if (DirectoryExists(folder, false, true))
                {
                    result = System.IO.Directory.GetFiles(folder);
                    result = FilterPathNames(result);
                }
            }
            SortPathNames(result);

            // Error message:
            if (result == null)
                WriteLine("[FileManagement.ListFiles] Can't read folder content, folder not found: " + folder);

            return result;
        }
        catch (System.Exception e)
        {
            WriteLine("[FileManagement.ListFiles] Error: " + e.Message, true);
            return result;
        }
    }
    /// <summary>List directory content (files only) filtering by extension</summary>
    public static string[] ListFiles(string folder, string[] filter, bool checkSA = true, bool fullPath = false, bool subfolders = false)
    {
        // The filtered result list:
        System.Collections.Generic.List<string> result = new System.Collections.Generic.List<string>();

        // Get complete file list:
        string[] files = ListFiles(folder, checkSA, fullPath);
        // Extension file filter:
        if (filter.Length > 0 && files != null)
        {
            // Check every file in the list:
            for (int i = 0; i < files.Length; i++)
            {
                // Check file extensions:
                for (int f = 0; f < filter.Length; f++)
                {
                    if (filter[f] == "" || filter[f].ToLower() == GetFileExtension(files[i]).ToLower())
                    {
                        result.Add(files[i]);
                        break;
                    }
                }
            }
        }

        // Search subfolders using the same settings:
        if (subfolders)
        {
            string[] folders = ListDirectories(folder, checkSA, fullPath);
            if (folders != null && folders.Length > 0)
            {
                for (int i = 0; i < folders.Length; i++)
                {
                    string subfolder = Combine(folder, folders[i]);
                    string[] tempFiles = ListFiles(subfolder, filter, checkSA, fullPath, subfolders);
                    if (tempFiles != null)
                    {
                        for (int f = 0; f < tempFiles.Length; f++)
                        {
                            result.Add(Combine(subfolder, tempFiles[f]));
                        }
                    }
                }
            }
        }

        return result.ToArray();        // Return file list.
    }

    /// <summary>List directory content (folders only)</summary>
    public static string[] ListDirectories(string folder, bool checkSA = true, bool fullPath = false)
    {
        // Check existance:
        string[] result = null;
        try
        {
            if (!fullPath)
            {
                // Check PersistentData path first:
                string path = Combine(persistentDataPath, folder);
                if (DirectoryExists(path, false, true))
                {
                    result = System.IO.Directory.GetDirectories(path);
                    result = FilterPathNames(result);
                }
                // Then check StreamingAssets path:
                if (checkSA)
                {
#if UNITY_ANDROID || UNITY_WEBGL
                    if (DirectoryExists(folder))
                    {
                        if (result != null)
                        {
                            string[] temp = GetNamesOnIndex("StreamingAssets/" + folder + "/", "D");    // Search for subfolders.
                            temp = FilterPathNames(temp);
                            result = result.Union(temp).ToArray();
                        }
                        else
                        {
                            result = GetNamesOnIndex("StreamingAssets/" + folder + "/", "D");    // Search for subfolders.
                            result = FilterPathNames(result);
                        }
                    }
#else
                    path = Combine(streamingAssetsPath, folder);
                    if (DirectoryExists(path, false, true))
                    {
                        if (result != null)
                        {
                            string[] temp = System.IO.Directory.GetDirectories(path);
                            temp = FilterPathNames(temp);
                            result = result.Union(temp).ToArray();
                        }
                        else
                        {
                            result = System.IO.Directory.GetDirectories(path);
                            result = FilterPathNames(result);
                        }
                    }
#endif
                }
            }
            else
            {
                // Checks directly:
                if (DirectoryExists(folder, false, true))
                {
                    result = System.IO.Directory.GetDirectories(folder);
                    result = FilterPathNames(result);
                }
            }
            SortPathNames(result);
            // Error message:
            if (result == null)
                WriteLine("[FileManagement.ListDirectories] Can't read folder content, folder not found: " + folder);
            return result;
        }
        catch (System.Exception e)
        {
            WriteLine("[FileManagement.ListDirectories] Error: " + e.Message, true);
            return result;
        }
    }

    /// <summary>Gets all files into a directory as a list of byte arrays</summary>
    public static System.Collections.Generic.List<byte[]> ReadDirectoryContent(string folder, bool enc = false, bool checkSA = true, bool fullPath = false)
    {
        System.Collections.Generic.List<byte[]> content = null;
        if (DirectoryExists(folder, checkSA, fullPath))
        {
            content = new System.Collections.Generic.List<byte[]>();
            string[] list = ListFiles(folder, checkSA, fullPath);
            for (int l = 0; l < list.Length; l++)
            {
                string path = Combine(folder, list[l]);
                byte[] buffer = ReadRawFile(path, enc, checkSA, fullPath);
                content.Add(buffer);
            }
        }
        else
        {
            WriteLine("[FileManagement.ReadDirectoryContent] Can't read folder content, folder not found: " + folder);
        }
        return content;
    }

    /// <summary>Copies a file (not directories)</summary>
    public static void CopyFile(string source, string dest, bool checkSA = true, bool fullPathSource = false, bool fullPathDest = false)
    {
        if (source != "" && dest != "")
        {
            source = NormalizePath(source);
            dest = NormalizePath(dest);
            if (FileExists(source, checkSA, fullPathSource)) // If source exists on persistent data, full path or StreamingAssets.
            {
                byte[] file = ReadRawFile(source, false, checkSA, fullPathSource);
                SaveRawFile(dest, file, false, fullPathDest);
            }
            else
            {
                WriteLine("[FileManagement.CopyFile] Source file not found: " + source);
            }
        }
    }

    /// <summary>Copies a directory with all its content recursively</summary>
    public static void CopyDirectory(string source, string dest, bool checkSA = true, bool fullPathSource = false, bool fullPathDest = false)
    {
        if (source != "" && dest != "")
        {
            source = NormalizePath(source);
            dest = NormalizePath(dest);
            if (DirectoryExists(source, checkSA, fullPathSource)) // If source exists on persistent data, full path or StreamingAssets.
            {
                string[] files = ListFiles(source, checkSA, fullPathSource);            // Get the list of files.
                string[] folders = ListDirectories(source, checkSA, fullPathSource);    // Get the list of folders.
                // Copy every file:
                for (int f = 0; f < files.Length; f++)
                {
                    string pathS = NormalizePath(Combine(source, files[f]));            // Source file path
                    string pathD = NormalizePath(Combine(dest, files[f]));              // Destination file path
                    CopyFile(pathS, pathD, checkSA, fullPathSource, fullPathDest);
                }
                // Iterate on every folder:
                for (int f = 0; f < folders.Length; f++)
                {
                    string pathS = NormalizePath(Combine(source, folders[f]));          // Source folder path.
                    string pathD = NormalizePath(Combine(dest, folders[f]));            // Destination folder path.
                    CopyDirectory(pathS, pathD, checkSA, fullPathSource, fullPathDest);
                }
                CreateDirectory(dest);
            }
            else
            {
                WriteLine("[FileManagement.CopyDirectory] Source path not found: " + source);
            }
        }
    }

    /// <summary>Moves files or directories</summary>
    public static void Move(string source, string dest, bool fullPathSource = false, bool fullPathDest = false)
    {
        if (source != "" && dest != "")
        {
            // If exists in persistent data or fullPath, then moves (StreamingAssets not allowed):
            if (FileExists(source, false, fullPathSource))
            {
                // We are moving a file:
                if (!fullPathSource)
                    source = Combine(persistentDataPath, source);
                if (!fullPathDest)
                    dest = Combine(persistentDataPath, dest);
                CreateDirectory(GetParentDirectory(dest));
                // Load the source file, deletes the source file and save it with the destination name:
                byte[] data = ReadRawFile(source, false, false, fullPathSource);
                DeleteFile(source, fullPathSource);
                SaveRawFile(dest, data, false, fullPathSource);
            }
            else if (DirectoryExists(source, false, fullPathSource))
            {
                // We are moving a folder:
                CopyDirectory(source, dest, false, fullPathSource, fullPathDest);
                DeleteDirectory(source, fullPathSource);
            }
            else
                WriteLine("[FileManagement.Move] Source file not found: " + source);
        }
    }
    /// <summary>Renames files or directories</summary>
    public static void Rename(string source, string dest, bool fullPathSource = false, bool fullPathDest = false)
    {
        // If exists in persistent data or fullPath, then moves (StreamingAssets not allowed):
        if (FileExists(source, false, fullPathSource) || DirectoryExists(source, false, fullPathSource))
            Move(source, dest, fullPathSource, fullPathDest);
        else
            WriteLine("[FileManagement.Rename] Source file not found: " + source);
    }

    /// <summary>Gets the parent directory of a file/folder</summary>
    public static string GetParentDirectory(string path)
    {
        path = NormalizePath(path);
        int slash = path.LastIndexOf('/');
        if (slash >= 0)
        {
            if (path == System.IO.Path.GetPathRoot(path))
                path = "";
            else
                path = path.Substring(0, slash);
        }
        else
            path = "";
        return NormalizePath(path);
    }

    /// <summary>Combines both paths into a single path correctly</summary>
    public static string Combine(string path1, string path2)
    {
        return System.IO.Path.Combine(path1, path2);
    }

    /// <summary>Normalizes a path name</summary>
    public static string NormalizePath(string path)
    {
        path = path.Replace('\\', '/');                         // Uses slashes only.
        path = path.Replace("//", "/");                         // Replaces double slashes.
        if (path.Length >= 1 && path[path.Length - 1] == '/')   // Delete the last slash (to prevent DirectoryExists from failure)
            path = path.Substring(0, path.Length - 1);
        if (path.Length > 1 && path[path.Length - 1] == ':')    // To prevent failure for "C:/" like paths.
            path += '/';
        return path;
    }

    /// <summary>Retrieves a file/folder name from a path</summary>
    public static string GetFileName(string path)
    {
        path = NormalizePath(path);
        return System.IO.Path.GetFileName(path);
    }

    /// <summary>Retrieves a file/folder name from a path</summary>
    public static string GetFileNameWithoutExtension(string path)
    {
        path = NormalizePath(path);
        string file = System.IO.Path.GetFileName(path);
        int point = file.LastIndexOf('.');
        if (point > 0)
            return file.Substring(0, point);
        else
            return file;
    }

    /// <summary>Gets the file extension (even from a path)</summary>
    public static string GetFileExtension(string path)
    {
        int point = path.LastIndexOf('.');
        string ext = "";
        if (point > 0)
            ext = path.Substring(point);
        return ext;
    }

#if UNITY_5_3_OR_NEWER
    /// <summary>Parse strings to requested types</summary>
    public static T CustomParser<T>(string content)
    {
        T val;                          // Return value.
        string[] values = { };          // Separate values to be parsed.
        switch (typeof(T).ToString())
        {
            case "UnityEngine.Vector2":
                content = content.Substring(1, content.Length - 2);     // Delete parentheses
                values = content.Split(',');
                val = (T)System.Convert.ChangeType(new Vector2(float.Parse(values[0]), float.Parse(values[1])), typeof(T));
                break;
            case "UnityEngine.Vector3":
                content = content.Substring(1, content.Length - 2);     // Delete parentheses
                values = content.Split(',');
                val = (T)System.Convert.ChangeType(new Vector3(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2])), typeof(T));
                break;
            case "UnityEngine.Vector4":
                content = content.Substring(1, content.Length - 2);     // Delete parentheses
                values = content.Split(',');
                val = (T)System.Convert.ChangeType(new Vector4(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]), float.Parse(values[3])), typeof(T));
                break;
            case "UnityEngine.Quaternion":
                content = content.Substring(1, content.Length - 2);     // Delete parentheses
                values = content.Split(',');
                val = (T)System.Convert.ChangeType(new Quaternion(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]), float.Parse(values[3])), typeof(T));
                break;
            case "UnityEngine.Rect":
                content = content.Substring(1, content.Length - 2);     // Delete parentheses
                values = content.Split(',');
                val = (T)System.Convert.ChangeType(new Rect(float.Parse(values[0].Split(':')[1]), float.Parse(values[1].Split(':')[1]), float.Parse(values[2].Split(':')[1]), float.Parse(values[3].Split(':')[1])), typeof(T));
                break;
            case "UnityEngine.Color":
                content = content.Substring(5, content.Length - 6);     // Delete parentheses
                values = content.Split(',');
                val = (T)System.Convert.ChangeType(new Color(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]), float.Parse(values[3])), typeof(T));
                break;
            case "UnityEngine.Color32":
                content = content.Substring(5, content.Length - 6);     // Delete parentheses
                values = content.Split(',');
                val = (T)System.Convert.ChangeType(new Color32(byte.Parse(values[0]), byte.Parse(values[1]), byte.Parse(values[2]), byte.Parse(values[3])), typeof(T));
                break;
            default:
                val = (T)System.Convert.ChangeType(content, typeof(T));
                break;
        }
        return val;
    }
#else
    /// <summary>Parse strings to requested types</summary>
    public static T CustomParser<T>(string content)
    {
        return (T)System.Convert.ChangeType(content, typeof(T));
    }
#endif


    /// <summary>Encryption call</summary>
    public static byte[] Encrypt(byte[] data, byte[] key)
    {
#if USE_AES
        return AesEncrypt(data, key);           // Aes encryption.
#else
        return XorEncryptDecrypt(data, key);    // Xor encryption.
#endif
    }
    /// <summary>Decryption call</summary>
    public static byte[] Decrypt(byte[] data, byte[] key)
    {
#if USE_AES
        return AesDecrypt(data, key);           // Aes decryption.
#else
        return XorEncryptDecrypt(data, key);   // Xor decryption.
#endif
    }

    /// <summary>Custom string conversion</summary>
    public static string ByteArrayToString(byte[] content)
    {
        string result = "";
        switch (stringConversion)
        {
#if !(UNITY_WINRT_8_1 || UNITY_WP_8_1 || UNITY_WSA_8_1)
            case FM_StringMode.ASCII:
                result = System.Text.Encoding.ASCII.GetString(content);
                break;
            case FM_StringMode.UTF32:
                result = System.Text.Encoding.UTF32.GetString(content);
                break;
            case FM_StringMode.UTF7:
                result = System.Text.Encoding.UTF7.GetString(content);
                break;
            case FM_StringMode.BigEndianUnicode:
                result = System.Text.Encoding.BigEndianUnicode.GetString(content);
                break;
            case FM_StringMode.Unicode:
                result = System.Text.Encoding.Unicode.GetString(content);
                break;
            case FM_StringMode.UTF8:
                result = System.Text.Encoding.UTF8.GetString(content);
                break;
#endif
            default:
                char[] chars = new char[content.Length];
                content.CopyTo(chars, 0);
                result = new string(chars);
                break;
        }
        return result;
    }

    /// <summary>Custom string conversion</summary>
    public static byte[] StringToByteArray(string content)
    {
        byte[] result = { };
        switch (stringConversion)
        {
#if !(UNITY_WINRT_8_1 || UNITY_WP_8_1 || UNITY_WSA_8_1)
            case FM_StringMode.ASCII:
                result = System.Text.Encoding.ASCII.GetBytes(content);
                break;
            case FM_StringMode.UTF32:
                result = System.Text.Encoding.UTF32.GetBytes(content);
                break;
            case FM_StringMode.UTF7:
                result = System.Text.Encoding.UTF7.GetBytes(content);
                break;
            case FM_StringMode.BigEndianUnicode:
                result = System.Text.Encoding.BigEndianUnicode.GetBytes(content);
                break;
            case FM_StringMode.Unicode:
                result = System.Text.Encoding.Unicode.GetBytes(content);
                break;
            case FM_StringMode.UTF8:
                result = System.Text.Encoding.UTF8.GetBytes(content);
                break;
#endif
            default:
                result = new byte[content.Length];
                for (int c = 0; c < result.Length; c++) result[c] = (byte)content[c];
                break;
        }
        return result;
    }

    /// <summary>Deletes the path strings and leaves the file/directory names</summary>
    private static string[] FilterPathNames(string[] names)
    {
        if (names != null)
        {
            for (int r = 0; r < names.Length; r++)
            {
                names[r] = System.IO.Path.GetFileName(names[r]);
            }
        }
        return names;
    }

    /// <summary>Sorts the names alphabetically</summary>
    private static string[] SortPathNames(string[] names)
    {
        if (names != null)
        {
            System.Array.Sort(names);
        }
        return names;
    }

#if USE_AES
    /// <summary>AES initialization</summary>
    private static readonly AesManaged aes;
    /// <summary>Class constructor</summary>
    static FileManagement()
    {
        aes = new AesManaged { Key = key };
        aes.BlockSize = key.Length * 8;     // Key size expresed in bits (16 * 8 = 128).
    }
    /// <summary>AES encryption</summary>
    private static byte[] AesEncrypt(byte[] data, byte[] key)
    {
        using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, key))
        {
            return encryptor.TransformFinalBlock(data, 0, data.Length);
        }
    }
    /// <summary>AES decryption</summary>
    private static byte[] AesDecrypt(byte[] data, byte[] key)
    {
        using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, key))
        {
            return decryptor.TransformFinalBlock(data, 0, data.Length);
        }
    }
#else
    /// <summary>Basic xor encryption/decryption algorithm</summary>
    private static byte[] XorEncryptDecrypt(byte[] data, byte[] key)
    {
        int d = 0;
        int k = 0;
        byte[] output = new byte[data.Length];

        while (d < data.Length)
        {
            while (k < key.Length && d < data.Length)
            {
                output[d] = (byte)(data[d] ^ key[k]);
                d++;
                k++;
            }
            k = 0;
        }
        return output;
    }
#endif

    /********************************
        PlayerPrefs compatibility:
    ********************************/
    /// <summary>Deletes all files into PersistentData folder</summary>
    public static void DeleteAll()
    {
        EmptyDirectory();
    }
    /// <summary>Deletes a key</summary>
    public static void DeleteKey(string key)
    {
        DeleteFile(key);
    }
    /// <summary>Reads a float value from disk</summary>
    public static float GetFloat(string key, float defaultValue = 0.0F)
    {
        float retVal = defaultValue;
        if (FileExists(key))
            retVal = ReadFile<float>(key);
        return retVal;
    }
    /// <summary>Reads an int value from disk</summary>
    public static int GetInt(string key, int defaultValue = 0)
    {
        int retVal = defaultValue;
        if (FileExists(key))
            retVal = ReadFile<int>(key);
        return retVal;
    }
    /// <summary>Reads a string from disk</summary>
    public static string GetString(string key, string defaultValue = "")
    {
        string retVal = defaultValue;
        if (FileExists(key))
            retVal = ReadFile<string>(key);
        return retVal;
    }
    /// <summary>Checks the existence of a key</summary>
    public static bool HasKey(string key)
    {
        return FileExists(key);
    }
    /// <summary>This has no efect (PlayerPrefs compatibility)</summary>
    public static void Save()
    {
        WriteLine("[FileManagement.Save] This method has no effect, data is already saved.");
    }
    /// <summary>Saves a float value to disk</summary>
    public static void SetFloat(string key, float value)
    {
        SaveFile(key, value);
    }
    /// <summary>Saves an int value to disk</summary>
    public static void SetInt(string key, int value)
    {
        SaveFile(key, value);
    }
    /// <summary>Saves a string to disk</summary>
    public static void SetString(string key, string value)
    {
        SaveFile(key, value);
    }

    // Non standard interfaces:
    // ------------------------
    /// <summary>Reads a bool value from disk</summary>
    public static bool GetBool(string key, bool defaultValue = false)
    {
        bool retVal = defaultValue;
        if (FileExists(key))
            retVal = ReadFile<bool>(key);
        return retVal;
    }
    /// <summary>Reads a double value from disk</summary>
    public static double GetDouble(string key, double defaultValue = 0)
    {
        double retVal = defaultValue;
        if (FileExists(key))
            retVal = ReadFile<double>(key);
        return retVal;
    }
    /// <summary>Saves a bool value to disk</summary>
    public static void SetBool(string key, bool value)
    {
        SaveFile(key, value);
    }
    /// <summary>Saves a double value to disk</summary>
    public static void SetDouble(string key, double value)
    {
        SaveFile(key, value);
    }

    /********************************
        Experimental functionality:
    ********************************/

    /// <summary>Open the work folder in system explorer (not widely supported)</summary>
#if UNITY_WEBGL || UNITY_ANDROID || UNITY_IOS || UNITY_WINRT
    public static void OpenFolder(string path = "", bool fullPath = false)
    {
        WriteLine("[FileManagement.OpenFolder] Not supported in this platform.", true);
    }
#else
    public static void OpenFolder(string path = "", bool fullPath = false)
    {
        if (!fullPath)
            path = persistentDataPath + "/" + path;
        System.Diagnostics.Process.Start(path);
    }
#endif

#if UNITY_WINRT
    /// <summary>Serialize object (works with default & user defined types only)</summary>
    public static byte[] ObjectToByteArray(object obj)
    {
        WriteLine("[FileManagement.ObjectToByteArray] Not supported in this platform.", true);
        return null;
    }
    /// <summary>Deseerialize object (works with default & user defined types only)</summary>
    public static object ByteArrayToObject(byte[] arrBytes)
    {
        WriteLine("[FileManagement.ByteArrayToObject] Not supported in this platform.", true);
        return null;
    }
#else
    /// <summary>Serialize object (works with default & user defined types only)</summary>
    public static byte[] ObjectToByteArray(object obj)
    {
        if (obj == null) return null;
        System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
        System.IO.MemoryStream ms = new System.IO.MemoryStream();
        bf.Serialize(ms, obj);
        return ms.ToArray();
    }
    /// <summary>Deseerialize object (works with default & user defined types only)</summary>
    public static object ByteArrayToObject(byte[] arrBytes)
    {
        System.IO.MemoryStream memStream = new System.IO.MemoryStream();
        System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
        memStream.Write(arrBytes, 0, arrBytes.Length);
        memStream.Seek(0, System.IO.SeekOrigin.Begin);
        return bf.Deserialize(memStream);
    }
#endif

    /// <summary>List logical drives</summary>
#if UNITY_WINRT
    public static string[] ListLogicalDrives()
    {
        WriteLine("[FileManagement.ListLogicDrives] Not supported in this platform.", true);
        return null;
    }
#else
    public static string[] ListLogicalDrives()
    {
        return System.IO.Directory.GetLogicalDrives();
    }
#endif

    /// <summary>Run a local APK file installation in an external process.</summary>
#if UNITY_ANDROID
    public static void InstallLocalApk(string file, bool fullPath = false)
    {
        if (!fullPath)
            file = Combine(Application.persistentDataPath, file);
        if (Application.platform == RuntimePlatform.Android)
        {
            if (FileExists(file))
            {
                // Get the main Activity, Context and classes:
                AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                AndroidJavaObject context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");
                AndroidJavaClass uri = new AndroidJavaClass("android.net.Uri");
                // Evaluate the Andrlid API level:
                AndroidJavaClass build = new AndroidJavaClass("android.os.Build$VERSION");
                int api = build.GetStatic<int>("SDK_INT");
                if (api <= 23)
                {
                    // Create path object to the apk file (Uri):
                    AndroidJavaObject targetUri = uri.CallStatic<AndroidJavaObject>("parse", new object[] { "file://" + file });
                    // Create and set the Intent object to be executed:
                    AndroidJavaClass intent = new AndroidJavaClass("android.content.Intent");
                    AndroidJavaObject promptInstall = new AndroidJavaObject("android.content.Intent", new object[] { intent.GetStatic<string>("ACTION_VIEW") });
                    promptInstall = promptInstall.Call<AndroidJavaObject>("setDataAndType", new object[] { targetUri, "application/vnd.android.package-archive" });
                    promptInstall = promptInstall.Call<AndroidJavaObject>("setFlags", new object[] { intent.GetStatic<int>("FLAG_ACTIVITY_NEW_TASK") });
                    // Call the installation:
                    context.Call("startActivity", new object[] { promptInstall });
                }
                else
                {
                    // Open file content:
                    AndroidJavaObject fileContent = new AndroidJavaObject("java.io.File", new object[] { file });
                    AndroidJavaObject targetUri = uri.CallStatic<AndroidJavaObject>("fromFile", new object[] { fileContent });
                    // Create and set the Intent object to be executed:
                    AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", new object[] { });
                    intent = intent.Call<AndroidJavaObject>("setDataAndType", new object[] { targetUri, "application/vnd.android.package-archive" });
                    intent = intent.Call<AndroidJavaObject>("addFlags", new object[] { intent.GetStatic<int>("FLAG_GRANT_READ_URI_PERMISSION") });
                    // Run the installation:
                    context.Call("startActivity", new object[] { intent });
                }
            }
            else
            {
                WriteLine("[FileManagement.InstallLocalApk] The requested APK file wasn't found.");
            }
        }
        else
        {
            WriteLine("[FileManagement.InstallLocalApk] Please export to some Android device.");
        }
    }
#else
    public static void InstallLocalApk(string file, bool fullPath = false)
    {
        WriteLine("[FileManagement.InstallLocalApk] Not supported in this platform.");
    }
#endif

    /// <summary>Writes to the console depending on the platform.</summary>
    private static void WriteLine(string line, bool error = false)
    {
#if UNITY_5_3_OR_NEWER
        if (error)
            Debug.LogError(line);
        else
            Debug.Log(line);
#else
        System.Console.WriteLine(line);
#endif
    }
}

/// <summary>Ini file parser</summary>
public class FM_IniFile
{
    private System.Collections.Generic.Dictionary<string, string> _iniFile = new System.Collections.Generic.Dictionary<string, string>();
    private char _separator = (char)0x01;

    /// <summary>Constructors</summary>
    public FM_IniFile() { }
    public FM_IniFile(string name, bool enc = false, bool checkSA = true, bool fullPath = false)
    {
        LoadNewIniFile(name, enc, checkSA, fullPath);
    }

    /// <summary>File parser</summary>
    public void LoadNewIniFile(string name, bool enc = false, bool checkSA = true, bool fullPath = false)
    {
        // Load the ini file content:
        string[] lines = FileManagement.ReadAllLines(name, enc, checkSA, fullPath);

        if (lines != null && lines.Length > 0)
        {
            // Analize the file content line by line:
            string section = "";
            for (int i = 0; i < lines.Length; i++)
            {
                string current = lines[i].Trim();                                       // Delete empty spaces at both sides of the text.
                if (!string.IsNullOrEmpty(current))                                     // Discard empty lines.
                {
                    // Check for section, comment or data:
                    switch (current[0])
                    {
                        // Comments are discarded:
                        case ';':
                            break;
                        // Section tag:
                        case '[':
                            section = current.Trim(new char[] { '[', ']' });
                            break;
                        // Everything else may be some valid data:
                        default:
                            string[] data = current.Split(new char[] { '=' }, 2);       // Splits by the first '=' character only.
                            string key = data[0].Trim();                                // Deletes spaces at begining and end.
                            if (!string.IsNullOrEmpty(section))
                                key = section + _separator + key;
                            string value = data[1].Split(new char[] { ';' }, 2)[0];     // Discard comments in the same line.
                            value = value.Trim().Trim(new char[] { '"' });              // Deletes spaces and quotes at begining and end.
                            if (_iniFile.ContainsKey(key))                              // If key already exists, then it's updated.
                                _iniFile[key] = value;
                            else
                                _iniFile.Add(key, value);
                            break;
                    }
                }
            }
        }
        else
        {
            WriteLine("[FM_IniFile.LoadNewIniFile] The requested INI file wouldn't be parsed.");
        }
    }

    /// <summary>File parser</summary>
    public T GetKey<T>(string key, T defaultValue, string section = "")
    {
        // Key:
        string _key = key;
        if (!string.IsNullOrEmpty(section))
            _key = section + _separator + key;
        // Get the value:
        if (_iniFile.ContainsKey(_key))
            return FileManagement.CustomParser<T>(_iniFile[_key]);
        else
            return defaultValue;
    }

    /// <summary>Modifies a key value</summary>
    public void SetKey<T>(string key, T value, string section = "")
    {
        if (KeyExists(key, section))
            AddKey(key, value, section);
        else
            WriteLine("[FM_IniFile.SetKey] Key not found.");
    }

    /// <summary>Add a new key value</summary>
    public void AddKey<T>(string key, T value, string section = "")
    {
        // Key:
        string _key = key;
        if (!string.IsNullOrEmpty(section))
            _key = section + _separator + key;
        // Add the value:
        if (!_iniFile.ContainsKey(_key))
            _iniFile.Add(_key, value.ToString());                                       // Creates the new value.
        else
            _iniFile[_key] = value.ToString();                                          // Updates the existing value.
    }

    /// <summary>Remove a key value</summary>
    public void RemoveKey(string key, string section = "")
    {
        // Key:
        string _key = key;
        if (!string.IsNullOrEmpty(section))
            _key = section + _separator + key;
        // Remove the value:
        if (_iniFile.ContainsKey(_key))
            _iniFile.Remove(_key);
        else
            WriteLine("[FM_IniFile.RemoveKey] Key not found.");
    }

    /// <summary>Remove a complete section</summary>
    public void RemoveSection(string section = "")
    {
        System.Collections.Generic.List<string> _rawKeys = new System.Collections.Generic.List<string>(_iniFile.Keys);
        for (int i = 0; i < _rawKeys.Count; i++)
        {
            int _index = _rawKeys[i].LastIndexOf(_separator);
            string _section = "";
            if (_index > 0)
                _section = _rawKeys[i].Substring(0, _index);

            if (section == _section)
            {
                _iniFile.Remove(_rawKeys[i]);
            }
        }
    }

    /// <summary>Checks if a value exists</summary>
    public bool KeyExists(string key, string section = "")
    {
        // Key:
        string _key = key;
        if (!string.IsNullOrEmpty(section))
            _key = section + _separator + key;
        // If the key exists:
        return _iniFile.ContainsKey(_key);
    }

    /// <summary>Get the list of the sections</summary>
    public string[] GetSectionList(bool sort = false)
    {
        System.Collections.Generic.List<string> _sections = new System.Collections.Generic.List<string>();
        System.Collections.Generic.List<string> _rawKeys = new System.Collections.Generic.List<string>(_iniFile.Keys);
        for (int i = 0; i < _rawKeys.Count; i++)
        {
            int _index = _rawKeys[i].LastIndexOf(_separator);
            string _section = "";
            if (_index > 0)
                _section = _rawKeys[i].Substring(0, _index);
            if (!_sections.Contains(_section))
                _sections.Add(_section);
        }
        if (sort) _sections.Sort();
        return _sections.ToArray();
    }

    /// <summary>Get the list of keys by section</summary>
    public string[] GetKeyList(string section = "", bool sort = false)
    {
        System.Collections.Generic.List<string> _keys = new System.Collections.Generic.List<string>();
        System.Collections.Generic.List<string> _rawKeys = new System.Collections.Generic.List<string>(_iniFile.Keys);
        for (int i = 0; i < _rawKeys.Count; i++)
        {
            string _section = "";
            int _index = _rawKeys[i].LastIndexOf(_separator);
            if (_index > 0)
                _section = _rawKeys[i].Substring(0, _index);
            if (section == _section)
                _keys.Add(_rawKeys[i].Substring(_index + 1));
        }
        if (sort) _keys.Sort();
        return _keys.ToArray();
    }

    /// <summary>Save the content into a file</summary>
    public void Save(string name, bool sort = false, bool enc = false, bool fullPath = false)
    {
        string iniContent = "";                     // Here goes the Ini file final content.
        string section = "";
        string[] sections = GetSectionList(sort);
        string[] keys;                              // Parameters without section goes first.

        // Add the keys by sections:
        for (int s = 0; s < sections.Length; s++)
        {
            // Go to the next section:
            section = sections[s];
            keys = GetKeyList(section, sort);
            // Add the section:
            if (section != "")
                iniContent += "[" + section + "]" + System.Environment.NewLine;
            // Add the keys:
            for (int k = 0; k < keys.Length; k++)
            {
                string val = GetKey(keys[k], "", section);
                if (val != "" && (val[0] == ' ' || val[val.Length - 1] == ' '))
                    iniContent += keys[k] + " = \"" + val + "\"" + System.Environment.NewLine;
                else
                    iniContent += keys[k] + " = " + val + System.Environment.NewLine;
            }
        }

        // Save the Ini file:
        FileManagement.SaveFile(name, iniContent, enc, fullPath);
    }

    /// <summary>Merge the source file with the local ini file</summary>
    public void Merge(FM_IniFile source)
    {
        string[] _sections = source.GetSectionList();
        for (int s = 0; s < _sections.Length; s++)
        {
            string[] _keys = source.GetKeyList(_sections[s]);
            for (int k = 0; k < _keys.Length; k++)
            {
                string value = source.GetKey(_keys[k], "", _sections[s]);
                AddKey(_keys[k], value, _sections[s]);
            }
        }
    }

    /// <summary>Writes to the console depending on the platform.</summary>
    private void WriteLine(string line, bool error = false)
    {
#if UNITY_5_3_OR_NEWER
        if (error)
            Debug.LogError(line);
        else
            Debug.Log(line);
#else
        System.Console.WriteLine(line);
#endif
    }
}
