using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.UI;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    [Header("Room Management")]
    [SerializeField] private TMP_InputField createInput;
    [SerializeField] private TMP_InputField joinInput;
    [SerializeField] private string mapSelectSceneName = "MapSelect";

    [Header("Username Popup")]
    [SerializeField] private GameObject usernamePopup;
    [SerializeField] private InputField usernameInput;
    [SerializeField] private TMP_Text validationMessage;

    private void Start()
    {
        usernamePopup.SetActive(true);
    }

    public void CreateRoom()
    {
        if (IsValidUsername(PhotonNetwork.NickName))
        {
            PhotonNetwork.CreateRoom(createInput.text);
        }
        else
        {
            ShowUsernamePopup();
        }
    }

    public void JoinRoom()
    {
        if (IsValidUsername(PhotonNetwork.NickName))
        {
            PhotonNetwork.JoinRoom(joinInput.text);
        }
        else
        {
            ShowUsernamePopup();
        }
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(mapSelectSceneName);
    }

    private void ShowUsernamePopup()
    {
        usernamePopup.SetActive(true);
    }

    public void OnSubmitUsername()
    {
        string username = usernameInput.text;

        if (IsValidUsername(username))
        {
            PhotonNetwork.NickName = username;
            validationMessage.text = "";
            usernamePopup.SetActive(false);
        }
        else
        {
            validationMessage.text = "Username must be at least 3 characters long.";
        }
    }

    private bool IsValidUsername(string username)
    {
        return !string.IsNullOrEmpty(username) && username.Length >= 3;
    }
}
