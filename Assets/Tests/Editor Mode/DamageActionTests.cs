// using System.Collections;
// using System.Collections.Generic;
// using NSubstitute;
// using NueGames.Action;
// using NueGames.Characters;
// using NueGames.Characters.Allies;
// using NueGames.Characters.Enemies;
// using NueGames.Combat;
// using NUnit.Framework;
// using UnityEngine;
// using UnityEngine.TestTools;
//
//
// public class DamageActionTests
// {
//     // A Test behaves as an ordinary method
//     [Test]
//     public void damage_action_give_10_damage_to_enemy()
//     {
//         // Arrange
//         var damageAction = new DamageAction();
//
//         // var selfGameObject = new GameObject();
//         // var self = selfGameObject.AddComponent<PlayerExample>();
//         //
//         // var targetGameObject = new GameObject();
//         // var target = targetGameObject.AddComponent<EnemyExample>();
//         //
//         // var canvasGameObject = new GameObject();
//         // var canvas = canvasGameObject.AddComponent<AllyCanvas>();
//
//         var self = Substitute.For<CharacterBase>();
//         var target = Substitute.For<CharacterBase>();
//         var canvas = Substitute.For<CharacterCanvas>();
//
//         var maxHealth = 100;
//         var targetStatus = new CharacterStats(maxHealth, target);
//         target.SetCharacterStatus(targetStatus);
//         self.SetCharacterStatus(targetStatus);
//         
//         var damage = 10;
//         var damageInfo = new DamageInfo();
//         // damageAction.SetValue(damageInfo, target);
//
//         // Act
//         damageAction.DoAction();
//         
//         // Assert
//         var currentHealth = target.CharacterStats.CurrentHealth;
//         var expectHealth = 90;
//         Assert.AreEqual(expectHealth, currentHealth);
//     }
//
//     
// }
