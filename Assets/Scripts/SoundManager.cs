using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager inst;
    private void Awake()
    {
        inst = this;
    }


    [SerializeField] private AudioClip[] ExplosionSmallClips;
    public void PlayExplosionSmall(AudioSource source)
    {
        source.PlayOneShot(ExplosionSmallClips[Random.Range(0, ExplosionSmallClips.Length)]);
    }
    [SerializeField] private AudioClip[] ExplosionMediumClips;
    public void PlayExplosionMedium(AudioSource source)
    {
        source.PlayOneShot(ExplosionMediumClips[Random.Range(0, ExplosionMediumClips.Length)]);
    }

    [SerializeField] private AudioClip[] ExplosionBigClips;
    public void PlayExplosionBig(AudioSource source)
    {
        source.PlayOneShot(ExplosionBigClips[Random.Range(0, ExplosionBigClips.Length)]);
    }

    [SerializeField] private AudioClip[] BotDieClips;
    [SerializeField] private AudioSource PlayBotDieSourceA;
    [SerializeField] private AudioSource PlayBotDieSourceB;
    public void PlayBotDie()
    {
        AudioClip clip = BotDieClips[Random.Range(0, BotDieClips.Length)];
        if (PlayBotDieSourceA.isPlaying)
            PlayBotDieSourceB.PlayOneShot(clip);
        else PlayBotDieSourceA.PlayOneShot(clip);
    }
    [SerializeField] private AudioClip[] BotGetDamageClips;
    [SerializeField] private AudioSource PlayBotGetDamageSourceA;
    [SerializeField] private AudioSource PlayBotGetDamageSourceB;
    public void PlayBotGetDamage()
    {
        AudioClip clip = BotGetDamageClips[Random.Range(0, BotGetDamageClips.Length)];
        if (PlayBotGetDamageSourceA.isPlaying)
            PlayBotGetDamageSourceB.PlayOneShot(clip);
        else PlayBotGetDamageSourceA.PlayOneShot(clip);
    }

    [SerializeField] private AudioClip[] FireGunClips;
    [SerializeField] private AudioClip[] FireGunSubClips;
    public void PlayFireGun(AudioSource source, AudioSource source2)
    {
        source.PlayOneShot(FireGunClips[Random.Range(0, FireGunClips.Length)]);
        source2.PlayOneShot(FireGunSubClips[Random.Range(0, FireGunSubClips.Length)]);
    }

    [SerializeField] private AudioClip[] ScoreClips;
    [SerializeField] private AudioSource ScoreSource;
    public void PlayScore()
    {
        ScoreSource.PlayOneShot(ScoreClips[Random.Range(0, ScoreClips.Length)]);
    }
    [SerializeField] private AudioClip[] NoScoreClips;
    public void PlayNoScore()
    {
        ScoreSource.PlayOneShot(NoScoreClips[Random.Range(0, NoScoreClips.Length)]);
    }
}