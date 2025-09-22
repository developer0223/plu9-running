using UnityEngine;
using System.Linq;

public class BgScroller : MonoBehaviour 
{
    [Tooltip("The speed at which the background scrolls to the left.")]
    [SerializeField] private float scrollSpeed = 5f;

    [Tooltip("The exact width of a single segment.")]
    [SerializeField] private float segmentWidth = 107.52f;

    [Tooltip("All segments that will be scrolled. They must be positioned side-by-side at the start.")]
    [SerializeField] private Transform[] bgSegments;

    private float leftBound;

    void Start()
    {
        if (bgSegments == null || bgSegments.Length < 2)
        {
            Debug.LogError("BgScroller requires at least 2 segments.");
            this.enabled = false;
            return;
        }
        
        // The left boundary is set based on the segment width, to detect when it's fully off-screen.
        leftBound = -segmentWidth;
    }

    void Update()
    {
        foreach (Transform segment in bgSegments)
        {
            // Move the segment to the left
            segment.position += Vector3.left * scrollSpeed * Time.deltaTime;

            // Check if the segment is fully off-screen
            if (segment.position.x < leftBound)
            {
                Reposition(segment);
            }
        }
    }

    /// <summary>
    /// Moves the given segment to the back of the line.
    /// </summary>
    private void Reposition(Transform segmentToMove)
    {
        // Find the current rightmost segment by checking all their positions
        Transform rightmostSegment = bgSegments[0];
        foreach (Transform seg in bgSegments)
        {
            if (seg.position.x > rightmostSegment.position.x)
            {
                rightmostSegment = seg;
            }
        }

        // Place the moved segment directly after the rightmost one
        segmentToMove.position = new Vector2(rightmostSegment.position.x + segmentWidth, segmentToMove.position.y);
    }
}
