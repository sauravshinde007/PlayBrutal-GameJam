using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class WinnerManager : MonoBehaviourPunCallbacks
{
    public static WinnerManager Instance;

    [SerializeField] private GameObject EndCanvas;

    private void Awake()
    {
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

    private HashSet<Player> alivePlayers = new HashSet<Player>();

    private void Start()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            alivePlayers.Add(player);
        }

        EndCanvas.SetActive(false);
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
        if (alivePlayers.Count <= 1)
        {
            photonView.RPC("GameOver", RpcTarget.All);
        }
    }

    [PunRPC]
    public void GameOver()
    {
        // Display game over UI or any other game over logic
        EndCanvas.SetActive(true);

        Debug.Log("Game Over");

        // Restart the game after a delay
        Invoke("RestartGame", 5f);
    }

    private void RestartGame()
    {
        // Optionally, reset any necessary game state here
        EndCanvas.SetActive(false);

        // Reload the current level
        PhotonNetwork.LoadLevel(PhotonNetwork.CurrentRoom.Name);
    }
}
