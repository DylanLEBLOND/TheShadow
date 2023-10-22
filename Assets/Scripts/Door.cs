using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum doorOpeningDirection { LEFT, RIGHT, TOP, BOTTOM };

public enum doorOpeningState { OPENED, OPENING, CLOSING, CLOSED }

public class Door : MonoBehaviour
{
	public bool isOpenAtStart = false;
	public bool isOpen = false;
	public doorOpeningDirection openingDirection = doorOpeningDirection.BOTTOM;

	[Range(1.0f,10.0f)]
	public float openDuration = 1.0f;
	[Range(0.001f,1.0f)]
	public float openingFrameTime = 0.001f;

	private float lastOpeningTime = 0.0f;
	private float lastFrameTime = 0.0f;
	private Vector2 doorClosedPosition;
	private doorOpeningState openingState;
	private float openingPercentage;

	void Start ()
	{
		if (! isOpenAtStart)
		{
			doorClosedPosition = this.transform.position;
			openingState = doorOpeningState.CLOSED;
			openingPercentage = 0.0f;
		}
		else
		{
			switch (openingDirection)
			{
				case doorOpeningDirection.LEFT:
					doorClosedPosition = new Vector2 (this.transform.position.x + 11, this.transform.position.y);
					break;
				case doorOpeningDirection.RIGHT:
					doorClosedPosition = new Vector2 (this.transform.position.x - 11, this.transform.position.y);
					break;
				case doorOpeningDirection.TOP:
					doorClosedPosition = new Vector2 (this.transform.position.x, this.transform.position.y - 11);
					break;
				case doorOpeningDirection.BOTTOM:
					doorClosedPosition = new Vector2 (this.transform.position.x, this.transform.position.y + 11);
					break;
				default:
					Debug.Log ("SNO DOOR OPENING DIRECTION UNKNOWN");
					break;
			}
			openingState = doorOpeningState.OPENED;
			openingPercentage = 1.0f;
		}
//		Physics2D.IgnoreLayerCollision (6 /* Shadow */, 8 /* Doors */, true);
	}

	public void openDoor ()
	{
		if (isOpen)
		{
			lastOpeningTime = Time.time;
			return;
		}

		isOpen = true;
		lastOpeningTime = Time.time;
		openingState = doorOpeningState.OPENING;
	}

	public void closeDoor ()
	{
		if (! isOpen)
			return;

		isOpen = false;
		openingState = doorOpeningState.CLOSING;
	}

	public void animDoor ()
	{
		if (openingState == doorOpeningState.OPENING)
			openingPercentage += 0.1f;
		else if (openingState == doorOpeningState.CLOSING)
			openingPercentage -= 0.1f;
		else
		{
			Debug.Log ("SNO: animDoor called with a OPENED or CLOSED door");
			return;
		}

		switch (openingDirection)
		{
			case doorOpeningDirection.LEFT:
				this.transform.position = new Vector2 (doorClosedPosition.x + (11 * openingPercentage), doorClosedPosition.y);
				break;
			case doorOpeningDirection.RIGHT:
				this.transform.position = new Vector2 (doorClosedPosition.x - (11 * openingPercentage), doorClosedPosition.y);
				break;
			case doorOpeningDirection.TOP:
				this.transform.position = new Vector2 (doorClosedPosition.x, doorClosedPosition.y + (11 * openingPercentage));
				break;
			case doorOpeningDirection.BOTTOM:
				this.transform.position = new Vector2 (doorClosedPosition.x, doorClosedPosition.y - (11 * openingPercentage));
				break;
			default:
				Debug.Log ("SNO DOOR OPENING DIRECTION UNKNOWN");
				break;
		}

		if (openingState == doorOpeningState.OPENING && openingPercentage >= 1.0f)
		{
			openingState = doorOpeningState.OPENED;
			openingPercentage = 1.0f;
		}

		if (openingState == doorOpeningState.CLOSING  && openingPercentage <= 0.0f)
		{
			openingState = doorOpeningState.CLOSED;
			openingPercentage = 0.0f;
		}
	}

	void Update ()
	{
		if (isOpen && ! isOpenAtStart && Time.time - lastOpeningTime >= openDuration)
		{
			closeDoor ();
		}

		float currentTime = Time.time;
		if (openingState == doorOpeningState.OPENING || openingState == doorOpeningState.CLOSING)
		{
			if (currentTime - lastFrameTime > openingFrameTime)
			{
				animDoor ();
				lastFrameTime = currentTime;
			}
		}
	}
}
