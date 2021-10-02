using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float speed = 15f;
	public float padding = 1f;
	public GameObject projectile;
	public float projectileSpeed;
	public float firingRate = 0.2f;
	public float health = 250f;
	public AudioClip laser;

	float xmin;
	float xmax;
	
	void Start(){
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftmost = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
		Vector3 rightmost = Camera.main.ViewportToWorldPoint(new Vector3(1,0,distance));
		xmin = leftmost.x;
		xmax = rightmost.x - padding;
	}
	void Fire(){
		Vector3 offset = new Vector3 (0, 1, 0);
		GameObject bullet = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
		bullet.rigidbody2D.velocity = new Vector3(0f, projectileSpeed, 0f); // if this dosent work remove the float
		AudioSource.PlayClipAtPoint(laser,transform.position);
	}
	void Update () {
	
		if(Input.GetKeyDown(KeyCode.Space)){
			InvokeRepeating("Fire", 0.000001f, firingRate);
			
		}
		if(Input.GetKeyUp(KeyCode.Space)){
			CancelInvoke("Fire");
		}
		
		if (Input.GetKey(KeyCode.LeftArrow)){
			//transform.position += new Vector3(-speed * Time.deltaTime, 0, 0);
			transform.position += Vector3.left * speed *Time.deltaTime;
		}else if (Input.GetKey(KeyCode.RightArrow)){
			//transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
			transform.position += Vector3.right * speed *Time.deltaTime;
		}
		//newX will be transform.position but will be clamped between xmin and xmax which we define
		float newX = Mathf.Clamp(transform.position.x, xmin, xmax);
		transform.position = new Vector3(newX, transform.position.y, transform.position.z);
	}	
	
	void OnTriggerEnter2D(Collider2D collider){
		Projectile missile = collider.gameObject.GetComponent<Projectile>();
		if(missile){
			Debug.Log("Player collided with missile");
			health -= missile.getDamage();
			missile.Hit();
			if (health <= 0) {
				Die ();
			//AudioSource.PlayClipAtPoint(playerDeath, transform.position);
				
			}
		}
	}
	void Die(){
		LevelManager man = GameObject.Find("LevelManager").GetComponent<LevelManager>();
		man.LoadLevel("Win Screen");
		Destroy (gameObject);
	}	
}