using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform firePoint;
	public GameObject bulletPrefab;
	public int damage = 40;
	

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			Shoot();
		}
	}

	void Shoot ()
	{
		Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
		RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right);
		if (hitInfo)
		{
			//Debug.Log(hitInfo.transform.name);
			Monster monster = hitInfo.transform.GetComponent<Monster>();
			if(monster != null)
			{
				monster.TakeDamage(damage);
			}
			//Instantiate(impactEffect, transform.position, transform.rotation);

		}
	
	}
}