using System.Collections.Generic;
using UnityEngine;

namespace Public.Scripts.Beta.Projectiles
{
    public class MissileControllerBeta : BulletControllerBeta
    {
        private float _splashRadius;

        public float splashRadius
        {
            get => _splashRadius;
            set
            {
                _splashRadius = value;
                splashRange.range = _splashRadius;
            }
        }

        public RangeHitboxControllerBeta splashRange;
        public float retargetDistance;

        [HideInInspector]
        public GameObject target;
        
        public override void Start()
        {
            base.Start();
            splashRange.range = splashRadius;
        }

        public override void Update()
        {
            base.Update();
        }

        protected override void Die()
        {
            Explode();
            base.Die();
        }

        protected override void Move()
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            
            if (target != null)
            {
                // transform.LookAt(target.transform);

                var dir = (target.transform.position - transform.position).normalized;
                var rot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, 1);
            }
            else
            {
                target = FindClosestValidEnemy();
            }
        }

        private GameObject FindClosestValidEnemy()
        {
            var enemies = GameObject.FindGameObjectsWithTag("Enemy");
            float closestDistance = float.PositiveInfinity;
            GameObject closestEnemy = null;
            
            foreach (var enemy in enemies)
            {
                float dist = Vector3.Distance(transform.position, enemy.transform.position);

                if (dist <= retargetDistance && dist < closestDistance)
                {
                    closestDistance = dist;
                    closestEnemy = enemy;
                }
            }

            return closestEnemy;
        }

        public override void Hit(GameObject other)
        {
            if (canHit.Contains(other.tag))
            {
                Die();
            }
        }

        private void Explode()
        {
            foreach (GameObject o in splashRange.tracked)
            {
                if (canHit.Contains(o.tag))
                {
                    var ccb = o.gameObject.GetComponent<CharacterControllerBeta>();
                    if (ccb != null)
                    {
                        float distanceModifier = (splashRadius - Vector3.Distance(o.transform.position, transform.position)) / splashRadius;
                        Debug.Log(distanceModifier);
                        distanceModifier = Mathf.Clamp(distanceModifier, 0f, 1f);

                        ccb.TakeDamage(damage * distanceModifier);
                        ccb.KnockBack(transform.position, knockback * distanceModifier);
                    }
                }
            }
        }
    }
}