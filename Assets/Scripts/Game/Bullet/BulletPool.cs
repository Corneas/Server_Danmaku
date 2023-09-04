using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoSingleton<BulletPool>
{
    private Bullet bullet;
    private Queue<Bullet> bulletQueue = new Queue<Bullet>();

    [SerializeField]
    private GameObject bulletPrefab = null;

    private void Start()
    {
        bullet = Instantiate(bulletPrefab, transform).GetComponent<Bullet>();
        bullet.gameObject.SetActive(false);

        CreatePool();
    }

    public void CreatePool(int amount = 1000)
    {
        for (int i = 0; i < amount; ++i)
        {
            Bullet bulletClone = Instantiate(bullet, transform);
            bulletClone.transform.SetParent(transform);
            bulletClone.gameObject.SetActive(false);
            bulletQueue.Enqueue(bulletClone);
        }
    }

    public Bullet Pop(Vector3 pos, Transform parent = null)
    {
        Bullet bulletClone = null;

        if (bulletQueue.Count <= 0)
        {
            bulletClone = Instantiate(bullet, pos, Quaternion.identity);
        }
        else
        {
            bulletClone = bulletQueue.Dequeue();
        }

        bulletClone.gameObject.SetActive(true);
        bulletClone.gameObject.transform.position = pos;
        bulletClone.gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
        bulletClone.transform.SetParent(parent);
        return bulletClone;
    }

    public void Push(Bullet bullet)
    {
        bulletQueue.Enqueue(bullet);
        bullet.gameObject.SetActive(false);
        bullet.transform.SetParent(transform);
        bullet.transform.position = Vector3.zero;
    }
}
