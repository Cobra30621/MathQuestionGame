using Data;
using UnityEngine;

namespace DataPersistence
{
    public static class ES3Handler
    {
        public static void Clear()
        {
            ES3.DeleteKey("gameData");
        }
        
        public static void Save(GameData gameData)
        {
            Debug.Log($"Save {JsonUtility.ToJson(gameData)}");
            ES3.Save("gameData", gameData);
        }

        public static GameData Load()
        {
            var gameData = ES3.Load("gameData", new GameData());
            Debug.Log($"Load {JsonUtility.ToJson(gameData)}");
            return gameData;
        }
    }
}