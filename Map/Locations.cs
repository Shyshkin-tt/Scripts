using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Locations", menuName = "Map/Location List")]
public class Locations : ScriptableObject
{
    public LocationData[] locations;

    public List<LocationData> GetLocationList()
    {
        var locationList = new List<LocationData>();

        foreach (var location in locations)
        {
            locationList.Add(location);
        }

        return locationList;
    }
}
