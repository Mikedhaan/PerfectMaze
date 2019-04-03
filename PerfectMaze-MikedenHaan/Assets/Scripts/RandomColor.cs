using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomColor : MonoBehaviour {
    public static Color newColor;
	// Use this for initialization
	void Start () {
        // apply it on current object's material
        GetComponent<Renderer>().material.color = newColor;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
