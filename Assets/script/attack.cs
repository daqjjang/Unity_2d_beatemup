using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack : MonoBehaviour {
    public bool enemy_hit=false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log(other.name);
        enemy_hit = true;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        enemy_hit = false;
    }
}
