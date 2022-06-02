using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
	public float shadowDelay = 5.0f;
	public float ShadowTangibleDelay = 0.5f;
	public float groundCheckOffset = 0.2f;		/* will match the Player groundCheckOffset */

	private Rigidbody2D mybody;
	private CapsuleCollider2D myCollider;
	private SpriteRenderer mySprite;
	private Animator myAnim;
	private Vector2 groundCheckPosition;

	private Color mainColor = new Color (0.25f, 0.25f, 0.25f, 0.5f);
	private Color tangibleColor = new Color (0.25f, 0.25f, 0.25f, 1.0f);
	private Collider2D _mainPlayerCollider;
	private bool _isTangible = false;
	public bool isTangible => _isTangible;

	private List<Vector2> mouvements = new List<Vector2>();
	private List<Vector2> mainPlayerPositions = new List<Vector2>();
	private bool begin;
	private float beginTime;

	// Start is called before the first frame update
	void Awake()
	{
		mybody = this.GetComponent<Rigidbody2D>();
		myCollider = this.GetComponent<CapsuleCollider2D>();
		mySprite = this.GetComponent<SpriteRenderer>();
		mySprite.color = mainColor;
		myAnim = this.GetComponent<Animator>();
		beginTime = Time.time;
		begin = false;
	}

	private void applyMouvement (Vector2 mouvement, Vector2 mainPlayerPosition)
	{
		mybody.velocity = mouvement;
		if (mybody.velocity == Vector2.zero)
		{
			this.transform.position = new Vector3 (mainPlayerPosition.x, mainPlayerPosition.y, this.transform.position.z);
		}

		if (mouvement.x != 0)
		{
			mySprite.flipX = mybody.velocity.x > 0 ? false : true;
		}
		myAnim.SetFloat ("velocityX", Mathf.Abs (mybody.velocity.x));
		myAnim.SetFloat ("velocityY", mybody.velocity.y);
	}

	private void applyFirstMouvement ()
	{
		if (this.mouvements.Count == 0)
			return;		/* nothing to do */

		Vector2 firstMouvement = this.mouvements [0];
		Vector2 firstMainPlayerPosition = this.mainPlayerPositions [0];
		applyMouvement (firstMouvement, firstMainPlayerPosition);
		this.mouvements.RemoveAt (0);
		this.mainPlayerPositions.RemoveAt (0);
	}

	private void groundCheck()
	{
		Collider2D[] colliderCheck;
		bool animGrounded = false;

		groundCheckPosition = new Vector2 (this.transform.position.x, myCollider.bounds.min.y + groundCheckOffset);
		colliderCheck = Physics2D.OverlapCircleAll (groundCheckPosition, myCollider.size.x * 0.4f);
		foreach (Collider2D truc in colliderCheck)
		{
			if (!truc.isTrigger && truc != myCollider)
			{
				animGrounded = true;
			}
		}
		myAnim.SetBool ("grounded", animGrounded);
	}

	public void setMainPlayerCollider (Collider2D mainPlayerCollider)
	{
		this._mainPlayerCollider = mainPlayerCollider;
	}

	public void setTangible (bool tangible)
	{
		if (tangible)
		{
			mySprite.color = tangibleColor;
			Physics2D.IgnoreCollision (myCollider, this._mainPlayerCollider, false);
		}
		else
		{
			mySprite.color = mainColor;
			Physics2D.IgnoreCollision (myCollider, this._mainPlayerCollider, true);
		}

		this._isTangible = tangible;
	}

	public void addMouvement (Vector2 mouvement, Vector2 mainPlayerPosition)
	{
		this.mouvements.Add (mouvement);
		this.mainPlayerPositions.Add (mainPlayerPosition);
	}

	void Update()
	{
		groundCheck ();
	}

	void FixedUpdate()
	{
		if (! begin)
		{
			if (Time.time - beginTime >= shadowDelay)
				begin = true;
		}

		if (begin)
		{
			// if (this._isTangible)
			// 	Physics2D.IgnoreCollision (myCollider, this._mainPlayerCollider, false);
			// else
			// 	Physics2D.IgnoreCollision (myCollider, this._mainPlayerCollider, true);

			applyFirstMouvement ();
		}
	}

	void OnCollisionEnter2D (Collision2D collision)
	{
		if (collision.collider.tag == "Player" && isTangible)
		{
			Debug.Log ("COLLIDED WITH: " + collision.collider);
			collision.collider.SendMessage ("Dead");
			this.gameObject.SetActive (false);
		}
	}

	private void OnDrawGizmos()
	{
		myCollider = this.GetComponent<CapsuleCollider2D>();
		groundCheckPosition = new Vector2 (this.transform.position.x, myCollider.bounds.min.y + groundCheckOffset);
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere (groundCheckPosition, myCollider.size.x * 0.4f);
	}
}
