using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
	public GameObject enemyPrefab;
	public float width = 10f;
	public float height = 5f;
	public float speed = 5f;
	public float spawnDelay = 0.5f;
	
	private bool movingRight = true;
	private float xmax;
	private float xmin;
	
	
	
	// Use this for initialization
	void Start () { 
		
		float distanceToCamara = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftBoundary = Camera.main.ViewportToWorldPoint(new Vector3(0,0, distanceToCamara));
		Vector3 rightBoundary = Camera.main.ViewportToWorldPoint(new Vector3(1,0, distanceToCamara));
		xmax = rightBoundary.x;
		xmin = leftBoundary.x;
		SpawnUntilFull();
	}
	
	void SpawnEnemies(){
		foreach( Transform child in transform){
			GameObject enemy = Instantiate(enemyPrefab, child.transform.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = child;
			
		}

	}
	void SpawnUntilFull(){//spawns one enemy until all the places an enemy can be are filled
		Transform freePosition = NextFreePosition();
		if(freePosition){
			GameObject enemy = Instantiate(enemyPrefab, freePosition.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = freePosition;
		}
		if(NextFreePosition()){
		Invoke ("SpawnUntilFull", spawnDelay);
		}
	}
	
	public void OnDrawGizmos(){
		Gizmos.DrawWireCube(transform.position, new Vector3(width,height));	
	
	}
	
	// Update is called once per frame
	void Update () {
		if(movingRight){ //speed of our movement framerate independant makes sure that a player playing on faster computer dosent go against faster enemys
			transform.position += Vector3.right * speed * Time.deltaTime;
		}else{
			transform.position += Vector3.left * speed * Time.deltaTime;
		}
		
		float rightEdgeOfFormation = transform.position.x + (0.5f * width);
		float leftEdgeOfFormation = transform.position.x - (0.5f * width);
		//this can be seperated to be on two lines.
		if(leftEdgeOfFormation < xmin || rightEdgeOfFormation > xmax){
			movingRight = !movingRight;
		}
		
		if(AllMembersDead()){
			Debug.Log("Empty Formation");
			SpawnUntilFull();
		}
	}
		Transform NextFreePosition(){
			foreach(Transform childPositionGameObject in transform){
				if (childPositionGameObject.childCount == 0){
					return childPositionGameObject;
			}
		}
		return null;
	}
	
	
	bool AllMembersDead(){
		foreach(Transform childPositionGameObject in transform){
			if (childPositionGameObject.childCount > 0){
				return false;
			}
		}
		return true;
	}
}
	







