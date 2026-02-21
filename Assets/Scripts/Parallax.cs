using UnityEngine;

public class Parallax : MonoBehaviour
{
    [Header("Only One Parameter")]
    [SerializeField] private float distance = 10f;
    
    private Transform cam;
    private Vector3 startPos;
    
    void Start()
    {
        cam = Camera.main.transform;
        startPos = transform.position;
    }
    
    void LateUpdate()
    {
        if (cam == null) return;
        
        float parallaxFactor = 1f / distance;
        
        Vector3 cameraDelta = cam.position - startPos;
        
        Vector3 newPosition = startPos;
        newPosition.x = startPos.x + cameraDelta.x * parallaxFactor;
        newPosition.y = startPos.y + cameraDelta.y * parallaxFactor;
        
        transform.position = newPosition;
    }
}