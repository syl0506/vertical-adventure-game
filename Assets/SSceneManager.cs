using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SSceneManager:MonoBehaviour
{
    public static SSceneManager Instance { get; private set; }

    private int _prevStageIndex;
    public string[] SceneNames; //0 index: start scene 1: over scene 2 ,3,4 stage scene

    private void Awake()
    {
        Instance = this;
        _prevStageIndex = 0;
        DontDestroyOnLoad(this.gameObject);
    }

    public void GoToLevel(int index)
    {
        GoTo(index + 2);
    }

    public void GoToNextLevel()
    {
        if (++_prevStageIndex >= SceneNames.Length)
            GoToGameOverMenu();
        else
            GoTo(_prevStageIndex);
    }

    public void GoToStartMenu()
    {
        GoTo(0);
    }

    public void GoToGameOverMenu()
    {
        GoTo(1);
    }

    public void GoToPrevious()
    {
        GoTo(_prevStageIndex);
    }

    private void GoTo(int index)
    {
        if (SceneNames.Length <= index)
            return;

        SceneManager.LoadScene(SceneNames[index]);
        _prevStageIndex = index;
    }

}

