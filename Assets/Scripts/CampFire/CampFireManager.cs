using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace CampFire
{
    public class CampFireManager : MonoBehaviour
    {
        public static CampFireManager Instance => GameManager.Instance.CampFireManager;

       

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
            Debug.Log("ThrowCardUI");
        }
    }
}