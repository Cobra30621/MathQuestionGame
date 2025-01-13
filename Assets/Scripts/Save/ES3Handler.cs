using Save.Data;
using UnityEngine;

namespace Save
{
    public static class ES3Handler
    {
        
        public static void ClearAllData()
        {
            ES3.DeleteKey("gameData");
            ES3.DeleteKey("permanentGameData");
            ES3.DeleteKey("firstEnterGame");
            ES3.DeleteKey("hasOngoingGame");
        }

        #region 永久資料

        public static bool IsFirstEnterGame()
        {
            bool firstEnterGame = ES3.Load("firstEnterGame", true);

            return firstEnterGame;
        }
        
        public static void SetHaveEnteredGame()
        {
            ES3.Save("firstEnterGame", false);
        }

        public static void SavePermanent(PermanentGameData permanentGameData)
        {
            ES3.Save("permanentGameData", permanentGameData);
        }

        public static PermanentGameData LoadPermanent()
        {
            var permanentGameData = ES3.Load("permanentGameData", new PermanentGameData());
            return permanentGameData;
        }
        

        #endregion
        
        
        #region 單局遊戲
        /// <summary>
        /// 清除單局遊戲資料
        /// </summary>
        public static void ClearSingleGameData()
        {
            ES3.DeleteKey("gameData");
            ES3.DeleteKey("hasOngoingGame");
        }
        
        /// <summary>
        /// 是否有正在遊玩中的單局遊戲存檔
        /// </summary>
        /// <returns></returns>
        public static bool LoadHasOngoingGame()
        {
            bool hasOngoingGame = ES3.Load("hasOngoingGame", false);
        
            return hasOngoingGame;
        }
        
        public static void SetHasOngoingGame()
        {
            ES3.Save("hasOngoingGame", true);
        }
        
        
        public static void SaveSingleGame(GameData gameData)
        {
            ES3.Save("gameData", gameData);
        }
        
        public static GameData LoadSingleGame()
        {
            var gameData = ES3.Load("gameData", new GameData());
            return gameData;
        }

        #endregion



        
    }
}