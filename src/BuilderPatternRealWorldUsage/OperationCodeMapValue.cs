namespace BuilderPatternRealWorldUsage;

/// <summary>
/// 操作日志编码对应的值
/// </summary>
public record OperationCodeMapValue
{
    internal OperationCodeMapValue()
    {
    }

    /// <summary>
    /// 菜单
    /// </summary>
    public string Menu { get; init; } = string.Empty;

    /// <summary>
    /// 一级功能
    /// </summary>
    public string FirstLevel { get; init; } = string.Empty;

    /// <summary>
    /// 二级功能
    /// </summary>
    public string SecondLevel { get; init; } = string.Empty;

    /// <summary>
    /// 三级功能
    /// </summary>
    public string? ThirdLevel { get; init; }

    /// <summary>
    /// 四级功能
    /// </summary>
    public string? FourthLevel { get; init; }

    /// <summary>
    /// 五级功能
    /// </summary>
    public string? FifthLevel { get; init; }

    /// <summary>
    /// 页签命名
    /// </summary>
    public string TabName { get; init; } = string.Empty;

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remarks { get; init; }

    /// <summary>
    /// 操作类型
    /// </summary>
    public OperationType OperationType { get; init; }

    /// <summary>
    /// 相关页面
    /// </summary>
    public string RelatedPage { get; init; } = string.Empty;

    /// <summary>
    /// 日志
    /// </summary>
    public string LogContent { get; set; } = string.Empty;
}
