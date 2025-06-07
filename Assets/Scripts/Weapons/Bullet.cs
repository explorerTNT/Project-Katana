using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletEffect { Lift, Pull, Stagger }

public class Bullet : MonoBehaviour
{
    public float speed = 30f;
    public BulletEffect effect;

    void Update() => transform.Translate(Vector3.forward * speed * Time.deltaTime);

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            var enemy = other.GetComponent<Enemy>();
            if (enemy != null) enemy.ApplyEffect(effect);
            Destroy(gameObject);
        }
    }
}
