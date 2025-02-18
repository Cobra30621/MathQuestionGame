using Save;
using Save.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VersionControl
{
    /// <summary>
    /// 遊戲系統目前的版本
    /// </summary>
    public class SystemGameVersion : MonoBehaviour, IPermanentDataPersistence
    {
        [LabelText("現在的遊戲版本")] public GameVersion systemVersion;


        public void LoadData(PermanentGameData data)
        {
        }

        public void SaveData(PermanentGameData data)
        {
            data.saveVersion = systemVersion;
        }
    }
}