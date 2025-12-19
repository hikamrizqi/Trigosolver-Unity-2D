using UnityEngine;
using TMPro;

/// <summary>
/// Alternative TriangleVisualizer menggunakan LineRenderer untuk hasil lebih baik
/// LineRenderer tidak akan distorsi saat diperpanjang
/// </summary>
public class lizerLineRenderer : MonoBehaviour
{
    [Header("LineRenderer References")]
    [Tooltip("LineRenderer untuk sisi Depan (vertikal)")]
    public LineRenderer depanLine;

    [Tooltip("LineRenderer untuk sisi Samping (horizontal)")]
    public LineRenderer sampingLine;

    [Tooltip("LineRenderer untuk sisi Miring (diagonal)")]
    public LineRenderer miringLine;

    [Header("Label References (TextMeshPro World Space)")]
    [Tooltip("Label untuk menampilkan nilai sisi Depan")]
    public TextMeshPro depanLabel;

    [Tooltip("Label untuk menampilkan nilai sisi Samping")]
    public TextMeshPro sampingLabel;

    [Tooltip("Label untuk menampilkan nilai sisi Miring")]
    public TextMeshPro miringLabel;

    [Header("Visual Settings")]
    [Tooltip("Skala dasar untuk sprites (1 = 1 unit Unity per nilai segitiga)")]
    public float baseScale = 0.5f;

    [Tooltip("Posisi pusat segitiga di world space")]
    public Vector3 centerPosition = Vector3.zero;

    [Tooltip("Offset label dari garis (jarak)")]
    public float labelOffset = 0.5f;

    [Tooltip("Ketebalan garis segitiga")]
    public float lineWidth = 0.1f;

    [Header("Colors")]
    public Color normalColor = Color.white;
    public Color highlightColor = Color.yellow;
    public Color correctColor = Color.green;
    public Color wrongColor = Color.red;

    [Header("Line Material")]
    [Tooltip("Material untuk LineRenderer (gunakan Sprites/Default atau Unlit/Color)")]
    public Material lineMaterial;

    // Data segitiga saat ini
    private int currentDepan;
    private int currentSamping;
    private int currentMiring;

    private void Start()
    {
        // Setup LineRenderer properties jika belum
        SetupLineRenderer(depanLine);
        SetupLineRenderer(sampingLine);
        SetupLineRenderer(miringLine);

        // Test draw
        if (currentDepan == 0 && currentSamping == 0 && currentMiring == 0)
        {
            DrawTriangle(3, 4, 5);
        }
    }

    private void SetupLineRenderer(LineRenderer line)
    {
        if (line == null) return;

        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.positionCount = 2;
        line.useWorldSpace = true;
        line.sortingOrder = 10;

        // Set material jika ada
        if (lineMaterial != null)
        {
            line.material = lineMaterial;
        }

        // Settings untuk rounded caps (tidak fade)
        line.numCapVertices = 5; // Lebih smooth rounded caps
        line.numCornerVertices = 5;
        line.textureMode = LineTextureMode.Tile; // Tile, bukan stretch!
    }

    public void DrawTriangle(int depan, int samping, int miring)
    {
        currentDepan = depan;
        currentSamping = samping;
        currentMiring = miring;

        // Hitung posisi-posisi vertex segitiga
        Vector3 bottomLeft = centerPosition;
        Vector3 bottomRight = bottomLeft + new Vector3(samping * baseScale, 0, 0);
        Vector3 topLeft = bottomLeft + new Vector3(0, depan * baseScale, 0);

        // SISI SAMPING (Horizontal - Bottom)
        DrawLine(sampingLine, bottomLeft, bottomRight);
        if (sampingLabel != null)
        {
            sampingLabel.text = samping.ToString();
            Vector3 midPoint = (bottomLeft + bottomRight) / 2f;
            sampingLabel.transform.position = midPoint + new Vector3(0, -labelOffset, 0);
        }

        // SISI DEPAN (Vertical - Left)
        DrawLine(depanLine, bottomLeft, topLeft);
        if (depanLabel != null)
        {
            depanLabel.text = depan.ToString();
            Vector3 midPoint = (bottomLeft + topLeft) / 2f;
            depanLabel.transform.position = midPoint + new Vector3(-labelOffset, 0, 0);
        }

        // SISI MIRING (Diagonal - Hypotenuse)
        DrawLine(miringLine, topLeft, bottomRight);
        if (miringLabel != null)
        {
            miringLabel.text = miring.ToString();
            Vector3 midPoint = (topLeft + bottomRight) / 2f;

            Vector3 direction = (bottomRight - topLeft).normalized;
            Vector3 perpendicular = new Vector3(-direction.y, direction.x, 0);
            miringLabel.transform.position = midPoint + perpendicular * labelOffset;
        }

        ResetColors();
    }

    private void DrawLine(LineRenderer line, Vector3 start, Vector3 end)
    {
        if (line == null) return;

        line.SetPosition(0, start);
        line.SetPosition(1, end);
    }

    public void HighlightSide(string sideName, Color color)
    {
        ResetColors();

        switch (sideName.ToLower())
        {
            case "depan":
                if (depanLine != null) depanLine.startColor = depanLine.endColor = color;
                break;
            case "samping":
                if (sampingLine != null) sampingLine.startColor = sampingLine.endColor = color;
                break;
            case "miring":
                if (miringLine != null) miringLine.startColor = miringLine.endColor = color;
                break;
        }
    }

    public void ResetColors()
    {
        if (depanLine != null) depanLine.startColor = depanLine.endColor = normalColor;
        if (sampingLine != null) sampingLine.startColor = sampingLine.endColor = normalColor;
        if (miringLine != null) miringLine.startColor = miringLine.endColor = normalColor;
    }

    public void HighlightSide(string sideName)
    {
        HighlightSide(sideName, highlightColor);
    }

    public void ShowFeedback(string sideName, bool isCorrect)
    {
        Color feedbackColor = isCorrect ? correctColor : wrongColor;
        HighlightSide(sideName, feedbackColor);
    }

    public void UpdateLabel(string sideName, string value)
    {
        switch (sideName.ToLower())
        {
            case "depan":
                if (depanLabel != null) depanLabel.text = value;
                break;
            case "samping":
                if (sampingLabel != null) sampingLabel.text = value;
                break;
            case "miring":
                if (miringLabel != null) miringLabel.text = value;
                break;
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Gizmos.color = Color.cyan;

        Vector3 bottomLeft = centerPosition;
        Vector3 bottomRight = bottomLeft + new Vector3(currentSamping * baseScale, 0, 0);
        Vector3 topLeft = bottomLeft + new Vector3(0, currentDepan * baseScale, 0);

        Gizmos.DrawLine(bottomLeft, bottomRight);
        Gizmos.DrawLine(bottomLeft, topLeft);
        Gizmos.DrawLine(topLeft, bottomRight);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(bottomLeft, 0.1f);
        Gizmos.DrawSphere(bottomRight, 0.1f);
        Gizmos.DrawSphere(topLeft, 0.1f);
    }
}
