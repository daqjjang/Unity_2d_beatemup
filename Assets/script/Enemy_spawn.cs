using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_spawn : MonoBehaviour {
    public GameObject Enemy_obj;
    public float xpos;
    public float ypos;
    public int enemy_count;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    IEnumerator Enemy_drop()
    {
        while(enemy_count<5)
        {
            //            xpos = Random.Range(1,50);
            //            ypos = Random.Range(1, 31);
            xpos = this.transform.position.x;
            ypos = this.transform.position.y;
            Instantiate(Enemy_obj, new Vector3(xpos, ypos, 0.0f), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
            enemy_count += 1;
        }
    }
}
