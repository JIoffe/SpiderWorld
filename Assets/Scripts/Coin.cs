using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JI.Unity.SpiderWorld.Items
{
    public class Coin : MonoBehaviour
    {
        public float spinSpeed = 80f;
        public GameObject collectionEffect;


        // Update is called once per frame
        void Update()
        {
            transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);
        }

        void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == 10)
            {
                if(collectionEffect != null)
                {
                    Instantiate(collectionEffect, transform.position, Quaternion.identity);
                }

                Destroy(gameObject);
            }
        }
    }
}