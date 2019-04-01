using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnValueResourceChangeText : MonoBehaviour
{

    public Text text;
    Animator anim;
    GameObject obj;
    Color currentColor;
    Color transColor = Color.black;
    float timer;
    int reset = 0;
    float clipLength;
    bool isTransitioning;
    private void Awake()
    {
        text = GetComponent<Text>();
        anim = GetComponent<Animator>();
        obj = GameObject.Find("OnOilValueChange");
        transColor.a = 0;
        currentColor = text.color;
    }

    private void Start()
    {
        RuntimeAnimatorController ac = anim.runtimeAnimatorController;    //Get Animator controller
        for (int i = 0; i < ac.animationClips.Length; i++)                 //For all animations
        {
            if (ac.animationClips[i].name == "FloatingTextMoveUp")        //If it has the same name as your clip
            {
                clipLength = ac.animationClips[i].length;
            }
        }
    }

    void Update()
    {
        if (!isTransitioning)
        {
            text.color = Color.clear;
            return;
        }
           

        timer += Time.deltaTime * (1 /clipLength);    
        text.color = Color.Lerp(currentColor, transColor, timer);

        if (timer > clipLength)
        {
            isTransitioning = false;
            anim.SetBool("isDone", true);
        }           
    }

    public void FireText(int valueChange)
    {
        text.text = valueChange.ToString();
        isTransitioning = true;
        timer = reset;           
        anim.SetTrigger("FireText");
    }
}
