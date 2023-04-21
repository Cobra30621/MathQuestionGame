using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public int health = 100;
	
    //public GameObject deathEffect;
    // Start is called before the first frame update
    public void TakeDamage (int damage)
	{
		health -= damage;
		

		if (health <= 0)
		{
			Die();
		}
	}
    void Die ()
	{
		//Instantiate(deathEffect, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}
}
