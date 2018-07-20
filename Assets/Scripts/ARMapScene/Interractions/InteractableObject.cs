using UnityEngine;

namespace ua.org.gdg.devfest
{
    public abstract class InteractableObject : MonoBehaviour
    {
        public abstract void Interact();

        public abstract void Disable();
    }
}