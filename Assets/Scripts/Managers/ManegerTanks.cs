using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class ManegerTanks : NetworkBehaviour
{
    [Networked] public TickTimer respawnDelay { get; set; }
    [Networked] public TickTimer hpBoxDelay { get; set; }

    [Networked] public TickTimer hpBoxDelays { get; set; }


public CameraControl cameraControl;

    public List<GameObject> allPlayer = new List<GameObject>();
    public TankFristAid thankfristAid;

    public KeepShll KeepShlls;


    //public Color[] playerColor;
    //public HPBox hpBox;
    public override void FixedUpdateNetwork()
    {
        SetCameraTargets();

        if (hpBoxDelay.ExpiredOrNotRunning(Runner))
        {
            hpBoxDelay = TickTimer.CreateFromSeconds(Runner, 25f);
            Runner.Spawn(thankfristAid, new Vector3(Random.Range(-29, 29), 1, Random.Range(-26, 26)), Quaternion.identity, Object.InputAuthority, (runner, o) => { o.GetComponent<TankFristAid>().Init(); });
        }

        if (hpBoxDelays.ExpiredOrNotRunning(Runner))
        {
            hpBoxDelays = TickTimer.CreateFromSeconds(Runner, 25f);
            Runner.Spawn(KeepShlls, new Vector3(Random.Range(-29, 29), 1, Random.Range(-26, 26)), Quaternion.identity, Object.InputAuthority, (runner, o) => { o.GetComponent<TankFristAid>().Init(); });
        }

    }

    public void AddPlayer(GameObject player)
    {
        MeshRenderer[] renderers = player.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = Random.ColorHSV();
        }
        allPlayer.Add(player);
        print(player.name + "have login to sever now!");
    }

   
    public void SetPlayer(GameObject player)
    {
        if (allPlayer.Contains(player))
        {
            player.SetActive(false);
            respawnDelay = TickTimer.CreateFromSeconds(Runner, 3f);
            StartCoroutine(OnRespawnPlayer(player));
        }
    }

    IEnumerator OnRespawnPlayer(GameObject player)
    {
        yield return new WaitUntil(() => respawnDelay.ExpiredOrNotRunning(Runner));
        player.SetActive(true);
        player.GetComponent<TankHealth>().Respawn();
    }

    private void SetCameraTargets()
    {

        Transform[] targets = new Transform[allPlayer.ToArray().Length];

        for (int i = 0; i < targets.Length; i++)
        {
            targets[i] = allPlayer[i].transform;
        }

        cameraControl.targets = targets;
    }
}
