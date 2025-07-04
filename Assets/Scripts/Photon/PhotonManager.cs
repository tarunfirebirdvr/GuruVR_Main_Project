using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using ReadyPlayerMe.PhotonSupport;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static PhotonManager Instance;

    [Header("Settings")]
    public string gameVersion = "1.0";
    public string roomName = "DefaultRoom";
    public byte maxPlayers = 4;
    public bool autoJoinLobby = true;
    [SerializeField] GameObject mobilePlayerPrefab;
    [SerializeField] GameObject vrPlayerPrefab;
    [SerializeField] Camera DefaultCamera;
    [SerializeField] GameObject OnspawnEnable;
    [SerializeField] GameObject PlayerJoystickCanvas;
    public playerControlCanvasManager playerControlCanvas;
    public GameObject Instantiated_Player;
    public RobotFollower robotFollower;


    void Awake()
    {
        robotFollower.enabled = false;
        OnspawnEnable.SetActive(false);
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ConnectToPhoton();
    }

    public void ConnectToPhoton()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Connecting to Photon...");
    }

    /* bool IsVRDevice()
     {
 #if UNITY_ANDROID
         // Use a more robust check here if you’re running XR on Android
         return UnityEngine.XR.XRSettings.isDeviceActive;
 #else
     return false;
 #endif
     }*/
    /*bool IsVRDevice()
    {
#if UNITY_EDITOR
        // In Editor, check if XR device is active (e.g., Quest Link or other PC VR)
        return UnityEngine.XR.XRSettings.isDeviceActive;
#elif UNITY_ANDROID
    // On standalone Quest (Android), check XR status
    return UnityEngine.XR.XRSettings.isDeviceActive;
#else
    return false;
#endif
    }*/
    bool IsVRDevice()
    {
#if UNITY_EDITOR
        // Check if a real XR device is active
        if (UnityEngine.XR.XRSettings.isDeviceActive)
            return true;

        // Check if XR Device Simulator is present and enabled
        GameObject simulator = GameObject.Find("XR Device Simulator");

        if (simulator != null && simulator.activeSelf && simulator.gameObject.activeInHierarchy)
            return true;

        return false;

#elif UNITY_ANDROID
    // On Quest or other Android XR device
    return UnityEngine.XR.XRSettings.isDeviceActive;

#else
    return false;
#endif
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Master Server.");
        if (autoJoinLobby)
            PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby.");
        JoinOrCreateRoom();
    }

    public void JoinOrCreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = maxPlayers,
            IsVisible = true,
            IsOpen = true
        };

        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
        Debug.Log($"Trying to join or create room: {roomName}");
    }

    /* public override void OnJoinedRoom()
     {
         PhotonNetwork.Instantiate(PlayerPrefab.name, new Vector3(0, 0, 0), Quaternion.identity);
         DefaultCamera.enabled = false;
         Debug.Log($"Joined Room: {PhotonNetwork.CurrentRoom.Name}");
         if (PhotonNetwork.IsMasterClient)
         {

             Debug.Log("You are the Master Client.");
             // Load scene or setup logic if needed
             // PhotonNetwork.LoadLevel("YourGameScene");
         }
     }*/

    public override void OnJoinedRoom()
    {
        robotFollower.enabled = true;
        Vector3 spawnPosition =new Vector3(0.8f,0,0);
        Quaternion spawnRotation = Quaternion.Euler(0,180,0);
        DefaultCamera.enabled = false;
        GameObject prefabToSpawn;

        if (IsVRDevice())
        {
            prefabToSpawn = vrPlayerPrefab;
            Debug.Log("Detected VR device. Spawning VR player.");
        }
        else
        {
            StartCoroutine(waitToStartOnboarding(2));
            prefabToSpawn = mobilePlayerPrefab;
            Debug.Log("Spawning Mobile player.");
        }

        Instantiated_Player = PhotonNetwork.Instantiate(prefabToSpawn.name, spawnPosition, spawnRotation);
        NetworkPlayer aa;
        if (IsVRDevice())
        {
             GameObject firstChild = Instantiated_Player.transform.GetChild(3).gameObject;
               aa = firstChild.GetComponent<NetworkPlayer>();
        }
        else
        {
             aa = Instantiated_Player.GetComponent<NetworkPlayer>();
        }
       

       

        aa.LoadAvatar("https://models.readyplayer.me/" + PlayerPrefs.GetString("AvatarID","683d84becee0589b1b763fd3") + ".glb");
        playerControlCanvas = Instantiated_Player.GetComponent<playerControlCanvasManager>();
        //playerControlCanvas.CanvasEnable();
        if (DefaultCamera != null)
            DefaultCamera.gameObject.SetActive(false);

        Debug.Log($"Joined Room: {PhotonNetwork.CurrentRoom.Name}");

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("You are the Master Client.");
            // Optional scene logic
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogWarning($"Join Room Failed: {message}");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogWarning($"Create Room Failed: {message}");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogError($"Disconnected from Photon. Cause: {cause}");
    }

    // Optional: Manual reconnect
    public void Reconnect()
    {
        if (!PhotonNetwork.IsConnected)
        {
            ConnectToPhoton();
        }
    }

    IEnumerator waitToStartOnboarding(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        OnspawnEnable.SetActive(true);
    }
}
