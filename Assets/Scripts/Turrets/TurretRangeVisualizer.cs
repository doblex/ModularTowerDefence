using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TurretRangeVisualizer : MonoBehaviour
{
    public float range = 5f;
    public int segments = 100;

    private LineRenderer line;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.loop = true;
        line.positionCount = segments + 1;

        DrawCircle();
    }

    void Update()
    {
        DrawCircle();
    }

    void DrawCircle()
    {
        float angle = 0f;
        for (int i = 0; i <= segments; i++)
        {
            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * range;
            float z = Mathf.Sin(Mathf.Deg2Rad * angle) * range;
            Vector3 pos = new Vector3(x, 0f, z) + transform.position;
            line.SetPosition(i, pos);
            angle += 360f / segments;
        }
    }
}
