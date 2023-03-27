using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using NueGames.Action;
using NueGames.Characters;
using NueGames.Combat;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;


public class DamageActionTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void damage_action_give_10_damage_to_enemy()
    {
        // Arrange
        DamageAction damageAction = new DamageAction();

        CharacterBase self = Substitute.For<CharacterBase>();
        CharacterBase target = Substitute.For<CharacterBase>();
        
        var damage = 10;
        DamageInfo damageInfo = new DamageInfo(damage, self);
        damageAction.SetValue(damageInfo, target);

        target.CharacterStats.MaxHealth = 100;

        // Act
        damageAction.DoAction();
        
        // Assert
        var currentHealth = target.CharacterStats.CurrentHealth;
        var expectHealth = 90;
        Assert.Equals(currentHealth, expectHealth);
    }
}
