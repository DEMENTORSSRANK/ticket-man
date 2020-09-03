using System;
using Magnet;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Element
{
    public class Coin : MonoBehaviour
    {
        private Collider Collider => GetComponent<Collider>();

        public bool IsInFly => !Hooker.instance.allCoins.Contains(this);
        
        private bool isGravity;

        private void Start()
        {
            var childCount = transform.childCount - 1;
            
            for (var i = 0; i < childCount; i++)
                transform.GetChild(i).gameObject.SetActive(false);
            
            var indexSet = Random.Range(0, childCount);
            
            transform.GetChild(indexSet).gameObject.SetActive(true);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!IsInFly)
                return;
            
            var otherObject = other.gameObject;
            
            var otherTag = otherObject.tag;

            var isDownFloor = otherTag == "Finish";

            var isCoin = otherObject.GetComponent<Coin>();

            if (isCoin)
            {
                var otherCoin = otherObject.GetComponent<Coin>();
                
                var isRightCoin = otherCoin.IsGravity && !otherCoin.IsInFly;
                
                if (!isRightCoin)
                    return;
            }else if (!isDownFloor)
            {
                return;
            }

            transform.SetParent(Hooker.instance.parentCoins);
            
            Hooker.instance.allCoins.Add(this);
        }

        public bool IsGravity
        {
            get => isGravity;

            set
            {
                isGravity = value;

                if (value)
                {
                    gameObject.AddComponent<Rigidbody>();
                }
                else
                {
                    var rb = gameObject.GetComponent<Rigidbody>();

                    rb.velocity = Vector3.zero;

                    Destroy(rb);
                }
            }
        }    
    }
}
