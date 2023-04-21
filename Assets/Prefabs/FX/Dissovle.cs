using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissovle : MonoBehaviour
{
	Material material;

	bool isDissolving = false;
	float fade = 1f;
    // Start is called before the first frame update
    void Start()
    {
        // Get a reference to the material
		material = GetComponent<SpriteRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
       if (Input.GetKeyDown(KeyCode.Space))
		{
			isDissolving = true;
		}

		if (isDissolving)
		{
			fade -= Time.deltaTime;

			if (fade <= 0f)
			{
				fade = 0f;
				isDissolving = false;
			}

			// Set the property
			material.SetFloat("_Fade", fade);
		} 
    }
}
