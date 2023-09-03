using System;
using System.Collections.Generic;
using System.Linq;
using CardAction;
using Data;
using NueGames.Enums;
using NueGames.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NueGames.Managers
{
    public class FxManager : Singleton<FxManager>
    {
        [Header("Floating Text")]
        [SerializeField] private FloatingText floatingTextPrefab;
        
        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout)]
        public Dictionary<FxSpawnPosition, Transform> FXSpawnPositionDictionary;



        #region Public Methods

        public void SpawnFloatingText(Transform targetTransform,string text, int xDir =0, int yDir =-1)
        {
            var cloneText =Instantiate(floatingTextPrefab, targetTransform.position, Quaternion.identity);
            
            if (xDir == 0)
                xDir = Random.value>=0.5f ? 1 : -1;
            cloneText.PlayText(text,xDir,yDir);
        }
        
        
        public void PlayFx(GameObject fxGo, Transform targetTransform)
        {
            Debug.Log($"fxGo {fxGo}");
            Instantiate(fxGo, targetTransform);
        }

        public void PlayFx(GameObject fxGo, Transform targetTransform, Vector3 fxPosition)
        {
            Debug.Log($"fxGo {fxGo}");
            var t = Instantiate(fxGo, targetTransform).GetComponent<Transform>();
            t.position = fxPosition;
        }
        
 

        public Transform GetFXSpawnPosition(FxSpawnPosition fxSpawnPosition)
        {
            if (FXSpawnPositionDictionary.ContainsKey(fxSpawnPosition))
            {
                return FXSpawnPositionDictionary[fxSpawnPosition];
            }
            else
            {
                Debug.LogError($"不存在 fx {fxSpawnPosition} spawn position，請去 FXManager 設定");
                return null;
            }
        }
        
        #endregion
        
    }

    
}