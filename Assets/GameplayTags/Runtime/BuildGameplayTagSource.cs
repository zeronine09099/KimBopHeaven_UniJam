using System;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace BandoWare.GameplayTags
{
    internal class BuildGameplayTagSource : IGameplayTagSource
    {
        public string Name => "Build";

        public void RegisterTags(GameplayTagRegistrationContext context)
        {
            try
            {
                string src = Path.Combine(Application.streamingAssetsPath, "GameplayTags");

                byte[] data = LoadStreamingAssetBytes(src);

                MemoryStream ms = new MemoryStream(data);
                BinaryReader reader = new BinaryReader(ms, Encoding.UTF8, false);

                while (ms.Position < ms.Length)
                {
                    string tagName = reader.ReadString();
                    context.RegisterTag(tagName, string.Empty, GameplayTagFlags.None, this);
                }

                reader.Dispose();
                ms.Dispose();
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load gameplay tags from StreamingAssets: {e.Message}");
            }
        }

        private static byte[] LoadStreamingAssetBytes(string absolutePath)
        {
            if (absolutePath.Contains("://") || absolutePath.StartsWith("jar:", StringComparison.OrdinalIgnoreCase))
            {
                UnityWebRequest req = UnityWebRequest.Get(absolutePath);
                UnityWebRequestAsyncOperation op = req.SendWebRequest();
                while (!op.isDone) { }

#if UNITY_2020_2_OR_NEWER
                if (req.result != UnityWebRequest.Result.Success)
#else
                if (req.isNetworkError || req.isHttpError)
#endif
                {
                    string err = req.error;
                    req.Dispose();
                    throw new IOException($"StreamingAssets load failed: {err} | {absolutePath}");
                }

                byte[] bytes = req.downloadHandler.data;
                req.Dispose();

                if (bytes == null || bytes.Length == 0)
                    throw new IOException($"Empty data from StreamingAssets: {absolutePath}");

                return bytes;
            }
            else
            {
                if (!File.Exists(absolutePath))
                    throw new FileNotFoundException($"File not found in StreamingAssets: {absolutePath}");
                return File.ReadAllBytes(absolutePath);
            }
        }
    }
}