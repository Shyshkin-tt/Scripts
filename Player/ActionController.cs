using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class ActionController : MonoBehaviour
{
    [SerializeField] private PlayerController _input;
    [SerializeField] private CharacterController _char;
    [SerializeField] private InventoryHolder _inv;
    [SerializeField] private Animator _anim;
    [SerializeField] private MakeDamage _hitBox;

    public Camera _camera;
    public Vector3 _targetPoint;
    private Coroutine _coroutine;
    private Vector3 _hitside;
    private int _objects;
    public bool _hit;
    public int _ground;
    public bool _move;
    private float _rotation = 500f;
    private float _movespeed;
    private UIController _uiController;
    public Transform InteractionPoint;
    public LayerMask InteractionLayer;
    public float InteractionPointRadius = 1f;

    public bool IsInteracting { get; private set; }

    private void Awake()
    {
        ResetTargetPoint();
        
        _input = new PlayerController();
        _ground = LayerMask.NameToLayer("Ground");
        _char = GetComponent<CharacterController>();
        _anim = GetComponent<Animator>();
        _inv = GetComponent<InventoryHolder>();
        _objects = LayerMask.NameToLayer("Objects");
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    private void Update()
    {
        _movespeed = _inv.Inventory.MoveSpeed;
        _hit = _anim.GetBool("Hit");

        if (_input.Player.Stop.ReadValue<float>() != 0)
        {
            StopMove();
        }

        var RCB = _input.Player.RMB.triggered;
        var LCB = _input.Player.LMB.triggered;
        var RHB = _input.Player.RMB.ReadValue<float>() != 0;
        var LHB = _input.Player.LMB.ReadValue<float>() != 0;

        if (!IsPointerOverUIObject() && !_hit)
        {
            if (RCB || RHB) TakePoint();

            if (LCB || LHB)
            {
                if (!RCB || !RHB)
                {
                    if (!_move)
                    {
                        Rotation();
                    }
                }
            }
        }

        if (!_hit)
        {
            MakeHit();
        }

        if (_hitBox == null)
        {
            _hitBox = GetComponentInChildren<MakeDamage>();
        }

        InteractWith();
    }

    private void TakePoint()
    {
        Ray ray = _camera.ScreenPointToRay(_input.Player.Position.ReadValue<Vector2>());
        RaycastHit hit;
        int mask = 1 << _ground;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }

            _coroutine = StartCoroutine(Move(hit.point));
            _targetPoint = hit.point;
        }

        if (IsInteracting)
        {
            EndInteraction();
        }
    }

    private IEnumerator Move(Vector3 target)
    {
        while (Vector3.Distance(transform.position, _targetPoint) > 0.3f)
        {
            _move = true;
            Vector3 direction = target - transform.position;
            direction.y = 0f;
            _char.Move(direction.normalized * _movespeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction.normalized), _rotation * Time.deltaTime);
            yield return null;
            _move = false;
        }
    }

    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = Mouse.current.position.ReadValue();
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    private void ResetTargetPoint()
    {
        _targetPoint = Vector3.zero;
    }

    public void StopMove()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _move = false;
        }
    }

    public void Rotation()
    {
        Ray ray = _camera.ScreenPointToRay(_input.Player.Position.ReadValue<Vector2>());
        RaycastHit hit;
        int mask = 1 << _ground;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
        {
            _targetPoint = hit.point;
            Vector3 direction = hit.point - transform.position;
            direction.y = 0f;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction.normalized), 500f * Time.deltaTime);
        }
    }

    private void MakeHit()
    {
        if (_input.Player.Attack.triggered)
        {
            Rotation();
            StopMove();
            _anim.SetTrigger("Attack");
            _hitBox._canHit = true;
        }
    }

    private void InteractWith()
    {
        var collider = Physics.OverlapSphere(InteractionPoint.position, InteractionPointRadius, InteractionLayer);
        Ray ray = _camera.ScreenPointToRay(_input.Player.Position.ReadValue<Vector2>());
        RaycastHit hit;
        int mask = 1 << _objects;
        if (Physics.Raycast(ray: ray, hitInfo: out hit, Mathf.Infinity, mask) && _input.Player.LMB.ReadValue<float>() > 0 && !IsPointerOverUIObject())
        {
            if (collider != null)
            {
                for (int i = 0; i < collider.Length; i++)
                {
                    var interactable = collider[i].GetComponent<IInteractable>();
                    if (interactable != null)
                    {
                        StartInteraction(interactable);
                    }
                }
            }
        }
    }

    void StartInteraction(IInteractable interactable)
    {
        interactable.Interact(this, out bool interactSuccessful);
        IsInteracting = true;
        _uiController = FindObjectOfType<UIController>();
    }

    public void EndInteraction()
    {
        IsInteracting = false;
        _uiController.CloseActiveWindows();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_hitside, 0.2f);
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(_targetPoint, 0.2f);
    }
}
