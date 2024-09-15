using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boon
{
    private static Boon[] boons = new Boon[]
    {
        new Boon("Size Increase", new BulletEffect(scale: new Vector3(1.5f, 1.5f, 1.5f))),
        new Boon("Damage Increase", new BulletEffect(damageModifier: 1.25f)),
        new Boon("Penetration", new BulletEffect(additionalCollisionMaxCount: 1)),
        new Boon("Speed Increase", new BulletEffect(speedModifier: 1.25f)),
        new Boon("Projectile Count Increase", new BulletEffect(additionalBulletCount: 1)),
    };

    private string _name = "Default Effect";
    public string Name { get { return _name; } }
    private BulletEffect _bulletEffect;
    public BulletEffect BulletEffect { get { return _bulletEffect; } }
    public Boon(string name, BulletEffect bulletEffect)
    {
        _name = name;
        _bulletEffect = bulletEffect;
    }

    public static Boon GetRandomBoon()
    {
        return boons[Random.Range(0, boons.Length)];
    }
}
