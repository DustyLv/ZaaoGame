using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialController : MonoBehaviour
{
    public Texture2D m_Albedo;
    public Texture2D m_Metalness;
    public Texture2D m_Roughness;
    public Texture2D m_Normal;
    public Texture2D m_AO;

    private Renderer _renderer;
    private MaterialPropertyBlock _propBlock;

    void Awake()
    {
        _propBlock = new MaterialPropertyBlock();
        _renderer = GetComponent<Renderer>();
    }

    void Start()
    {
        SetupMaterial();
    }


    void SetupMaterial()
    {
        _renderer.GetPropertyBlock(_propBlock);

        if (m_Albedo != null) _propBlock.SetTexture("_MainTex", m_Albedo);
        if (m_Roughness != null) _propBlock.SetTexture("_SpecGlossMap", m_Roughness);
        if (m_Metalness != null) _propBlock.SetTexture("_MetallicGlossMap", m_Metalness);
        if (m_Normal != null) _propBlock.SetTexture("_BumpMap", m_Normal);
        if (m_AO != null) _propBlock.SetTexture("_OcclusionMap", m_AO);

        _renderer.SetPropertyBlock(_propBlock);
    }
}
