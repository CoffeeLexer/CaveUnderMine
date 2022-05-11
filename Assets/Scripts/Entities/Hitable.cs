using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;
using PlayerControl.Weapon;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Environment
{
    public class Hitable : MonoBehaviour
    {
        private UnityEvent onHit;
        private CapsuleCollider capsule;
        private Dictionary<Weapon, Swing> uniqueWeaponHits;
        private Animator animator;
        private IMovable movableEntity;

        public Vector3 force;

        private bool triggerOn = false;
        
        public void Awake()
        {
            capsule = GetComponent<CapsuleCollider>();
            movableEntity = GetComponent<IMovable>();
            uniqueWeaponHits = new Dictionary<Weapon, Swing>();
            animator = GetComponent<Animator>();
            async Task Func() => await Task.Delay(TimeSpan.FromMilliseconds(20));
            Func().ContinueWith((_) => triggerOn = true);
        }
        
        private async Task triggerOnDelay()
        { 
            await Task.Delay(TimeSpan.FromMilliseconds(20));
        }

        private IEnumerable Invulnerability()
        {
            yield return new WaitForSeconds(0.2f);
            triggerOn = true;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (triggerOn)
            {
                if (other.CompareTag("Damaging"))
                {
                    var weapon = other.GetComponent<Weapon>();
                    if (!uniqueWeaponHits.ContainsKey(weapon) || uniqueWeaponHits[weapon] != weapon.swing)
                    {
                        uniqueWeaponHits[weapon] = weapon.swing;
                        animator.SetTrigger("GetDamage");
                        force = Vector3.Normalize(weapon.swing.force) * 5;
                        movableEntity.ApplyForce(force);
                        Debug.Log("Enemy hit");
                    }
                }
            }
        }
    }
}