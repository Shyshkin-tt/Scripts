using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float _speed;
    public float _speedSpin;
    public int _count;
    public int _countEnd;

    private Quaternion _direction;

    public void SetDirection(Quaternion direction)
    {
        _direction = direction;
    }

    private void Update()
    {
        if (_count < _countEnd)
        {
            // Двигаем пулю в заданном направлении
            transform.position += _direction * Vector3.forward * _speed * Time.deltaTime;
            // Если необходимо также вращать пулю вокруг своей оси, можно добавить следующую строку:
            transform.Rotate(Vector3.up * _speedSpin * Time.deltaTime);

            _count++;
        }
        else
        {
            Destroy(this.gameObject); // удаляем игровой объект, к которому прикреплен скрипт
        }
    }
}
