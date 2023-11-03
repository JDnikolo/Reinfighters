using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
    [SerializeField] int roundsToWin = 2;
    int playerOneWins=0, playerTwoWins=0;
    [SerializeField] GameObject playerOnePrefab;
    [SerializeField] GameObject playerTwoPrefab;
    [SerializeField] CinemachineVirtualCamera roundStartCamera,fightingCamera;
    [SerializeField] float roundStartDelay = 2f;
    [SerializeField] float destroyDelay = 2f;
    [SerializeField] float nextRoundDelay = 3.5f;

    GameObject playerOne,playerTwo;
    CombatantController controllerOne,controllerTwo;

    void Awake(){
        playerOne = Instantiate(playerOnePrefab);
        controllerOne = playerOne.GetComponent<CombatantController>();
        playerTwo = Instantiate(playerTwoPrefab);
        controllerTwo = playerTwo.GetComponent<CombatantController>();
        FindObjectOfType<CinemachineTargetGroup>().AddMember(playerOne.transform,1,0);
        FindObjectOfType<CinemachineTargetGroup>().AddMember(playerTwo.transform,1,0);
    }

    void Start(){
        roundStartCamera.Priority=11;
        fightingCamera.Priority=10;      
        StartCoroutine(RoundStart());
    }

    IEnumerator RoundStart(float extraDelay=0f){
        controllerOne.RoundStartSwing();
        controllerTwo.RoundStartSwing();
        yield return new WaitForSeconds(roundStartDelay+extraDelay);
        roundStartCamera.Priority=10;
        fightingCamera.Priority=11;
    }

    public void RoundEnd(GameObject destroyedPlayer){
        Debug.Log("Round End!");
        GameObject winner = null;
        controllerOne.SetStun(true);
        controllerTwo.SetStun(true);
        if (playerOne==destroyedPlayer){
            winner=playerTwo;
            playerTwoWins++;
            controllerOne.DeathSwing();
            StartCoroutine(VictorySwingAfterDelay(controllerTwo));
            
        }
        else if (playerTwo==destroyedPlayer){
            winner=playerOne;
            playerOneWins++;
            controllerTwo.DeathSwing();
            StartCoroutine(VictorySwingAfterDelay(controllerOne));
        }
        Destroy(destroyedPlayer,destroyDelay);
        if (playerOneWins==roundsToWin || playerTwoWins==roundsToWin){
            //TODO:Handle Game End
        }
        else {
            StartCoroutine(NextRound(winner));
        }
    }

    IEnumerator NextRound(GameObject toDestroy){
        yield return new WaitForSeconds(nextRoundDelay);
        Destroy(toDestroy);
        playerOne = Instantiate(playerOnePrefab);
        controllerOne = playerOne.GetComponent<CombatantController>();
        playerTwo = Instantiate(playerTwoPrefab);
        controllerTwo = playerTwo.GetComponent<CombatantController>();
        FindObjectOfType<CinemachineTargetGroup>().AddMember(playerOne.transform,1,0);
        FindObjectOfType<CinemachineTargetGroup>().AddMember(playerTwo.transform,1,0);
        roundStartCamera.Priority=11;
        fightingCamera.Priority=10;      
        StartCoroutine(RoundStart(extraDelay:0.5f));
    }

    IEnumerator VictorySwingAfterDelay(CombatantController controller){
        yield return new WaitForSeconds(destroyDelay*1/3);
        controller.VictorySwing();
    }
}
