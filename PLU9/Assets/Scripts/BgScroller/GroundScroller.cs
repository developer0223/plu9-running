using UnityEngine;

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
        // 화면 밖으로 나간 조각을 (전체 조각 개수 * 조각 넓이) 만큼 오른쪽으로 이동시킵니다.
        float offset = bgSegments.Length * segmentWidth;
        segmentToMove.position += new Vector3(offset, 0, 0);
    }
}
