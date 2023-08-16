using System.Collections.Generic;
using Data;
using NueGames.Data.Collection;
using NueGames.Encounter;
using Question;

namespace Data
{
    public class GameData
    {
        public PlayerData PlayerData;

        public string MapJson;
        public MapEncounter MapEncounter;

        public QuestionSetting QuestionSetting;
        

        public List<string> CardDataGuids;
    }
}