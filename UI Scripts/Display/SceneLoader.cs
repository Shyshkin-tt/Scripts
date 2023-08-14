using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public string Location;
    public Vector3 Position;
    public string CharName;
    public GameObject Char;    

    private void Awake()
    {
        
    }

    public void SetStats(string loc, Vector3 spawn, string name)
    {
        Location = loc;
        Position = spawn;
        CharName = name;
    }

    public void GetChar(GameObject character)
    {
        Char = character;        
    }

    private void Start()
    {
        DontDestroyOnLoad(this);        
    }
    
    
}
