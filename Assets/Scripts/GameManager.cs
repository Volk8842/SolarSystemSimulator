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
        ShowPath();
        foreach (SpaceObject spaceObject in spaceObjects)
        {
            Vector2 resultGravityForce = Vector2.zero;
            foreach (SpaceObject otherSpaceObject in spaceObjects)
            {
                if (spaceObject == otherSpaceObject)
                    continue;

                Vector2 gravityForce = GetGravityForce(spaceObject.Position, spaceObject.Mass, otherSpaceObject.Position, otherSpaceObject.Mass);
                resultGravityForce += gravityForce;
            }
            spaceObject.GetComponent<Rigidbody2D>().AddForce(resultGravityForce);
        }
   }

    private void ShowPath()
    {
        Vector2[] currentPosition = new Vector2[spaceObjects.Count];
        Vector2[] currentVelocity = new Vector2[spaceObjects.Count];
        for (int i = 0; i < spaceObjects.Count; ++i)
        {
            currentPosition[i] = spaceObjects[i].Position;
            currentVelocity[i] = spaceObjects[i].Velocity;
        }
        for (int i = 0; i < SpaceObject.PathDotCount * 25; ++i)
        {
            Vector2[] nextPosition = new Vector2[spaceObjects.Count];
            Vector2[] nextVelocity = new Vector2[spaceObjects.Count];

            for (int j = 0; j < spaceObjects.Count; ++j)
            {
                Vector2 resultVelocity = currentVelocity[j];
                Vector2 resultPosition = currentPosition[j];
                for (int k = 0; k < spaceObjects.Count; ++k)
                {
                    if (spaceObjects[j] == spaceObjects[k])
                        continue;

                    Vector2 gravityForce = GetGravityForce(currentPosition[j], spaceObjects[j].Mass, currentPosition[k], spaceObjects[k].Mass);
                    resultVelocity += gravityForce / spaceObjects[j].Mass * Time.fixedDeltaTime;
                }

                nextVelocity[j] = resultVelocity;
                resultPosition += nextVelocity[j] * Time.fixedDeltaTime;
                nextPosition[j] = resultPosition;
                if ((i + 1) % 25 == 0)
                {
                    spaceObjects[j].SetPathDot(((i + 1) / 25) - 1, resultPosition);
                }
            }
            currentPosition = nextPosition;
            currentVelocity = nextVelocity;
        }
    }

    private Vector2 GetGravityForce(Vector2 position, float mass, Vector2 affectingPosition, float affectingMass)
    {
        float distance = Vector2.Distance(position, affectingPosition);
        float gravityForceValue = G * mass * affectingMass / (distance * distance);
        return (affectingPosition - position) / distance * gravityForceValue;
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
