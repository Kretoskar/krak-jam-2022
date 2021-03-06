using System.Collections;
using System.Collections.Generic;
using Game.Combat;
using UnityEngine;

namespace Game.Control.Enemy.Dolphin
{
    [RequireComponent(typeof(Animator))]
    public class DolphinStateAttack : MonoBehaviour, IState
    {
        [SerializeField] private Bullet bullet;
        [SerializeField] private float bulletSpawnDistance = 1;
        [SerializeField] private float jumpAttackXMove = 2;
        
        private Collider2D coll;
        private Animator animator;
        private Rigidbody2D rb;

        private void Awake()
        {
            coll = GetComponent<Collider2D>();
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
        }
        
        public void Enter(StateMachine sm)
        {
            coll.enabled = false;
            animator.SetTrigger("Attack");
        }

        public void Execute()
        {
        }

        public void Exit()
        {
            Vector3 desiredPosition = new Vector3(transform.position.x + jumpAttackXMove, transform.position.y,
                transform.position.z);
            
            if(Physics2D.OverlapCircleAll(transform.position, 2,~0).Length < 2)
                rb.MovePosition(desiredPosition);

            Shoot();
        }

        private void Shoot()
        {
            //Vertical, Horizontal
            var topBullet = Instantiate(bullet, (Vector2)transform.position + Vector2.up * bulletSpawnDistance, Quaternion.identity);
            topBullet.EnemyShoot(DestroyBullet, Vector2.up);
            var botBullet = Instantiate(bullet, (Vector2)transform.position + Vector2.down * bulletSpawnDistance, Quaternion.identity);
            botBullet.EnemyShoot(DestroyBullet, Vector2.down);
            var leftBullet = Instantiate(bullet, (Vector2)transform.position + Vector2.left * bulletSpawnDistance, Quaternion.identity);
            leftBullet.EnemyShoot(DestroyBullet, Vector2.left);
            var righBullet = Instantiate(bullet, (Vector2)transform.position + Vector2.right * bulletSpawnDistance, Quaternion.identity);
            righBullet.EnemyShoot(DestroyBullet, Vector2.right);
            
            //Diagonal
            var topRightBullet = Instantiate(bullet, (Vector2)transform.position + (Vector2.up + Vector2.right) * bulletSpawnDistance, Quaternion.identity);
            topRightBullet.EnemyShoot(DestroyBullet, Vector2.up + Vector2.right);
            var topLeftBullet = Instantiate(bullet, (Vector2)transform.position + (Vector2.up + Vector2.left) * bulletSpawnDistance, Quaternion.identity);
            topLeftBullet.EnemyShoot(DestroyBullet, Vector2.up + Vector2.left);
            var botRightBullet = Instantiate(bullet, (Vector2)transform.position + (Vector2.down + Vector2.right) * bulletSpawnDistance, Quaternion.identity);
            botRightBullet.EnemyShoot(DestroyBullet, Vector2.down + Vector2.right);
            var botLeftBullet = Instantiate(bullet, (Vector2)transform.position + (Vector2.down + Vector2.left) * bulletSpawnDistance, Quaternion.identity);
            botLeftBullet.EnemyShoot(DestroyBullet, Vector2.down + Vector2.left);
        }

        public void DestroyBullet(Bullet bullet)
        {
            Destroy(bullet.gameObject);
        }
        
        public bool Finished => true;
        public int Priority { get; set; }
    }
}