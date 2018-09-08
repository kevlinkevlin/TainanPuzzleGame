using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {
    float time_f = 0;
    int time_i = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        time_f += Time.deltaTime;
        time_i = (int)time_f;
        Debug.Log(time_f);
	}
}
