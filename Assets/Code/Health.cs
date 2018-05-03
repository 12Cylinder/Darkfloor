using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DFCore;

public class Health : MonoBehaviour {

    public DefenseProfile baseDefense;
    public DefenseProfile item1;
    public DefenseProfile item2;

    public DefenseProfile finalDefense;

    public LootDrop[] Loot;
    private int maxHitpoints;
    public int currentHitpoints;

    private void Start()
    {
        setDefense();
        currentHitpoints = maxHitpoints;
    }

    private void Update()
    {
        if (currentHitpoints <= 0)
        {
            Die();
        }
    }

    public void setDefense()
    {
        float fMR = baseDefense.magic_resist + item1.magic_resist + item2.magic_resist;
        float fPR = baseDefense.physics_resist + item2.physics_resist + item2.physics_resist;
        float fHP = baseDefense.hitpoints + item1.hitpoints + item2.hitpoints;

        finalDefense = new DefenseProfile(fMR, fPR, fHP);
        maxHitpoints = Convert.ToInt32(finalDefense.hitpoints);
    }

    public void setItem(int slot, DefenseProfile prof)
    {
        if(slot < 2)
        {
            item1 = prof;
        }
        else
        {
            item2 = prof;
        }

        setDefense();
    }

    public void takeDamage(float d, DamageTypes t)
    {
        float totalDamage = 0;
        //if the damage is positive (and therefore an attack), calculate gain with resistances
        if(d > 0)
        {
            print("damage taken");
            switch (t)
            {
                case DamageTypes.magic:
                    totalDamage = d * (1 - finalDefense.magic_resist);
                    break;
                case DamageTypes.physical:
                    totalDamage = d * (1 - finalDefense.physics_resist);
                    break;
            }
        }
        //Otherwise, the damage is a heal, do not calculate with resistances
        currentHitpoints -= Convert.ToInt32(totalDamage);
        currentHitpoints = Mathf.Clamp(currentHitpoints, 0, maxHitpoints);
    }

    public void takeHeal (int a)
    {
        print("heals taken");
        currentHitpoints += a;
        currentHitpoints = Mathf.Clamp(currentHitpoints, 0, maxHitpoints);
    }

    private void Die()
    {
        int r = UnityEngine.Random.Range(0, 100);
        int min = 100;
        LootDrop newLoot = new LootDrop();
        foreach(LootDrop ld in Loot)
        {
            if(r <= ld.rate)
            {
                if(r < min)
                {
                    min = r;
                    newLoot = ld;
                }
            }
        }

        if (newLoot.loot != null)
        {
            int q = UnityEngine.Random.Range(0, newLoot.maxQuantity);
            for(int i = 0; i <= q; i++)
            {
                Instantiate(newLoot.loot, transform.position, newLoot.loot.transform.rotation);
            }
        }

        Destroy(gameObject);
    }
}
