using System;
using UnityEngine;
using Pathfinding;

namespace Collectif.BeatEmUp
{
    public class EnemyController : Entity
    {
        [Header("Enemy Parameters")]
        public Player _Target;
        public float _NextWaypointDistance = 3.0f;

        private Transform playerTransform;
        private Path path;
        private int currentWaypoint = 0;
        private bool reachedEnd = false;
        private Seeker seeker;
        private bool staggerNext;

        protected override void Start()
        {
            base.Start();
            seeker = GetComponent<Seeker>();
            playerTransform = _Target.transform;

            InvokeRepeating(nameof(CreatePath), 0, 0.5f);
        }

        private void Update()
        {
            UpdateSprite();
            
            //
        }

        protected void FixedUpdate()
        {
            MoveToPoint();
            if (reachedEnd && !isPlayerHere)
            {
                Debug.Log("No Player ! D:");
                ForceFlipSprite();
            }
        }
        
        #region Pathfinding

        private void MoveToPoint()
        {
            if (path == null)  return;
            if (currentWaypoint >= path.vectorPath.Count)
            {
                reachedEnd = true;
                return;
            }

            reachedEnd = false;

            Vector2 direction = ((Vector2) path.vectorPath[currentWaypoint] - rb.position).normalized;
            
            //Vector2 force = direction * (_RunSpeed * 100);
            //rb.AddForce((force * 20) * Time.deltaTime);
            
            rb.MovePosition(rb.position + direction * Time.fixedDeltaTime);

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
            if (distance < _NextWaypointDistance) currentWaypoint++;
        }
        
        private void CreatePath()
        {
            if (!seeker.IsDone()) return;

            var target = SetDestination();
            if (Vector2.Distance(rb.position, target) >= _NextWaypointDistance)
                seeker.StartPath(rb.position, target, OnPathCreated);
        }

        private void OnPathCreated(Path p)
        {
            if (p.error) return;

            path = p;
            currentWaypoint = 0;
        }

        private Vector2 SetDestination()
        {
            return playerTransform.position.x <= rb.position.x ? _Target._RightTransform.position : _Target._LeftTransform.position;
        }
        
        #endregion

        public void Attack()
        {
            
        }

        public void TakeDamage(bool isFinisher = false)
        {
            if (staggerNext)
            {
                //On rend le personnage invulnérable
                //On fait reculer le personnage
                //On fait "sauter" et tomber le personnage
                //On le rend incontrôlable pendant 1 seconde
                //On le relève et on lui rend le contrôle et on le rend vulnérable ensuite
                return;
            }
            
            //On fait prendre un coup au personnage
            //On le rend incontrôlable pendant l'animation
            //Si il prend plus de 2 coups durant son animation il sera ensuite mis au sol (staggerNext)
        }
    }
}