using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kalkatos.DottedArrow;

namespace Kalkatos.DottedArrow
{
    public class ArrowController : MonoBehaviour
    {
        [SerializeField] private Arrow arrow;
        [SerializeField] private ArrowColor enterEnemyColor;
        [SerializeField] private ArrowColor leaveEnemyColor;

        public void Activate() => arrow.Activate();
        public void Deactivate() => arrow.Deactivate();

        public void SetCamera(Camera camera)
        {
            arrow.SetCamera(camera);
        }
        public void SetupAndActivate (Transform origin)
        {
            arrow.SetupAndActivate(origin);
        }

        public void OnEnterEnemy()
        {
            arrow.SetColor(enterEnemyColor);
        }

        public void OnLeaveEnemy()
        {
            arrow.SetColor(leaveEnemyColor);
        }
        
    }
    
    

}
