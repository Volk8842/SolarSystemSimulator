using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class SpaceObject : MonoBehaviour
{
    public static int PathDotCount = 20;
    private GameObject[] path = new GameObject[PathDotCount];

    private void Awake()
    {
        for (int i = 0; i < path.Length; ++i)
        {
            path[i] = (GameObject)GameObject.Instantiate(Resources.Load("PathDot"));
            path[i].transform.parent = transform;
            path[i].transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
        }
    }

    public void SetPathDot(int number, Vector2 position)
    {
        path[number].transform.localPosition = position - (Vector2)transform.position; 
    }

    public float Mass
    {
        get { return GetComponent<Rigidbody2D>().mass; }
    }

    public Vector2 Position
    {
        get { return transform.position; }
    }

    public Vector2 Velocity
    {
        get { return GetComponent<Rigidbody2D>().velocity; }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.rigidbody.mass >= GetComponent<Rigidbody2D>().mass)
    //    {
    //        GameObject explosion = (GameObject)GameObject.Instantiate(Resources.Load("Explosion"));
    //        explosion.transform.position = transform.position;
    //        GameManager.Instance.RemoveSpaceObject(this);
    //        Destroy(gameObject);
    //    }
    //}        
}
