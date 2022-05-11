using UnityEngine;

namespace Entities
{
    public interface IMovable
    {
        public void ApplyForce(Vector2 force);
    }
}