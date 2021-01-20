using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Tooltip("Use prefab")] [SerializeField] GameObject EnemyDeathFX;
    [Tooltip("Use prefab")] [SerializeField] Transform parent;
    [SerializeField] int scorePerHit = 100;
    [SerializeField] int hitPoints = 10;


    ScoreBoard scoreBoard;

    // Start is called before the first frame update
    void Start()
    {
        AddBoxCollider();
        scoreBoard = FindObjectOfType<ScoreBoard>();
    }

    private void AddBoxCollider()
    {
        Collider BoxCollider = gameObject.AddComponent<BoxCollider>();
        BoxCollider.isTrigger = false;
    }

    public void OnParticleCollision(GameObject other)
    {
        ProcessHit();
        if (hitPoints <= 1)
        {
            KillEnemy();
        }
    }

    private void ProcessHit() // do some FX
    {
        scoreBoard.ScoreHit(scorePerHit);
        hitPoints = hitPoints - 1;
    }

    private void KillEnemy()
    {
        GameObject fx = Instantiate(EnemyDeathFX, transform.position, Quaternion.identity);
        fx.transform.parent = parent;
        Destroy(gameObject);
    }
}
