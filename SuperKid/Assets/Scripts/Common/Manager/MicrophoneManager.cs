using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using NAudio.Lame;
using NAudio.Wave.WZT;
using QFramework;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;

public enum RecordStatus
{
    NoRecord,
    Recording,
    Pause,
    RecordComplete
}
public enum PlayStatus
{
    NoPlaying,
    Playing,
    Pause,
    Stop
}

public class MicrophoneManager : MonoBehaviour
{
    private static MicrophoneManager mInstance;

    private static string[] micArray = null; //录音设备列表
    private AudioClip mAudioClip;
    private AudioClip mTempAudioClip;
    private AudioSource mAudioSource;
    private List<AudioClip> mAudioClipList;
    public float mRecordTime = 0f;
    public IntReactiveProperty mPlayingTime = new IntReactiveProperty();
    public IntReactiveProperty mTotleTime = new IntReactiveProperty();
    
    public BoolReactiveProperty mIsPlaying = new BoolReactiveProperty(false);
    public ReactiveProperty<PlayStatus> mPlayStatus = new ReactiveProperty<PlayStatus>(PlayStatus.NoPlaying);

    public ReactiveProperty<RecordStatus> mRecordStatus =
        new ReactiveProperty<RecordStatus>(global::RecordStatus.NoRecord);

    public const int RECORD_TIME = 301; // 比规定时间多1秒，避免录音资源提前释放
    const int RECORD_RATE = 44100; //录音采样率
    public float recordTime;
    public const int MIN_TIME = 2;

    public static MicrophoneManager GetInstance()
    {
        if (mInstance == null)
        {
            micArray = Microphone.devices;
            if (micArray.Length == 0)
            {
                Debug.LogError("no mic device");
            }

            foreach (string deviceStr in Microphone.devices)
            {
                Debug.Log("device name = " + deviceStr);
            }

            GameObject micManager = new GameObject("MicManager");
            mInstance = micManager.AddComponent<MicrophoneManager>();
            mInstance.mAudioSource = micManager.AddComponent<AudioSource>();
            mInstance.mAudioClipList = new List<AudioClip>();
        }

        return mInstance;
    }

    /// <summary>
    /// 开始录音
    /// </summary>
    public void StartRecord()
    {
        if (micArray.Length == 0)
        {
            Debug.Log("No Record Device!");
            return;
        }
        Microphone.End(null); //录音时先停掉录音，录音参数为null时采用默认的录音驱动
        Resources.UnloadUnusedAssets();
        mTempAudioClip = Microphone.Start(null, false, RECORD_TIME, RECORD_RATE);
        mRecordStatus.Value = RecordStatus.Recording;
    }

    public void PauseRecord()
    {
        StopRecording();
        mRecordStatus.Value = RecordStatus.Pause;
    }

    /// <summary>
    /// 停止录音
    /// </summary>
    public void StopRecord()
    {
        StopRecording();
        mRecordStatus.Value = RecordStatus.RecordComplete;
    }

    private void StopRecording()
    {
        if (micArray.Length == 0)
        {
            Debug.Log("No Record Device!");
            return;
        }

        if (!Microphone.IsRecording(null))
        {
            return;
        }

        int position = Microphone.GetPosition(null);
        Microphone.End(null);
        Resources.UnloadUnusedAssets();
        Log.I("mTempAudioClip.samples=="+mTempAudioClip.samples );
        Log.I("mTempAudioClip.channels=="+mTempAudioClip.channels );
        Log.I("mTempAudioClip.samples * mTempAudioClip.channels=="+mTempAudioClip.samples * mTempAudioClip.channels );
        var soundData = new float[mTempAudioClip.samples * mTempAudioClip.channels];
        mTempAudioClip.GetData(soundData, 0);

        //Create shortened array for the data that was used for recording
        var newData = new float[position * mTempAudioClip.channels];


        //Copy the used samples to a new array
        for (int i = 0; i < newData.Length; i++)
        {
            newData[i] = soundData[i];
        }

        //One does not simply shorten an AudioClip,
        //so we make a new one with the appropriate length
        if (position > 0)
        {
            mTempAudioClip = AudioClip.Create(mTempAudioClip.name,
                position,
                mTempAudioClip.channels,
                mTempAudioClip.frequency,
                false);
            mTempAudioClip.SetData(newData, 0); //Give it the data from the old clip
            soundData = null;
            newData = null;
            GC.Collect();
            mAudioClipList.Add(mTempAudioClip);
            mAudioClip = Combine(mAudioClipList);
            mAudioSource.clip = mAudioClip;
        }
        
    }

    /// <summary>
    ///播放录音 
    /// </summary>
    public void PlayRecord()
    {
        PlayRecord(mAudioClip, 0);
    }

    public void PlayRecord(int time)
    {
        PlayRecord(mAudioClip, time);
    }

    public void PlayRecord(AudioClip clip, int time)
    {
        if (clip == null)
        {
            Debug.Log("audioClip is null");
            return;
        }
        mAudioSource.clip = clip;
        mAudioSource.time = time;
        mAudioSource.Play();
        mPlayStatus.Value = PlayStatus.Playing;
        Debug.Log("PlayRecord");
    }

    public void PlayRecord(AudioClip clip)
    {
        PlayRecord(clip, Vector3.zero);
    }
    public void PlayRecord(AudioClip clip, Vector3 pos)
    {
        if (clip == null)
        {
            Debug.Log("audioClip is null");
            return;
        }
        mAudioSource.clip = clip;
        mAudioSource.Play();
        mPlayStatus.Value = PlayStatus.Playing;
        Debug.Log("PlayRecord");
    }

    
    public void PlayRecord(string path, int time = 0)
    {
        if (!path.StartsWith("http://"))
        {
            path = "file://" + path;
        }

        StartCoroutine(GetAudioClip(path, time));
    }

    public float GetClipLength()
    {
        //clips[i].samples * clips[i].channels
        return mAudioSource.clip.length;
    }
    
    
    IEnumerator GetAudioClip(string path, int time = 0)
    {
        // using (var uwr = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.MPEG))
        // {
        //     yield return uwr.SendWebRequest();
        //     if (uwr.isNetworkError)
        //     {
        //         Log.E(uwr.error);
        //         yield break;
        //     }
        //
        //     AudioClip clip = DownloadHandlerAudioClip.GetContent(uwr);
        //     mAudioSource.clip = clip;
        //     mAudioSource.time = time;
        //     mAudioSource.Play();
        //     mPlayStatus.Value = PlayStatus.Playing;
        // }
        using (var uwr = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.UNKNOWN))
        {
            //不卡顿的2行代码
            ((DownloadHandlerAudioClip)uwr.downloadHandler).compressed = false;
            ((DownloadHandlerAudioClip)uwr.downloadHandler).streamAudio = true;
            yield return uwr.SendWebRequest();
            if (uwr.isNetworkError)
            {
                Log.E(uwr.error);
            }
            else
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(uwr);
                mAudioSource.clip = clip;
                mAudioSource.time = time;
                mAudioSource.Play();//播放
                mPlayStatus.Value = PlayStatus.Playing;
            }
        }
    }
    
    

    private void Update()
    {
        if (Microphone.IsRecording(null))
        {
            var position = Microphone.GetPosition(null);
            float scend = position * 1.0f / 44100f;
            var totalTime = GetTotalTime(mAudioClipList);
            recordTime += Time.deltaTime;
            mRecordTime = totalTime + scend;
        }
        else
        {
            if (mAudioClip.IsNotNull() && mAudioSource.clip.IsNotNull())
            {
                mRecordTime = mAudioClip.length;
                mTotleTime.Value = (int)mAudioSource.clip.length;
            }
        }
        
        if (mAudioSource.IsNotNull() && mAudioSource.clip.IsNotNull() && mAudioSource.isPlaying)
        {
            mPlayingTime.Value = (int)mAudioSource.time;
            mTotleTime.Value = (int)mAudioSource.clip.length;
        }

        if (mAudioSource.IsNotNull() && mAudioSource.clip.IsNotNull())
        {
            mIsPlaying.Value = mAudioSource.isPlaying;
        }
        if (mAudioSource.IsNotNull() && mAudioSource.clip.IsNotNull() && mPlayStatus.Value == PlayStatus.Playing)
        {
            if (mAudioSource.time >= mAudioSource.clip.length)
            {
                mPlayStatus.Value = PlayStatus.Stop;
            }
        }
    }

    public void Save(out string path)
    {
        if (micArray.Length == 0)
        {
            Debug.Log("No Record Device!");
            path = null;
            return;
        }
        Save(mAudioClip, out path);
        string pathdir = Path.GetDirectoryName(path);
        pathdir = pathdir + "/temp.mp3";
        WaveToMP3(path, pathdir);
        File.Delete(path);
        path = pathdir;
    }

    public void Save(AudioClip clip, out string path)
    {
        if (clip == null)
        {
            Debug.Log("clip is null,can't be saved");
            path = null;
            return;
        }

        WavUtility.FromAudioClip(clip, out path, true);
    }

    public void StopPlay()
    {
        mAudioSource.Stop();
    }

    public void PausePlay()
    {
        mPlayStatus.Value = PlayStatus.Pause;
        mAudioSource.Pause();
    }

    public static void WaveToMP3(string waveFileName, string mp3FileName, LAMEPreset bitRate = LAMEPreset.ABR_128)
    {
        using (var reader = new WaveFileReader(waveFileName))
        using (var writer = new LameMP3FileWriter(mp3FileName, reader.WaveFormat, bitRate))
            reader.CopyTo(writer);
    }

    private float GetTotalTime(List<AudioClip> clips)
    {
        if (clips == null || clips.Count == 0)
            return 0f;
        float length = 0;
        for (int i = 0; i < clips.Count; i++)
        {
            if (clips[i] == null)
                continue;

            length += clips[i].length;
        }

        return length;
    }

    public AudioClip Combine(List<AudioClip> clips)
    {
        if (clips == null || clips.Count == 0)
            return null;

        int length = 0;
        for (int i = 0; i < clips.Count; i++)
        {
            if (clips[i] == null)
                continue;

            length += clips[i].samples * clips[i].channels;
        }

        float[] data = new float[length];
        length = 0;
        for (int i = 0; i < clips.Count; i++)
        {
            if (clips[i] == null)
                continue;

            float[] buffer = new float[clips[i].samples * clips[i].channels];
            clips[i].GetData(buffer, 0);
            //System.Buffer.BlockCopy(buffer, 0, data, length, buffer.Length);
            buffer.CopyTo(data, length);
            length += buffer.Length;
        }

        if (length == 0)
            return null;

        AudioClip result = AudioClip.Create("Combine", length, mTempAudioClip.channels, mTempAudioClip.frequency, false,
            false);
        result.SetData(data, 0);

        return result;
    }

    public void Destory()
    {
        recordTime = 0;
        mAudioClipList.Clear();
        Resources.UnloadUnusedAssets();
    }
}