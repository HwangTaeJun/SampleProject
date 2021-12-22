using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CombatState
{
    Idle,
    Run,
    Attack,
    Death,
}

public class CombatManager : MonoBehaviour
{
    [SerializeField] private GameObject playerObj = null;
    [SerializeField] private GameObject monsterObj = null;

    private List<Agent> playerList = null;
    private List<Agent> monsterList = null;

    private int monsterCount = 5;

    private void Start()
    {
        monsterObj.SetActive(false);

        playerList = new List<Agent>();
        monsterList = new List<Agent>(monsterCount);

        CreatePlayer();
        SpawnPlayer();

        CreateMonster();
        SpawnMonster();
        StartCombat();
    }

    private void CreatePlayer()
    {
        GameObject newPlayer = Instantiate(playerObj);
        playerList.Add(newPlayer.GetComponent<Agent>());
    }

    private void CreateMonster()
    {
        for (int i = 0; i < monsterCount; i++)
        {
            GameObject newMonster = Instantiate(monsterObj);
            monsterList.Add(newMonster.GetComponent<Agent>());
        }
    }

    private void SpawnPlayer()
    {
        playerList[0].Setup(monsterList, new Vector3(0,0,0), new Ability(1.5f,3.0f,1000,1));
    }

    private void SpawnMonster()
    {
        for (int i = 0; i < monsterCount; i++)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-10.0f, 10.0f), 0, Random.Range(10f, 10.0f));
            monsterList[i].Setup(playerList, spawnPos, new Ability(1.5f, 2.0f, 2, 0));
        }
    }

    private void StartCombat()
    {
        for (int i = 0; i < monsterCount; i++)
        {
            monsterList[i].StartCombat();
        }

        for (int i = 0; i < playerList.Count; i++)
        {
            playerList[i].StartCombat();
        }
    }
}
