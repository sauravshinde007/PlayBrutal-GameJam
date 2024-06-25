using UnityEngine;
using Photon.Pun;

public class MapSelectManager : MonoBehaviourPunCallbacks
{
    private PhotonView photonView;

    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        photonView = GetComponent<PhotonView>();

        if (photonView == null)
        {
            Debug.LogError("PhotonView component is missing on this GameObject!");
        }
    }

    public void SelectMap(string mapName)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (photonView != null)
            {
                photonView.RPC("RPC_SelectMap", RpcTarget.All, mapName);
            }
            else
            {
                Debug.LogError("PhotonView is null when attempting to call RPC_SelectMap!");
            }
        }
    }

    [PunRPC]
    void RPC_SelectMap(string mapName)
    {
        PhotonNetwork.LoadLevel(mapName);
    }
}
