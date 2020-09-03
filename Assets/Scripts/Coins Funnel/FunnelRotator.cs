using System;
using Magnet;
using UnityEngine;

namespace Coins_Funnel
{
    public class FunnelRotator : MonoBehaviour
    {
        public float rotateForce;
        
        public Transform rotator;

        private static Hooker Hooker => Hooker.instance;
        
        private void FixedUpdate()
        {
            RotateAll();   
        }

        private void RotateAll()
        {
            if (Hooker.instance.CurrentTypeMove != Hooker.TypeMove.FollowMagnet)
                return;
            
            var rotation = rotator.eulerAngles;

            rotation.y += rotateForce * Time.deltaTime;

            rotator.eulerAngles = rotation;
        }
    }
}
