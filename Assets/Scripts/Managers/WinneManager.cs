using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class WinnerManager : MonoBehaviourPunCallbacks
{
    public static WinnerManager Instance;

    private List<Player> alivePlayers = new List<Player>();
    [SerializeField] private Text winnerText; // Text component to display the winner's name
    [SerializeField] private Text countdownText; // Text component to display the countdown
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

        // Ensure the winner text and countdown text are hidden at the start of the game
        winnerText.gameObject.SetActive(false);
        countdownText.gameObject.SetActive(false);
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
        winnerText.text = $"{winnerName} is swamp master";
        winnerText.gameObject.SetActive(true);
        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        countdownText.gameObject.SetActive(true);
        int countdown = 5; // countdown from 5 seconds
        while (countdown > 0)
        {
            countdownText.text = $"Restarting in {countdown}...";
            yield return new WaitForSeconds(1);
            countdown--;
        }
        countdownText.text = "Restarting now...";
        yield return new WaitForSeconds(1);
        RestartGame();
    }

    private void RestartGame()
    {
        // Assuming you want to reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
