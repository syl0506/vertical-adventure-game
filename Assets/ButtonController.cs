using System;
using System.Collections.Generic;
using UnityEngine;


public class ButtonController: MonoBehaviour
{
    public void GoToFirstStage()
    {
        Debug.Log("hehe");
        SSceneManager.Instance.GoToLevel(0);
    }

    public void GoToPreviousStage()
    {
        SSceneManager.Instance.GoToPrevious();
    }
}

