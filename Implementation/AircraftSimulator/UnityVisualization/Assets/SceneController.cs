using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{

    private CustomNetworkManager customNetworkManager;

    private Toggle longitudinalRotation, longitudinalTranslation;
    private Toggle lateralRotation, lateralTranslation;

    // Use this for initialization
    void Start()
    {
        customNetworkManager =
            GameObject.FindGameObjectWithTag(Tags.NetworkManager).GetComponent<CustomNetworkManager>();
        SetupMenuScene();
    }

    public bool LongitudinalRotationActive
    {
        get
        {
            return longitudinalRotation.isOn;
        }
    }

    public bool LongitudinalTranslationActive
    {
        get
        {
            return longitudinalTranslation.isOn;
        }
    }

    public bool LateralRotationActive
    {
        get
        {
            return lateralRotation.isOn;
        }
    }

    public bool LateralTranslationActive
    {
        get
        {
            return lateralTranslation.isOn;
        }
    }


    private void dropdownValueChanged(int index)
    {
        var dropdown = GameObject.FindGameObjectWithTag(Tags.DropdownRemoteAddress);
        string content = dropdown.GetComponent<Dropdown>().options[index].text;
        dropdown.transform.FindChild("Label").GetComponent<Text>().text = content;
    }

    public void SetupMenuScene()
    {
        NetworkManager.singleton.StartMatchMaker();

        GameObject.FindGameObjectWithTag(Tags.ButtonStartLocal).GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.FindGameObjectWithTag(Tags.ButtonStartLocal).GetComponent<Button>().onClick.AddListener(customNetworkManager.StartupHost);

        GameObject.FindGameObjectWithTag(Tags.ButtonJoinLocal).GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.FindGameObjectWithTag(Tags.ButtonJoinLocal).GetComponent<Button>().onClick.AddListener(customNetworkManager.JoinGame);

        GameObject.FindGameObjectWithTag(Tags.ButtonStartRemote).GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.FindGameObjectWithTag(Tags.ButtonStartRemote).GetComponent<Button>().onClick.AddListener(customNetworkManager.StartRemoteHost);

        GameObject.FindGameObjectWithTag(Tags.ButtonJoinRemote).GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.FindGameObjectWithTag(Tags.ButtonJoinRemote).GetComponent<Button>().onClick.AddListener(customNetworkManager.JoinRemote);

        GameObject.FindGameObjectWithTag(Tags.DropdownRemoteAddress)
            .GetComponent<Dropdown>()
            .onValueChanged.RemoveAllListeners();
        GameObject.FindGameObjectWithTag(Tags.DropdownRemoteAddress).GetComponent<Dropdown>().onValueChanged.AddListener(new UnityAction<int>(dropdownValueChanged));


        //GameObject.FindGameObjectWithTag(Tags.LongitudinalRotation).GetComponent<Toggle>().onValueChanged.RemoveAllListeners();
        //GameObject.FindGameObjectWithTag(Tags.LongitudinalRotation).GetComponent<Toggle>().onValueChanged.AddListener(modeChanged);

        //GameObject.FindGameObjectWithTag(Tags.LongitudinalTranslation).GetComponent<Toggle>().onValueChanged.RemoveAllListeners();
        //GameObject.FindGameObjectWithTag(Tags.LongitudinalTranslation).GetComponent<Toggle>().onValueChanged.AddListener(modeChanged);

        //GameObject.FindGameObjectWithTag(Tags.LateralRotation).GetComponent<Toggle>().onValueChanged.RemoveAllListeners();
        //GameObject.FindGameObjectWithTag(Tags.LateralRotation).GetComponent<Toggle>().onValueChanged.AddListener(modeChanged);

        //GameObject.FindGameObjectWithTag(Tags.LateralTranslation).GetComponent<Toggle>().onValueChanged.RemoveAllListeners();
        //GameObject.FindGameObjectWithTag(Tags.LateralTranslation).GetComponent<Toggle>().onValueChanged.AddListener(modeChanged);
    }

    public void SetupOtherSceneButtons()
    {
        GameObject.FindGameObjectWithTag(Tags.ButtonDisconnect).GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.FindGameObjectWithTag(Tags.ButtonDisconnect)
            .GetComponent<Button>()
            .onClick.AddListener(customNetworkManager.DisconnectFromServer);


        longitudinalRotation = GameObject.FindGameObjectWithTag(Tags.LongitudinalRotation).GetComponent<Toggle>();
        longitudinalTranslation = GameObject.FindGameObjectWithTag(Tags.LongitudinalTranslation).GetComponent<Toggle>();
        lateralRotation = GameObject.FindGameObjectWithTag(Tags.LateralRotation).GetComponent<Toggle>();
        lateralTranslation = GameObject.FindGameObjectWithTag(Tags.LateralTranslation).GetComponent<Toggle>();
    }
}
