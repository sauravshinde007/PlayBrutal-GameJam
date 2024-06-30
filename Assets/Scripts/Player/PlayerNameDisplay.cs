using UnityEngine;
using TMPro;
using Photon.Pun;

public class PlayerNameDisplay : MonoBehaviourPun
{
    [SerializeField] private TMP_Text usernameText;

    private void Start()
    {
        if (photonView.IsMine)
        {
            usernameText.text = PhotonNetwork.NickName;
        }
        else
        {
            usernameText.text = photonView.Owner.NickName;
        }
    }

    private void Update()
    {
        // Ensure the name tag follows the player
        Vector3 namePos = Camera.main.WorldToScreenPoint(transform.position);
        usernameText.transform.position = namePos;
    }
}
