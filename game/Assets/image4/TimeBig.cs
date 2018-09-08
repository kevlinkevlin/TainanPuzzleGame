using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBig : MonoBehaviour {
	public GameObject m_object ; 
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}



	public void rotate(){
		m_object.transform.Rotate(0, 0, -20);
	}
}
