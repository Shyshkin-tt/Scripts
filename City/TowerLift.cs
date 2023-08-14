using System.Collections;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class TowerLift : MonoBehaviour
{
    public GameObject _liftOne;
    public GameObject[] _gateOne;
    public GameObject[] _liftGateOne;
    public GameObject _liftTwo;
    public GameObject[] _gateTwo;
    public GameObject[] _liftGateTwo;
    public float _heightMoveLiftOne;
    public float _heightMoveLiftTwo;
    public float _waitTime;
    public float _speedMove;

    private Vector3 _initialLiftOnePosition;
    private Vector3 _initialLiftTwoPosition;

    private void Start()
    {
        _initialLiftOnePosition = _liftOne.transform.position;
        _initialLiftTwoPosition = _liftTwo.transform.position;

        // Начинаем цикл лифта
        StartCoroutine(LiftDelay());
    }
    private IEnumerator LiftDelay()
    {
        StartCoroutine(LiftOne());
        yield return new WaitForSeconds(_waitTime);
        StartCoroutine(LiftTwo());
    }
    private IEnumerator LiftOne()
    {
        while (true)
        {
            // Опускаем лифт 1 и открываем ворота 1
            _gateOne[0].SetActive(true); _gateOne[1].SetActive(true);           
            yield return LowerLift(_liftOne, _initialLiftOnePosition.y - _heightMoveLiftOne);
            _liftGateOne[0].SetActive(false); _liftGateOne[1].SetActive(false);

            // Ждем указанное время
            yield return new WaitForSeconds(_waitTime);

            _liftGateOne[0].SetActive(true); _liftGateOne[1].SetActive(true);
            // Закрываем ворота 1 и поднимаем лифт 1
            yield return RaiseLift(_liftOne, _initialLiftOnePosition.y);
            _gateOne[0].SetActive(false); _gateOne[1].SetActive(false);

            // Ждем указанное время перед повторением цикла
            yield return new WaitForSeconds(_waitTime);
        }
    }
    private IEnumerator LiftTwo()
    {
        while (true)
        {
            // Опускаем лифт 2 и открываем ворота 2
            _gateTwo[0].SetActive(true); _gateTwo[1].SetActive(true);
            yield return LowerLift(_liftTwo, _initialLiftTwoPosition.y - _heightMoveLiftTwo);
            _liftGateTwo[0].SetActive(false); _liftGateTwo[1].SetActive(false);

            // Ждем указанное время
            yield return new WaitForSeconds(_waitTime);

            // Закрываем ворота 2 и поднимаем лифт 2
            _liftGateTwo[0].SetActive(true); _liftGateTwo[1].SetActive(true);
            yield return RaiseLift(_liftTwo, _initialLiftTwoPosition.y);
            _gateTwo[0].SetActive(false); _gateTwo[1].SetActive(false);

            yield return new WaitForSeconds(_waitTime);
        }
    }

    private IEnumerator LowerLift(GameObject lift, float targetHeight)
    {
        while (lift.transform.position.y > targetHeight)
        {
            lift.transform.position -= Vector3.up * _speedMove * Time.deltaTime;
            yield return null;
        }
        lift.transform.position = new Vector3(lift.transform.position.x, targetHeight, lift.transform.position.z);
    }

    private IEnumerator RaiseLift(GameObject lift, float targetHeight)
    {
        while (lift.transform.position.y < targetHeight)
        {
            lift.transform.position += Vector3.up * _speedMove * Time.deltaTime;
            yield return null;
        }
        lift.transform.position = new Vector3(lift.transform.position.x, targetHeight, lift.transform.position.z);
    }
}
