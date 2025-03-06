using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SoundManager : MonoBehaviour
{
    /*
     ・ＳＥ
	・ゲームオーバー
	・ゲームクリア
	・襲い掛かるときのSE
	・動くSE
	・フェーズの切り替えのSE(朝には鶏の鳴き声、夜には虫の鳴き声)
	・ボタンタップのSE
	・巣を作った時の音、壊したときの音
	・食べ物食べたときの音
    */
    public AudioSource audioSource;
    public AudioSource bgmSource;

    //辞書作成
    private Dictionary<string, AudioClip> soundDictonary;

    //辞書に登録する名前の管理
    //SE
    public AudioClip click;　　//クリック、タップ音
    public AudioClip pMove;　　//プレイヤーが動く音
    public AudioClip crtNest;　//巣の作成
    public AudioClip brkNest;　//巣の破壊
    public AudioClip morning;　//朝のフェーズ
    public AudioClip night;　　//夜のフェーズ
    public AudioClip eAtk;     //エネミーが襲い掛かるときのSE
    public AudioClip eat;      //食べる
    public AudioClip clear;    //クリア
    public AudioClip gameover; //ゲームオーバ

    //BGM
    public AudioClip startBGM; //スタート画面のBGM
    public AudioClip gameBGM;  //ゲーム画面のBGM

    public　static SoundManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            bgmSource = gameObject.AddComponent<AudioSource>();//BGM用のAudioSourceの追加
            bgmSource.loop = true;
        }
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);//シーン偏移しても再生を続ける

            //辞書に登録したKeyのみの取得
            soundDictonary = new Dictionary<string, AudioClip>
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
                {"gameover", gameover },
            };
        }
        else
        {
            Destroy(gameObject);//再生が重複しないように
        }

    }

    public void PlaySE(string soundName, float volume = 1.0f)
    {
        //使い方:SoundManger.Instance.PlaySE("命名", 0.5f); 50% の音量で再生
        if (audioSource == null)
        {
            Debug.LogError("AudioSourceが設定されてない");
            return;
        }
        //辞書から名前で検索して呼び出したい
        if(soundDictonary.TryGetValue(soundName, out AudioClip clip))
        {
            //PlayOneShotで他の音を止めずに再生できる
            audioSource.PlayOneShot(clip);
            Debug.LogWarning(soundName + "ながすよ");
        }
        else
        {
            //存在しない場合
            Debug.LogWarning(soundName + "設定されてない");
        }
    }
    public void PlayBGM(AudioClip bgmClip, float volume = 1.0f)
    {
        if (bgmSource == null)
        {
            Debug.LogError("AudioSorceが設定されてない");
            return;
        }

        if(bgmSource.clip == bgmClip && bgmSource.isPlaying)

        //BGMのループ再生
        bgmSource.clip = bgmClip;
        bgmSource.volume = Mathf.Clamp(volume, 0f, 1f);
        bgmSource.Play();
        Debug.Log(bgmClip.name + "ながれてるよ");
    }

    public void SwitchBGM(bool isGameScene)
    {
        if(isGameScene)
        {
            PlayBGM(gameBGM, 1.0f);//ゲームBGM
        }
        else
        {
            PlayBGM(startBGM, 1.0f);//スタートBGM
        }
    }

    //BGMの停止
    public void StopBGM()
    {
        if (bgmSource == null)
        {
            Debug.LogError("AudioSourceが設定されてない");
            return; 
        }

        bgmSource.Stop();
        Debug.Log("BGMを停止");
    }
}
