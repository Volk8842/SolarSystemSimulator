using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class SpaceObject : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.rigidbody.mass >= GetComponent<Rigidbody2D>().mass)
        {
            GameObject explosion = (GameObject)GameObject.Instantiate(Resources.Load("Explosion"));
            explosion.transform.position = transform.position;
            GameManager.Instance.RemoveSpaceObject(this);
            Destroy(gameObject);
        }
    }        
}
