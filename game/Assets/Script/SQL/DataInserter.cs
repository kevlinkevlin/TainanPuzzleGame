using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;

public class DataInserter : MonoBehaviour {



    public string inputUserName;
    public string inputPassword;
    public string inputEmail;
    string CreateUserURL = "https://guayguay.000webhostapp.com/Insertuser.php";
    [SerializeField]
    Text user;
    [SerializeField]
    Text password;
    [SerializeField]
    Text email;
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
    public void RegisterButton()
    {
        if (user.text != "" && password.text != "" && email.text != "")
        {
            inputUserName = user.text.Trim();
            inputPassword = password.text.Trim();
            inputEmail = email.text.Trim();
           StartCoroutine(CreateUser(inputUserName, inputPassword, inputEmail));
        }else {
            print("不能為空值!!!");
            message.text = "不能空白~~";
        }
      
    }
    public IEnumerator CreateUser(string username, string password, string email)
    {
        WWWForm form = new WWWForm();
        form.AddField("usernamePost",username);
        form.AddField("passwordPost",password);
        form.AddField("emailPost",email);

        WWW www = new WWW(CreateUserURL, form);
        yield return www;
        Debug.Log(www.text);
        message.text = www.text;
        print("User Name:"+username+"  password:"+password+"  email:"+email);
    }
}
