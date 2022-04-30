    using System;
    using UnityEngine;

    public class Rotator :MonoBehaviour
    {
        [SerializeField] bool counterclockwise = false;
        private void FixedUpdate()
        {
            transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + (counterclockwise ? 1 : -1));
        }
    }