using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JI.Unity.SpiderWorld.Effects
{
    public class PlayAndDie : MonoBehaviour
    {

        private ParticleSystem ParticleSystem { get; set; }
        // Use this for initialization
        void Start()
        {
            ParticleSystem = GetComponent<ParticleSystem>();
            ParticleSystem.Play();
        }

        // Update is called once per frame
        void Update()
        {
            if (!ParticleSystem.isPlaying)
                Destroy(gameObject);
        }
    }
}