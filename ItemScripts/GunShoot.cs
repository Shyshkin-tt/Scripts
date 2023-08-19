using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShoot : MonoBehaviour
{
    private ActionController _action;
    [SerializeField] private GameObject _inventory;

    public GameObject _bulletPrefab;
    private GameObject _bulletSpawn;
    public ParticleSystem _shootExpl;

    private void Awake()
    {
        _action = GetComponentInParent<ActionController>();
        _inventory = _action.transform.gameObject;
        _bulletSpawn = _action.UpperForwardSpawmPoint;
    }

    public void MakeShoot()
    {
        _shootExpl.Play();

        Quaternion directionTo = _action.transform.rotation;

        // Создаем пулю с такой же ориентацией как у _action
        GameObject bulletObject = Instantiate(_bulletPrefab, _bulletSpawn.transform.position, directionTo);

        var owner = bulletObject.GetComponent<BulletMakeDamage>();

        owner.SetOwner(_inventory);

        // Получаем компонент Bullet из созданной пули
        Bullet bullet = bulletObject.GetComponent<Bullet>();

        // Задаем направление движения пули
        bullet.SetDirection(directionTo);
    }
}
