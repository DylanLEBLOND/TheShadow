using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperJumpGiver : MonoBehaviour
{
	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Player")
		{
			other.SendMessage ("superJumpActivate");
			this.gameObject.SetActive (false);
		}
	}

	/* We use the two following functions (Stay and Exit) to make the opening condition smoother with player's shadow timing activation */

	void OnTriggerStay2D (Collider2D other)
	{
		if (other.tag == "Player")
		{
			other.SendMessage ("superJumpActivate");
			this.gameObject.SetActive (false);
		}
	}

	void OnTriggerExit2D (Collider2D other)
	{
		if (other.tag == "Player")
		{
			other.SendMessage ("superJumpActivate");
			this.gameObject.SetActive (false);
		}
	}
}
