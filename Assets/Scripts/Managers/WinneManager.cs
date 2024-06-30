using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Collections.Generic;

public class WinnerManager : MonoBehaviourPunCallbacks
{
    public static WinnerManager Instance;

    private List<Player> alivePlayers = new List<Player>();
    [SerializeField] private Text winnerText; // Text component to display the winner's name
    private PhotonView photonView;

    private void Awake()
    {
        Instance = this;
        photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            alivePlayers.Add(player);
        }

        // Ensure the winner text is hidden at the start of the game
        winnerText.gameObject.SetActive(false);
    }

    public void PlayerDied(Player player)
    {
        if (alivePlayers.Contains(player))
        {
            alivePlayers.Remove(player);
            CheckForWinner();
        }
    }

    private void CheckForWinner()
    {
        if (alivePlayers.Count == 1)
        {
            Player winner = alivePlayers[0];
            photonView.RPC("ShowWinner", RpcTarget.All, winner.NickName);
        }
    }

    [PunRPC]
    private void ShowWinner(string winnerName)
    {
        winnerText.text = $"Winner: {winnerName}";
        winnerText.gameObject.SetActive(true);
    }
}
