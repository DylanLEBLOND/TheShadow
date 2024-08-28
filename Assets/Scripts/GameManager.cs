using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public Player mainPlayer;
	public GameObject EndGameUIs;

	private LevelOver endGame;
	private bool gameOver;

	void Start()
	{
		endGame = this.transform.GetChild(0).GetComponent<LevelOver>();
		this.EndGameUIs.SetActive(false);
		this.gameOver = false;
	}

	public void Restart()
	{
		SceneManager.LoadScene ("ProtoLevel");
	}

	public void Quit()
	{
		Application.Quit();
	}

	private void LoadEndGameUIs(bool won)
	{
		this.EndGameUIs.SetActive(true);
		if (won)
		{
			this.EndGameUIs.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(true);
			this.EndGameUIs.transform.GetChild(0).GetChild(0).GetChild(1).gameObject.SetActive(false);
		}
		else
		{
			this.EndGameUIs.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(false);
			this.EndGameUIs.transform.GetChild(0).GetChild(0).GetChild(1).gameObject.SetActive(true);
		}
	}

	void Update()
	{
		if (! this.gameOver)
		{
			if (! mainPlayer.isAlive)
			{
				Debug.Log ("GAME OVER");
				this.LoadEndGameUIs(false);
				this.gameOver = true;
			}
			else if (endGame.isOver)
			{
				Debug.Log ("LEVEL COMPLETED");
				this.LoadEndGameUIs(true);
				this.gameOver = true;
			}
		}
	}
}
