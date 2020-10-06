using System;
using System.Collections;
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
        
        [ContextMenu("Save Position")]
        private void SavePosition()
        {
            var gameObjectName = gameObject.name;
            
            var position = transform.position;

            PlayerPrefs.SetFloat("x" + gameObjectName, position.x);
            
            PlayerPrefs.SetFloat("y" + gameObjectName, position.y);
            
            PlayerPrefs.SetFloat("z" + gameObjectName, position.z);

            var rotation = transform.eulerAngles;
            
            PlayerPrefs.SetFloat("xR" + gameObjectName, rotation.x);
            
            PlayerPrefs.SetFloat("yR" + gameObjectName, rotation.y);
            
            PlayerPrefs.SetFloat("zR" + gameObjectName, rotation.z);
        }

        [ContextMenu("Set Position")]
        private void SetPosition()
        {
            var gameObjectName = gameObject.name;
            
            var pos = new Vector3()
            {
                x = PlayerPrefs.GetFloat("x" + gameObjectName),
                y = PlayerPrefs.GetFloat("y" + gameObjectName),
                z = PlayerPrefs.GetFloat("z" + gameObjectName)
            };

            transform.position = pos;
            
            var rot = new Vector3()
            {
                x = PlayerPrefs.GetFloat("xR" + gameObjectName),
                y = PlayerPrefs.GetFloat("yR" + gameObjectName),
                z = PlayerPrefs.GetFloat("zR" + gameObjectName)
            };

            transform.eulerAngles = rot;
        }
        
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
                
                var isRightCoin = !otherCoin.IsGravity && !otherCoin.IsInFly;
                
                if (!isRightCoin)
                    return;
            }
            else if (!isDownFloor)
            {
                return;
            }

            transform.SetParent(Hooker.instance.parentCoins);

            StartCoroutine(WaitStopGravity());

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

        private IEnumerator WaitStopGravity()
        {
            yield return new WaitForSeconds(5);
            
            IsGravity = false;
        }
    }
}
