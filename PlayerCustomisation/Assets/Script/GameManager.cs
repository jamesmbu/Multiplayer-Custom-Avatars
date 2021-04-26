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
using System.IO;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{ 
    public static GameManager instance;

    GameManager_Delegates delegates;
    #region Fields
    
    public PerspectiveChanger perspectiveChanger;
    [SerializeField]
    private Canvas customiserCanvas;
    private bool matchInProgress = false;

    [Header("Game Settings")]
    
    
    [SerializeField] [Tooltip("Score required by a player to end the match")] private int MatchScore = 20;
    
    [Header("GameObject References")]
    [Tooltip("Prefab to spawn as the player")]
    public GameObject player;
    [Tooltip("Reference to the GameObject of the player which is previewed on the customisation menu")]
    public GameObject playerPreview;
    [SerializeField] private SpawnObjects Gameplay_ObjectSpawner;

   
    [Header("Text Fields")]
    public string[] PlayersNames;
    public Text[] PlayersNameText;
    public int[] PlayerScore;
    public Text[] PlayerScoreText;
    public Transform[] scoreOrder;
    [SerializeField] public Text scoreText;
    [SerializeField] public Text bigStatusText;

    [Header("Button References")]
    [SerializeField] private Button buttonPlay;
    [SerializeField] private Button buttonLeave;
    #endregion
    private GameObject newplayer;
    [SerializeField] private PlayerSpawnManager playerSpawnManager;
    
    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += GameSceneLoad;
        delegates.EventGameStart += OnEventStart;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= GameSceneLoad;
        delegates.EventGameStart -= OnEventStart;
    }

    void GameSceneLoad(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.buildIndex == 1)
        {
            customiserCanvas.gameObject.SetActive(true);
        } 
    }
    
    private void Awake()
    {
        instance = this;
        delegates = GetComponent<GameManager_Delegates>();
    }
 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Onleave()
    {
        PhotonNetwork.LoadLevel(0);
    }
    public void OnClickStart()
    {
        buttonLeave.gameObject.SetActive(true);
        scoreText.transform.parent.gameObject.SetActive(true);
        customiserCanvas.gameObject.SetActive(false);
        perspectiveChanger.SetCameraPerspective(PerspectiveChanger.CameraSetting.GameTop);
        delegates.CallEventOnStart();
    }
    void OnEventStart()
    {
        if (photonView.IsMine)
        {
            photonView.RPC("createPlayerMaster", RpcTarget.All);

            matchInProgress = true;
            Gameplay_ObjectSpawner.enabled = true;

        }
    }

    [PunRPC]
    void createPlayerMaster()
    {
        Debug.Log("Creating Player master...");
        PhotonNetwork.Instantiate(Path.GetFileName("PlayerMaster"), Vector3.zero, Quaternion.identity);
        PlayerMaster.instance.CallEventplayerspawn();
    }
    public void OnPlayerScore(int score)
    {
        // update the score display text
        scoreText.text = score.ToString();
        // check if the score is enough to end the match
        if (score >= MatchScore)
        {
          //  status.text = "You win!";
            //scoreText.GetComponent<TemporaryOnScreen>().ShowTempText("You Win!");
            matchInProgress = false;
            Gameplay_ObjectSpawner.enabled = false;
        }
    }
}
