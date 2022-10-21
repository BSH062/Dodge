using UnityEngine;
using System.Collections;

public class EnemyAttackScript : MonoBehaviour {
	
	public EnemyScript _EnemySt;

	// Use this for initialization
	void Start () {
		
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
    void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "player")
        {
            
			other.SendMessage("Damaged",1.0f);
        }
        
    }
}
