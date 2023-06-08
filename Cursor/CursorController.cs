using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CursorController : MonoBehaviour
{
    PlayerController _inputs;

    private Camera _camera;
    
    public Texture2D cursor;
    public Texture2D cursorMove;
    public Texture2D cursorAction;   
    
    private int _objects;    

    private void Awake()
    {
        _camera = Camera.main;
        _inputs = new PlayerController();
        ChangeCursor(cursor);
        Cursor.lockState = CursorLockMode.Confined;
        _objects = LayerMask.NameToLayer("Objects");       
    }
    private void OnEnable()
    {
        _inputs.Enable();
    }

    private void OnDisable()
    {
        _inputs.Disable();
    }  

    private void Update()
    {
        if (!IsPointerOverUIObject())
        {
            if (_inputs.Player.RMB.ReadValue<float>() > 0)
            {
                ChangeCursor(cursorMove);
            }
            else if (_inputs.Player.RMB.ReadValue<float>() == 0)
            {
                ChangeCursor(cursor);
            }
            OnObject();
        }
            
    }
   

    private void OnObject()
    {
        Ray ray = _camera.ScreenPointToRay(_inputs.Player.Position.ReadValue<Vector2>());
        int mask = (1 << _objects);
        if (Physics.Raycast(ray: ray, hitInfo: out RaycastHit hit, Mathf.Infinity, mask))
        {
            if (hit.collider)
            {
                Cursor.SetCursor(cursorAction, Vector2.zero, CursorMode.Auto);
                Debug.Log("Find object");
            }
            else
            {
                ChangeCursor(cursor);
            }
        }
    }

    private void ChangeCursor(Texture2D cursorType)
    {
        Cursor.SetCursor(cursorType, Vector2.zero, CursorMode.Auto);
    }
    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = Mouse.current.position.ReadValue();
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        return results.Count > 0;
    }
}
