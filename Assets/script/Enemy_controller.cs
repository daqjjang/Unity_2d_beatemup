using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_controller : MonoBehaviour {
    public float lookRadius = 4.0f;
    public float speed = 10.0f;
    public Transform target;
  
	// Use this for initialization
	void Start () {
//        target = player_manager.instance.transform;
 	}
	
	// Update is called once per frame
	void Update () {

        float distance = Vector3.Distance(target.position, transform.position);

     //   Debug.Log("distance"+ distance);
        if(distance<=lookRadius)
        {

            transform.position = Vector3.MoveTowards(transform.position,target.position, speed*Time.deltaTime);
        }
	}
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
//        Quaternion lookRotation= Quaternion.LookRotation(new Vector3(di))
    }
}
