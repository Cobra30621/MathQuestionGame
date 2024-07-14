using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Action.Sequence
{
    public class FXPlayer :  MonoBehaviour
    {
        public List<GameObject> fxs;
        public void Play()
        {
            
        }

        public bool IsPlaying()
        {
            foreach (var fx in fxs)
            {
                if (fx != null)
                {
                    return true;
                }
            }

            return false;
        }
    }
}