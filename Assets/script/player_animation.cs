using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_animation : MonoBehaviour {
    private Animator _anim;
    // Use this for initialization
	void Start () {
        _anim = GetComponentInChildren<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Move(float move)
    {
        _anim.SetFloat("Move",Mathf.Abs (move));
    }
}
