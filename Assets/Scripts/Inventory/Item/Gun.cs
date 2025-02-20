using DG.Tweening.Plugins;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float interval;

    public GameObject bulletPrefab;

    public Transform muzzlePos;

    private Vector2 mousePos;

    private Vector2 direction;

    private float timer;
    private float flipY;

    void Start()
    {
        muzzlePos = transform.Find("Muzzle");
        flipY = transform.localScale.y;
    }

    private void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousePos.x < transform.position.x)
            transform.localScale = new Vector3(flipY, -flipY, 1);
        else
            transform.localScale = new Vector3(flipY, flipY, 1);
        Shoot();
    }

    void Shoot()
    {
        direction = (mousePos - new Vector2(transform.position.x, transform.position.y)).normalized;
        transform.right = direction;

        if(timer!=0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
                timer = 0;
        }

        if(Input.GetKey(KeyCode.Mouse0))
        {
            if(timer==0)
            {
                Fire();
                timer = interval;
            }
        }
    }

    void Fire()
    {
        GameObject bullet = ObjectPool.Instance.GetObject(bulletPrefab);
        bullet.transform.position = muzzlePos.position;

        float angel = Random.Range(-5f, 5f);
        bullet.GetComponent<Bullet>().SetSpeed(Quaternion.AngleAxis(angel, Vector3.forward) * direction);
    }
}
