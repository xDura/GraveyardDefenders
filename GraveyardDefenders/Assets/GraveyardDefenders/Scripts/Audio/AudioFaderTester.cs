#if UNITY_EDITOR
using UnityEngine;
using DG.Tweening;

namespace XD.Audio
{
    public class AudioFaderTester : MonoBehaviour
    {
        public AudioClip clip0;
        public AudioClip clip1;
        public AudioSource source0;
        public AudioSource source1;
        public int currentSourceIndex = 0;
        public float fadeTime = 1.0f;

        void Awake()
        {
            source0.clip = clip0;
            source1.clip = clip1;
            PlayAudioSource(0);
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                PlayAudioSource(0);
            if(Input.GetKeyDown(KeyCode.Alpha2))
                PlayAudioSource(1);
        }

        void PlayAudioSource(int source)
        {
            if(source == 1)
            {
                if (currentSourceIndex == source) return;
                source0.DOFade(0.0f, fadeTime).OnComplete(OnComplete0).SetId("Fade0");
                float t = source0.time;
                source1.DOFade(1.0f, fadeTime).SetId("Fade1");
                source1.time = t;
                source1.Play();
                currentSourceIndex = source;
            }
            if(source == 0)
            {
                if (currentSourceIndex == source) return;
                source1.DOFade(0.0f, fadeTime).OnComplete(OnComplete1).SetId("Fade1");
                float t = source1.time;
                source0.DOFade(1.0f, fadeTime).SetId("Fade0");
                source0.time = t;
                source0.Play();
                currentSourceIndex = source;
            }
        }

        void OnComplete1()
        {
            source1.Stop();
        }

        void OnComplete0()
        {
            source0.Stop();
        }
    }   
}
#endif
