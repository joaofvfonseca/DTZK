using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Game_ZombieController : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private Rigidbody[] _bones;
    private AudioSource _audioSource;

    private GameObject player;
    private bool isAttacking;
    private bool isMakingSound;
    private Game_Manager gameManager;

    [SerializeField] private AudioClip[] randomSounds;
    [SerializeField] private AudioClip[] dyingSounds;

    public enum ZombieState
    {
        Chasing,
        Attacking,
        RmvBarricades,
        Fleeing,
        Dying,
        Dead,
        Idling
    }
    public ZombieState currentState;

    // Start is called before the first frame update
    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _bones = transform.root.GetComponentsInChildren<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
        
        player = GameObject.Find("/Player");
        gameManager = GameObject.Find("/!MANAGER").GetComponent<Game_Manager>();
        isAttacking = false;
        isMakingSound = false;
        currentState = ZombieState.Chasing;

        if(Random.Range(0, gameManager.GetRoundNumber()) >= 3)
        {
            _animator.SetBool("running", true);
            _navMeshAgent.speed = 3.5f;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        StateSwitcher();
        if (currentState != ZombieState.Dying && currentState != ZombieState.Dead) StartCoroutine(PlaySound());
    }

    private void StateSwitcher()
    {
        float distance = Vector3.SqrMagnitude(player.transform.position - transform.position);

        switch (currentState)
        {
            case ZombieState.Chasing:

                FChasing();
                if (distance <= 1.4) currentState = ZombieState.Attacking;
                if (player.GetComponent<Game_PlayerHealth>().isPlayerDead()) currentState = ZombieState.Idling;
                break;

            case ZombieState.Attacking:

                FAttacking();
                if (distance > 1.4) currentState = ZombieState.Chasing;
                if (player.GetComponent<Game_PlayerHealth>().isPlayerDead()) currentState = ZombieState.Idling;
                break;

            case ZombieState.RmvBarricades:

                _animator.SetInteger("animState", (int)currentState);
                if (player.GetComponent<Game_PlayerHealth>().isPlayerDead()) currentState = ZombieState.Idling;
                break;

            case ZombieState.Fleeing:

                _animator.SetInteger("animState", (int)currentState);
                if (player.GetComponent<Game_PlayerHealth>().isPlayerDead()) currentState = ZombieState.Idling;
                break;

            case ZombieState.Dying:

                FDying();
                currentState = ZombieState.Dead;
                break;

            default:
                break;
        }
    }

    private void FChasing()
    {
        _animator.SetInteger("animState", (int)currentState);
        _navMeshAgent.SetDestination(player.transform.position);
    }

    private void FAttacking()
    {
        _animator.SetInteger("animState", (int)currentState);
        StartCoroutine(Attack(1.4f));
    }

    private void FDying()
    {
        _animator.enabled = false;
        _navMeshAgent.enabled = false;

        foreach (Rigidbody bone in _bones) bone.isKinematic = false;
        Destroy(gameObject, 7);

        gameManager.AddToScoreQueue(Class_Score.ScoreID.kill, 80, "Killed a zombie");

        PlayDeathSound();

        gameManager.DescreaseZombiesOnMap();
    }

    private IEnumerator Attack(float param)
    {
        if (isAttacking) yield break;
        isAttacking = true;
        yield return new WaitForSeconds(param);
        isAttacking = false;
        if (currentState != ZombieState.Attacking) yield break;
        StartCoroutine(player.GetComponent<Game_PlayerHealth>().TakeDamage(50));
    }

    private IEnumerator PlaySound()
    {
        if (isMakingSound) yield break;
        _audioSource.clip = randomSounds[Random.Range(0, randomSounds.Length - 1)];
        isMakingSound = true;
        _audioSource.Play();
        yield return new WaitForSeconds(Random.Range(9, 23));
        isMakingSound = false;
    }

    private void PlayDeathSound()
    {
        _audioSource.Stop();
        _audioSource.clip = dyingSounds[Random.Range(0, dyingSounds.Length - 1)];
        _audioSource.Play();
    }
}
