using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
	public int damage = 40;
	public Rigidbody2D rb;
	
	//public GameObject impactEffect;
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.right * speed;
    }

	void OnTriggerEnter2D (Collider2D hitInfo)
	{
		//RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right);

		Monster monster = hitInfo.GetComponent<Monster>();
		if (monster != null)
		{
			monster.TakeDamage(damage);
		}
		//Instantiate(impactEffect, transform.position, transform.rotation);

		Destroy(gameObject);
    }
}
