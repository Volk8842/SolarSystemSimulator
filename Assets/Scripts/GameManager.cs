using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {

    public GameObject SpaceObjectsHolder;
    public float G; 

    private List<SpaceObject> spaceObjects = new List<SpaceObject>();

    private SpaceObject pressedObject;

    private void Start()
    {
        spaceObjects.AddRange(SpaceObjectsHolder.GetComponentsInChildren<SpaceObject>());
    }

    private void FixedUpdate()
    {
        foreach (SpaceObject spaceObject in spaceObjects)
        {
            Vector2 resultGravityForce = Vector2.zero;
            foreach (SpaceObject otherSpaceObject in spaceObjects)
            {
                if (spaceObject == otherSpaceObject)
                    continue;

                float distance = Vector2.Distance(spaceObject.transform.position, otherSpaceObject.transform.position);
                float gravityForceValue = G * spaceObject.GetComponent<Rigidbody2D>().mass * otherSpaceObject.GetComponent<Rigidbody2D>().mass / (distance * distance);
                Vector2 gravityForce = (otherSpaceObject.transform.position - spaceObject.transform.position) / distance * gravityForceValue;
                resultGravityForce += gravityForce;
            }
            spaceObject.GetComponent<Rigidbody2D>().AddForce(resultGravityForce);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (pressedObject != null)
            {
                Vector2 forceTarget = (Vector2)pressedObject.transform.position;
                Vector2 forceOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                float mass = pressedObject.GetComponent<Rigidbody2D>().mass;
                pressedObject.GetComponent<Rigidbody2D>().AddForce((forceTarget - forceOrigin) * mass * 40);
                pressedObject = null;
                SetSimulationPaused(false);
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (pressedObject == null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D raycastHit = Physics2D.Raycast(ray.origin, ray.direction);
                if (raycastHit.collider != null)
                {
                    SpaceObject spaceObject = raycastHit.collider.GetComponent<SpaceObject>();
                    if (spaceObject != null)
                    {
                        pressedObject = spaceObject;
                        SetSimulationPaused(true);
                    }
                }
                
            }
        }
    }

    public void AddSpaceObject(SpaceObject spaceObject)
    {
        spaceObject.transform.SetParent(SpaceObjectsHolder.transform);
        spaceObjects.Add(spaceObject);
    }

    public void RemoveSpaceObject(SpaceObject spaceObject)
    {
        spaceObjects.Remove(spaceObject);
    }

    public void SetSimulationPaused(bool value)
    {
        foreach (SpaceObject spaceObject in spaceObjects)
        {
            spaceObject.GetComponent<Rigidbody2D>().simulated = !value;
        }
    }
}
