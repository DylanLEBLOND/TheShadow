using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public float speed = 10.0f;
	public float jumpingHeight = 8.0f;
	public bool	superJump = false;
	public float groundCheckOffset = 0.2f;

	public GameObject Camera;
	public Shadow Shadow;
	public bool isAlive;

	private Rigidbody2D mybody;
	private CapsuleCollider2D myCollider;
	private SpriteRenderer mySprite;
	private Animator myAnim;

	private bool _grounded = true;
	public bool grounded => _grounded;
	private float _horizontalMovement = 0.0f;
	public float horizontalMovement => _horizontalMovement;
	private bool _jump = false;
	public bool jump => _jump;
	private bool _superJumpUsed = false;
	public bool superJumpUsed  => _superJumpUsed;
	private Vector2 groundCheckPosition;
	private float lastTangibleTriggering = 0.0f;

	// Start is called before the first frame update
	void Start()
	{
		mybody = this.GetComponent<Rigidbody2D>();
		myCollider = this.GetComponent<CapsuleCollider2D>();
		mySprite = this.GetComponent<SpriteRenderer>();
		myAnim = this.GetComponent<Animator>();

		Shadow.groundCheckOffset = groundCheckOffset;
		Shadow.setMainPlayerCollider (this.GetComponent<Collider2D>());
		Shadow.setTangible (false);

		isAlive = true;
	}

	private void jumping ()
	{
		if (!_jump)		/* nothing to do */
			return;

		Vector2 newVelocity = new Vector2 (mybody.velocity.x, jumpingHeight);

		Shadow.addMouvement (newVelocity, this.transform.position);

		mybody.velocity = newVelocity;
		if (!_grounded && ! _superJumpUsed)
			_superJumpUsed = true;
		else
			_grounded = false;
		_jump = false;

		myAnim.SetFloat ("velocityX", Mathf.Abs (mybody.velocity.x));
		myAnim.SetFloat ("velocityY", mybody.velocity.y);
	}

	private void moveCharacter ()
	{
		Vector2 newVelocity = new Vector2 (_horizontalMovement * speed, mybody.velocity.y);

		Shadow.addMouvement (newVelocity, this.transform.position);

		mybody.velocity = newVelocity;

		if (_horizontalMovement != 0)
		{
			mySprite.flipX = _horizontalMovement > 0 ? false : true;
		}

		myAnim.SetFloat ("velocityX", Mathf.Abs (mybody.velocity.x));
		myAnim.SetFloat ("velocityY", mybody.velocity.y);
	}

	private void groundCheck()
	{
		Collider2D[] colliderCheck;

		groundCheckPosition = new Vector2 (this.transform.position.x, myCollider.bounds.min.y + groundCheckOffset);
		colliderCheck = Physics2D.OverlapCircleAll (groundCheckPosition, myCollider.size.x * 0.4f);
		_grounded = false;
		foreach (Collider2D truc in colliderCheck)
		{
			if (!truc.isTrigger && truc != myCollider)
			{
				_grounded = true;
				if (superJump)
					_superJumpUsed = false;
			}
		}

		myAnim.SetBool ("grounded", _grounded);
	}

	public void superJumpActivate ()
	{
		superJump = true;
	}
	public void Dead ()
	{
		isAlive = false;
		mySprite.color = new Color (mySprite.color.r, mySprite.color.g, mySprite.color.b, 0.0f);
		mybody.constraints = RigidbodyConstraints2D.FreezePosition;
	}

	void Update()
	{
		if (! isAlive)		/* nothing to do */
			return;

		groundCheck ();

		_horizontalMovement = Input.GetAxisRaw("Horizontal");
		if (Input.GetButtonDown ("Jump"))
		{
			if (_grounded)
				_jump = true;

			if (superJump && ! _superJumpUsed)
				_jump = true;
		}
		if (Input.GetButtonDown ("Tangible"))
		{
			if (Time.time - lastTangibleTriggering >= Shadow.ShadowTangibleDelay)
			{
				lastTangibleTriggering = Time.time;
				Shadow.setTangible (! Shadow.isTangible);
			}
		}
	}

	void FixedUpdate()
	{
		if (! isAlive)		/* nothing to do */
			return;

		jumping ();
		moveCharacter ();
	}

	private void OnDrawGizmos()
	{
		myCollider = this.GetComponent<CapsuleCollider2D>();
		groundCheckPosition = new Vector2 (this.transform.position.x, myCollider.bounds.min.y + groundCheckOffset);
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere (groundCheckPosition, myCollider.size.x * 0.4f);
	}
}
