using Data;
using UnityEngine;

namespace DataPersistence
{
    public static class ES3Handler
    {
        public static void ClearGameData()
        {
            ES3.DeleteKey("gameData");
        }


        public static void ClearAllData()
        {
            ES3.DeleteKey("gameData");
            ES3.DeleteKey("permanentGameData");
        }

        public static bool IsFirstEnterGame()
        {
             bool firstEnterGame = ES3.Load("firstEnterGame", true);

             return firstEnterGame;
        }
        
        public static void SetHaveEnterGame()
        {
            ES3.Save("firstEnterGame", false);
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

        public static void SavePermanent(PermanentGameData permanentGameData)
        {
            Debug.Log($"Save Permanent {JsonUtility.ToJson(permanentGameData)}");
            ES3.Save("permanentGameData", permanentGameData);
        }

        public static PermanentGameData LoadPermanent()
        {
            var permanentGameData = ES3.Load("permanentGameData", new PermanentGameData());
            Debug.Log($"Load Permanent {JsonUtility.ToJson(permanentGameData)}");
            return permanentGameData;
        }
    }
}