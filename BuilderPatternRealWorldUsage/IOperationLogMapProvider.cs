using System.Diagnostics.CodeAnalysis;

namespace BuilderPatternRealWorldUsage;

/// <summary>
/// 操作日志对照表提供程序
/// </summary>
public interface IOperationLogMapProvider
{
    /// <summary>
    /// 获取操作日志编码映射-同步方式
    /// </summary>
    /// <returns></returns>
    bool TryGetValue(string code, [MaybeNullWhen(false)] out OperationCodeMapValue operationCodeMapValue);

    /// 判断操作日志编码是否有效-同步方式
    /// </summary>
    /// <returns></returns>
    bool IsValid(string code);


    /// 判断操作日志编码是否有效-异步方式
    /// </summary>
    /// <returns></returns>
    ValueTask<bool> TryGetValueAsync(string code, [MaybeNullWhen(false)] out OperationCodeMapValue operationCodeMapValue);

    /// <summary>
    /// 获取操作日志编码映射-异步方式
    /// </summary>
    /// <returns></returns>
    ValueTask<bool> IsValidAsync(string code);
}
