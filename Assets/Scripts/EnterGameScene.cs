/***************************************************************
*file: Turret_Targeting.cs
*author: Sean Butler
*author: Ahmad Alkadi
*class: CS 4700 – Game Development
*assignment: program 3
*date last modified: 10/6/2024
*
*purpose: Stage one will start
*
*References:
*https://docs.unity3d.com/ScriptReference/index.html
*
****************************************************************/
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterGameScene : MonoBehaviour
{

    public void EnterNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
    }

}
