using UnityEngine;

namespace NueGames.Data.Settings
{
    [CreateAssetMenu(fileName = "Scene Data", menuName = "NueDeck/Settings/Scene", order = 2)]
    public class SceneData : ScriptableObject
    {
        public int mainMenuSceneIndex = 0;
        public int mapSceneIndex = 1;
        public int combatSceneIndex = 2;

        // public string mainMenuScene = "0- Main Menu";
        // public string mapScene = "1- Map";
        // public string combatScene = "2- Combat Scene";
    }
}