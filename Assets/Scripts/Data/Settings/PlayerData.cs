// using System;
// using System.Collections.Generic;
// using NueGames.Characters;
// using NueGames.Data.Characters;
// using NueGames.Data.Settings;
// using NueGames.Relic;
//
// namespace Data
// {
//     /// <summary>
//     /// 玩家資料(存檔用)
//     /// </summary>
//     [Serializable]
//     public class PlayerData
//     {
//
//         public AllyHealthData AllyHealthData;
//         
//         public string AllyDataGuid;
//         
//         
//
//         public PlayerData()
//         {
//             AllyHealthData = new AllyHealthData();
//         }
//         
//         public PlayerData(AllyData allyData)
//         {
//             AllyHealthData = new AllyHealthData()
//             {
//                 CurrentHealth = allyData.MaxHealth,
//                 MaxHealth = allyData.MaxHealth
//             };
//         }
//
//         public void SetHealth(int newCurrentHealth, int newMaxHealth)
//         {
//             AllyHealthData.CurrentHealth = newCurrentHealth;
//             AllyHealthData.MaxHealth = newMaxHealth;
//         }
//
//     }
// }