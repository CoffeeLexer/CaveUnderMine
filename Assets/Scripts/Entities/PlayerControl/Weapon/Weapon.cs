using UnityEngine;

namespace PlayerControl.Weapon
{
    public class Weapon : MonoBehaviour
    {
        private Collider _collider;
        public Transform owner;
        public Swing swing { get; private set; }

        void Awake()
        {
            _collider = GetComponent<Collider>();
            _collider.enabled = false;
        }

        public void MakeDamaging()
        {
            _collider.enabled = true;
            swing = new Swing(owner.transform.forward);
        }

        public void MakePassive()
        {
            _collider.enabled = false;
            swing = null;
        }
    }
}