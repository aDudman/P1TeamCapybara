using System.Collections;
using UnityEngine;

namespace Environment.Interactables
{
    public class ShovableBoulder : AShoveable
    {
        [SerializeField, Tooltip("The sound that will play when the boulder is shoved.")]
        private AudioSource shoveSound1;

        [SerializeField, Tooltip("The second audio source for crossfading.")]
        private AudioSource shoveSound2;

        [SerializeField, Tooltip("The duration of the fade-out effect in seconds.")]
        private float fadeOutDuration = 0.5f;

        [SerializeField, Tooltip("The duration of the crossfade effect in seconds.")]
        private float crossfadeDuration = 0.5f;

        private Coroutine fadeOutCoroutine;
        private Coroutine crossfadeCoroutine;

        private float startVolume;
        private bool isPlayingFirstSource = true;

        protected override void Start()
        {
            startVolume = shoveSound1.volume;
            base.Start();
        }

        protected override void FixedUpdate()
        {
            if (shoveFrameCount <= 0)
            {
                if ((shoveSound1.isPlaying || shoveSound2.isPlaying) && fadeOutCoroutine == null)
                {
                    StopCoroutine(crossfadeCoroutine);
                    fadeOutCoroutine = StartCoroutine(FadeOutSound());
                }
            }
            else
            {
                if (fadeOutCoroutine != null)
                {
                    StopCoroutine(fadeOutCoroutine);
                    fadeOutCoroutine = null;
                }
            }
            base.FixedUpdate();
        }

        public override Vector3 Shoving(Vector3 direction)
        {
            if (!shoveSound1.isPlaying && !shoveSound2.isPlaying)
            {
                PlaySound(shoveSound1);
            }
            return base.Shoving(direction);
        }

        private void PlaySound(AudioSource audioSource)
        {
            audioSource.volume = startVolume;
            audioSource.Play();
            crossfadeCoroutine = StartCoroutine(CrossfadeSound(audioSource));
        }

        private IEnumerator CrossfadeSound(AudioSource currentSource, bool prefaded = false)
        {
            // On the first call, the coroutine starts immediately and crossfade happens [crossfadeDuration] seconds before the end of the clip
            // On the second call, the coroutine starts after the previous clip crossfades so the wait needs to be that much shorter
            if (prefaded)
            {
                yield return new WaitForSeconds(currentSource.clip.length - (2*crossfadeDuration));
            }
            else
            {
                yield return new WaitForSeconds(currentSource.clip.length - crossfadeDuration);
            }

            print ("Starting crossfade");

            AudioSource nextSource = isPlayingFirstSource ? shoveSound2 : shoveSound1;
            isPlayingFirstSource = !isPlayingFirstSource;

            nextSource.volume = 0;
            nextSource.Play();

            float elapsedTime = 0;
            while (elapsedTime < crossfadeDuration)
            {
                nextSource.volume = Mathf.Lerp(0, startVolume, elapsedTime / crossfadeDuration);
                currentSource.volume = Mathf.Lerp(startVolume, 0, elapsedTime / crossfadeDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            nextSource.volume = startVolume;
            currentSource.Stop();

            print ("Finished crossfade");
            crossfadeCoroutine = StartCoroutine(CrossfadeSound(nextSource, true));
        }

        private IEnumerator FadeOutSound()
        {
            float elapsedTime = 0;

            while (elapsedTime < fadeOutDuration)
            {
                shoveSound1.volume = Mathf.Lerp(startVolume, 0, elapsedTime / fadeOutDuration);
                shoveSound2.volume = Mathf.Lerp(startVolume, 0, elapsedTime / fadeOutDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            shoveSound1.Stop();
            shoveSound2.Stop();
            fadeOutCoroutine = null;
        }
    }
}