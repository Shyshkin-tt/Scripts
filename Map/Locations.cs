using UnityEngine;

[CreateAssetMenu(fileName = "Locations", menuName = "Map/Location List")]
public class Locations : ScriptableObject
{
    public LocationData[] locations;
}
