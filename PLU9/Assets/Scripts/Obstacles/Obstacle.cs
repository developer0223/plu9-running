using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float scrollSpeed = 5f; 
    public float destroyXPosition = -10f; 

    void Update()
    {
        transform.Translate(Vector3.left * scrollSpeed * Time.deltaTime);

        if (transform.position.x < destroyXPosition)
        {
            Destroy(gameObject);
        }
    }
}