using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class TankFristAid : NetworkBehaviour
{
    [Networked] private TickTimer life { get; set; }

    private List<LagCompensatedHit> _areaHits = new List<LagCompensatedHit>();
    public LayerMask collisionLayer;
    public AudioSource audioplayer;


    private void Start()
    {
        audioplayer = GetComponent<AudioSource>();
        audioplayer.Play();
    }
    public void Init()
    {
        life = TickTimer.CreateFromSeconds(Runner, 20.0f);

    }

    public override void FixedUpdateNetwork()
    {
        if (life.Expired(Runner))
        {
            Runner.Despawn(Object);
        }
        else
        {
            checkCollosion();
        }
    }

    private void checkCollosion()
    {
        int col = Runner.LagCompensation.OverlapSphere(transform.position, 1.0f, Object.InputAuthority, _areaHits, collisionLayer, HitOptions.IncludePhysX);
        if (col > 0)
        {
            GameObject player = _areaHits[0].GameObject;
            if (player)
            {
                TankHealth target = player.GetComponent<TankHealth>();
                if (target != null)
                {
                    target.TankTakeHP(30);
                    print("HEAL!!");
                    Runner.Despawn(Object);
                }
            }
        }

    }



}
