using UnityEngine;
using System.Linq;

public class BgScroller : MonoBehaviour 
{
    [SerializeField] private float scrollSpeed = 5f;

    [SerializeField] private float segmentWidth = 107.52f;

    [SerializeField] private Transform[] bgSegments;

    private float leftBound;

    void Start()
    {
        if (bgSegments == null || bgSegments.Length < 2)
        {
            this.enabled = false;
            return;
        }
        
        leftBound = -segmentWidth;
    }

    void Update()
    {
        foreach (Transform segment in bgSegments)
        {
            segment.position += Vector3.left * scrollSpeed * Time.deltaTime;

            if (segment.position.x < leftBound)
            {
                Reposition(segment);
            }
        }
    }

    private void Reposition(Transform segmentToMove)
    {
        Transform rightmostSegment = bgSegments[0];
        foreach (Transform seg in bgSegments)
        {
            if (seg.position.x > rightmostSegment.position.x)
            {
                rightmostSegment = seg;
            }
        }

        segmentToMove.position = new Vector2(rightmostSegment.position.x + segmentWidth, segmentToMove.position.y);
    }
}
