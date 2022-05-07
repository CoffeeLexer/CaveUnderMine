using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        private CharacterController charController;

        public Vector3 force;

        private bool triggerOn = false;
        
        public void Awake()
        {
            capsule = GetComponent<CapsuleCollider>();
            charController = GetComponent<CharacterController>();
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
                        Debug.Log("Enemy hit");
                    }
                }
            }
        }

        void Update()
        {
            if (force.magnitude > 0.2)
            {
                charController.Move(force * Time.deltaTime);
            }
            charController.SimpleMove(Vector3.zero);
            force = Vector3.Lerp(force, Vector3.zero, 5*Time.deltaTime);
        }
    }
}