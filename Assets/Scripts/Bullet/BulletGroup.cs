using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGroup : IEnumerable
{
    private List<Bullet> bullets = new List<Bullet>();

    public IEnumerator GetEnumerator()
    {
        return bullets.GetEnumerator();
    }

    public void ShootAll()
    {
        foreach (Bullet bullet in this)
        {
            bullet.Shoot();
        }
    }

    public void Add(Bullet bullet)
    {
        bullet.shootOnStart = false;
        bullets.Add(bullet);
    }
}
