﻿using System;
using UnityEngine;
using Pathfinding;

namespace Collectif.BeatEmUp
{
    public class EnemyController : EntityController
    {
        [Header("Enemy Parameters")]
        public PlayerController _Target;
        public float _NextWaypointDistance = 3.0f;

        private Transform playerTransform;
        private Path path;
        private int currentWaypoint = 0;
        private bool reachedEnd = false;
        private Seeker seeker;

        protected override void Start()
        {
            base.Start();
            seeker = GetComponent<Seeker>();
            playerTransform = _Target.transform;

            InvokeRepeating(nameof(CreatePath), 0, 0.5f);
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

        private void Update()
        {
            UpdateSprite();
        }

        protected void FixedUpdate()
        {
            if (path == null)  return;
            if (currentWaypoint >= path.vectorPath.Count)
            {
                reachedEnd = true;
                return;
            }

            reachedEnd = false;

            Vector2 direction = ((Vector2) path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 force = direction * (_RunSpeed * 100);
            
            rb.AddForce((force * 20) * Time.deltaTime);

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
            if (distance < _NextWaypointDistance) currentWaypoint++;
        }

        private Vector2 SetDestination()
        {
            return playerTransform.position.x <= rb.position.x ? _Target._RightTransform.position : _Target._LeftTransform.position;
        }
    }
}