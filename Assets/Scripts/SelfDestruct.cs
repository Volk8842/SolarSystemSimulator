using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour {

    public float DestructionTimer;

	void Update () {
        DestructionTimer -= Time.deltaTime;
        if (DestructionTimer <= 0.0f)
            Destroy(gameObject);
    }
}
