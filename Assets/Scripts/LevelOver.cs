using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOver : MonoBehaviour
{
	public bool isOver = false;

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Player")
		{
			isOver = true;
		}
	}

	/* We use the two following functions (Stay and Exit) to make the opening condition smoother with player's shadow timing activation */

	void OnTriggerStay2D (Collider2D other)
	{
		if (other.tag == "Player")
		{
			isOver = true;
		}
	}

	void OnTriggerExit2D (Collider2D other)
	{
		if (other.tag == "Player")
		{
			isOver = true;
		}
	}
}
