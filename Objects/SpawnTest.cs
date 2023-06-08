using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTest : MonoBehaviour
{
    public LootBage lootBage;

    public GameObject _spawn;


    public void SpawnSome()
    {
        LootBage existingBag = _spawn.GetComponentInChildren<LootBage>();

        if (existingBag == null)
        {
            LootBage bag = Instantiate(lootBage, _spawn.transform.position, _spawn.transform.rotation);
            bag.transform.parent = _spawn.transform;
        }

    }
}
