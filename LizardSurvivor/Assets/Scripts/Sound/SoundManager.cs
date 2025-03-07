using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public AudioSource audioSource;  // SE用
    public AudioSource bgmSource;    // BGM用

    private Dictionary<string, AudioClip> soundDictionary;

    // SE
    public AudioClip click;
    public AudioClip pMove;
    public AudioClip crtNest;
    public AudioClip brkNest;
    public AudioClip morning;
    public AudioClip night;
    public AudioClip eAtk;
    public AudioClip eat;
    public AudioClip clear;
    public AudioClip gameover;

    // BGM
    public AudioClip startBGM;
    public AudioClip gameBGM;

    private AudioClip currentBGM;   // 現在流れているBGM

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // BGM用AudioSourceの初期化
            if (bgmSource == null)
            {
                bgmSource = gameObject.AddComponent<AudioSource>();
                bgmSource.loop = true;
            }

            // SE用AudioSourceの初期化
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            // SE辞書の作成
            soundDictionary = new Dictionary<string, AudioClip>
            {
                {"click", click },
                {"pMove", pMove },
                {"crtNest", crtNest },
                {"brkNest", brkNest },
                {"morning", morning },
                {"night", night },
                {"eAtk", eAtk },
                {"eat", eat },
                {"clear", clear },
                {"gameover", gameover }
            };
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ChangeBGMByScene();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ChangeBGMByScene();
    }

    private void ChangeBGMByScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        Debug.Log("現在のシーン: " + sceneName);

        if (sceneName == "TitleScene")
        {
            SwitchBGM(false);
        }
        else if (sceneName == "GameScene")
        {
            SwitchBGM(true);
        }
    }

    public void PlaySE(string soundName)
    {
        if (audioSource == null)
        {
            Debug.LogError("AudioSourceが設定されてない");
            return;
        }

        if (soundDictionary.TryGetValue(soundName, out AudioClip clip))
        {
            audioSource.PlayOneShot(clip);
            Debug.Log(soundName + " を再生");
        }
        else
        {
            Debug.LogWarning(soundName + " は登録されていません");
        }
    }

    public void PlayBGM(AudioClip bgmClip)
    {
        if (bgmSource == null)
        {
            Debug.LogError("AudioSourceが設定されてない");
            return;
        }

        if (bgmSource.clip == bgmClip && bgmSource.isPlaying)
        {
            return;
        }

        bgmSource.clip = bgmClip;
        bgmSource.Play();
        currentBGM = bgmClip;
        Debug.Log(bgmClip.name + " を再生");
    }

    public void SwitchBGM(bool isGameScene)
    {
        if (isGameScene)
        {
            PlayBGM(gameBGM);
        }
        else
        {
            PlayBGM(startBGM);
        }
    }

    public void StopBGM()
    {
        if (bgmSource == null)
        {
            Debug.LogError("AudioSourceが設定されてない");
            return;
        }

        bgmSource.Stop();
        Debug.Log("BGM を停止");
    }
}
