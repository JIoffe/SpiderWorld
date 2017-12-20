using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JI.Unity.SpiderWorld.Projectiles
{
    public class Webshot : MonoBehaviour
    {
        public float speed = 2f;
        public GameObject webbingPrefab;

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(Vector3.forward, 200f * Time.deltaTime);
            var pos = transform.position;
            transform.position = pos + transform.forward * speed * Time.deltaTime;
            RaycastHit hitInfo;
            if (!Physics.Raycast(transform.position, transform.forward, out hitInfo, 0.5f, (1 << 8)))
                return;

            var angle = Random.Range(0f, 359f);

            var webbing = Instantiate(webbingPrefab);
            webbing.transform.position = hitInfo.point + hitInfo.normal * 0.01f;
            webbing.transform.forward = hitInfo.normal;
            webbing.transform.Rotate(Vector3.forward, angle);
            webbing.transform.localScale = Vector3.one * Random.Range(0.05f, 0.15f);

            Destroy(gameObject);
        }
    }
}