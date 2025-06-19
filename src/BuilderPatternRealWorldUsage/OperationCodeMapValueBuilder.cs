namespace BuilderPatternRealWorldUsage;

/// <summary>
/// OperationCodeMapValue µÄ¹¹ÔìÆ÷
/// </summary>
public class OperationCodeMapValueBuilder
{
    private OperationCodeMapValueBuilder()
    {
    }
    public static OperationCodeMapValueBuilder Empty() => new();

    private string _menuName = string.Empty;
    private string _firstLevel = string.Empty;
    private string _secondLevel = string.Empty;
    private string? _thirdLevel;
    private string? _fourthLevel;
    private string? _fifthLevel;
    private string _tabName = string.Empty;
    private string? _remarks;
    private OperationType _operationType;
    private string _relatedPage = default!;
    private string _logContent = default!;

    public OperationCodeMapValueBuilder WithMenuName(string menuName)
    {
        _menuName = menuName;
        return this;
    }

    public OperationCodeMapValueBuilder WithFirstLevel(string firstLevel)
    {
        _firstLevel = firstLevel;
        return this;
    }

    public OperationCodeMapValueBuilder WithSecondLevel(string secondLevel)
    {
        _secondLevel = secondLevel;
        return this;
    }

    public OperationCodeMapValueBuilder WithThirdLevel(string? thirdLevel)
    {
        _thirdLevel = thirdLevel;
        return this;
    }

    public OperationCodeMapValueBuilder WithFourthLevel(string? fourthLevel)
    {
        _fourthLevel = fourthLevel;
        return this;
    }

    public OperationCodeMapValueBuilder WithFifthLevel(string? fifthLevel)
    {
        _fifthLevel = fifthLevel;
        return this;
    }

    public OperationCodeMapValueBuilder WithTabName(string tabName)
    {
        _tabName = tabName;
        return this;
    }

    public OperationCodeMapValueBuilder WithRemarks(string? remarks)
    {
        _remarks = remarks;
        return this;
    }

    public OperationCodeMapValueBuilder WithOperationType(OperationType operationType)
    {
        _operationType = operationType;
        return this;
    }

    public OperationCodeMapValueBuilder WithRelatedPage(string relatedPage)
    {
        _relatedPage = relatedPage;
        return this;
    }

    public OperationCodeMapValueBuilder WithLogContent(string logContent)
    {
        _logContent = logContent;
        return this;
    }

    public OperationCodeMapValueBuilder Clone()
    {
        return new OperationCodeMapValueBuilder
        {
            _menuName = this._menuName,
            _firstLevel = this._firstLevel,
            _secondLevel = this._secondLevel,
            _thirdLevel = this._thirdLevel,
            _fourthLevel = this._fourthLevel,
            _fifthLevel = this._fifthLevel,
            _tabName = this._tabName,
            _remarks = this._remarks,
            _operationType = this._operationType,
            _relatedPage = this._relatedPage,
            _logContent = this._logContent
        };
    }

    public OperationCodeMapValue Build()
    {
        return new OperationCodeMapValue
        {
            Menu = _menuName,
            FirstLevel = _firstLevel,
            SecondLevel = _secondLevel,
            ThirdLevel = _thirdLevel,
            FourthLevel = _fourthLevel,
            FifthLevel = _fifthLevel,
            TabName = _tabName,
            Remarks = _remarks,
            OperationType = _operationType,
            RelatedPage = _relatedPage,
            LogContent = _logContent
        };
    }
}
