using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayFabManager : MonoBehaviour
{
    public TMP_InputField inputField;
    UiManager uiManager;
    AudioManager audioManager;

    public GameObject rowPrefab;
    public Transform rowsParent;

    public GameObject leaderBoard;
    public GameObject titleBoard;
    public GameObject tutorialBoard;

    float transitionTime = 0.1f;

    // Start is called before the first frame update

    private void Awake()
    {
        login();
        uiManager = FindAnyObjectByType<UiManager>();
        audioManager = FindAnyObjectByType<AudioManager>();
       
    }
    void Start()
    {
        if(titleBoard != null)
        {
            StartCoroutine(ShowTitle(0.8f));
        }
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            Debug.Log("Working");
            audioManager.Play("SecondBackground");
        }



    }

    void login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
            
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }

    void OnSuccess(LoginResult result)
    {
        Debug.Log("Successful Login/accouht create!");
        string name = null;
        if(result.InfoResultPayload.PlayerProfile != null)
        {
            name = result.InfoResultPayload.PlayerProfile.DisplayName;
        }      
    }

    void OnError(PlayFabError error)
    {
        Debug.Log("Error while logging in/createin");
        Debug.Log(error.GenerateErrorReport());
    }

    public void SendLeaderBoard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                   
                    StatisticName =  "SlimeScore",                      
                    Value = score
                   
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderBoardUpdate, OnError);
    }
    void OnLeaderBoardUpdate(UpdatePlayerStatisticsResult result)
    {
        audioManager.Play("Confirm");
        uiManager.UpdateDebug("Succesfully Saved");
        Debug.Log("Successful Leaderboard sent");
    }

    public void GetLeaderBoard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "SlimeScore",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderBoardGet, OnError);
    }
    void OnLeaderBoardGet(GetLeaderboardResult result)
    {
        if(rowsParent != null) {

            foreach (Transform item in rowsParent)
            {
                Destroy(item.gameObject);
            }

        }
       
        foreach (var item in result.Leaderboard)
        {
            if(rowsParent != null && rowsParent!= null)
            {
                GameObject newGo = Instantiate(rowPrefab, rowsParent);
                TextMeshProUGUI[] texts = newGo.GetComponentsInChildren<TextMeshProUGUI >();
                Debug.Log(texts.Length);
                texts[0].text = (item.Position +1 ).ToString();
                texts[1].text = item.DisplayName.ToString();
                texts[2].text = item.StatValue.ToString();
                Debug.Log(item.Position + " " + item.DisplayName + " " + item.StatValue);
            }
            
        }
    }

   public void SubmitNameButton()
    {
        string name = inputField.text;
        audioManager.Play("Click");
        if(name != "")
        {

            var request = new UpdateUserTitleDisplayNameRequest
            {
                DisplayName = inputField.text
            };
            PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);
            SendLeaderBoard(uiManager.kills);
            GetLeaderBoard();
            
        }  
        else
        {
            uiManager.UpdateDebug("Please enter your Name!");
            Debug.Log("Write a name");
        }

    }

    private void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("Update display Name");
        
    }
    public void ShowLeaderBoardMethod()
    {
        StartCoroutine(ShowLeaderBoard());
    }
    public void ShowTitleMethod()
    {
        StartCoroutine(ShowTitle(transitionTime));    
    }
    public void PlayGameMethod()
    {
        StartCoroutine (PlayGame());    
    }
    public void EndGameMethod()
    {
        StartCoroutine(EndGame());  
    }
    public void ShowTutorialMethod()
    {
        StartCoroutine(ShowTutorialBoard());
    }
     IEnumerator ShowLeaderBoard()
    {
        leaderBoard.SetActive(true);
        audioManager.Play("Click");
        LeanTween.scale(titleBoard, new Vector3(0, 0, 0), transitionTime);
        yield return new WaitForSeconds(transitionTime);
        LeanTween.scale(leaderBoard, new Vector3(1, 1, 1), transitionTime);       
        titleBoard.SetActive(false);
        GetLeaderBoard();
    }
     IEnumerator ShowTitle(float time)
    {
        audioManager.Play("Click");
        LeanTween.scale(leaderBoard, new Vector3(0, 0, 0), time);
        LeanTween.scale(tutorialBoard, new Vector3(0, 0, 0), time);
        yield return new WaitForSeconds(transitionTime);
        LeanTween.scale(titleBoard, new Vector3(1, 1, 1), time);
        tutorialBoard.SetActive(false);
        titleBoard.SetActive(true);
        leaderBoard.SetActive(false);
    }
     IEnumerator PlayGame()
    {
        audioManager.Play("Click");
        LeanTween.scale(titleBoard, new Vector3(0, 0, 0), transitionTime);
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene("SampleScene");
    }
     IEnumerator EndGame()
    {
        audioManager.Play("Click");
        LeanTween.scale(titleBoard, new Vector3(0, 0, 0), transitionTime);
        yield return new WaitForSeconds(transitionTime);
        Application.Quit(); 
    }

    IEnumerator ShowTutorialBoard()
    {
        tutorialBoard.SetActive(true);
        audioManager.Play("Click");
        LeanTween.scale(titleBoard, new Vector3(0, 0, 0), transitionTime);     
        yield return new WaitForSeconds(transitionTime);
        LeanTween.scale(tutorialBoard, new Vector3(1, 1, 1), transitionTime);
    }

}
