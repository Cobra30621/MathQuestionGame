using System.Collections.Generic;
using NueGames.Action;
using NueGames.Characters;
using NueGames.Managers;
using UnityEngine;

namespace CampFire
{
    public class CampFireManager : MonoBehaviour
    {
        #region Instance(Singleton)

        private static CampFireManager instance;

        public static CampFireManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<CampFireManager>();

                    if (instance == null)
                    {
                        Debug.LogError($"The GameObject with {typeof(CampFireManager)} does not exist in the scene, " +
                                       $"yet its method is being called.\n" +
                                       $"Please add {typeof(CampFireManager)} to the scene.");
                    }
                }

                return instance;
            }
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }


        #endregion

        public float healPercent = 0.3f;

        public void Heal()
        {
            Debug.Log("Heal");
            GameManager.Instance.HealAlly(healPercent);
        }

        public void MathLevelUp()
        {
            Debug.Log("MathLevelUp");
        }

        public void ThrowCard()
        {
            Debug.Log("ThrowCard");
        }
    }
}