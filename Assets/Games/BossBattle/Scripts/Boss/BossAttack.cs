using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BossBattle
{
    public class BossAttack : MonoBehaviour
    {
        #region Variables

        [Header("References")]
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _headSpawn;
        [SerializeField] private Transform _groundSpawn;

        [Header("Multiple phases")]
        [SerializeField] private GameObject _shockWaveGameObject;
        [SerializeField] private GameObject _homingGameObject;
        
        [Header("Phase 1")]
        [SerializeField] private BossObject _piercingGameObject_p1;
        [SerializeField] private BossObject _hColumnGameObject_p1;
        [SerializeField] private BossObject _vColumnGameObject_p1;
        
        [Header("Phase 2")]
        [SerializeField] private BossObject _piercingGameObject_p2;
        [SerializeField] private BossObject _hColumnGameObject_p2;
        [SerializeField] private BossObject _vColumnGameObject_p2;
        
        [Header("Phase 3")]
        [SerializeField] private GameObject _swarmGameObject_p3;
        [SerializeField] private GameObject _clawWaveGameObject_p3;
        [SerializeField] private BossSpecialAttack _rageSpecialAttack_p3;
        [SerializeField] private BossSpecialAttack _stompSpecialAttack_p3;

        private PlayerController _player;
        private HealthSystem _playerHealth;
        private BossAI _bossAI;

        private GameObject _projectileToSpawn;
        private string _attackId;
        private bool _canAttack;
        private bool _initialized;
        private int _projectilesFired;
        private int _projectilesFiredKilled;

        private List<Action> _phase1Attack;
        private List<Action> _phase2Attack;
        private List<Action> _phase3Attack;
        
        private static readonly int IsAttacking = Animator.StringToHash("isAttacking");
        private static readonly int Atk_Rage_Param = Animator.StringToHash("Atk_Rage");
        private static readonly int Atk_Stomp_Param = Animator.StringToHash("Atk_Stomp");
        private static readonly int Atk_Swarm_Param = Animator.StringToHash("Atk_Swarm");
        private static readonly int Atk_Claws_Param = Animator.StringToHash("Atk_Claws");
        private static readonly int Atk_Piercing_Param = Animator.StringToHash("Atk_Piercing");
        private static readonly int Atk_Column_H_Param = Animator.StringToHash("Atk_Column_H");
        private static readonly int Atk_Column_V_Param = Animator.StringToHash("Atk_Column_V");
        private static readonly int Atk_Claws_More_Param = Animator.StringToHash("Atk_Claws_More");
        private static readonly int Atk_Shock_Wave_Param = Animator.StringToHash("Atk_Shock_Wave");
        private static readonly int Atk_Homing_Missile_Param = Animator.StringToHash("Atk_Homing_Missile");
        private static readonly int Atk_Homing_Missile_Multiple_Param = Animator.StringToHash("Atk_Homing_Missile_Multiple");
        
        #endregion

        #region Public Functions

        public void Attack() => _canAttack = true;

        public void GetRandomAttack(BattlePhase _currentPhase)
        {
            //P1
            //Attack_Shock_Wave(); Validé
            //Attack_Piercing_P1(); Validé
            //Attack_Column_V_P1(); Validé
            //Attack_Column_H_P1(); Validé
            
            //P2
            //Attack_Shock_Wave(); Validé
            //Attack_Piercing_P2(); Validé
            //Attack_Column_V_P2(); Validé
            //Attack_Column_H_P2(); Validé
            //Attack_Homing_Projectile(); Validé

            //P3
            //Attack_Swarm(); Validé
            //Attack_Homing_Projectile_Multiple(); Validé
            //Attack_Claw_Combo(); Validé
            //Attack_Rage(); Validé
            //Attack_Stomp(); Validé
            //Attack_Stomp(); Validé

            switch (_currentPhase)
            {
                case BattlePhase.PHASE_1:
                    if (_phase1Attack == null || _phase1Attack.Count == 0) return;
                    _phase1Attack[Random.Range(0, _phase1Attack.Count)].Invoke();
                    break;

                case BattlePhase.PHASE_2:
                    if (_phase2Attack == null || _phase2Attack.Count == 0) return;
                    _phase2Attack[Random.Range(0, _phase2Attack.Count)].Invoke();
                    break;

                case BattlePhase.PHASE_3:
                    if (_phase3Attack == null || _phase3Attack.Count == 0) return;
                    _phase3Attack[Random.Range(0, _phase3Attack.Count)].Invoke();
                    break;
            }
        }

        #endregion

        private void OnEnable()
        {
            BossProjectile.OnProjectileDeath += OnProjectileDead;
            
            _piercingGameObject_p1.OnTriggerEnd += OnObjectFinished;
            _piercingGameObject_p2.OnTriggerEnd += OnObjectFinished;
            
            _hColumnGameObject_p1.OnTriggerEnd += OnObjectFinished;
            _hColumnGameObject_p2.OnTriggerEnd += OnObjectFinished;
            
            _vColumnGameObject_p1.OnTriggerEnd += OnObjectFinished;
            _vColumnGameObject_p2.OnTriggerEnd += OnObjectFinished;

            _rageSpecialAttack_p3.OnTriggerEnd += OnSpecialAttackFinished;
            _stompSpecialAttack_p3.OnTriggerEnd += OnSpecialAttackFinished;
        }
        private void OnDisable()
        {
            BossProjectile.OnProjectileDeath -= OnProjectileDead;
            
            _piercingGameObject_p1.OnTriggerEnd -= OnObjectFinished;
            _piercingGameObject_p2.OnTriggerEnd -= OnObjectFinished;
            
            _hColumnGameObject_p1.OnTriggerEnd -= OnObjectFinished;
            _hColumnGameObject_p2.OnTriggerEnd -= OnObjectFinished;
            
            _vColumnGameObject_p1.OnTriggerEnd -= OnObjectFinished;
            _vColumnGameObject_p2.OnTriggerEnd -= OnObjectFinished;
            
            _rageSpecialAttack_p3.OnTriggerEnd -= OnSpecialAttackFinished;
            _stompSpecialAttack_p3.OnTriggerEnd -= OnSpecialAttackFinished;
        }
        private void Start()
        {
            _bossAI = GetComponent<BossAI>();
            _player = FindAnyObjectByType<PlayerController>();
            _playerHealth = _player.GetComponent<HealthSystem>();
            
            _phase1Attack = new List<Action>();
            _phase2Attack = new List<Action>();
            _phase3Attack = new List<Action>();
            
            _phase1Attack.Add(Attack_Piercing_P1);
            _phase1Attack.Add(Attack_Column_H_P1);
            _phase1Attack.Add(Attack_Column_V_P1);
            _phase1Attack.Add(Attack_Shock_Wave);
            
            _phase2Attack.Add(Attack_Piercing_P2);
            _phase2Attack.Add(Attack_Column_H_P2);
            _phase2Attack.Add(Attack_Column_V_P2);
            _phase2Attack.Add(Attack_Shock_Wave);
            _phase2Attack.Add(Attack_Homing_Projectile);
            
            _phase3Attack.Add(Attack_Homing_Projectile_Multiple);
            _phase3Attack.Add(Attack_Stomp);
            _phase3Attack.Add(Attack_Rage);
            _phase3Attack.Add(Attack_Claw_Combo);
            _phase3Attack.Add(Attack_Swarm);
        }

        #region Callbacks
        
        private void OnProjectileDead(BossProjectile bossProjectile)
        {
            _projectilesFiredKilled++;
            if (_projectilesFiredKilled >= _projectilesFired)
            {
                _projectilesFired = _projectilesFiredKilled = 0;
                AttackFinished();
            }
        }
        private void OnObjectFinished()
        {
            AttackFinished();
        }
        private void OnSpecialAttackFinished()
        {
            Attack();
        }
        private void AttackFinished()
        {
            _animator.SetBool(Atk_Rage_Param, false);
            _animator.SetBool(Atk_Swarm_Param, false);
            _animator.SetBool(Atk_Claws_Param, false);
            _animator.SetBool(Atk_Stomp_Param, false);
            _animator.SetBool(Atk_Piercing_Param, false);
            _animator.SetBool(Atk_Column_H_Param, false);
            _animator.SetBool(Atk_Column_V_Param, false);
            _animator.SetBool(Atk_Shock_Wave_Param, false);
            _animator.SetBool(Atk_Shock_Wave_Param, false);
            _animator.SetBool(Atk_Homing_Missile_Param, false);
            _animator.SetBool(Atk_Homing_Missile_Multiple_Param, false);
            
            _animator.SetBool(IsAttacking, false);
            
            _bossAI.LastAttackFinished();
        }

        #endregion

        #region Attack Handling

        private void InitializeAtttack(int id)
        {
            if (_initialized) return;
            
            // Préparation de l'attaque
            _animator.SetBool(id, true);
            _initialized = true;
        }
        private void AttackWithBossObject(BossObject bossObject)
        {
            bossObject.Attack();
            _initialized = false;
        }
        private void InstantiateProjectile(GameObject projectile, Transform spawnPoint)
        {
            _projectilesFired = 1;

            // Instantiate removed for SetActive
            BossProjectile _spawned = Instantiate(projectile, spawnPoint.position, Quaternion.identity).GetComponent<BossProjectile>();
            _spawned.Initialize(_player);
            _initialized = false;
        }
        private void InstantiateProjectileMultiple(GameObject projectile, Transform spawnPoint, int amount = 1)
        {
            _projectilesFired = amount;

            // Instantiate removed for SetActive
            BossProjectile _spawned = Instantiate(projectile, spawnPoint.position, Quaternion.identity).GetComponent<BossProjectile>();
            _spawned.Initialize(_player, true, this);
            _initialized = false;
        }
        private void AttackWithSpecialAttack(BossSpecialAttack specialAttackObject)
        {
            specialAttackObject.Attack();
            _initialized = false;
            _canAttack = false;
        }

        #endregion

        #region Objects & Projectiles
        
        private bool CanAttack()
        {
            if (!_canAttack) return false;
            _canAttack = false;
            return true;
        }
        
        // COROUTINES
        private IEnumerator Attack_With_Object(int animId, BossObject bossObject)
        {
            InitializeAtttack(animId);
            yield return new WaitUntil(CanAttack);
            AttackWithBossObject(bossObject);
        }
        private IEnumerator Attack_With_Projectile(int animId, GameObject projectile, Transform spawnPoint, int amount = 1)
        {
            InitializeAtttack(animId);
            yield return new WaitUntil(CanAttack);
            InstantiateProjectile(projectile, spawnPoint);
        }
        private IEnumerator Attack_With_Multiple_Projectile(int animId, GameObject projectiles, Transform spawnPoint, int amount = 1, int animationHandle = 0)
        {
            InitializeAtttack(animId);
            yield return new WaitUntil(CanAttack);

            _projectilesFired = amount;
            for (int i = 0; i < amount; i++)
            {
                if (animationHandle != 0) _animator.SetTrigger(animationHandle);
                InstantiateProjectileMultiple(projectiles, spawnPoint, 0);
                yield return new WaitUntil(CanAttack);
            }
        }
        private IEnumerator Attack_With_Special_Attack(int animId, BossSpecialAttack specialAttack)
        {
            InitializeAtttack(animId);
            yield return new WaitUntil(CanAttack);
            AttackWithSpecialAttack(specialAttack);
            yield return new WaitUntil(CanAttack);
            _animator.SetBool(animId, false);
        }

        // SPAWN OBJET
        private void Attack_Piercing_P1() => StartCoroutine(Attack_With_Object(Atk_Piercing_Param, _piercingGameObject_p1));
        private void Attack_Column_H_P1() => StartCoroutine(Attack_With_Object(Atk_Column_H_Param, _hColumnGameObject_p1));
        private void Attack_Column_V_P1() => StartCoroutine(Attack_With_Object(Atk_Column_V_Param, _vColumnGameObject_p1));
        
        private void Attack_Piercing_P2() => StartCoroutine(Attack_With_Object(Atk_Piercing_Param, _piercingGameObject_p2));
        private void Attack_Column_H_P2() => StartCoroutine(Attack_With_Object(Atk_Column_H_Param, _hColumnGameObject_p2));
        private void Attack_Column_V_P2() => StartCoroutine(Attack_With_Object(Atk_Column_V_Param, _vColumnGameObject_p2));
        
        // SPAWN PROJECTILE
        private void Attack_Shock_Wave() => StartCoroutine(Attack_With_Projectile(Atk_Shock_Wave_Param, _shockWaveGameObject, _groundSpawn));
        private void Attack_Homing_Projectile() => StartCoroutine(Attack_With_Projectile(Atk_Homing_Missile_Param, _homingGameObject, _headSpawn));
        private void Attack_Swarm() => StartCoroutine(Attack_With_Projectile(Atk_Swarm_Param, _swarmGameObject_p3, _headSpawn));
        
        // SPAWN MULTIPLE PROJECTILE
        private void Attack_Homing_Projectile_Multiple() => StartCoroutine(Attack_With_Multiple_Projectile(Atk_Homing_Missile_Multiple_Param, _homingGameObject, _headSpawn, Random.Range(1, 4)));
        private void Attack_Claw_Combo() => StartCoroutine(Attack_With_Multiple_Projectile(Atk_Claws_Param, _clawWaveGameObject_p3, _groundSpawn, 3, Atk_Claws_More_Param)); //Random.Range(0, 4)

        // ATTAQUE SPECIALE
        private void Attack_Rage() => StartCoroutine(Attack_With_Special_Attack(Atk_Rage_Param, _rageSpecialAttack_p3));
        private void Attack_Stomp() => StartCoroutine(Attack_With_Special_Attack(Atk_Stomp_Param, _stompSpecialAttack_p3));

        #endregion

    }
}