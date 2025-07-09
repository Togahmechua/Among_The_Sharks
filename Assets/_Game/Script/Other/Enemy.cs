using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    public enum FishType
    {
        PatrolFish,
        HunterShark,
        PathFish
    }

    [Header("Fish Type")]
    public FishType fishType;

    [Header("Common Settings")]
    public Transform player;
    public float normalSpeed = 10f;
    public float chaseSpeed = 15f;

    [Header("Patrol / Path Settings")]
    public List<Transform> targetPoints;

    [SerializeField] private Animator anim;

    private NavMeshAgent agent;
    private int pathIndex = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (fishType == FishType.HunterShark)
        {
            agent.speed = chaseSpeed;
        }
        else
        {
            agent.speed = normalSpeed;

            if (targetPoints.Count > 0)
            {
                pathIndex = 0;
                agent.SetDestination(targetPoints[pathIndex].position);
            }
        }
    }

    void Update()
    {
        switch (fishType)
        {
            case FishType.HunterShark:
                agent.SetDestination(player.position);
                break;

            case FishType.PatrolFish:
            case FishType.PathFish:
                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                {
                    pathIndex = (pathIndex + 1) % targetPoints.Count;
                    agent.SetDestination(targetPoints[pathIndex].position);
                }
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("💀 Player KILLED by: " + fishType.ToString());
            anim.Play("attack");
            other.gameObject.SetActive(false);
            // TODO: GameOver logic here

            StartCoroutine(IEWait());
        }
    }

    private IEnumerator IEWait()
    {
        UIManager.Ins.CloseUI<MainCanvas>();
        yield return new WaitForSeconds(1f);
        UIManager.Ins.OpenUI<LooseCanvas>();
    }
}
