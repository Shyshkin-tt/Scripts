using UnityEngine;

public class Gate : MonoBehaviour
{
    public string _sceneName; // Название целевой сцены   
    public string _spawnPoint;
    private string _type = "World";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var holder = other.GetComponent<InventoryHolder>();
            
            LocationManager.Instance.MovePlayer(_sceneName, _spawnPoint, holder, _type);
            if (holder != null)
                Debug.Log("NO HOLDER");
        }
    }
}
