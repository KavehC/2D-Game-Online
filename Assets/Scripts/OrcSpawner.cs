using Mirror;
using UnityEngine;

public class OrcSpawner : NetworkBehaviour
{
    public GameObject orcPrefab;

    public override void OnStartServer()
    {
        Debug.Log("Spawner running on server");

        Vector3 pos = transform.position;
        pos.z = 0f;

        GameObject npc = Instantiate(orcPrefab, pos, Quaternion.identity);
        NetworkServer.Spawn(npc);
    }
}
