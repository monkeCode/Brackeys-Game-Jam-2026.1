using System;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class UpAndVanish : MonoBehaviour
{
    [SerializeField] private float time;
    [SerializeField] private float speed;


    private float now = 0;
    private SpriteRenderer sp;
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        Destroy(gameObject, time);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += speed * Time.deltaTime * Vector3.up;
        sp.color = new Color(1,1,1, 1-now/time);
        now += Time.deltaTime;
    }
}
