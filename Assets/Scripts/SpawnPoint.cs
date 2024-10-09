using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public GameObject cameraLeftColid;
    public EntityMovement enemy;
    public int numberOfEnemy = 1;
    public float delay = 1f;
    private List<EntityMovement> listOfEnemy;
    private float frameTime;

    private void Start()
    {
        listOfEnemy = new List<EntityMovement>();
        for(int i = 0; i< numberOfEnemy; i++)
        {
            EntityMovement newEnemy = Instantiate(enemy);
            newEnemy.setCameraColid(ref cameraLeftColid);
            //var newEnemyRidgid = newEnemy.GetComponent<Rigidbody2D>();
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
            enemy.setStartPosition(this.transform.position);
        }
        if (enemyIndex > -1 && frameTime > delay)
        {
            var currentEnemy = listOfEnemy[enemyIndex];
            currentEnemy.gameObject.SetActive(true);
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
