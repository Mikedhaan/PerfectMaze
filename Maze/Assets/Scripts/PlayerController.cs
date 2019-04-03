using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        
        //Moves Forward and back along y axis
        transform.Translate(Vector3.up * Time.deltaTime * Input.GetAxis("Vertical") * speed);
      
        //Moves Left and right along x Axis
        transform.Translate(Vector3.right * Time.deltaTime * Input.GetAxis("Horizontal") * speed);
    }
}

