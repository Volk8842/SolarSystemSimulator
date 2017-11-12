using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpaceObjectCreation : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler 
{
    public SpaceObject prefab;
    public SpaceObject createdObject;

    private Camera camera;

    private void Start()
    {
        camera = Camera.main;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Cursor.visible = false;
        createdObject = GameObject.Instantiate(prefab);
        GameManager.Instance.AddSpaceObject(createdObject);
        GameManager.Instance.SetSimulationPaused(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        createdObject.transform.position = (Vector2)camera.ScreenToWorldPoint(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Cursor.visible = true;
        createdObject = null;
        GameManager.Instance.SetSimulationPaused(false);
    }

}
