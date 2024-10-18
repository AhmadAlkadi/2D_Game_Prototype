/***************************************************************
*file: Turret_Targeting.cs
*author: Sean Butler
*author: Ahmad Alkadi
*class: CS 4700 � Game Development
*assignment: program 3
*date last modified: 10/6/2024
*
*purpose: Return the player after winning to the main menu
*
*References:
*https://docs.unity3d.com/ScriptReference/index.html
*
****************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMainFromWin : MonoBehaviour
{
    public void ReturnToMainScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 3, LoadSceneMode.Single);
    }
}
