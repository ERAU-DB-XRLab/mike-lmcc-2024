using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MIKESFXManager : MonoBehaviour
{
    public static MIKESFXManager main;
    [SerializeField] private GameObject sfxPrefab;

    void Awake()
    {
        main = this;    
    }

    public void PlaySFX(string name, float volume, Vector3? loc = null)
    {


        AudioSource newSFX = Instantiate(sfxPrefab).GetComponent<AudioSource>();
        newSFX.clip = Resources.Load<AudioClip>("SFX/" + name);
        newSFX.volume = volume;

        if(loc != null){
            newSFX.transform.position = (Vector3) loc;
            newSFX.spatialBlend = 1f;
        }

        newSFX.Play();

        Destroy(newSFX.gameObject, newSFX.clip.length);

    }

    public AudioSource FadeInLoop(string name, float fadeTime)
    {

        AudioSource newSFX = Instantiate(sfxPrefab).GetComponent<AudioSource>();
        newSFX.clip = Resources.Load<AudioClip>("SFX/" + name);
        newSFX.volume = 0f;
        newSFX.Play();

        StartCoroutine(FadeIn(newSFX, fadeTime));

        return newSFX;

    }

    public IEnumerator FadeIn(AudioSource source, float fadeTime)
    {

        float elapsedTime = 0;

        while(elapsedTime < fadeTime)
        {
            yield return new WaitForSeconds(0.05f);
            elapsedTime += 0.05f;
            if (source)
            {
                source.volume = elapsedTime / fadeTime;
            } else
            {
                break;
            }
        }

    }

}
