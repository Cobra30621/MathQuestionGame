
using System.Collections.Generic;
using Action.Sequence;
using Managers;
using NueGames.Enums;
using NueGames.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NueGames.Managers
{
    public class FxManager : SerializedMonoBehaviour
    {
        public static FxManager Instance => GameManager.Instance.FxManager;
        
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
        
        
        public FXPlayer PlayFx(FXPlayer fxGo, Transform targetTransform)
        {
            var fxPlayer = Instantiate(fxGo, targetTransform);
            fxPlayer.Play();

            return fxPlayer;
        }

        public FXPlayer PlayFx(FXPlayer fxGo, Transform targetTransform, Vector3 fxPosition)
        {
            var fxPlayer = Instantiate(fxGo, targetTransform);
            fxPlayer.Play();
            fxPlayer.GetComponent<Transform>().position = fxPosition;
            return fxPlayer;
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