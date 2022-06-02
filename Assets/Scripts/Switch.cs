using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
	public Door associatedDoor;

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Shadow" && other.GetComponent<Shadow>().isTangible)
		{
			associatedDoor.openDoor ();
		}
	}

	/* We use the two following functions (Stay and Exit) to make the opening condition smoother with player's shadow timing activation */

	void OnTriggerStay2D (Collider2D other)
	{
		if (other.tag == "Shadow" && other.GetComponent<Shadow>().isTangible)
		{
			associatedDoor.openDoor ();
		}
	}

	void OnTriggerExit2D (Collider2D other)
	{
		if (other.tag == "Shadow" && other.GetComponent<Shadow>().isTangible)
		{
			associatedDoor.openDoor ();
		}
	}
}
