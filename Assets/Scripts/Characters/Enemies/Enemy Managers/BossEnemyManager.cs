using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemyManager : Enemy
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Kill()
    {
        // Stub, add death sequence/game win
        Destroy(gameObject);
    }
}
