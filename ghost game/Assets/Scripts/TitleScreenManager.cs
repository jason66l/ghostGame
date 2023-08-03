using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using Mirror;

public class TitleScreenManager : MonoBehaviour
{
    public static TitleScreenManager instance;
    public Custom customNetworkManager;


    [Header("UI Panels")]
    [SerializeField] private GameObject HostOrJoinPanel;
    [SerializeField] private GameObject EnterIPAddressPanel;

    [Header("Enter IP UI")]
    [SerializeField] private TMP_InputField IpAddressField;

    [Header("Misc. UI")]
    [SerializeField] private Button returnToMainMenu;

    public GameObject playerPrefab; // Assign your player prefab here in the Inspector

    private string ipAddressToConnect = "";

    void Awake()
    {
        ReturnToMainMenu();
        MakeInstance();
    }

    void Start()
    {
        // Find the Custom NetworkManager in the scene
        customNetworkManager = FindObjectOfType<Custom>();

        // Make sure customNetworkManager is not null before using it
        if (customNetworkManager == null)
        {
            Debug.LogError("Custom NetworkManager not found in the scene.");
            return;
        }

        HostOrJoinPanel.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        HostOrJoinPanel.SetActive(true);
        EnterIPAddressPanel.SetActive(false);
        returnToMainMenu.gameObject.SetActive(false);
        ClearUIElements();
    }

    public void HostGame()
    {
        Debug.Log("Hosting a game...");

        // Ensure there's only one NetworkManager in the scene
        NetworkManager[] networkManagers = FindObjectsOfType<NetworkManager>();
        if (networkManagers.Length > 1)
        {
            Debug.LogWarning("Multiple NetworkManagers detected in the scene. Removing duplicates...");
            for (int i = 1; i < networkManagers.Length; i++)
            {
                Destroy(networkManagers[i].gameObject);
            }
        }

        // Start the server if it's not already running
        if (!NetworkServer.active)
        {
            customNetworkManager.StartHost();
            Debug.Log("Running host from custom");
        }



        //SceneManager.LoadScene("main");

        ClearUIElements();
    }

    public void JoinGame()
    {
        HostOrJoinPanel.SetActive(false);
        EnterIPAddressPanel.SetActive(true);
        returnToMainMenu.gameObject.SetActive(true);
        //ClearUIElements();
    }

    public void ConnectToGame()
    {
        if (!string.IsNullOrEmpty(IpAddressField.text))
        {
            ipAddressToConnect = IpAddressField.text;
            Debug.Log("Client will connect to: " + ipAddressToConnect);
            
            // Start the client with the entered IP address
            customNetworkManager.networkAddress = ipAddressToConnect;
            customNetworkManager.StartClient();

            // Note: Do not load the scene here as the server should handle it.
        }
    }

    void MakeInstance()
    {
        if (instance == null)
            instance = this;
    }

    private void OnEnable()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    private void OnSceneUnloaded(Scene scene)
    {
        ClearUIElements();
    }

    private void ClearUIElements()
    {
        if (IpAddressField != null)
        {
            IpAddressField.text = "";
        }
        HostOrJoinPanel.SetActive(false);
        EnterIPAddressPanel.SetActive(false);
        returnToMainMenu.gameObject.SetActive(false);
    }
}
