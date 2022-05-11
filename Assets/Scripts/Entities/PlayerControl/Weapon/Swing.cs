using UnityEngine;

namespace PlayerControl.Weapon
{
    public class Swing
    {
        public Vector3 force;

        public Swing(Vector3 force)
        {
            this.force = force;
        }
        //Damage or some shit
        //Currently used to identify a swing, so there would not be duplicate hits
    }
}