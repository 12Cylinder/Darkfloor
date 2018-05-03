using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DFCore
{
    [System.Serializable]
    public struct DefenseProfile
    {
        public float magic_resist;
        public float physics_resist;
        public float hitpoints;

        public DefenseProfile(float m, float p, float h)
        {
            magic_resist = m;
            physics_resist = p;
            hitpoints = h;
        }
    }

    [System.Serializable]
    public enum DamageTypes
    {
        magic,
        physical,
        heal
    }

    [System.Serializable]
    public class LootDrop
    {
        public GameObject loot;
        public int rate;
        public int maxQuantity;
    }

    [System.Serializable]
    public enum Classes
    {
        Wizard,
        Ranger,
        Paladin,
        Cleric
    }

    [System.Serializable]
    public enum attackTypes
    {
        lineOfSight,
        AreaOfEffect
    }

    [System.Serializable]
    public class attack : ScriptableObject
    {
        public attackTypes attackType;
        public Texture2D icon;
    }

    [System.Serializable]
    public class AOEattack : attack
    {
        public GameObject AOEObject;
    }

    [System.Serializable]
    public class LOSAttack : attack
    {
        public float range;
        public float damage;
        public DamageTypes damageType;
    }

    [System.Serializable]
    public class GoldWallet
    {
        public int gold = 0;

        public void modifyGold(int a)
        {
            gold += a;
        }
    }
}
