using UnityEngine;

public class Gate : MonoBehaviour
{
    public string _sceneName; // �������� ������� �����   
    public string _spawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LocationManager.Instance.MovePlayer(_sceneName, _spawnPoint);
        }
    }
}
