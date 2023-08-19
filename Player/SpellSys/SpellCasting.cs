using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Windows;

public class SpellCasting : MonoBehaviour
{
    private InventoryHolder _holder;
    private ActionController _actionController;
    [SerializeField] private UIController _uiPlayer;

    [SerializeField] private GameObject _upperForwardSpawnPoint;
    [SerializeField] private GameObject _upperCenterSpawnPoint;
    [SerializeField] private GameObject _groundForwardSpawnPoint;
    [SerializeField] private GameObject _groundCenterSpawnPoint;
    [SerializeField] private bool _casting;

    public SpellData _spellQ;
    [SerializeField] private float _cooldownQ;
    public SpellData _spellW;
    [SerializeField] private float _cooldownW;
    public SpellData _spellE;
    [SerializeField] private float _cooldownE;
    public SpellData _spellR;
    [SerializeField] private float _cooldownR;

    [SerializeField] private bool _testing;

    [Header("_______________SETS_________________")]
    [SerializeField] private Vector3 _spellSpawnPoint;
    [SerializeField] private Quaternion _direction;
    [SerializeField] private float _spawnCordX;
    [SerializeField] private float _spawnCordZ;
    [SerializeField] private Vector3 _targetPoint;
    [SerializeField] private Vector3 _toTargetPoint;
    [SerializeField] private float _targetCordX;
    [SerializeField] private float _targetCordZ;


    public static UnityAction BreakCastTrigger;

    private void Awake()
    {
        BreakCastTrigger += BreakCast;

        _holder = GetComponent<InventoryHolder>();
        _uiPlayer = FindObjectOfType<UIController>();
        _actionController = GetComponent<ActionController>();
        //_spellSpawnPoint = _actionController.SpawnFromPlayer;
    }
    private void Start()
    {
        _uiPlayer.PlayerActivUI.CastbarOff();
        _uiPlayer.PlayerActivUI._qButton.onClick.AddListener(OnQButtonPressed);
        _uiPlayer.PlayerActivUI._wButton.onClick.AddListener(OnWButtonPressed);
        _uiPlayer.PlayerActivUI._eButton.onClick.AddListener(OnEButtonPressed);
        //_uiPlayer.PlayerActivUI._rButton.onClick.AddListener(OnRButtonPressed);


        _actionController.Controlls.Player.Q.started += ctx => OnQButtonPressed();
        _actionController.Controlls.Player.W.started += ctx => OnWButtonPressed();
        _actionController.Controlls.Player.E.started += ctx => OnEButtonPressed();
        //_actionController.Controlls.Player.R.started += ctx => OnRButtonPressed();
    }

    public void TestMode()
    {
        if (!_testing) _testing = true;
        else if (_testing) _testing = false;
    }

    private void OnQButtonPressed()
    {
        string button = "Q";
        if (_testing)
        {            
            if (_cooldownQ == 0 && !_casting)
            {
                FormationSpell(_spellQ, _uiPlayer.PlayerActivUI._cdTextQ, button);
                _cooldownQ = _spellQ.Cooldown;

            }
            else Debug.Log("Can't spell now");
        }
        else
        {
            PressUIButton(_uiPlayer.PlayerActivUI._qButton);
            if (_cooldownQ == 0 && !_casting && _holder.Characteristics.MPValue >= _holder.Spells.QSpell.ManaCost)
            {
                SpellCast(_holder.Spells.QSpell, button, _uiPlayer.PlayerActivUI._cdTextQ); //
                _cooldownQ = _holder.Spells.QSpell.Cooldown;
                _holder.Characteristics.ManaCoast(_holder.Spells.QSpell.ManaCost);
            }
            else if (_holder.Spells.QSpell == null) Debug.Log("No spell");
            else if (_holder.Characteristics.MPValue <= _holder.Spells.QSpell.ManaCost) Debug.Log("No mana");
            else Debug.Log("Can't spell now");
        }        
    }
    private void OnWButtonPressed()
    {
        string button = "W";

        if (_testing)
        {
            if (_cooldownW == 0 && !_casting)
            {
                FormationSpell(_spellW, _uiPlayer.PlayerActivUI._cdTextW, button);
                _cooldownW = _spellW.Cooldown;

            }
            else Debug.Log("Can't spell now");
        }
        else
        {
            PressUIButton(_uiPlayer.PlayerActivUI._wButton);
            if (_cooldownW == 0 && !_casting && _holder.Characteristics.MPValue >= _holder.Spells.WSpell.ManaCost)
            {
                SpellCast(_holder.Spells.WSpell, button, _uiPlayer.PlayerActivUI._cdTextW);
                _cooldownW = _holder.Spells.WSpell.Cooldown;
                _holder.Characteristics.ManaCoast(_holder.Spells.WSpell.ManaCost);
            }
            else if (_holder.Spells.WSpell == null) Debug.Log("No spell");
            else if (_holder.Characteristics.MPValue <= _holder.Spells.WSpell.ManaCost) Debug.Log("No mana");
            else Debug.Log("Can't spell now");
        }
           
    }
    private void OnEButtonPressed()
    {

        string button = "E";
        if (_testing)
        {
            if (_cooldownE == 0 && !_casting)
            {
                FormationSpell(_spellE, _uiPlayer.PlayerActivUI._cdTextE, button);
                _cooldownE = _spellE.Cooldown;

            }
            else Debug.Log("Can't spell now");
        }
        else
        {
            PressUIButton(_uiPlayer.PlayerActivUI._eButton);
            if (_cooldownE == 0 && !_casting && _holder.Characteristics.MPValue >= _holder.Spells.ESpell.ManaCost)
            {
                SpellCast(_holder.Spells.ESpell, button, _uiPlayer.PlayerActivUI._cdTextE);
                _cooldownE = _holder.Spells.ESpell.Cooldown;
                _holder.Characteristics.ManaCoast(_holder.Spells.ESpell.ManaCost);
            }
            else if (_holder.Spells.ESpell.GameObject() == null) Debug.Log("No spell");
            else if (_holder.Characteristics.MPValue <= _holder.Spells.ESpell.ManaCost) Debug.Log("No mana");
            else Debug.Log("Can't spell now");
        }
    }
    private void OnRButtonPressed()
    {
        //string button = "R";
        //PressUIButton(_uiPlayer.PlayerActivUI._eButton);
        //if (_cooldownE == 0 && !_casting && _holder.Characteristics.MPValue >= _holder.Spells.RSpell.ManaCost)
        //{
        //    SpellCast(_holder.Spells.RSpell, button, _uiPlayer.PlayerActivUI._cdTextE);
        //    _cooldownE = _holder.Spells.RSpell.Cooldown;
        //    _holder.Characteristics.ManaCoast(_holder.Spells.RSpell.ManaCost);
        //}
        //else if (_holder.Spells.RSpell.GameObject() == null) Debug.Log("No spell");
        //else if (_holder.Characteristics.MPValue <= _holder.Spells.ESpell.ManaCost) Debug.Log("No mana");
        //else Debug.Log("Can't spell now");
    }
    private void BreakCast()
    {
        _casting = false;
    }
    private void PressUIButton(Button button)
    {
        if (button != null)
        {
            ExecuteEvents.Execute(button.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerDownHandler);
            ExecuteEvents.Execute(button.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerUpHandler);
        }
    }
    private void SpellCast(SpellData spell, string button, TextMeshProUGUI cdText)
    {
        _actionController.StopMove();
        _actionController.Rotation();
        var castType = spell.GetCastType();

        switch (castType)
        {
            case CastType.Instant:

                InstantCast(spell);
                StartCoroutine(SpellCooldown(spell.Cooldown, cdText, button));
                break;

            case CastType.Channeled:
                ChanneledCast(spell, cdText, button);
                break;

            case CastType.TimeCast:
                TimedCast(spell, cdText, button);
                break;

            default:
                Debug.LogWarning("Неизвестное тип каста!");
                break;
        }
    }
   
    private void SetNullCD(string litera)
    {
        if (litera == "Q")
        {
            _cooldownQ = 0;
        }
        else if (litera == "W")
        {
            _cooldownW = 0;
        }
        else if (litera == "E")
        {
            _cooldownE = 0;
        }
        //else if (litera == "R")
        //{
        //    _cooldownR = 0;
        //}
    }
    private void InstantCast(SpellData spell)
    {
        var spelltype = spell.GetSpellType();
        Ray ray = _actionController._camera.ScreenPointToRay(_actionController.Controlls.Player.Position.ReadValue<Vector2>());
        int mask = 1 << _actionController._ground;

        switch (spelltype)
        {
           
            case SpellType.Throw:

                if (_testing) CreateCustomSpell(spell);
                else CreateTrowSpell(spell);

                break;
            case SpellType.Fall:

                if (_testing) CreateCustomSpell(spell);
                else CreateFallSpell(spell, ray, mask);

                break;
            case SpellType.Point:

                if (_testing) CreateCustomSpell(spell);
                else CreatePointSpell(spell, ray, mask);

                break;
           
            default:
                Debug.LogWarning("Неизвестное направление каста!");
                break;
        }
    }
    private void ChanneledCast(SpellData spell, TextMeshProUGUI cdText, string button)
    {
        var spelltype = spell.GetSpellType();
        _casting = true;

        _uiPlayer.PlayerActivUI.CastbarOn();
        _uiPlayer.PlayerActivUI.SetCastBar(spell.CastingTime);        

        StartCoroutine(CastBar(spell, cdText, button));

        switch (spelltype)
        {
            case SpellType.Fall:
                StartCoroutine(ChannelFallCasting(spell, cdText, button));
                break;
           

            default:
                Debug.LogWarning("Неизвестное направление каста!");
                break;
        }

    }
    private IEnumerator CastBar(SpellData spell, TextMeshProUGUI cdText, string button)
    {
        float elapsedTime = 0;

        while (elapsedTime < spell.CastingTime)
        {
            if (!_casting)
            {
                StopCast(spell, cdText, button);
                yield break;
            }

            _uiPlayer.PlayerActivUI.SetCastBarAddValue(Time.deltaTime);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        _uiPlayer.PlayerActivUI.CastbarOff();        
        _casting = false;
        StartCoroutine(SpellCooldown(spell.Cooldown, cdText, button));
    }
    private IEnumerator ChannelFallCasting(SpellData spell, TextMeshProUGUI cdText, string button)
    {
        int castCount = 0;

        float timeCast = spell.SpellRepeatCount > 0 ? spell.CastingTime / spell.SpellRepeatCount : 0;

        while (castCount < spell.SpellRepeatCount)
        {
            if (!_casting)
            {
                StopCast(spell, cdText, button);
                yield break;
            }

            yield return new WaitForSeconds(timeCast);

            Ray ray = _actionController._camera.ScreenPointToRay(_actionController.Controlls.Player.Position.ReadValue<Vector2>());
            int mask = 1 << _actionController._ground;

            if (_testing) CreateCustomSpell(spell);
            else CreateFallSpell(spell, ray, mask);

            castCount++;
        }
    }
    private IEnumerator ChannelRayCasting(SpellData spell, TextMeshProUGUI cdText, string button)
    {
        int castCount = 0;

        float timeCast = spell.SpellRepeatCount > 0 ? spell.CastingTime / spell.SpellRepeatCount : 0;

        while (castCount < spell.SpellRepeatCount)
        {
            if (!_casting)
            {
                StopCast(spell, cdText, button);
                yield break;
            }           

            if (_testing) CreateCustomSpell(spell);
            else CreateRaytSpell(spell);

            castCount++;
            yield return new WaitForSeconds(timeCast);
        }
    }
    private void TimedCast(SpellData spell, TextMeshProUGUI cdText, string button)
    {
        _uiPlayer.PlayerActivUI.CastbarOn();
        _uiPlayer.PlayerActivUI.SetCastBar(spell.CastingTime);        

        Ray ray = _actionController._camera.ScreenPointToRay(_actionController.Controlls.Player.Position.ReadValue<Vector2>());

        int mask = 1 << _actionController._ground;

        _casting = true;

        StartCoroutine(SpellingCast(spell, button, cdText, ray, mask));
    }
    private IEnumerator SpellingCast(SpellData spell, string button, TextMeshProUGUI cdText, Ray ray, int mask)
    {
        float elapsedTime = 0;

        while (elapsedTime < spell.CastingTime)
        {
            if (!_casting)
            {
                StopCast(spell, cdText, button);
                yield break;
            }

            _uiPlayer.PlayerActivUI.SetCastBarAddValue(Time.deltaTime);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        _uiPlayer.PlayerActivUI.CastbarOff();        

        AfterTimeCast(spell, ray, mask);
        _casting = false;
        StartCoroutine(SpellCooldown(spell.Cooldown, cdText, button));
    }
    private void AfterTimeCast(SpellData spell, Ray ray, int mask)
    {
        var spelltype = spell.GetSpellType();

        switch (spelltype)
        {
            case SpellType.Throw:

                if (_testing) CreateCustomSpell(spell);
                else CreateTrowSpell(spell);

                break;
            case SpellType.Fall:

                if (_testing) CreateCustomSpell(spell);
                else CreateFallSpell(spell, ray, mask);

                break;
            case SpellType.Point:

                if (_testing) CreateCustomSpell(spell);
                else CreatePointSpell(spell, ray, mask);

                break;
            default:
                Debug.LogWarning("Неизвестное направление каста!");
                break;
        }
    }
    private void CreateTrowSpell(SpellData spell)
    {
        Quaternion directionTo = this.transform.rotation;
        GameObject spellThrow = Instantiate(spell.SpellPrefab, _upperForwardSpawnPoint.transform.position, directionTo);

        ThrowSpell spellCastedThrow = spellThrow.GetComponent<ThrowSpell>();
        spellCastedThrow.SetDirection(directionTo);
        spellCastedThrow.SetOwner(_holder.gameObject);
    }
    private void CreateFallSpell(SpellData spell, Ray ray, int mask)
    {
        RaycastHit hit;

        Vector3 spawnPoint;

        var spawnType = spell.GetSpawnType();

        if (spawnType.ToString() == "Random")
        {
            _spawnCordX = UnityEngine.Random.Range(0, 5);
            _spawnCordZ = UnityEngine.Random.Range(0, 5);
        }
        else if (spawnType.ToString() == "Point")
        {
            _spawnCordX = 0;
            _spawnCordZ = 0;
        }

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
        {

            spawnPoint = hit.point + new Vector3(_spawnCordX, 5, _spawnCordZ);
            GameObject spellFall = Instantiate(spell.SpellPrefab, spawnPoint, Quaternion.identity);

            FallSpell spellCastedFall = spellFall.GetComponent<FallSpell>();

            spellCastedFall.SetDirection(hit.point);
            spellCastedFall.SetOwner(_holder.gameObject);
        }
    }
    private void CreatePointSpell(SpellData spell, Ray ray, int mask)
    {
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
        {
            GameObject spellPoint = Instantiate(spell.SpellPrefab, hit.point, Quaternion.identity);

            PointSpell spellCastedPoint = spellPoint.GetComponent<PointSpell>();
            spellCastedPoint.ForDestroy();
            spellCastedPoint.SetOwner(_holder.gameObject);
        }
    }
    private void CreateRaytSpell(SpellData spell)
    {
        Quaternion directionRay = this.transform.rotation;
        GameObject spellRay = Instantiate(spell.SpellPrefab, _upperForwardSpawnPoint.transform.position, directionRay);

        RaySpell spellCastedRay = spellRay.GetComponent<RaySpell>();

        spellCastedRay.ForDestroy();
        spellCastedRay.SetOwner(_holder.gameObject);
    }

    private void FormationSpell(SpellData spell, TextMeshProUGUI cdText, string button)
    {
        _actionController.Rotation();
        _actionController.StopMove();
        RaycastHitGetPoint();        
               
        SetSpellTarget(spell);
        SetSpellSpawnpointCursor(spell);
        SetSpellTypeCast(spell, cdText, button);
    }    
    private void RaycastHitGetPoint()
    {
        Ray ray = _actionController._camera.ScreenPointToRay(_actionController.Controlls.Player.Position.ReadValue<Vector2>());
        RaycastHit hit;
        int mask = 1 << _actionController._ground;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
        {
            _targetPoint = hit.point;
        }
    }
    private void SetSpellType(SpellData spell)
    {
        var spellType = spell.GetSpellType();
        switch (spellType)
        {
            case SpellType.Throw:                
                _direction = this.gameObject.transform.rotation;                
                break;
            case SpellType.Fall:
                Vector3 directionVector = _targetPoint - transform.position;
                _direction = Quaternion.LookRotation(directionVector);
                break;
            case SpellType.Point:
                _direction = Quaternion.identity;
                break;
            case SpellType.AroundPoint:
                _direction = Quaternion.identity;
                break;
        }
    }
    private void SetSpellTarget(SpellData spell)
    {
        var targetType = spell.GetTargetType();
        RaycastHitGetPoint();
        switch (targetType)
        {
            case TargetType.Solo:

                break;
            case TargetType.Mass:
                _targetCordX = UnityEngine.Random.Range(-4, 4);
                _targetCordZ = UnityEngine.Random.Range(-4, 4);
                _targetPoint += new Vector3(_spawnCordX, 0, _spawnCordZ);
                break;

        }
    }
    private void SetSpellSpawnpointCursor(SpellData spell)
    {

        var spawnPoint = spell.GetSpawnPoint();
        switch (spawnPoint)
        {
            case SpawnPoint.AtPoint:
                _spellSpawnPoint = _targetPoint;
                break;
            case SpawnPoint.OverPoint:

                _spellSpawnPoint = _targetPoint;
                _spellSpawnPoint += new Vector3(0, 1, 0);
                break;
            case SpawnPoint.OnTopPoint:

                _spellSpawnPoint = _targetPoint;
                _spellSpawnPoint += new Vector3(0, 5, 0);
                break;
            case SpawnPoint.UpperForward:
                _spellSpawnPoint = _upperForwardSpawnPoint.transform.position;
                break;
            case SpawnPoint.UpperCenter:
                _spellSpawnPoint = _upperCenterSpawnPoint.transform.position;
                break;
            case SpawnPoint.GroundForward:
                _spellSpawnPoint = _groundForwardSpawnPoint.transform.position;
                break;
            case SpawnPoint.GroundCenter:
                _spellSpawnPoint = _groundCenterSpawnPoint.transform.position;
                break;
        }
    }    
    private void SetSpellSpawnType(SpellData spell)
    {
        var spawnType = spell.GetSpawnType();
        switch (spawnType)
        {
            case SpawnType.Point:
                
                break;
            case SpawnType.Random:
                _spawnCordX = UnityEngine.Random.Range(-4, 4);
                _spawnCordZ = UnityEngine.Random.Range(-4, 4);
                _spellSpawnPoint += new Vector3(_spawnCordX, 0, _spawnCordZ);
                break;
           
        }
    }
    private void SetSpellTypeCast(SpellData spell, TextMeshProUGUI cdText, string button)
    {

        var castType = spell.GetCastType();
        switch (castType)
        {
            case CastType.Instant:
                CreateCustomSpell(spell);
                break;
            case CastType.TimeCast:
                _casting = true;
                _uiPlayer.PlayerActivUI.CastbarOn();
                _uiPlayer.PlayerActivUI.SetCastBar(spell.CastingTime);
                StartCoroutine(SpellingTimeCast(spell, cdText, button));
                break;
            case CastType.Channeled:
                _casting = true;
                _uiPlayer.PlayerActivUI.CastbarOn();
                _uiPlayer.PlayerActivUI.SetCastBar(spell.CastingTime);
                StartCoroutine(SpellingChannelCast(spell, cdText, button));
                StartCoroutine(CastingCastBar(spell, cdText, button));
                break;
        }
        StartCoroutine(SpellCooldown(spell.Cooldown, cdText, button));
    }
    private IEnumerator SpellingTimeCast(SpellData spell, TextMeshProUGUI cdText, string button)
    {
        float elapsedTime = 0;

        while (elapsedTime < spell.CastingTime)
        {
            if (!_casting)
            {
                StopCast(spell, cdText, button);
                yield break;
            }

            _uiPlayer.PlayerActivUI.SetCastBarAddValue(Time.deltaTime);
            elapsedTime += Time.deltaTime;
            //SetSpellSpawnType(spell);
            yield return null;
        }

        _uiPlayer.PlayerActivUI.CastbarOff();        
        _casting = false;
        CreateCustomSpell(spell);
    }
    private IEnumerator SpellingChannelCast(SpellData spell, TextMeshProUGUI cdText, string button)
    {
        int castCount = 0;        

        float timeCast = spell.SpellRepeatCount > 0 ? spell.CastingTime / spell.SpellRepeatCount : 0;

        while (castCount < spell.SpellRepeatCount)
        {
            if (!_casting)
            {
                StopCast(spell, cdText, button);
                yield break;
            }
            SetSpellSpawnpointCursor(spell);
            //SetSpellSpawnType(spell);

            CreateCustomSpell(spell);
           
            castCount++;
            yield return new WaitForSeconds(timeCast);
            
        }

        _casting = false;
    }
    private IEnumerator CastingCastBar(SpellData spell, TextMeshProUGUI cdText, string button)
    {
        float elapsedTime = 0;

        while (elapsedTime < spell.CastingTime)
        {
            if (!_casting)
            {
                StopCast(spell, cdText, button);
                yield break;
            }

            _uiPlayer.PlayerActivUI.SetCastBarAddValue(Time.deltaTime);
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        _uiPlayer.PlayerActivUI.CastbarOff();
    }
    private void StopCast(SpellData spell, TextMeshProUGUI cdText, string button)
    {
        _uiPlayer.PlayerActivUI.CastbarOff();

        StartCoroutine(SpellCooldown(spell.Cooldown, cdText, button));
        _uiPlayer.PlayerActivUI.SetCastBarValue(0);
    }
    private IEnumerator SpellCooldown(float spellCD, TextMeshProUGUI buttonCount, string litera)
    {
        while (spellCD > 0)
        {
            buttonCount.text = spellCD.ToString();
            yield return new WaitForSeconds(1f);
            spellCD--;
        }

        SetNullCD(litera);
        buttonCount.text = litera;
    }



    private void CreateCustomSpell(SpellData spell)
    {

        SetSpellSpawnType(spell);
        SetSpellType(spell);

        GameObject createrCustomSpell = Instantiate(spell.SpellPrefab, _spellSpawnPoint, _direction);

        var customSpell = createrCustomSpell.GetComponent<CustomSpell>();
        customSpell.SetSpawnPosition();
        customSpell.SetOwner(_holder.gameObject);
        customSpell.SetDirectionForward(_direction);
        customSpell.SetDirectionPoint(_targetPoint);

        if (spell._destroyOnTriggerEnter) customSpell.SetDestroyOnTriggerEnter();
        if (spell._moveForward) customSpell.StartSpellMoveForward();
        if (spell._dealDamageBeforeDestroy) customSpell.DealDamageBeforeDestroy();
        if (spell._moveToPoint) customSpell.StartSpellMoveToPoint();
        if (spell._delayDestroy) customSpell.SetDestroyTime();

        //customSpell.PlaySpellParticle();
        //ReturnSpawnCoords();
    }

    private void ReturnSpawnCoords()
    {
        _toTargetPoint = new Vector3(_targetPoint.x, _targetPoint.y, _targetPoint.z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        
        Gizmos.DrawSphere(_targetPoint, 0.2f);
    }
}
