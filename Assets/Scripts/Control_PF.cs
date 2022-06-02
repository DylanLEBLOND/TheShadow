using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control_PF : MonoBehaviour
{

	// IMPORTANT
	// Votre COLLIDER doit avoir un PHYSICS MATERIAL 2D avec 0 de FRICTION pour que ce script marche parfaitement (sinon votre personnage se bloquera contre les murs)
	// Pour créer un Physics Material 2D, clic droit dans l'onglet project, "create/ Physics Material 2D"
	// Puis dans la case FRICTION du physics material vous mettez 0 et enfin vous le glissez dans la case Material de votre COLLIDER

	// Maintenant on déclare nos variables
	public float speed = 5f;	//Vitesse du joueur
	public float jump = 8f;	 //Hauteur du saut
	public float groundCheckOffset; // Cette valeur nous servira à décaler la zone de détection du sol si besoin
	private bool grounded;	  // Ce booléen deviendra vrai quand on touche le sol et faux quand on sera en l'air, pratique pour savoir si on sauter
	private Vector2 groundCheckPosition;	// Un vector2 (valeur x et y), nous servira à calculer la position de la zone de détection du sol (en prenant en compte "groundCheckOffset")
	private Collider2D[] colliderCheck; //La on déclare un tableau (les signes [] signifient que c'est un tableau). En gros ça veut dire qu'il y aura plusieurs collider2D stocker dans cette variable.

	// Et on déclare les composants qu'on va utiliser (c'est les "COMPONENT" présent sur le personnage, visible dans l'inspector quand on sélectionne le personage)
	private Rigidbody2D rb;	// Le rigidbody du personnage, servira à le faire avancer et sauter
	private CapsuleCollider2D monCollider;	// Le collider du personnage, servira pour placer le détecter de sol
	private SpriteRenderer monSprite; // Le sprite (visuel) du personnage, servira à retourner le personnage quand on va à gauche ou à droite
	private Animator anim; // Le gestionnaire d'animation du personnage, servira pour enclencher les bonne animation en fonction de ce que notre personnage fait.


	// Première fonction qui aura lieu qu'une seule fois, quand on lancera le jeu
	void Start() {
		// On commence par récupérer les composants (ceux qu'on a déclaré au dessus)
		rb = GetComponent<Rigidbody2D>();
		monCollider = GetComponent<CapsuleCollider2D>();
		monSprite = GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();

		// Et on vérifie 2 choses
		// 1 - Que la rotation de notre personnage est bien vérouillé (pour pas qu'il tombe lamentablement quand on le déplace)
		// 2 - Que les raycast qu'on va effectuer ici vont ignorer le collider de notre personnage ainsi que les triggers
		rb.freezeRotation = true;
		Physics2D.queriesStartInColliders = false;
		Physics2D.queriesHitTriggers = false;
	}

	void Update() {
		moveCheck();	// On lance la fonction movecheck
		groundCheck();  // On lance la fonction groundCheck
		jumpCheck();	// On lance la fonction jumpCheck
		flipCheck();	// On lance la fonction flipCheck
		animationCheck(); // On lance la fonction animationCheck
	}

	// La fonction moveCheck, qui sert à déplacer le personnage à gauche et à droite
	void moveCheck() {
		rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y);
	}

	// La fonction jumpCheck, qui sert à faire sauter le personnage quand
	void jumpCheck() {
		if (Input.GetButtonDown("Jump") && grounded) {	  // Si le joueur appuie sur le bouton de saut et qu'on en au sol (grounded). Alors on envoie le saut.
			rb.velocity = new Vector2(rb.velocity.x, jump);
		}
	}

	// La fonction groundCheck sert à vérifier si on touche le sol.
	private void groundCheck() {
		groundCheckPosition = new Vector2(transform.position.x, monCollider.bounds.min.y + groundCheckOffset); // Ici on positionne automatiquement la zone de détection en bas de notre collider
		colliderCheck = Physics2D.OverlapCircleAll(groundCheckPosition, monCollider.size.x * 0.4f); // Maintenant on fait cette vérification, et on stock tous les collider que notre détection va trouver dans un tableau
		grounded = false; // d'abord on passe le booléen en faux
		foreach (Collider2D truc in colliderCheck) { // enfin on fait une boucle pour chaque collider que notre détecteur va trouver
			if (!truc.isTrigger && truc != monCollider) { //Dans cette boucle, on élimine le collider du personnage et les trigger
				grounded = true; // si on trouve un collider (autre que celui du joueur ou les trigger) On passe alors grounded en vrai
			}
		}
	}

	// La fonction qui va vérifier si le joueur appuie à gauche ou à droite, et on retourne le sprite en fonction.
	void flipCheck() {
		if (Input.GetAxisRaw("Horizontal") < 0) {
			monSprite.flipX = true;
		}
		if (Input.GetAxisRaw("Horizontal") > 0) {
			monSprite.flipX = false;
		}
	}

	// Cette fonction sert à envoyer les info nécessaire à l'Animator (le gestionnaire d'animation) pour qu'il puisse changer d'animation quand nécessaire
	void animationCheck() {
		if (anim == null) {
			return;
		}
		anim.SetFloat("velocityX", Mathf.Abs(rb.velocity.x));
		anim.SetFloat("velocityY", rb.velocity.y);
		anim.SetBool("grounded", grounded);
	}


	// cette fonction est optionnel, elle sert à faire apparaitre le détecteur de sol dans la scene (pour pouvoir le réglé correctement)
	private void OnDrawGizmos() {
		if (monCollider == null) {
			monCollider = GetComponent<CapsuleCollider2D>();
		}
		groundCheckPosition = new Vector2(transform.position.x, monCollider.bounds.min.y + groundCheckOffset);
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(groundCheckPosition, monCollider.size.x * 0.4f);
	}
}
