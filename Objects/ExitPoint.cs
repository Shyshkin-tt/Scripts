using UnityEngine;

public class ExitPoint : MonoBehaviour
{    
    public int _exitID; // Идентификатор выхода
    public string _sceneName; // Название целевой сцены

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           
        }
    }
}
