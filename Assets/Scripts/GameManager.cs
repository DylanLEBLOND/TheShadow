using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public Player mainPlayer;

	void Start()
	{

	}

	void Update()
	{
		if (! mainPlayer.isAlive)
		{
			Debug.Log ("GAME OVER");
			SceneManager.LoadScene ("ProtoLevel");
		}
	}
}
