using System.Collections;
using System.Collections.Generic;
using GeocoordinateTransformer;
using UnityEngine;

public class GPSConverter : MonoBehaviour
{
    public static GPSConverter Main { get; private set; }

    CoordinateTransformer transformer;

    void Awake()
    {
        if (Main == null)
            Main = this;
        else
            Destroy(this);

        transformer = GetComponent<CoordinateTransformer>();
    }

    public Vector3 UTMToUCS(double easting, double northing)
    {
        UTMCoordinates utm = new UTMCoordinates(easting, northing, 0);
        return transformer.GetUnityCoordinates(utm);
    }

    public UTMCoordinates UCSToUTM(Vector3 position)
    {
        return transformer.GetUTMCoordinates(position);
    }
}
