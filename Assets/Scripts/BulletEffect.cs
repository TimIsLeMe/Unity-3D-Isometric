using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEffect
{
    public Vector3 Scale { get; set; }
    public float SpeeModifier { get; set; }
    public int AdditionalBulletCount { get; set; }
    public float DamageModifier { get; set; }
    public int AdditionalCollisionMaxCount { get; set; }
    public BulletEffect(Vector3 scale = default(Vector3), float damageModifier = 1, float speedModifier = 1, int additionalCollisionMaxCount = 0, int additionalBulletCount = 0)
    {
        Scale = scale.Equals(default(Vector3)) ? new Vector3(1, 1, 1) : scale;
        SpeeModifier = speedModifier;
        AdditionalBulletCount = additionalBulletCount;
        DamageModifier = damageModifier;
        AdditionalCollisionMaxCount = additionalCollisionMaxCount;
    }

    public static BulletEffect MergeEffects(BulletEffect be1, BulletEffect be2)
    {
        Debug.Log("NEW EFFECTS: " + be1.ToString() + "; ; ; " + be2.ToString());
        BulletEffect be = new BulletEffect();
        be.Scale = be1.Scale + be2.Scale - new Vector3(1, 1, 1);
        be.DamageModifier = be1.DamageModifier + be2.DamageModifier - 1f;
        be.SpeeModifier = be1.SpeeModifier + be2.SpeeModifier - 1f;
        be.AdditionalBulletCount = be1.AdditionalCollisionMaxCount + be2.AdditionalCollisionMaxCount;
        be.AdditionalBulletCount = be1.AdditionalBulletCount + be2.AdditionalBulletCount;
        return be;
    }

    override
    public string ToString()
    {
        return "Scale: " + Scale + ", Damage: " + DamageModifier + ", Speed: " + SpeeModifier + ", BulletCount: " + AdditionalBulletCount + ", CollisionCount: " + AdditionalCollisionMaxCount;
    }
}
