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

        // ������� ���� � ����� �� ����������� ��� � _action
        GameObject bulletObject = Instantiate(_bulletPrefab, _bulletSpawn.transform.position, directionTo);

        var owner = bulletObject.GetComponent<BulletMakeDamage>();

        owner.SetOwner(_inventory);

        // �������� ��������� Bullet �� ��������� ����
        Bullet bullet = bulletObject.GetComponent<Bullet>();

        // ������ ����������� �������� ����
        bullet.SetDirection(directionTo);
    }
}
