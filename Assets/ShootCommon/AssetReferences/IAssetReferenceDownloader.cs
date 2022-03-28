using System;
using UnityEngine;
using UnityEngine.U2D;
using Object = UnityEngine.Object;

namespace ShootCommon.AssetReferences
{
    public interface IAssetReferenceDownloader
    {
        void SpawnScriptableById(string id, Action<ScriptableObject> callback);
        public bool AllUploded { get; }
        public int WorkersCount{ get; }
        void SpawnById(string id, Action<GameObject> callback);
        void SpawnSpriteById(string id, Action<Sprite> callback);
        void SpawnObjectByIdAndDispose(string id, Action<Object> callback);
        void CheckPreloadUpdate(Action<bool> callbackResult);
        void SpawnUnknownById(string id,
            Action<Sprite> callbackSprite,
            Action<GameObject> callbackGameObject,
            Action<SpriteAtlas> callbackSpriteAtlas = null);
    }

}