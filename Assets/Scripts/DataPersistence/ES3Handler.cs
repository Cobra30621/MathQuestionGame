using Data;

namespace DataPersistence
{
    public static class ES3Handler
    {
        public static void Save(GameData gameData)
        {
            ES3.Save("gameData", gameData, new ES3Settings(ES3.ReferenceMode.ByRefAndValue));
        }

        public static GameData Load()
        {
            return ES3.Load<GameData>("gameData", new GameData(), new ES3Settings(ES3.ReferenceMode.ByRefAndValue));
        }
    }
}