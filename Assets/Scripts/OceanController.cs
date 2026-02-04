using System;
using UnityEngine;

public class OceanController : MonoBehaviour
{
    [Header("Wave Settings")]
    public Octave[] octaves;
    
    public float seaLevel = 0f;
    public bool sendToShader = true;

    void Update()
    {
        if (!sendToShader) return;

        Shader.SetGlobalFloat("_SeaLevel", seaLevel);
        Shader.SetGlobalInt("_OctaveCount", octaves.Length);

        for (int i = 0; i < octaves.Length; i++)
        {
            Shader.SetGlobalVector($"_OctaveScale{i}", octaves[i].scale);
            Shader.SetGlobalVector($"_OctaveSpeed{i}", octaves[i].speed);
            Shader.SetGlobalFloat($"_OctaveHeight{i}", octaves[i].height);
            //Shader.SetGlobalFloat($"_OctaveAlternate{i}", octaves[i].alternate ? 1f : 0f);
            
            //Debug.Log($"[OceanController] Octave {i} - Scale: {octaves[i].scale} Speed: {octaves[i].speed} Height: {octaves[i].height} Alternate: {octaves[i].alternate}");
        }

        //Debug.Log($"[OceanController] SeaLevel: {seaLevel}");
    }
    
    public float GetWaterHeight(Vector3 worldPos)
    {
        float y = seaLevel;

        for (int o = 0; o < octaves.Length; o++)
        {
            if (octaves[o].alternate)
            {
                float perl = Mathf.PerlinNoise(
                    worldPos.x * octaves[o].scale.x,
                    worldPos.z * octaves[o].scale.y
                ) * Mathf.PI * 2f;

                y += Mathf.Cos(perl + octaves[o].speed.magnitude * Time.time)
                     * octaves[o].height;
            }
            else
            {
                float perl = Mathf.PerlinNoise(
                    worldPos.x * octaves[o].scale.x + Time.time * octaves[o].speed.x,
                    worldPos.z * octaves[o].scale.y + Time.time * octaves[o].speed.y
                ) - 0.5f;

                y += perl * octaves[o].height;
            }
        }

        return y;
    }

    public Vector3 GetWaterNormal(Vector3 worldPos, float offset = 0.5f)
    {
        float hL = GetWaterHeight(worldPos + Vector3.left * offset);
        float hR = GetWaterHeight(worldPos + Vector3.right * offset);
        float hD = GetWaterHeight(worldPos + Vector3.back * offset);
        float hU = GetWaterHeight(worldPos + Vector3.forward * offset);

        Vector3 normal = new Vector3(hL - hR, 2f, hD - hU);
        return normal.normalized;
    }

    [Serializable]
    public struct Octave
    {
        public Vector2 scale;
        public Vector2 speed;
        public float height;
        public bool alternate;
    }
}
