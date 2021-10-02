using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {
	public GameObject projectile;
	public float projectileSpeed = 10;
	public float health = 150;
	public float shotsPerSecond = 0.5f;
	public int scoreValue = 150;
	public AudioClip EnemyLaser;
	public AudioClip deathSound;
	
	
	private ScoreKeeper scoreKeeper;
	
	
	void Start(){
		scoreKeeper = GameObject.Find("Score").GetComponent<ScoreKeeper>();
	}
	void Update(){
		float probability =  Time.deltaTime * shotsPerSecond;
		if(Random.value < probability){
		 fire ();
		}
	}
	void fire(){
		Vector3 startPosition = transform.position + new Vector3(0, -1, 0);
		GameObject missile = Instantiate(projectile, startPosition, Quaternion.identity)as GameObject;
		missile.rigidbody2D.velocity = new Vector2(0, -projectileSpeed);
		AudioSource.PlayClipAtPoint(EnemyLaser,transform.position);
	}


	void OnTriggerEnter2D(Collider2D collider){
		 Projectile missile = collider.gameObject.GetComponent<Projectile>();
		 if(missile){
	 		health -= missile.getDamage();
	 		missile.Hit();
	 		if (health <= 0) {
	 			Die ();
	 	}	
	  }
	}
	void Die(){
		AudioSource.PlayClipAtPoint(deathSound, transform.position);
		Destroy(gameObject);
		scoreKeeper.Score(scoreValue);
	}
}