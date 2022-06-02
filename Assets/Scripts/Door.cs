using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
	public bool isOpen = false;
	public float openingDuration;

	private Animator anim;

	private float lastOpeningTime = 0.0f;

	void Start ()
	{
		anim = this.GetComponent<Animator>();
//		Physics2D.IgnoreLayerCollision (6 /* Shadow */, 8 /* Doors */, true);
	}

	public void openDoor ()
	{
		if (isOpen)
		{
			lastOpeningTime = Time.time;
			return;
		}

		anim.SetBool ("isOpen", true);
		isOpen = true;
		lastOpeningTime = Time.time;
	}

	public void closeDoor ()
	{
		if (! isOpen)
			return;

		anim.SetBool ("isOpen", false);
		isOpen = false;
	}

	void Update ()
	{
		if (isOpen && Time.time - lastOpeningTime >= openingDuration)
		{
				closeDoor ();
		}
	}
}
