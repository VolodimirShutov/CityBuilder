using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;
using Object = UnityEngine.Object;

namespace ShootCommon.AssetReferences
{
    public class AssetReferenceStorage : IAssetReferenceStorage
    {
        private static int _maxUploaders = 10;
        private readonly List<string> _uploadedInGeneral = new List<string>();
        private List<string> _waiteForUpload = new List<string>();
        private readonly List<UploadingModel> _workers = new List<UploadingModel>();

        private int _inUploading;

        public bool AllUploded => _workers.Count == 0;
        public int WorkersCount => _workers.Count;

        public void SpawnScriptableById(string id, Action<ScriptableObject> callback)
        {
            Debug.Log("SpawnScriptableById " + id);

            Addressables.LoadAssetAsync<ScriptableObject>(id).Completed += (obj =>
            {
                ScriptableObject myGameObject = obj.Result;
                callback?.Invoke(myGameObject);
            });
        }
        
        private bool CheckUploadedInGeneral(string id)
        {
            return _uploadedInGeneral.Contains(id);
        }

        private void ClearWaiteForUpload()
        {
            List<string> waiteForUpload = new List<string>();
            foreach (string id in _waiteForUpload)
            {
                if (!CheckUploadedInGeneral(id))
                {
                    waiteForUpload.Add(id);
                }
            }
            _waiteForUpload = waiteForUpload;
        }

        private void TryUpload()
        {
            if (_inUploading < _maxUploaders && _workers.Count > _inUploading )
            {
                UploadingModel uploadingModel = _workers[_inUploading];
                uploadingModel.Worker.Invoke(uploadingModel);
                _workers.Remove(uploadingModel);
                _inUploading++;
            }
        }
        
        public void SpawnById(string id, Action<GameObject> callback)
        {
            UploadingModel uploadingModel = new UploadingModel();
            uploadingModel.Id = id;
            uploadingModel.CallbackGameObject = callback;
            uploadingModel.Worker = SpawnByIdWorker;
            
            _workers.Add(uploadingModel);
            _waiteForUpload.Add(id);
            TryUpload();
        }

        private void SpawnByIdWorker(UploadingModel uploadingModel)
        {
            Addressables.LoadAssetAsync<GameObject>(uploadingModel.Id).Completed += (obj =>
            {
                if(!_uploadedInGeneral.Contains(uploadingModel.Id))
                    _uploadedInGeneral.Add(uploadingModel.Id);
                GameObject myGameObject = obj.Result;
                uploadingModel.CallbackGameObject?.Invoke(myGameObject);
                ClearWaiteForUpload();
                _inUploading--;
                TryUpload();
            });
        }

        public void SpawnSpriteById(string id, Action<Sprite> callback)
        {
            UploadingModel uploadingModel = new UploadingModel();
            uploadingModel.Id = id;
            uploadingModel.CallbackSprite = callback;
            uploadingModel.Worker = SpawnSpriteByIdWorker;
            
            _workers.Add(uploadingModel);
            _waiteForUpload.Add(id);
            TryUpload();
        }

        private void SpawnSpriteByIdWorker(UploadingModel uploadingModel)
        {
            //Debug.Log("SpawnSpriteById " + uploadingModel.Id);
            Addressables.LoadAssetAsync<Sprite>(uploadingModel.Id).Completed += (obj =>
            {
                if(!_uploadedInGeneral.Contains(uploadingModel.Id))
                    _uploadedInGeneral.Add(uploadingModel.Id);
                Sprite sprite = obj.Result;
                uploadingModel.CallbackSprite?.Invoke(sprite);
                ClearWaiteForUpload();
                _inUploading--;
                TryUpload();
            });
        }
        
        private void SpawnObjectById(string id, Action<Object> callback)
        {
            UploadingModel uploadingModel = new UploadingModel();
            uploadingModel.Id = id;
            uploadingModel.CallbackObject = callback;
            uploadingModel.Worker = SpawnObjectByIdWorker;
            
            _workers.Add(uploadingModel);
            _waiteForUpload.Add(id);
            TryUpload();
        }

        private void SpawnObjectByIdWorker(UploadingModel uploadingModel)
        {
            //Debug.Log("SpawnObjectById " + uploadingModel.Id);
            Addressables.LoadAssetAsync<Object>(uploadingModel.Id).Completed += (obj =>
            {
                if(!_uploadedInGeneral.Contains(uploadingModel.Id))
                    _uploadedInGeneral.Add(uploadingModel.Id);
                Object result = obj.Result;
                uploadingModel.CallbackObject?.Invoke(result);
                ClearWaiteForUpload();
                _inUploading--;
                TryUpload();
            });
        }
        
        public void SpawnObjectByIdAndDispose(string id, Action<Object> callback)
        {
            UploadingModel uploadingModel = new UploadingModel();
            uploadingModel.Id = id;
            uploadingModel.CallbackObject = callback;
            uploadingModel.Worker = SpawnObjectByIdAndDisposeWorker;
            
            _workers.Add(uploadingModel);
            _waiteForUpload.Add(id);
            TryUpload();
        }

        public void CheckPreloadUpdate(Action<bool> callbackResult)
        {
            Addressables.InitializeAsync().Completed += completed =>
            {
                Addressables.CheckForCatalogUpdates().Completed += checkForUpdates=>
                {
                    if(checkForUpdates.Status == AsyncOperationStatus.Failed)
                    {
                        Debug.LogWarning("Fetch failed!");
                    }
 
                    if (checkForUpdates.Result.Count > 0)
                    {
                        Debug.Log("Available Update:");
                        foreach(var update in checkForUpdates.Result)
                        {
                            Debug.Log(update);
                        }
                        // proceed with downloading new content
                    }
                    else
                    {
                        Debug.LogError("No Available Update");
                        // proceed with loading from cache
                    }
                
                    callbackResult?.Invoke(checkForUpdates.Result.Count > 0);
                };
            };
        }
        
        private void SpawnObjectByIdAndDisposeWorker(UploadingModel uploadingModel)
        {
            Addressables.LoadAssetAsync<Object>(uploadingModel.Id).Completed += (obj =>
            {
                if (!_uploadedInGeneral.Contains(uploadingModel.Id))
                    _uploadedInGeneral.Add(uploadingModel.Id);
                Object result = obj.Result;
                uploadingModel.CallbackObject?.Invoke(result);
                ClearWaiteForUpload();
                _inUploading--;
                Addressables.Release(obj);

                TryUpload();
            });
        }

        public void SpawnUnknownById(string id, 
            Action<Sprite> callbackSprite, 
            Action<GameObject> callbackGameObject,
            Action<SpriteAtlas> callbackSpriteAtlas = null)
        {
            //Debug.Log("SpawnUnknownById " + id);
            SpawnObjectById(id, o =>
            {
                if (o is Sprite sprite)
                {
                    callbackSprite?.Invoke(sprite);
                }

                if (o is GameObject gameObject)
                {
                    callbackGameObject?.Invoke(gameObject);
                }

                if (o is Texture2D)
                {
                    SpawnSpriteById(id, callbackSprite);
                }

                if (o is SpriteAtlas spriteAtlas)
                {
                    callbackSpriteAtlas?.Invoke(spriteAtlas);
                }
            });
        }

        private class UploadingModel
        {
            public string Id;
            public Action<UploadingModel> Worker;
            public Action<Sprite> CallbackSprite;
            public Action<GameObject> CallbackGameObject;
            public Action<SpriteAtlas> CallbackSpriteAtlas;
            public Action<Object> CallbackObject;
        }
    }
}