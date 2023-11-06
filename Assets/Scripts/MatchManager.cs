using System.Collections;
using Cinemachine;
using UnityEngine;
using TMPro;

public class MatchManager : MonoBehaviour
{
    [SerializeField] int roundsToWin = 2;
    int playerLeftWins=0, playerRightWins=0;
    [SerializeField] GameObject playerLeftPrefab;
    [SerializeField] GameObject playerRightPrefab;
    [SerializeField] CinemachineVirtualCamera roundStartCamera,fightingCamera;
    [SerializeField] float roundStartDelay = 2f;
    [SerializeField] float destroyDelay = 2f;
    [SerializeField] float nextRoundDelay = 3.5f;
    [SerializeField] TextMeshProUGUI endGameText;
    [SerializeField] GameObject endGameButtons;

    GameObject playerLeft,playerRight;
    CombatantController controllerLeft,controllerRight;

    void Awake(){
        playerLeft = Instantiate(playerLeftPrefab);
        controllerLeft = playerLeft.GetComponent<CombatantController>();
        playerRight = Instantiate(playerRightPrefab);
        controllerRight = playerRight.GetComponent<CombatantController>();
        playerRight.GetComponent<AutoController>().SetOpponent(controllerLeft);
        FindObjectOfType<CinemachineTargetGroup>().AddMember(playerLeft.transform,1,0);
        FindObjectOfType<CinemachineTargetGroup>().AddMember(playerRight.transform,1,0);
    }

    void Start(){
        roundStartCamera.Priority=11;
        fightingCamera.Priority=10;      
        StartCoroutine(RoundStart());
    }

    IEnumerator RoundStart(float extraDelay=0f){
        controllerLeft.StartRoundStartSwing();
        controllerRight.StartRoundStartSwing();
        yield return new WaitForSeconds(roundStartDelay+extraDelay);
        roundStartCamera.Priority=10;
        fightingCamera.Priority=11;
    }

    public void RoundEnd(GameObject destroyedPlayer){
        Debug.Log("Round End!");
        GameObject winner = null;
        controllerLeft.SetStun(true);
        controllerRight.SetStun(true);
        if (playerLeft==destroyedPlayer){
            winner=playerRight;
            playerRightWins++;
            FindObjectOfType<UIHandler>().RegisterWinRight(playerRightWins);
            controllerLeft.StartDeathSwing();
            StartCoroutine(VictorySwingAfterDelay(controllerRight));
            
        }
        else if (playerRight==destroyedPlayer){
            winner=playerLeft;
            playerLeftWins++;
            controllerRight.StartDeathSwing();
            FindObjectOfType<UIHandler>().RegisterWinLeft(playerLeftWins);
            StartCoroutine(VictorySwingAfterDelay(controllerLeft));
        }
        Destroy(destroyedPlayer,destroyDelay);
        if (playerLeftWins==roundsToWin || playerRightWins==roundsToWin){
            EndMatch(winner);
        }
        else {
            StartCoroutine(NextRound(winner));
        }
    }

    private void EndMatch(GameObject winner)
    {
        if (playerLeftWins>playerRightWins){
            endGameText.text="You Win!";
        }
        else{
            endGameText.text="You Lose...";
        }
        endGameText.alpha=0;
        StartCoroutine(ShowEndGameText());
        endGameText.gameObject.SetActive(true);
        StartCoroutine(ShowEndGameButtons());
        StartCoroutine(EndlessVictorySwing(winner.GetComponent<CombatantController>()));
    }

    IEnumerator ShowEndGameText(){
        yield return new WaitForSeconds(destroyDelay/2);
        while (endGameText.alpha<0.99){
            endGameText.alpha +=0.1f;
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator ShowEndGameButtons()
    {
        yield return new WaitForSeconds(destroyDelay);
        endGameButtons.SetActive(true);
    }

    IEnumerator NextRound(GameObject toDestroy){
        yield return new WaitForSeconds(nextRoundDelay);
        Destroy(toDestroy);
        playerLeft = Instantiate(playerLeftPrefab);
        controllerLeft = playerLeft.GetComponent<CombatantController>();
        playerRight = Instantiate(playerRightPrefab);
        controllerRight = playerRight.GetComponent<CombatantController>();
        playerRight.GetComponent<AutoController>().SetOpponent(controllerLeft);
        FindObjectOfType<CinemachineTargetGroup>().AddMember(playerLeft.transform,1,0);
        FindObjectOfType<CinemachineTargetGroup>().AddMember(playerRight.transform,1,0);
        roundStartCamera.Priority=11;
        fightingCamera.Priority=10;      
        StartCoroutine(RoundStart(extraDelay:0.5f));
    }

    IEnumerator VictorySwingAfterDelay(CombatantController controller){
        yield return new WaitForSeconds(destroyDelay*1/3);
        controller.StartVictorySwing();
    }
    IEnumerator EndlessVictorySwing(CombatantController controller){
        while(true){
            yield return new WaitForSeconds(destroyDelay+Random.Range(0,2));
            controller.StartVictorySwing();
        }
        
    }
}
