using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEditor;
using UnityEngine.UI;


public class NetworkManager : MonoBehaviourPunCallbacks
{
    #region Fields
    [SerializeField]
    private PerspectiveChanger perspectiveChanger;
    [SerializeField] 
    private Canvas customiserCanvas;
    [SerializeField]
    private InputField playerNameInput;
    private bool matchInProgress = false;

    [Header("Game Settings")]
    [SerializeField] private byte maxPlayersPerRoom = 4;
    [SerializeField] [Tooltip("Number of players required for game to start")] private int StartMatchRequirement = 2;
    [SerializeField] [Tooltip("Score required by a player to end the match")] private int MatchScore = 20;

    [Header("GameObject References")]

    [Tooltip("Prefab to spawn as the player")]
    public GameObject player;
    [Tooltip("Reference to the GameObject of the player which is previewed on the customisation menu")]
    public GameObject playerPreview;
    [SerializeField] private SpawnObjects Gameplay_ObjectSpawner;

    [Header("Text Fields")] 
    [SerializeField] private Text nickname;
    [SerializeField] private Text status;
    [SerializeField] private Text room;
    [SerializeField] private Text players;
    [SerializeField] public Text scoreText;
    [SerializeField] public Text bigStatusText;

    [Header("Button References")] 
    [SerializeField] private Button buttonPlay;
    [SerializeField] private Button buttonLeave;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        status.text = "Connecting...";
        buttonPlay.gameObject.SetActive(false);
        buttonLeave.gameObject.SetActive(false);
        playerNameInput.gameObject.SetActive(false);

        if (!PhotonNetwork.IsConnected)
            PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.InRoom)
        {
            nickname.text = "Hello, " + PhotonNetwork.NickName;
            room.text = "Room: " + PhotonNetwork.CurrentRoom.Name;
            players.text = "Players: " + PhotonNetwork.CurrentRoom.PlayerCount + " of " + PhotonNetwork.CurrentRoom.MaxPlayers;
            players.text += ":\n";
            Dictionary<int, Player> mydict = PhotonNetwork.CurrentRoom.Players;
            int i = 1;
            foreach (var item in mydict)
                players.text += string.Format("{0,2}. {1}\n", (i++), item.Value.NickName);
        }
        else if (PhotonNetwork.IsConnected)
        {
            nickname.text = "Customise your character and press SAVE!";
            room.text = "Not yet in a room...";
            players.text = "Players: 0";
        }
        else
            nickname.text = room.text = players.text = "";
    }
    public void Play()
    {
        PlayerPrefs.SetString("PlayerName", playerNameInput.text);
        PhotonNetwork.NickName = playerNameInput.text;
        PhotonNetwork.JoinRandomRoom();
    }
    public void Leave()
    {
        PhotonNetwork.LeaveRoom();
        // Make GUI adjustments
        customiserCanvas.gameObject.SetActive(true);
        playerNameInput.gameObject.SetActive(true);
        buttonLeave.gameObject.SetActive(false);
        scoreText.transform.parent.gameObject.SetActive(false);

        // Reset score
        scoreText.text = 0.ToString();

        // Change the view of the player
        perspectiveChanger.SetCameraPerspective(PerspectiveChanger.CameraSetting.Menu);
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster was called by PUN.");
        status.text = "Connected to Photon.";
        buttonPlay.gameObject.SetActive(true);
        playerNameInput.gameObject.SetActive(true);
        buttonLeave.gameObject.SetActive(false);
        playerNameInput.text = PlayerPrefs.GetString("PlayerName");
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Oops, tried to join a room and failed. Calling CreateRoom!");

        // failed to join a random room, so create a new one
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Yep, you managed to join a room!");
        status.text = "Yep, you managed to join a room!";
        //buttonPlay.gameObject.SetActive(false);
        customiserCanvas.gameObject.SetActive(false);
        playerNameInput.gameObject.SetActive(false);
        buttonLeave.gameObject.SetActive(true);
        scoreText.transform.parent.gameObject.SetActive(true);
        perspectiveChanger.SetCameraPerspective(PerspectiveChanger.CameraSetting.GameTop);
        

        // Instantiate new player
        GameObject newPlayer = PhotonNetwork.Instantiate(player.name,
            new Vector3(Random.Range(-15, 15), 1, Random.Range(15, 35)),
            Quaternion.Euler(0, 180/*Random.Range(-180, 180)*/, 0)
            , 0);
        // Customise the newly spawned player - send the saved settings from the preview to the new player
        playerPreview = GameObject.FindGameObjectWithTag("PlayerPreview");
        
        CharacterCustomisation newPlayerCustomiser = newPlayer.GetComponent<CharacterCustomisation>();

        newPlayerCustomiser.ApplySavedAppearance(playerPreview.GetComponent<CharacterCustomisation>().Save_Model);
        // Hide the preview version of the player
        
        

    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        status.text = newPlayer.NickName + " has just entered.";
        if (PhotonNetwork.CurrentRoom.PlayerCount >= StartMatchRequirement && PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Starting game...");
            matchInProgress = true;
            Gameplay_ObjectSpawner.enabled = true;
            
        }
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        status.text = otherPlayer.NickName + " has just left.";
        // Win by "last one standing"
        if (matchInProgress && PhotonNetwork.CurrentRoom.PlayerCount < 2)
        {
            status.text = otherPlayer.NickName + " has just left. You win by default!";
            matchInProgress = false;
            Gameplay_ObjectSpawner.enabled = false;
            //Gameplay_ObjectSpawner.
        }
    }

    public void OnPlayerScore(int score)
    {
        // update the score display text
        scoreText.text = score.ToString();
        // check if the score is enough to end the match
        if (score >= MatchScore)
        {
            status.text = "You win!";
            //scoreText.GetComponent<TemporaryOnScreen>().ShowTempText("You Win!");
            matchInProgress = false;
            Gameplay_ObjectSpawner.enabled = false;
        }
    }

}
