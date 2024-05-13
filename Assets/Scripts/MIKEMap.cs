using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MIKEMap : MonoBehaviour
{
    public static MIKEMap Main { get; private set; }
    [SerializeField] private Transform mapCorrection;

    private char[] alphabet = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

    [SerializeField] private Transform mapStart, mapEnd, ignore;
    [Space]
    [SerializeField] private LayerMask mapLayer;

    void Awake()
    {
        if (Main == null)
            Main = this;
        else
            Destroy(this);
    }

    public Vector3 GetPositionFromCode(string code)
    {
        if (code.Length == 2)
        {

            char letter = code[0];
            int numberIndex = int.Parse(code[1].ToString());

            int letterIndex = 0;

            for (int i = 0; i < alphabet.Length; i++)
            {
                if (alphabet[i].ToString().Equals(letter.ToString().ToLower()))
                {
                    letterIndex = i;
                }
            }

            float x = Mathf.Lerp(mapStart.localPosition.x, mapEnd.localPosition.x, (float)numberIndex / 27f);
            float z = Mathf.Lerp(mapStart.localPosition.z, mapEnd.localPosition.z, (float)letterIndex / 25f);

            ignore.transform.localPosition = new Vector3(x, mapStart.position.y, z);
            return ignore.transform.position;

        }
        else
        {
            Debug.LogWarning("MIKEMap: Code length must be size 2, of the format <Letter><Number>");
        }

        return Vector3.zero;
    }

    public Vector2 NormalizePosition(Vector3 localPosition)
    {
        float x = Mathf.InverseLerp(mapStart.localPosition.x, mapEnd.localPosition.x, localPosition.x);
        float z = Mathf.InverseLerp(mapStart.localPosition.z, mapEnd.localPosition.z, localPosition.z);

        return new Vector2(x, z);
    }

    public Vector3 GetPositionFromNormalized(Vector2 normalizedPosition, bool localPosition = false)
    {
        float x = Mathf.Lerp(mapStart.localPosition.x, mapEnd.localPosition.x, normalizedPosition.x);
        float y = ignore.transform.position.y;
        float z = Mathf.Lerp(mapStart.localPosition.z, mapEnd.localPosition.z, normalizedPosition.y);

        ignore.transform.localPosition = new Vector3(x, y, z);

        if (IsPositionOnMap(ignore.transform.position, out RaycastHit hit))
        {
            ignore.transform.position = new Vector3(ignore.transform.position.x, hit.point.y, ignore.transform.position.z);
        }
        else
        {
            Debug.LogWarning("MIKEMap: No hit found for normalized position. Using default height.");
        }

        return localPosition ? ignore.transform.localPosition : ignore.transform.position;
    }

    public Vector3 GetPositionFromUTM(double easting, double northing, bool localPosition = false)
    {
        Vector3 pos = mapCorrection.TransformPoint(GPSConverter.Main.UTMToUCS(easting, northing));
        if (IsPositionOnMap(pos, out RaycastHit hit))
        {
            return localPosition ? transform.InverseTransformPoint(hit.point) : hit.point;
        }
        else
        {
            Debug.LogWarning("MIKEMap: No hit found for UTM position. Using default height of 0.");
            return localPosition ? transform.InverseTransformPoint(pos) : pos;
        }
    }

    public bool IsPositionOnMap(Vector3 position, out RaycastHit hit, float height = 500, float maxRayDistance = 1000)
    {
        return Physics.Raycast(position + Vector3.up * height, Vector3.down, out hit, maxRayDistance, mapLayer);
    }
}
