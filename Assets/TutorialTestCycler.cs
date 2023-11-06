using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialTestCycler : MonoBehaviour
{
    readonly string[] tutorialText={"Move left and right using W and D.",
                        "Press S for an Attack. It is quick but has short range.",
                        "Press W for a Lunge. It has longer reach but takes time to charge.",
                        "Press Space to Parry the opponent's Lunges. Time it well!",
                        "Press Esc to return from here to the Main Menu."};
    TextMeshProUGUI currentText;
    [SerializeField] float cycleDelay=4;
    int textIndex=0;

    void Start(){
        currentText=GetComponent<TextMeshProUGUI>();
        currentText.text=tutorialText[0];
        StartCoroutine(CycleText());
    }

    IEnumerator CycleText(){
        while (true){
            yield return new WaitForSeconds(cycleDelay);
            textIndex++;
            if (textIndex>tutorialText.Length-1){
                textIndex=0;
            }
            currentText.text=tutorialText[textIndex];
        }
    }

}
