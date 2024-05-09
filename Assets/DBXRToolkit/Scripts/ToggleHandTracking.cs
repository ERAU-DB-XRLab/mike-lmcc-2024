using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleHandTracking : MonoBehaviour
{

    [SerializeField] private GameObject handTrackingPlayer, mainPlayer;

    // Start is called before the first frame update
    public void Toggle()
    {
        if(mainPlayer.activeSelf)
        {
            mainPlayer.SetActive(false);
            handTrackingPlayer.SetActive(true);
        }
    }
}
