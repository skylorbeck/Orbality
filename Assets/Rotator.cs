    using System;
    using UnityEngine;

    public class Rotator :MonoBehaviour
    {
        private void FixedUpdate()
        {
            transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + 1);
        }
    }