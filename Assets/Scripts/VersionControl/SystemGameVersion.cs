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
        [Required]
        [SerializeField] private GameVersionData _gameVersionData;
        
        public void LoadData(PermanentGameData data)
        {
        }

        public void SaveData(PermanentGameData data)
        {
            var systemVersion = _gameVersionData.GetLatestVersion();
            data.saveVersion = systemVersion;
        }

        public GameVersion SystemVersion()
        {
            return _gameVersionData.GetLatestVersion();
        }
    }
}