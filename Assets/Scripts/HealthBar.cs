using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{

	public float dampening = 5f;
	public float changeSpeed = .5f;

	float timeout = 0f;
	Material mat;
	float fillTarget;
	float delta = 0f;

	void Awake()
	{
		Renderer rend = GetComponent<Renderer>();
		Image img = GetComponent<Image>();
		if (rend != null)
		{
			mat = new Material(rend.material);
			rend.material = mat;
		}
		else if (img != null)
		{
			mat = new Material(img.material);
			img.material = mat;
			// Access the stats by using PlayerStats.Instance.{StatName}
			float health = PlayerStats.Instance.Health / PlayerStats.Instance.MaxHealth;
			mat.SetFloat("_Fill", fillTarget);
			fillTarget = health;
		}
		else
		{
			Debug.LogWarning("No Renderer or Image attached to " + name);
		}


	}

	void Update()
	{
		timeout += Time.deltaTime * changeSpeed;
		if (timeout > 1.0f)
		{
			timeout = 0f;

			float health = PlayerStats.Instance.Health / PlayerStats.Instance.MaxHealth;
			float newFill = health;

			// Modify delta by how much fillTarget will change
			delta = 0;
			//delta -= fillTarget - newFill;
			fillTarget = newFill;

		}

		// The main idea of animating the bar this way is 
		// 1. Set "_Fill" to whatever value the bar actually has [0, 1]
		// 2. Gradually bring "_Delta" to zero

		// For a slightly different effect, 
		// 1. Keep "_Delta" at zero 
		// 2. Lerp "_Fill" to the target value [0, 1]

		// Also: See the included shader for more information about other properties.

		//delta = Mathf.Lerp(delta, 0, Time.deltaTime * dampening);

		mat.SetFloat("_Delta", delta);
		mat.SetFloat("_Fill", fillTarget);
	}
}
