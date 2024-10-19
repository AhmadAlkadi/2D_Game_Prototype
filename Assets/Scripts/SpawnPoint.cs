/***************************************************************
*file: Turret_Targeting.cs
*author: Sean Butler
*author: Ahmad Alkadi
*class: CS 4700 – Game Development
*assignment: program 3
*date last modified: 10/6/2024
*
*purpose: Enemies will spawn from this script, and when they die they this will allow them to spawn again
*
*References:
*https://docs.unity3d.com/ScriptReference/index.html
*
****************************************************************/
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public GameObject cameraLeftColid;
    public GameObject cameraRightColid;
    public GameObject edgeRightColid;
    public GameObject edgeLeftColid;
    public GameObject edgeBottomColid;
    public GameObject player;
    public float jumpHight = 15;
    public float speed = 3;
    public EntityMovement enemy;
    public int numberOfEnemy = 1;
    public float delayMin = 1.0f;
    public float delayMax = 3.0f;
    private float delay = 1.0f;
    private List<EntityMovement> listOfEnemy;
    private float frameTime;
    private float randomValue;
    [Range(0.0f,1.0f)]
    public float randomValueSet = 0.25f;

    private void Start()
    {
        listOfEnemy = new List<EntityMovement>();
        for(int i = 0; i< numberOfEnemy; i++)
        {
            EntityMovement newEnemy = Instantiate(enemy);
            newEnemy.setPlayer(ref player);
            newEnemy.setJumpHight(ref jumpHight);
            newEnemy.setSpeed(ref speed);
            newEnemy.setCameraLeftColid(ref cameraLeftColid);
            newEnemy.setCameraRightColid(ref cameraRightColid);
            newEnemy.setEdgeRightColid(ref edgeRightColid);
            newEnemy.setEdgeLeftColid(ref edgeLeftColid);
            newEnemy.setEdgeBottomColid(ref edgeBottomColid);
            newEnemy.gameObject.SetActive(false);
            listOfEnemy.Add(newEnemy);
        }
    }

    private void Update()
    {
        frameTime += Time.deltaTime;
        int enemyIndex = FindEnemy();
        foreach (EntityMovement enemy in listOfEnemy)
        {
            enemy.setStartPosition(transform.position);
        }
        if (enemyIndex > -1 && frameTime > delay)
        {
            delay = (Random.value % (delayMax - delayMin)) + delayMin;

            var currentEnemy = listOfEnemy[enemyIndex];
            currentEnemy.gameObject.SetActive(true);
            randomValue = Random.value;
            if (randomValue < randomValueSet)
            {
                currentEnemy.setEdgeWalkBack(1);
            }
            frameTime = 0;
        }

    }

    private int FindEnemy()
    {
        for (int i = 0; i < listOfEnemy.Count; i++)
        {
            if (!listOfEnemy[i].gameObject.activeInHierarchy)
            {
                return i;
            }
        }

        return -1;
    }

}
