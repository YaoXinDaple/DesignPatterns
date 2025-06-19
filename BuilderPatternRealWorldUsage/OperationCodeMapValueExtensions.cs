using System.Text;
namespace BuilderPatternRealWorldUsage;


public static class OperationCodeMapValueExtensions
{
    /// <summary>
    /// 将LogContent中的模板数据替换为实际数据
    /// </summary>
    /// <param name="operationCodeMapValue"></param>
    /// <param name="dataName"></param>
    /// <param name="dataValue"></param>
    /// <returns></returns>
    public static OperationCodeMapValue WithContentData(this OperationCodeMapValue operationCodeMapValue, string dataName, string? dataValue)
    {
        // 从 operationCodeMapValue Content中找到 {dataName}格式,替换为 dataValue
        var content = operationCodeMapValue.LogContent;

        if (string.IsNullOrWhiteSpace(content))
        {
            return operationCodeMapValue;
        }

        string newContent;
        if (string.IsNullOrWhiteSpace(dataValue))
        {
            newContent = content.Replace($"{{{dataName}}}", "", StringComparison.OrdinalIgnoreCase);
        }
        else
        {
            newContent = content.Replace($"{{{dataName}}}", dataValue, StringComparison.OrdinalIgnoreCase);
        }

        // 使用 with 表达式创建一个新的 OperationCodeMapValue 实例
        return operationCodeMapValue with { LogContent = newContent };
    }

    public static OperationCodeMapValue WithContentData(this OperationCodeMapValue operationCodeMapValue, string dataName, int dataValue)
    {
        return operationCodeMapValue.WithContentData(dataName, dataValue.ToString());
    }

    public static OperationCodeMapValue WithContentData(this OperationCodeMapValue operationCodeMapValue, string dataName, DateTime dataValue)
    {
        return operationCodeMapValue.WithContentData(dataName, dataValue.ToFormatString());
    }

    public static string BuildPageName(this OperationCodeMapValue operationCodeMapValue)
    {
        StringBuilder pageNameBuilder = new StringBuilder();
        pageNameBuilder.Append(operationCodeMapValue.FirstLevel);
        if (operationCodeMapValue.SecondLevel.IsNullOrWhiteSpace())
        {
            return pageNameBuilder.ToString();
        }
        pageNameBuilder.Append("/").Append(operationCodeMapValue.SecondLevel);

        if (operationCodeMapValue.ThirdLevel.IsNullOrWhiteSpace())
        {
            return pageNameBuilder.ToString();
        }
        pageNameBuilder.Append("/").Append(operationCodeMapValue.ThirdLevel);

        if (operationCodeMapValue.FourthLevel.IsNullOrWhiteSpace())
        {
            return pageNameBuilder.ToString();
        }
        pageNameBuilder.Append("/").Append(operationCodeMapValue.FourthLevel);

        if (operationCodeMapValue.FifthLevel.IsNullOrWhiteSpace())
        {
            return pageNameBuilder.ToString();
        }
        pageNameBuilder.Append("/").Append(operationCodeMapValue.FifthLevel);

        return pageNameBuilder.ToString();
    }

    #region Temporary Static Method
    public static string ToFormatString(this DateTime dateTimeValue)
    {
        // 使用自定义格式化字符串
        return dateTimeValue.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public static bool IsNullOrWhiteSpace(this string? value)
    {
        return string.IsNullOrWhiteSpace(value);
    }
    #endregion
}

