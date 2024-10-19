/***************************************************************
*file: Turret_Targeting.cs
*author: Sean Butler
*author: Ahmad Alkadi
*class: CS 4700 � Game Development
*assignment: program 3
*date last modified: 10/6/2024
*
*purpose: This allows the player to try again
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


public class ContinueGame : MonoBehaviour
{
    public void EnterPrevScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1, LoadSceneMode.Single);
    }

}
