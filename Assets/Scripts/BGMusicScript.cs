using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMusicScript : MonoBehaviour
{
    [Header("Параметры звука")]
    [SerializeField]
    private float pauseBetweenBeatMax = 1f;
    [SerializeField]
    private float pauseBetweenBeatMin = 0.4f;
    private float pauseBetweenBeat;
    [SerializeField]
    private float pauseBetweenDecrease = 0.001f;

    private bool isCanPlay = false;
    private float beatTimer = 0;
    private bool isFirstBeat = false;

    private void Start()
    {
        PauseReset();
    }

    void Update()
    {
        PlayBGM();
    }

    void PlayBGM()
    {
        if (isCanPlay)
        {
            if (beatTimer > 0)
                beatTimer -= Time.deltaTime;
            else
            {
                if (!isFirstBeat)
                {
                    transform.Find("Beat1").GetComponent<AudioSource>().Play();
                    isFirstBeat = true;
                }
                else
                {
                    transform.Find("Beat2").GetComponent<AudioSource>().Play();
                    isFirstBeat = false;
                }
                beatTimer = pauseBetweenBeat;
                if (pauseBetweenBeat > pauseBetweenBeatMin)
                    pauseBetweenBeat -= pauseBetweenDecrease;
            }
        }
    }
    public void PauseReset()
    {
        pauseBetweenBeat = pauseBetweenBeatMax;
    }
    public void CanPlay(bool play)
    {
        isCanPlay = play;
    }

}
