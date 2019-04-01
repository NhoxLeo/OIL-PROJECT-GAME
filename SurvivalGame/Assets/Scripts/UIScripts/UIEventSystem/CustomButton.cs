using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomButton : MonoBehaviour {

    // Use this for initialization   
    int height, width;
    Vector2 pos;
    //GameObject button;
    Button button;

    public CustomButton(int _height, int _width, Vector2 _pos)
    {
        height = _height;
        width = _width;
        pos = _pos;
        
        
    }
	void Start () {
       // button = Instantiate(Button) as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
