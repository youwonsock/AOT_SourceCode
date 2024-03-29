using UnityEngine;

/// <summary>
/// 쉐이더를 관리하는 스크립트
/// </summary>
/// @author 이은수
/// @date last change 2023/05/03
public class ShaderManager : MonoBehaviour
{
    #region fields

    public static Shader[] shaders = new Shader[10]; // 총 10개의 쉐이더를 등록                   

    #endregion

    #region unity events

    /// <summary>
    /// 게임 내에서 동적 전환할 쉐이더 등록 과정
    /// </summary>
    /// <remarks>
    /// 쉐이더의 이름을 통해 쉐이더 정보를 shaders 배열에 저장
    /// </remarks>
    private void Awake()
    {
        shaders[(int)ShaderType.Standard] = Shader.Find("Standard");
        shaders[(int)ShaderType.StandardSpecular] = Shader.Find("Standard (Specular setup)");
        shaders[(int)ShaderType.StylizedSurface] = Shader.Find("FlatKit/Stylized Surface");
        shaders[(int)ShaderType.GradientSkybox] = Shader.Find("FlatKit/GradientSkybox");
        shaders[(int)ShaderType.UnlitTexture] = Shader.Find("Unlit/Texture");
        shaders[(int)ShaderType.UnlitColor] = Shader.Find("Unlit/Color");
        shaders[(int)ShaderType.ParticlesStandardSurface] = Shader.Find("Particles/Standard Surface");
        shaders[(int)ShaderType.Custom] = Shader.Find("Custom/SurfaceShader_VC");
        shaders[(int)ShaderType.TransparentCutout] = Shader.Find("Unlit/Transparent Cutout");
        shaders[(int)ShaderType.UV_Scroll] = Shader.Find("SyntyStudios/Polygon_UVScroll");
    }

    #endregion
}

/// <summary>
/// 동적 전환할 쉐이더의 enum형
/// </summary>
public enum ShaderType
{
    Standard,
    StandardSpecular,
    StylizedSurface,
    GradientSkybox,
    UnlitTexture,
    UnlitColor,
    ParticlesStandardSurface,
    Custom,
    TransparentCutout,
    UV_Scroll,
}