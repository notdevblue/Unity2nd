using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static public GameManager instance = null;
    public Transform playerTR;

    [Header("Enemy Create Info")]
    public Transform[] spawnPoints;
    public GameObject enemyPrefab;
    public float createTime = 5.0f;
    public int maxEnemy = 5;
    public bool isGameOver = false;

    private int enemyCount = 0;
    private List<EnemyHealth> enemyList = new List<EnemyHealth>();
    private WaitForSeconds wsSpawn;

    void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("다수의 게임 메니저가 실행중입니다.");
        }
        instance = this;

        for (int i = 0; i < maxEnemy + 1; ++i)
        {
            GameObject e = CreateEnemy();
            EnemyHealth eh = e.GetComponent<EnemyHealth>();
            e.SetActive(false);
            enemyList.Add(eh);
        }

        wsSpawn = new WaitForSeconds(createTime);
    }

    private void Start()
    {
        spawnPoints = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>(); // 부모 꺼도 가지고 옴
        StartCoroutine(SpawnEnemy());

        PlayerHealth ph = playerTR.GetComponent<PlayerHealth>();
        ph.OnDeath += SetGameOver;
    }

    private void SetGameOver()
    {
        isGameOver = true;
        playerTR.GetComponent<PlayerHealth>().OnDeath -= SetGameOver;
        enemyList.FindAll(x => x.gameObject.activeSelf).ForEach(x => x.Die());
    }

    private IEnumerator SpawnEnemy()
    {
        while(!isGameOver)
        {
            if(enemyCount < maxEnemy)
            {
                int idx = UnityEngine.Random.Range(1, spawnPoints.Length); // 부모는 0번째에 들어가 있음

                EnemyHealth eh = enemyList.Find(x => !x.gameObject.activeSelf);
                if(eh == null)
                {
                    GameObject e = CreateEnemy();
                    eh = e.GetComponent<EnemyHealth>();
                    enemyList.Add(eh);
                }
                eh.transform.position = spawnPoints[idx].position;
                eh.gameObject.SetActive(true);
                ++enemyCount;

                Action handler = null;
                handler = () => 
                {
                    --enemyCount;
                    eh.OnDeath -= handler;
                };

                eh.OnDeath += handler;
            }

            yield return wsSpawn;
        }
    }

    private GameObject CreateEnemy()
    {
        return Instantiate(enemyPrefab, transform.position, Quaternion.identity, transform);
    }
}
