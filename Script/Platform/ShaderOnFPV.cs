using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 시점전환에 따라 쉐이더를 전환하는데 사용하는 컴포넌트
/// </summary>
/// <remarks>
/// 초기 쉐이더와 동적 전환할 쉐이더를 선택하고, 
/// 쉐이더를 적용할 머티리얼들을 등록하면, 
/// 시점이 전환될 때마다 등록한 머티리얼들의 쉐이더가 일괄 전환됩니다.
/// </remarks>
/// @author 이은수
/// @date last change 2023/05/03
public class ShaderOnFPV : MonoBehaviour
{
    #region fields

    Shader shader;  // 1인칭에 적용할 쉐이더
    Shader initShader; // 초기 쉐이더

    [SerializeField, Tooltip("초기 쉐이더를 저장하세요")] ShaderType initShaderType = ShaderType.Standard;
    [SerializeField, Tooltip("1인칭 시점에 적용할 쉐이더를 선택하세요")] ShaderType shaderType;
    [SerializeField, Tooltip("쉐이더를 적용할 머티리얼을 등록하세요")] List<Material> mats = new();
    [SerializeField] int initRenderQueue = 0;
    [SerializeField] int renderQueue = 0;

    #endregion

    #region unity events

    private void Awake()
    {
        initializeShader();

        GameManager.Instance.AddChangePVEvent(ChangeAciveFPV, 1);
        GameManager.Instance.AddChangePVEvent(ChangeAciveTPV, 3);
    }
    private void OnDestroy()
    {
        initializeShader();

        if (GameManager.Instance == null) return;
        GameManager.Instance.RemoveChangePVEvent(ChangeAciveFPV, 1);
        GameManager.Instance.RemoveChangePVEvent(ChangeAciveTPV, 3);
    }

    #endregion

    #region methods
    /// <summary>
    /// 쉐이더 초기화
    /// </summary>
    private void initializeShader()
    {
        initShader = ShaderManager.shaders[(int)initShaderType];
        shader = ShaderManager.shaders[(int)shaderType];

        
        for (int i = 0; i < mats.Count; i++)
        {
            if (initRenderQueue != 0)
                mats[i].renderQueue = initRenderQueue; // 머티리얼의 Render Queue 초기화

            mats[i].shader = initShader;               // 머티리얼의 쉐이더 초기화
        }
    }

    /// <summary>
    /// 1인칭 전환 시 쉐이더 변경
    /// </summary>
    private void ChangeAciveFPV()
    {
        if (!GameManager.Instance.ShaderChange)
            return;

        for (int i = 0; i < mats.Count; i++)
        {
            if (this.renderQueue != 0)
                mats[i].renderQueue = this.renderQueue; // 머티리얼과 쉐이더의 Render Queue 일치시키는 과정
            mats[i].shader = this.shader;               // 머티리얼의 쉐이더 전환
        }
    }

    /// <summary>
    /// 3인칭 전환 시 쉐이더 초기화
    /// </summary>
    private void ChangeAciveTPV()
    {
        if (!GameManager.Instance.ShaderChange)
            return;

        for (int i = 0; i < mats.Count; i++)
        {
            if (initRenderQueue != 0)
                mats[i].renderQueue = initRenderQueue;  // 머티리얼의 Render Queue 초기화
            mats[i].shader = initShader;                // 머티리얼의 쉐이더 초기화
        }
    }

    #endregion
}
