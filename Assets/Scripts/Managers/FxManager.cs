using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using NueGames.Enums;
using NueGames.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NueGames.Managers
{
    public class FxManager : SerializedMonoBehaviour
    {
        public FxManager(){}
        public static FxManager Instance { get; private set; }
    
        [InlineEditor()]
        [SerializeField] private FXData fxData;
        
        [Header("Floating Text")]
        [SerializeField] private FloatingText floatingTextPrefab;
        
        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout)]
        public Dictionary<FxSpawnPosition, Transform> FXSpawnPositionDictionary;



        #region Setup
        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                transform.parent = null;
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
        #endregion

        #region Public Methods

        public void SpawnFloatingText(Transform targetTransform,string text, int xDir =0, int yDir =-1)
        {
            var cloneText =Instantiate(floatingTextPrefab, targetTransform.position, Quaternion.identity);
            
            if (xDir == 0)
                xDir = Random.value>=0.5f ? 1 : -1;
            cloneText.PlayText(text,xDir,yDir);
        }
        
        
        public void PlayFx(FxName targetFx, Transform targetTransform)
        {
            Instantiate(fxData.GetFX(targetFx), targetTransform);
        }

        public void PlayFx(FxName targetFx, Transform targetTransform, Vector3 fxPosition)
        {
            var t = Instantiate(fxData.GetFX(targetFx), targetTransform).GetComponent<Transform>();
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