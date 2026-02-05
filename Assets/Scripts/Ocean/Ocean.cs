using UnityEngine;

public class Ocean : MonoBehaviour
{
    [Header("Wave params (match shader)")]
    public float seaLevel = 0f;
    public float amp = 0.5f;
    public float speed = 1.5f;
    public float length = 8f;
    public Vector2 dir = new Vector2(1f, 0.3f);

    [Header("Send to shader globals")]
    public bool sendToShader = true;

    void Update()
    {
        if (!sendToShader) return;

        Shader.SetGlobalFloat("_OC_SeaLevel", seaLevel);
        Shader.SetGlobalFloat("_OC_Amp", amp);
        Shader.SetGlobalFloat("_OC_Speed", speed);
        Shader.SetGlobalFloat("_OC_Length", length);
        Shader.SetGlobalVector("_OC_Dir", new Vector4(dir.x, dir.y, 0f, 0f));
    }

    public float GetHeight(Vector3 worldPos, float time)
    {
        Vector2 d = dir.sqrMagnitude > 0.0001f ? dir.normalized : Vector2.right;
        float k = 2f * Mathf.PI / Mathf.Max(0.001f, length);

        float dot = d.x * worldPos.x + d.y * worldPos.z;
        float phase = dot * k + speed * time;

        return seaLevel + Mathf.Sin(phase) * amp;
    }

    public Vector3 GetNormal(Vector3 worldPos, float time, float sampleRadius = 0.7f)
    {
        float hL = GetHeight(worldPos + new Vector3(-sampleRadius, 0, 0), time);
        float hR = GetHeight(worldPos + new Vector3( sampleRadius, 0, 0), time);
        float hD = GetHeight(worldPos + new Vector3(0, 0, -sampleRadius), time);
        float hU = GetHeight(worldPos + new Vector3(0, 0,  sampleRadius), time);

        Vector3 n = new Vector3(hL - hR, 2f * sampleRadius, hD - hU);
        return n.normalized;
    }
}