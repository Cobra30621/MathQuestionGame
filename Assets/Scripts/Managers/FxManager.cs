using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using NueGames.Enums;
using NueGames.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NueGames.Managers
{
    public class FxManager : MonoBehaviour
    {
        public FxManager(){}
        public static FxManager Instance { get; private set; }
    

        [SerializeField] private FXData fxData;
        
        [Header("Floating Text")]
        [SerializeField] private FloatingText floatingTextPrefab;
        


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
        public void PlayFx(Transform targetTransform, FxName targetFx)
        {
            Instantiate(fxData.GetFX(targetFx), targetTransform);
        }
        #endregion
        
    }

    
}