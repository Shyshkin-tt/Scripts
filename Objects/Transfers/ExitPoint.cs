using UnityEngine;

public class ExitPoint : MonoBehaviour
{    
    public int _exitID; // ������������� ������
    public string _sceneName; // �������� ������� �����

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           
        }
    }
}
