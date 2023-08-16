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
            return ES3.Load("gameData", new GameData());
        }
    }
}