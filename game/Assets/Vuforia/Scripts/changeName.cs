using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;

public class changeName : MonoBehaviour {
    public Flowchart flowchart;
    public MoveToView view;
    public void EnterPlayerName(Text enterText)
    {
        if (enterText.text != "")
        {
            flowchart.SetStringVariable("name", enterText.text);
            flowchart.SetBooleanVariable("setName",true);
            
        }
        return ;
    }
	
}
