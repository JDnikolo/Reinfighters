using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    Slider leftHealth,rightHealth;
    Image[] leftWins, rightWins;
    enum Side {left,right};
    

    void Awake(){
        leftWins=GameObject.FindGameObjectWithTag("UIWinsLeft").GetComponentsInChildren<Image>();
        rightWins=GameObject.FindGameObjectWithTag("UIWinsRight").GetComponentsInChildren<Image>();
        leftHealth = GameObject.FindGameObjectWithTag("UIHealthLeft").GetComponent<Slider>();
        rightHealth = GameObject.FindGameObjectWithTag("UIHealthRight").GetComponent<Slider>();
        
    }

    public void SetMaxHealth(int health, int side){
        if ((Side)side==Side.left) {
            leftHealth.maxValue = health;
            leftHealth.value = health;
            leftHealth.fillRect.GetComponent<Image>().color = Color.green;

            }
        else if ((Side)side==Side.right) {
            rightHealth.maxValue = health;
            rightHealth.value = health;
            rightHealth.fillRect.GetComponent<Image>().color = Color.green;
        }
    }

    public void SetHealth(int health, int side){
        if ((Side)side==Side.left){
            leftHealth.value = health;
            if (health<=leftHealth.maxValue/3+Mathf.Epsilon){
                leftHealth.fillRect.GetComponent<Image>().color = Color.red;
            }
            else if (health<=leftHealth.maxValue*2/3+Mathf.Epsilon){
                leftHealth.fillRect.GetComponent<Image>().color = Color.yellow;
            }
        }
        if ((Side)side==Side.right) {
            rightHealth.value = health;
            if (health<=rightHealth.maxValue/3+Mathf.Epsilon){
                rightHealth.fillRect.GetComponent<Image>().color = Color.red;
            }
            else if (health<=rightHealth.maxValue*2/3+Mathf.Epsilon){
                rightHealth.fillRect.GetComponent<Image>().color = Color.yellow;
            }
             
        }
        
    }

    internal void RegisterWinRight(int wins)
    {
        rightWins[wins-1].color = Color.green;
    }
    internal void RegisterWinLeft(int wins){
        leftWins[wins-1].color = Color.green;
    }
}
