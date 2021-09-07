using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Transform playerTR;

    [Header("Enemy Create Info")]
    public Transform[] spawnPoints;
    public GameObject enemyPrefab;
    public float createTime = 5f;
    public int maxEnemy = 5;
    public bool isGameOver = false;

    private int enemyCount = 0;
    private List<EnemyHealth> enemyList = new List<EnemyHealth>(); //풀매니징
    private WaitForSeconds wsSpawn; //스폰 코루틴을 위한 코드

    void Awake()
    {
        if(instance != null){
            Debug.LogError("다수의 게임매니저가 실행중입니다.");
        }
        instance = this;
        
        for(int i = 0; i < maxEnemy + 1; i++)
        {
            GameObject e = CreateEnemy();
            EnemyHealth eh = e.GetComponent<EnemyHealth>();
            e.SetActive(false);
            enemyList.Add(eh);
        }
        wsSpawn = new WaitForSeconds(createTime);
    }

    private GameObject CreateEnemy()
    {
        return Instantiate(
            enemyPrefab,
            transform.position,
            Quaternion.identity,
            transform);
    }

    private void Start()
    {
        spawnPoints = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        StartCoroutine(SpawnEnemy());

        playerTR.GetComponent<PlayerHealth>().OnDeath += SetGameOver;
    }

    private void SetGameOver()
    {
        isGameOver = true;
        playerTR.GetComponent<PlayerHealth>().OnDeath -= SetGameOver;
        enemyList
            .FindAll(x => x.gameObject.activeSelf)
            .ForEach(x => x.Die());
        //for(int i = 0; i < enemyList.Count; i++)
        //{
        //    if (enemyList[i].gameObject.activeSelf)
        //    {
        //        enemyList[i].Die();
        //    }
        //}
    }

    IEnumerator SpawnEnemy()
    {
        while(!isGameOver)
        {
            if(enemyCount < maxEnemy)
            {
                //어디에 생성시킬 것인가를 결정
                int idx = UnityEngine.Random.Range(1, spawnPoints.Length);
                //부모는 0번째에 들어가 있으니 1번부터 랜덤하게 가져오면 된다.

                EnemyHealth eh = enemyList.Find(x => !x.gameObject.activeSelf);
                if(eh == null)
                {
                    GameObject e = CreateEnemy();
                    eh = e.GetComponent<EnemyHealth>();
                    enemyList.Add(eh);
                }
                eh.transform.position = spawnPoints[idx].position;
                eh.gameObject.SetActive(true);
                enemyCount++; //적의 갯수를 증가시킨다.

                Action handler = null;
                handler = () =>
                {
                    enemyCount--;
                    eh.OnDeath -= handler;
                };

                eh.OnDeath += handler;
            }
            yield return wsSpawn;
        }
    }

    

    public static Transform GetPlayer()
    {
        return instance.playerTR;
    }
}
