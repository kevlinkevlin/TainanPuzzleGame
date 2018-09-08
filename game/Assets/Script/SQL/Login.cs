using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;
public class Login : MonoBehaviour {

    public string inputUserName;
    public string inputPassword;
    string LoginURL = "https://guayguay.000webhostapp.com/Login.php";
    [SerializeField]
    Text user;
    [SerializeField]
    Text password;
    [SerializeField]
    Text message;
    [SerializeField]
    Flowchart flowchart;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
   
	}
   public void LoginButton(Text enterText)
    {
        if (user.text != "" && password.text != "")
        {
            inputUserName = user.text.Trim();
            inputPassword = password.text.Trim();
            StartCoroutine(LoginToDB(inputUserName, inputPassword));
        }
        else {
            print("不能為空值!!!");
            message.text = "不能空白~~";
        }
        
    }
   public IEnumerator LoginToDB(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("usernamePost", username);
        form.AddField("passwordPost", password);
        
        WWW www = new WWW(LoginURL, form);
        yield return www;
        Debug.Log(www.text);
        message.text = www.text;
        if (www.text.Trim() == "login success")
        {
            flowchart.SetStringVariable("name", inputUserName);
            flowchart.SetBooleanVariable("setName", true);
            flowchart.SendFungusMessage("setsuccess");  
        }
    }
}
