using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

namespace BuilderPatternRealWorldUsage;

/// <summary>
/// 操作日志对照表提供程序
/// </summary>
public class OperationLogMapProvider : IOperationLogMapProvider
{
    /// <summary>
    /// 定义一个冻结字典
    /// </summary>
    private readonly FrozenDictionary<string, OperationCodeMapValue> _operationCodeMapValues;

    public OperationLogMapProvider()
    {
        _operationCodeMapValues = BuildOperationCodeMapValues();
    }

    /// <summary>
    /// 获取操作代码对应的映射值
    /// </summary>
    /// <param name="code">操作代码</param>
    /// <param name="operationCodeMapValue">输出的操作代码映射值</param>
    /// <returns>如果找到映射值返回 true，否则返回 false</returns>
    public ValueTask<bool> TryGetValueAsync(string code, [NotNullWhen(true)] out OperationCodeMapValue? operationCodeMapValue)
    {
        return ValueTask.FromResult(_operationCodeMapValues.TryGetValue(code.Trim(), out operationCodeMapValue));
    }

    /// <summary>
    /// 验证操作代码是否有效
    /// </summary>
    /// <param name="code">要验证的操作代码</param>
    /// <returns>如果操作代码有效返回 true，否则返回 false</returns>
    public ValueTask<bool> IsValidAsync(string code)
    {
        return ValueTask.FromResult(_operationCodeMapValues.ContainsKey(code.Trim()));
    }

    /// <summary>
    /// 获取操作代码对应的映射值-同步方式
    /// </summary>
    /// <param name="code">操作代码</param>
    /// <param name="operationCodeMapValue">输出的操作代码映射值</param>
    /// <returns>如果找到映射值返回 true，否则返回 false</returns>
    [Obsolete("请使用 TryGetValueAsync 方法")]
    public bool TryGetValue(string code, [MaybeNullWhen(false)] out OperationCodeMapValue operationCodeMapValue)
    {
        return _operationCodeMapValues.TryGetValue(code.Trim(), out operationCodeMapValue);
    }

    /// <summary>
    /// 验证操作代码是否有效-同步方式
    /// </summary>
    /// <param name="code">要验证的操作代码</param>
    /// <returns>如果操作代码有效返回 true，否则返回 false</returns>
    [Obsolete("请使用 IsValidAsync 方法")]
    public bool IsValid(string code)
    {
        return _operationCodeMapValues.ContainsKey(code.Trim());
    }

    /// <summary>
    /// 构建操作代码映射值字典
    /// </summary>
    /// <returns>返回冻结的操作代码映射值字典</returns>
    private FrozenDictionary<string, OperationCodeMapValue> BuildOperationCodeMapValues()
    {
        var initialDic = new Dictionary<string, OperationCodeMapValue>();

        BuildMainPageOperationCodeMapValues(initialDic);
        BuildInvoiceIssueManagementOperationCodeMapValues(initialDic);
        BuildDigitalAuthMap(initialDic);
        BuildRedInvoiceManagementMap(initialDic);
        BuildStatisticsMap(initialDic);
        BuildCustomerManagementMap(initialDic);
        BuildInvoiceItemManagementMap(initialDic);
        BuildAdditionalInformationManagementMap(initialDic);
        BuildSettingManagementMap(initialDic);

        return initialDic.ToFrozenDictionary();
    }

    /// <summary>
    /// 构建首页相关的操作日志映射
    /// </summary>
    /// <param name="map">操作代码映射字典</param>
    private void BuildMainPageOperationCodeMapValues(Dictionary<string, OperationCodeMapValue> map)
    {
        //获取菜单
        var menuName = CustomClaims.MainPage;

        var baseBuilder = OperationCodeMapValueBuilder.Empty()
            .WithMenuName(menuName)
            .WithFirstLevel(menuName);

        string baseKey = "100";

        //100002     登出 首页 退出登录
        var key = baseKey + "002";
        var value = baseBuilder.Clone()
            .WithOperationType(OperationType.Logout)
            .WithLogContent("退出登录")
            .Build();
        map.Add(key, value);

        //    100003		创建	首页	新增企业：{企业名称}
        key = baseKey + "003";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Create)
            .WithLogContent("新增企业：{企业名称}")
            .Build();
        map.Add(key, value);
    }

    /// <summary>
    /// 构建开票管理相关的操作日志映射
    /// </summary>
    /// <param name="map">操作代码映射字典</param>
    private void BuildInvoiceIssueManagementOperationCodeMapValues(Dictionary<string, OperationCodeMapValue> map)
    {
        BuildImmediateInvoiceMap(map);
        BuildBatchInvoiceMap(map);
        BuildShortcutInvoiceMap(map);
        BuildReceiptInvoiceMap(map);
        BuildTableScanInvoiceMap(map);
    }

    /// <summary>
    /// 构建立即开票相关的操作日志映射
    /// </summary>
    /// <param name="map">操作代码映射字典</param>
    private void BuildImmediateInvoiceMap(Dictionary<string, OperationCodeMapValue> map)
    {
        var menuName = CustomClaims.InvoiceIssueManagement;
        var secondLevel = "立即开票";

        // 创建基础 builder，包含共同属性
        var baseBuilder = OperationCodeMapValueBuilder.Empty()
            .WithMenuName(menuName)
            .WithFirstLevel(menuName)
            .WithSecondLevel(secondLevel);

        string baseKey = "101101";

        /*
        101101001		访问	开票管理/立即开票	    访问页面
        101101003		读取	开票管理/立即开票	    复制发票：{数电票号码}
        101101004		创建	开票管理/立即开票	    新增发票订单号：{订单号}
        101101006		创建	开票管理/立即开票	    保存草稿发票：{客户名称}
        101102001		访问	开票管理/批量开票	    访问页面
        */

        // 使用基础 builder构建不同的值
        var key = baseKey + "001";
        var value = baseBuilder.Clone()
            .WithOperationType(OperationType.View)
            .WithLogContent("访问页面")
            .Build();
        map.Add(key, value);

        key = baseKey + "003";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Read)
            .WithLogContent("复制发票{数电票号码}")
            .Build();
        map.Add(key, value);

        key = baseKey + "004";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Create)
            .WithLogContent("新增发票订单号：{订单号}")
            .Build();
        map.Add(key, value);

        // 101101006  创建	开票管理/立即开票	保存草稿发票：{客户名称}
        key = baseKey + "006";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Create)
            .WithLogContent("保存草稿发票：{客户名称}")
            .Build();
        map.Add(key, value);
    }

    /// <summary>
    /// 构建批量开票相关的操作日志映射
    /// </summary>
    /// <param name="map">操作代码映射字典</param>
    private void BuildBatchInvoiceMap(Dictionary<string, OperationCodeMapValue> map)
    {
        var menuName = CustomClaims.InvoiceIssueManagement;
        var secondLevel = "批量开票";

        var baseBuilder = OperationCodeMapValueBuilder.Empty()
            .WithMenuName(menuName)
            .WithFirstLevel(menuName)
            .WithSecondLevel(secondLevel);

        string baseKey = "101102";

        //101102001 访问	开票管理/批量开票	访问页面
        var key = baseKey + "001";
        var value = baseBuilder.Clone()
            .WithOperationType(OperationType.View)
            .WithLogContent("访问页面")
            .Build();
        map.Add(key, value);

        //101102003 开票管理/批量开票	编辑发票：{订单号}
        key = baseKey + "003";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Update)
            .WithLogContent("编辑发票：{订单号}")
            .Build();
        map.Add(key, value);

        //101102005 删除	开票管理/批量开票	删除{数量}张发票，订单号：{订单号;订单号;订单号}
        key = baseKey + "005";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Delete)
            .WithLogContent("删除{数量}张发票，订单号：{订单号}")
            .Build();
        map.Add(key, value);

        //101102006 更新	开票管理/批量开票	修改发票类型为{发票类型}
        key = baseKey + "006";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Update)
            .WithLogContent("修改发票类型为{发票类型}")
            .Build();
        map.Add(key, value);

        //1011020081 创建	开票管理/批量开票	"一键导入：导入失败
        key = baseKey + "0081";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Create)
            .WithLogContent("一键导入：导入失败")
            .Build();
        map.Add(key, value);

        //1011020082 创建	开票管理/批量开票	一键导入：导入成功，{数量}张发票，金额(含税)：{价税合计}元
        key = baseKey + "0082";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Create)
            .WithLogContent("一键导入：导入成功，{数量}张发票，金额(含税)：{价税合计}元")
            .Build();
        map.Add(key, value);


        //1011020091  创建	开票管理/批量开票 税局模板导入：导入失败
        key = baseKey + "0091";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Create)
            .WithLogContent("税局模板导入：导入失败")
            .Build();
        map.Add(key, value);

        //1011020092  创建	开票管理/批量开票 税局模板导入：导入成功，{数量}张发票，金额(含税)：{价税合计}元
        key = baseKey + "0092";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Create)
            .WithLogContent("税局模板导入：导入成功，{数量}张发票，金额(含税)：{价税合计}元")
            .Build();
        map.Add(key, value);
    }

    /// <summary>
    /// 构建快捷开票相关的操作日志映射
    /// </summary>
    /// <param name="map">操作代码映射字典</param>
    private void BuildShortcutInvoiceMap(Dictionary<string, OperationCodeMapValue> map)
    {
        var menuName = CustomClaims.InvoiceIssueManagement;
        var secondLevel = "快捷开票";

        var baseBuilder = OperationCodeMapValueBuilder.Empty()
            .WithMenuName(menuName)
            .WithFirstLevel(menuName)
            .WithSecondLevel(secondLevel);

        string baseKey = "101104";

        //101104001 访问	开票管理/快捷开票	访问页面
        var key = baseKey + "001";
        var value = baseBuilder.Clone()
            .WithOperationType(OperationType.View)
            .WithLogContent("访问页面")
            .Build();
        map.Add(key, value);

        //101104003 更新 开票管理/快捷开票	编辑快捷方式：{快捷开票名称}
        key = baseKey + "003";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Update)
            .WithLogContent("编辑快捷方式：{快捷开票名称}")
            .Build();
        map.Add(key, value);

        //101104004 删除 开票管理/快捷开票	删除快捷方式：{快捷开票名称}
        key = baseKey + "004";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Delete)
            .WithLogContent("删除快捷方式：{快捷开票名称}")
            .Build();
        map.Add(key, value);

        //101104006 删除	开票管理/快捷开票	删除{数量}个快捷方式：{快捷开票名称}、{快捷开票名称}
        key = baseKey + "006";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Delete)
            .WithLogContent("删除{数量}个快捷方式：{快捷开票名称}、{快捷开票名称}")
            .Build();
        map.Add(key, value);

        //101104007 创建	开票管理/快捷开票	新增快捷方式：{快捷开票名称}
        key = baseKey + "007";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Create)
            .WithLogContent("新增快捷方式：{快捷开票名称}")
            .Build();
        map.Add(key, value);
    }

    /// <summary>
    /// 构建扫码开票相关的操作日志映射
    /// </summary>
    /// <param name="map">操作代码映射字典</param>
    private void BuildReceiptInvoiceMap(Dictionary<string, OperationCodeMapValue> map)
    {
        var menuName = CustomClaims.InvoiceIssueManagement;
        var secondLevel = "扫码开票";

        var baseBuilder = OperationCodeMapValueBuilder.Empty()
            .WithMenuName(menuName)
            .WithFirstLevel(menuName)
            .WithSecondLevel(secondLevel);

        string baseKey = "101105";

        //编辑、新增、下载 不需要区分小票状态


        //101105010 更新 开票管理/ 扫码开票   编辑备注信息：{ 备注内容}
        var key = baseKey + "010";
        var value = baseBuilder.Clone()
            .WithOperationType(OperationType.Update)
            .WithLogContent("编辑备注信息：{备注内容}")
            .Build();
        map.Add(key, value);

        //101105011 创建 开票管理/ 扫码开票   新增小票
        key = baseKey + "011";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Create)
            .WithLogContent("新增小票")
            .Build();
        map.Add(key, value);

        BuildReceiptInvoiceWaitForIssueMap(map);
        BuildReceiptInvoiceIssuedMap(map);
        BuildReceiptInvoiceIssueFailedMap(map);
    }

    /// <summary>
    /// 构建扫码开票-待开具相关的操作日志映射
    /// </summary>
    /// <param name="map">操作代码映射字典</param>
    private void BuildReceiptInvoiceWaitForIssueMap(Dictionary<string, OperationCodeMapValue> map)
    {
        var menuName = CustomClaims.InvoiceIssueManagement;
        var secondLevel = "扫码开票";
        var thirdLevel = "待开具";

        var baseBuilder = OperationCodeMapValueBuilder.Empty()
            .WithMenuName(menuName)
            .WithFirstLevel(menuName)
            .WithSecondLevel(secondLevel)
            .WithThirdLevel(thirdLevel);

        string baseKey = "101105";

        //101105001 访问	开票管理/扫码开票/待开具	访问页面
        var key = baseKey + "001";
        var value = baseBuilder.Clone()
            .WithOperationType(OperationType.View)
            .WithLogContent("访问页面")
            .Build();
        map.Add(key, value);


        //101105003 读取	 开票管理/扫码开票	下载{创建时间}创建的小票
        key = baseKey + "003";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Read)
            .WithLogContent("下载{创建时间}创建的小票")
            .Build();
        map.Add(key, value);

        //101105004 读取 	开票管理/扫码开票/待开具	复制{创建时间}创建的小票
        key = baseKey + "004";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Read)
            .WithLogContent("复制{创建时间}创建的小票")
            .Build();
        map.Add(key, value);

        //101105005 删除	 开票管理/扫码开票/待开具	删除{创建时间}创建的小票
        key = baseKey + "005";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Delete)
            .WithLogContent("删除{创建时间}创建的小票")
            .Build();
        map.Add(key, value);

        //101105007 删除	 开票管理/扫码开票/待开具	删除{数量}张小票
        key = baseKey + "007";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Delete)
            .WithLogContent("删除{数量}张小票")
            .Build();
        map.Add(key, value);

        /*
        101105009 读取	开票管理/扫码开票/待开具	打印小票
        */

        key = baseKey + "009";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Read)
            .WithLogContent("打印小票")
            .Build();
        map.Add(key, value);
    }

    /// <summary>
    /// 构建扫码开票-开票失败相关的操作日志映射
    /// </summary>
    /// <param name="map">操作代码映射字典</param>
    private void BuildReceiptInvoiceIssueFailedMap(Dictionary<string, OperationCodeMapValue> map)
    {
        var menuName = CustomClaims.InvoiceIssueManagement;
        var secondLevel = "扫码开票";
        var thirdLevel = "开票失败";

        var baseBuilder = OperationCodeMapValueBuilder.Empty()
            .WithMenuName(menuName)
            .WithFirstLevel(menuName)
            .WithSecondLevel(secondLevel)
            .WithThirdLevel(thirdLevel);

        string baseKey = "101105";

        /*
        101105012		访问	开票管理/扫码开票/开具失败	访问页面
        101105013		读取	开票管理/扫码开票/开具失败	复制{创建时间}创建的小票
        101105014		删除	开票管理/扫码开票/开具失败	删除{创建时间}创建的小票
        101105016		删除	开票管理/扫码开票/开具失败	删除{数量}张小票
        101105017		更新	开票管理/扫码开票/开具失败	编辑备注信息：{备注内容}
        101105018		创建	开票管理/扫码开票/开具失败	新增小票
        */

        string key = baseKey + "012";
        var value = baseBuilder.Clone()
            .WithOperationType(OperationType.View)
            .WithLogContent("访问页面")
            .Build();
        map.Add(key, value);

        key = baseKey + "013";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Read)
            .WithLogContent("复制{创建时间}创建的小票")
            .Build();
        map.Add(key, value);

        key = baseKey + "014";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Delete)
            .WithLogContent("删除{创建时间}创建的小票")
            .Build();
        map.Add(key, value);

        key = baseKey + "016";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Delete)
            .WithLogContent("删除{数量}张小票")
            .Build();
        map.Add(key, value);
    }

    /// <summary>
    /// 构建扫码开票-已开具相关的操作日志映射    
    /// </summary>
    /// <param name="map">操作代码映射字典</param>
    private void BuildReceiptInvoiceIssuedMap(Dictionary<string, OperationCodeMapValue> map)
    {
        var menuName = CustomClaims.InvoiceIssueManagement;
        var secondLevel = "扫码开票";
        var thirdLevel = "已开具";

        var baseBuilder = OperationCodeMapValueBuilder.Empty()
            .WithMenuName(menuName)
            .WithFirstLevel(menuName)
            .WithSecondLevel(secondLevel)
            .WithThirdLevel(thirdLevel);

        /*
        101105019		访问	开票管理/扫码开票/已开具	访问页面
        101105021		读取	开票管理/扫码开票/已开具	复制发票：{数电票号码}
        */
        //101105019 访问 开票管理/扫码开票/已开具 访问页面
        var key = "101105019";
        var value = baseBuilder.Clone()
            .WithOperationType(OperationType.View)
            .WithLogContent("访问页面")
            .Build();
        map.Add(key, value);

        //101105021 读取 开票管理/扫码开票/已开具 复制发票：{数电票号码}
        key = "101105021";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Read)
            .WithLogContent("复制发票：{数电票号码}")
            .Build();
        map.Add(key, value);
    }

    /// <summary>
    /// 构建桌牌开票相关的操作日志映射
    /// </summary>
    /// <param name="map">操作代码映射字典</param>
    private void BuildTableScanInvoiceMap(Dictionary<string, OperationCodeMapValue> map)
    {
        var menuName = CustomClaims.InvoiceIssueManagement;
        var secondLevel = "桌牌开票";

        var baseBuilder = OperationCodeMapValueBuilder.Empty()
            .WithMenuName(menuName)
            .WithFirstLevel(menuName)
            .WithSecondLevel(secondLevel);

        string baseKey = "101106";

        /*
        101106001		访问	开票管理/桌牌开票	访问页面
        101106002		读取	开票管理/桌牌开票	选择{客户名称}去开票
        101106003		删除	开票管理/桌牌开票	删除桌牌客户：{客户名称}
        101106005		删除	开票管理/桌牌开票	删除{数量}个桌牌客户
        101106006		创建	开票管理/桌牌开票	保存至客户信息：{客户名称}
        101106008		读取	开票管理/桌牌开票	下载桌牌二维码
        */
        //101106001 访问 开票管理/桌牌开票 访问页面
        var key = baseKey + "001";
        var value = baseBuilder.Clone()
            .WithOperationType(OperationType.View)
            .WithLogContent("访问页面")
            .Build();
        map.Add(key, value);

        //101106002 读取 开票管理/桌牌开票 选择{客户名称}去开票
        key = baseKey + "002";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Read)
            .WithLogContent("选择{客户名称}去开票")
            .Build();
        map.Add(key, value);

        //101106003 删除 开票管理/桌牌开票 删除桌牌客户：{客户名称}
        key = baseKey + "003";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Delete)
            .WithLogContent("删除桌牌客户：{客户名称}")
            .Build();
        map.Add(key, value);

        //101106005 删除 开票管理/桌牌开票 删除{数量}个桌牌客户
        key = baseKey + "005";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Delete)
            .WithLogContent("删除{数量}个桌牌客户：{客户名称}")
            .Build();
        map.Add(key, value);

        //101106006 创建 开票管理/桌牌开票 保存至客户信息：{客户名称}
        key = baseKey + "006";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Create)
            .WithLogContent("保存至客户信息：{客户名称}")
            .Build();
        map.Add(key, value);

        //101106008 读取 开票管理/桌牌开票 下载桌牌二维码
        key = baseKey + "008";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Read)
            .WithLogContent("下载桌牌二维码")
            .Build();
        map.Add(key, value);
    }

    /// <summary>
    /// 构建数电认证相关的操作日志映射
    /// </summary>
    /// <param name="map">操作代码映射字典</param>
    private void BuildDigitalAuthMap(Dictionary<string, OperationCodeMapValue> map)
    {
        var menuName = CustomClaims.InvoiceIssueManagement;
        var secondLevel = "数电认证";

        var baseBuilder = OperationCodeMapValueBuilder.Empty()
            .WithMenuName(menuName)
            .WithFirstLevel(menuName)
            .WithSecondLevel(secondLevel);

        string baseKey = "101107";

        /*
        101107001		更新	开票管理/数电认证	电子税务局APP认证成功
        101107002		更新	开票管理/数电认证	个税APP认证成功
        101107003		读取	开票管理/数电认证	复制长期链接
        */
        //101107001 更新 开票管理/数电认证 电子税务局APP认证成功
        var key = baseKey + "001";
        var value = baseBuilder.Clone()
            .WithOperationType(OperationType.Update)
            .WithLogContent("电子税务局APP认证成功")
            .Build();
        map.Add(key, value);

        //101107002 更新 开票管理/数电认证 个税APP认证成功
        key = baseKey + "002";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Update)
            .WithLogContent("个税APP认证成功")
            .Build();
        map.Add(key, value);

        //101107003 读取 开票管理/数电认证 复制长期链接
        key = baseKey + "003";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Read)
            .WithLogContent("复制长期链接")
            .Build();
        map.Add(key, value);
    }

    /// <summary>
    /// 构建红票管理相关的操作日志映射
    /// </summary>
    /// <param name="map">操作代码映射字典</param>
    private void BuildRedInvoiceManagementMap(Dictionary<string, OperationCodeMapValue> map)
    {
        BuildAddRedInvoiceMap(map);
        BuildRedInvoiceDetailMap(map);
    }

    /// <summary>
    /// 构建新增红字确认单相关的操作日志映射
    /// </summary>
    /// <param name="map">操作代码映射字典</param>
    private void BuildAddRedInvoiceMap(Dictionary<string, OperationCodeMapValue> map)
    {
        var menuName = CustomClaims.RedInvoiceManagement;
        var secondLevel = "新增红字确认单";

        var baseBuilder = OperationCodeMapValueBuilder.Empty()
            .WithMenuName(menuName)
            .WithFirstLevel(menuName)
            .WithSecondLevel(secondLevel);

        string baseKey = "102101";

        /*
        102101001		访问	红票管理/新增红字确认单	访问页面
        102101003		读取	红票管理/新增红字确认单	选择发票：{数电票号码}
        1021010041      更新	红票管理/新增红字确认单	从数电平台同步成功，开票日期：{开始日期}至{结束日期}
        1021010042      更新	红票管理/新增红字确认单	 从数电平台同步失败，开票日期：{开始日期}至{结束日期}
        102101007		访问	红票管理/新增红字确认单	访问历史开具记录
        102101008		创建	红票管理/新增红字确认单	{发票号码/数电票号码}新增红字发票
        */
        var key = baseKey + "001";
        var value = baseBuilder.Clone()
            .WithOperationType(OperationType.View)
            .WithLogContent("访问页面")
            .Build();
        map.Add(key, value);

        key = baseKey + "003";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Read)
            .WithLogContent("选择发票：{数电票号码}")
            .Build();
        map.Add(key, value);

        key = baseKey + "0041";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Update)
            .WithLogContent("从数电平台同步成功，开票日期：{开始日期}至{结束日期}")
            .Build();
        map.Add(key, value);

        key = baseKey + "0042";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Update)
            .WithLogContent("从数电平台同步失败，开票日期：{开始日期}至{结束日期}")
            .Build();
        map.Add(key, value);

        key = baseKey + "007";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.View)
            .WithLogContent("访问历史开具记录")
            .Build();
        map.Add(key, value);

        key = baseKey + "008";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Create)
            .WithLogContent("{发票号码/数电票号码}新增红字发票")
            .Build();
        map.Add(key, value);
    }

    /// <summary>
    /// 构建红字确认信息相关的操作日志映射
    /// </summary>
    /// <param name="map">操作代码映射字典</param>
    private void BuildRedInvoiceDetailMap(Dictionary<string, OperationCodeMapValue> map)
    {
        var menuName = CustomClaims.RedInvoiceManagement;
        var secondLevel = "红字确认信息";

        var baseBuilder = OperationCodeMapValueBuilder.Empty()
            .WithMenuName(menuName)
            .WithFirstLevel(menuName)
            .WithSecondLevel(secondLevel);

        string baseKey = "102102";

        /*
        102102001		访问	红票管理/红字确认信息	访问页面
        102102003		更新	红票管理/红字确认信息	拒绝红字确认单：{红字通知单编号}
        102102004		更新	红票管理/红字确认信息	确认红字确认单：{红字通知单编号}
        102102005		更新	红票管理/红字确认信息	撤销红字确认单：{红字通知单编号}
        102102006		读取	红票管理/红字确认信息	下载红字确认单
        102102008		读取	红票管理/红字确认信息	导出红字确认单
        102102009		读取	红票管理/红字确认信息	批量下载红字确认单
        1021020101（成功）更新	红票管理/红字确认信息	从数电平台同步成功，开票日期：{开始日期}至{结束日期}
        1021020102（失败）更新	红票管理/红字确认信息	从数电平台同步失败，开票日期：{开始日期}至{结束日期}
        */

        var key = baseKey + "001";
        var value = baseBuilder.Clone()
            .WithOperationType(OperationType.View)
            .WithLogContent("访问页面")
            .Build();
        map.Add(key, value);

        key = baseKey + "003";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Update)
            .WithLogContent("拒绝红字确认单：{红字通知单编号}")
            .Build();
        map.Add(key, value);

        key = baseKey + "004";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Update)
            .WithLogContent("确认红字确认单：{红字通知单编号}")
            .Build();
        map.Add(key, value);

        key = baseKey + "005";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Update)
            .WithLogContent("撤销红字确认单：{红字通知单编号}")
            .Build();
        map.Add(key, value);

        key = baseKey + "006";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Read)
            .WithLogContent("下载红字确认单")
            .Build();
        map.Add(key, value);

        key = baseKey + "008";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Read)
            .WithLogContent("导出红字确认单")
            .Build();
        map.Add(key, value);

        key = baseKey + "009";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Read)
            .WithLogContent("批量下载红字确认单")
            .Build();
        map.Add(key, value);

        key = baseKey + "0101";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Update)
            .WithLogContent("从数电平台同步成功，开票日期：{开始日期}至{结束日期}")
            .Build();
        map.Add(key, value);

        key = baseKey + "0102";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Update)
            .WithLogContent("从数电平台同步失败，开票日期：{开始日期}至{结束日期}")
            .Build();
        map.Add(key, value);
    }

    /// <summary>
    /// 构建查询统计相关的操作日志映射
    /// </summary>
    /// <param name="map">操作代码映射字典</param>
    private void BuildStatisticsMap(Dictionary<string, OperationCodeMapValue> map)
    {
        BuildStatisticsIssueRecordMap(map);
        BuildStatisticsIssueStatisticsMap(map);
        BuildStatisticsArchivedInvoiceMap(map);
    }

    /// <summary>
    /// 构建开票记录相关的操作日志映射
    /// </summary>
    /// <param name="map">操作代码映射字典</param>
    private void BuildStatisticsIssueRecordMap(Dictionary<string, OperationCodeMapValue> map)
    {
        //全部
        BuildStatisticsIssueRecordAllMap(map);
        //开票中
        BuildStatisticsIssueRecordIssuingMap(map);
        //开票失败
        BuildStatisticsIssueRecordFailedMap(map);
        //开票成功
        BuildStatisticsIssueRecordSuccessMap(map);
    }

    /// <summary>
    /// 构建开票记录-全部相关的操作日志映射
    /// </summary>
    /// <param name="map">操作代码映射字典</param>
    private void BuildStatisticsIssueRecordAllMap(Dictionary<string, OperationCodeMapValue> map)
    {
        var menuName = CustomClaims.Statistics;
        var secondLevel = "开票记录";
        var thirdLevel = "全部";

        var baseBuilder = OperationCodeMapValueBuilder.Empty()
            .WithMenuName(menuName)
            .WithFirstLevel(menuName)
            .WithSecondLevel(secondLevel)
            .WithThirdLevel(thirdLevel);

        string baseKey = "103101";

        /*
        103101001		访问	查询统计/开票记录/全部	访问页面
        103101003		读取	查询统计/开票记录/全部	下载{发票文件类型}发票：{数电票号码}
        103101004		更新	查询统计/开票记录/全部	交付发票：{数电票号码}
        103101005		读取	查询统计/开票记录/全部	导出开票记录
        */

        var key = baseKey + "001";
        var value = baseBuilder.Clone()
            .WithOperationType(OperationType.View)
            .WithLogContent("访问页面")
            .Build();
        map.Add(key, value);

        key = baseKey + "003";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Read)
            .WithLogContent("下载{发票文件类型}发票：{数电票号码}")
            .Build();
        map.Add(key, value);

        key = baseKey + "004";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Update)
            .WithLogContent("交付发票：{数电票号码}")
            .Build();
        map.Add(key, value);

        key = baseKey + "005";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Read)
            .WithLogContent("导出开票记录")
            .Build();
        map.Add(key, value);
    }

    /// <summary>
    /// 构建开票记录-开票中相关的操作日志映射
    /// </summary>
    /// <param name="map">操作代码映射字典</param>
    private void BuildStatisticsIssueRecordIssuingMap(Dictionary<string, OperationCodeMapValue> map)
    {
        var menuName = CustomClaims.Statistics;
        var secondLevel = "开票记录";
        var thirdLevel = "开票中";

        var baseBuilder = OperationCodeMapValueBuilder.Empty()
            .WithMenuName(menuName)
            .WithFirstLevel(menuName)
            .WithSecondLevel(secondLevel)
            .WithThirdLevel(thirdLevel);

        string baseKey = "103101";
        /*
        103101006		访问	查询统计/开票记录/开票中	访问页面
        103101009		读取	查询统计/开票记录/开票中	导出开票记录
        */
        var key = baseKey + "006";
        var value = baseBuilder.Clone()
            .WithOperationType(OperationType.View)
            .WithLogContent("访问页面")
            .Build();
        map.Add(key, value);

        key = baseKey + "009";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Read)
            .WithLogContent("导出开票记录")
            .Build();
        map.Add(key, value);
    }

    /// <summary>
    /// 构建开票记录-开票失败相关的操作日志映射
    /// </summary>
    /// <param name="map">操作代码映射字典</param>
    private void BuildStatisticsIssueRecordFailedMap(Dictionary<string, OperationCodeMapValue> map)
    {
        var menuName = CustomClaims.Statistics;
        var secondLevel = "开票记录";
        var thirdLevel = "开票失败";

        var baseBuilder = OperationCodeMapValueBuilder.Empty()
            .WithMenuName(menuName)
            .WithFirstLevel(menuName)
            .WithSecondLevel(secondLevel)
            .WithThirdLevel(thirdLevel);

        string baseKey = "103101";
        /*
        103101010		访问	查询统计/开票记录/开票失败	访问页面
        103101011		更新	查询统计/开票记录/开票失败	重新开票：订单号{订单号}
        103101014		读取	查询统计/开票记录/开票失败	导出开票记录
        */
        var key = baseKey + "010";
        var value = baseBuilder.Clone()
            .WithOperationType(OperationType.View)
            .WithLogContent("访问页面")
            .Build();
        map.Add(key, value);

        key = baseKey + "011";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Update)
            .WithLogContent("重新开票：订单号{订单号}")
            .Build();
        map.Add(key, value);

        key = baseKey + "014";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Read)
            .WithLogContent("导出开票记录")
            .Build();
        map.Add(key, value);
    }

    /// <summary>
    /// 构建开票记录-开票成功相关的操作日志映射
    /// </summary>
    /// <param name="map">操作代码映射字典</param>
    private void BuildStatisticsIssueRecordSuccessMap(Dictionary<string, OperationCodeMapValue> map)
    {
        var menuName = CustomClaims.Statistics;
        var secondLevel = "开票记录";
        var thirdLevel = "开票成功";

        var baseBuilder = OperationCodeMapValueBuilder.Empty()
            .WithMenuName(menuName)
            .WithFirstLevel(menuName)
            .WithSecondLevel(secondLevel)
            .WithThirdLevel(thirdLevel);

        string baseKey = "103101";
        /*
        103101015		访问	查询统计/开票记录/开票成功	访问页面
        103101017		读取	查询统计/开票记录/开票成功	下载{发票文件类型}发票：{数电票号码}
        1031010181		更新	查询统计/开票记录/开票成功	向{手机号码}短信交付{数电票号码}
        1031010182		更新	查询统计/开票记录/开票成功	向{邮箱地址}邮箱交付{数电票号码}
        103101020		读取	查询统计/开票记录/开票成功	批量下载{发票文件类型}发票
        103101021		读取	查询统计/开票记录/开票成功	打印发票
        103101022		读取	查询统计/开票记录/开票成功	导出开票记录
        1031010241		更新	查询统计/开票记录/开票成功	向{手机号码}批量短信交付{数电票号码}
        1031010242		更新	查询统计/开票记录/开票成功	向{邮箱地址}批量邮箱交付{数电票号码}
        */

        var key = baseKey + "015";
        var value = baseBuilder.Clone()
            .WithOperationType(OperationType.View)
            .WithLogContent("访问页面")
            .Build();
        map.Add(key, value);

        key = baseKey + "017";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Read)
            .WithLogContent("下载{发票文件类型}发票：{数电票号码}")
            .Build();
        map.Add(key, value);

        key = baseKey + "0181";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Update)
            .WithLogContent("向{手机号码}短信交付{数电票号码}")
            .Build();
        map.Add(key, value);

        key = baseKey + "0182";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Update)
            .WithLogContent("向{邮箱地址}邮箱交付{数电票号码}")
            .Build();
        map.Add(key, value);

        key = baseKey + "020";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Read)
            .WithLogContent("批量下载{发票文件类型}发票")
            .Build();
        map.Add(key, value);

        key = baseKey + "021";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Read)
            .WithLogContent("打印发票")
            .Build();
        map.Add(key, value);

        key = baseKey + "022";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Read)
            .WithLogContent("导出开票记录")
            .Build();
        map.Add(key, value);

        key = baseKey + "0241";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Update)
            .WithLogContent("向{手机号码}短信交付{数量}张发票，数电票号码：{数电票号码}")
            .Build();
        map.Add(key, value);

        key = baseKey + "0242";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Update)
            .WithLogContent("向{邮箱地址}邮箱交付{数量}张发票，数电票号码：{数电票号码}")
            .Build();
        map.Add(key, value);
    }

    /// <summary>
    /// 构建开票统计相关的操作日志映射
    /// </summary>
    /// <param name="map">操作代码映射字典</param>
    private void BuildStatisticsIssueStatisticsMap(Dictionary<string, OperationCodeMapValue> map)
    {
        var menuName = CustomClaims.Statistics;
        var secondLevel = "开票统计";

        var baseBuilder = OperationCodeMapValueBuilder.Empty()
            .WithMenuName(menuName)
            .WithFirstLevel(menuName)
            .WithSecondLevel(secondLevel);

        string baseKey = "103102";
        //按发票统计
        /*
        103102001		访问	查询统计/开票统计/按发票统计	访问页面
        103102003		读取	查询统计/开票统计/按发票统计	导出
        */

        var key = baseKey + "001";
        var value = baseBuilder.Clone()
            .WithOperationType(OperationType.View)
            .WithLogContent("访问页面")
            .Build();
        map.Add(key, value);

        key = baseKey + "003";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Read)
            .WithLogContent("导出")
            .Build();
        map.Add(key, value);

        //按客户统计
        /*
        103102004		访问	查询统计/开票统计/按客户统计	访问页面
        103102006		读取	查询统计/开票统计/按客户统计	导出
        */

        key = baseKey + "004";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.View)
            .WithLogContent("访问页面")
            .Build();
        map.Add(key, value);

        key = baseKey + "006";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Read)
            .WithLogContent("导出")
            .Build();
        map.Add(key, value);
    }

    /// <summary>
    /// 构建已开票相关的操作日志映射
    /// </summary>
    /// <param name="map">操作代码映射字典</param>
    private void BuildStatisticsArchivedInvoiceMap(Dictionary<string, OperationCodeMapValue> map)
    {
        var menuName = CustomClaims.Statistics;
        var secondLevel = "发票池";

        var baseBuilder = OperationCodeMapValueBuilder.Empty()
            .WithMenuName(menuName)
            .WithFirstLevel(menuName)
            .WithSecondLevel(secondLevel);

        string baseKey = "103103";
        /*
        1031030101		更新	查询统计/发票池	"从数电平台同步成功，开票日期：{开始日期}至{结束日期}"
        1031030102		更新	查询统计/发票池	"从数电平台同步失败，开票日期：{开始日期}至{结束日期}"
        */

        var key = baseKey + "0101";
        var value = baseBuilder.Clone()
            .WithOperationType(OperationType.Update)
            .WithLogContent("从数电平台同步成功，开票日期：{开始日期}至{结束日期}")
            .Build();
        map.Add(key, value);

        key = baseKey + "0102";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Update)
            .WithLogContent("从数电平台同步失败，开票日期：{开始日期}至{结束日期}")
            .Build();
        map.Add(key, value);

        //销项发票
        BuildStatisticsArchivedInvoiceSalesMap(map);
        //进销发票
        BuildStatisticsArchivedInvoicePurchaseMap(map);
    }

    /// <summary>
    /// 构建已开票-销项发票相关的操作日志映射
    /// </summary>
    /// <param name="map">操作代码映射字典</param>
    private void BuildStatisticsArchivedInvoiceSalesMap(Dictionary<string, OperationCodeMapValue> map)
    {
        var menuName = CustomClaims.Statistics;
        var secondLevel = "发票池";
        var thirdLevel = "销项发票";

        var baseBuilder = OperationCodeMapValueBuilder.Empty()
            .WithMenuName(menuName)
            .WithFirstLevel(menuName)
            .WithSecondLevel(secondLevel)
            .WithThirdLevel(thirdLevel);

        string baseKey = "103103";
        /*
        103103001		访问	查询统计/发票池/销项发票	访问页面
        103103003		读取	查询统计/发票池/销项发票	下载{发票文件类型}发票：{数电票号码}
        103103004		更新	查询统计/发票池/销项发票	短信交付
        103103005		更新	查询统计/发票池/销项发票	邮箱交付
        103103007		读取	查询统计/发票池/销项发票	导出
        103103008		读取	查询统计/发票池/销项发票	打印
        103103009		读取	查询统计/发票池/销项发票	批量下载{发票文件类型}发票
        */

        var key = baseKey + "001";
        var value = baseBuilder.Clone()
            .WithOperationType(OperationType.View)
            .WithLogContent("访问页面")
            .Build();
        map.Add(key, value);

        key = baseKey + "003";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Read)
            .WithLogContent("下载{发票文件类型}发票：{数电票号码}")
            .Build();
        map.Add(key, value);

        key = baseKey + "004";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Update)
            .WithLogContent("向{手机号码}短信交付{发票号码}")
            .Build();
        map.Add(key, value);

        key = baseKey + "005";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Update)
            .WithLogContent("向{邮箱地址}邮箱交付{发票号码}")
            .Build();
        map.Add(key, value);

        key = baseKey + "007";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Read)
            .WithLogContent("导出")
            .Build();
        map.Add(key, value);

        key = baseKey + "008";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Read)
            .WithLogContent("打印")
            .Build();
        map.Add(key, value);

        key = baseKey + "009";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Read)
            .WithLogContent("批量下载{发票文件类型}发票")
            .Build();
        map.Add(key, value);
    }

    /// <summary>
    /// 构建已开票-进项发票相关的操作日志映射
    /// </summary>
    /// <param name="map">操作代码映射字典</param>
    private void BuildStatisticsArchivedInvoicePurchaseMap(Dictionary<string, OperationCodeMapValue> map)
    {
        var menuName = CustomClaims.Statistics;
        var secondLevel = "发票池";
        var thirdLevel = "进项发票";

        var baseBuilder = OperationCodeMapValueBuilder.Empty()
            .WithMenuName(menuName)
            .WithFirstLevel(menuName)
            .WithSecondLevel(secondLevel)
            .WithThirdLevel(thirdLevel);

        string baseKey = "103103";
        /*
        103103012		访问	查询统计/发票池/进项发票	访问页面
        103103014		读取	查询统计/发票池/进项发票	下载{发票文件类型}发票：{数电票号码}
        103103016		读取	查询统计/发票池/进项发票	导出
        103103017		读取	查询统计/发票池/进项发票	打印
        103103018		读取	查询统计/发票池/进项发票	批量下载{发票文件类型}发票
        */

        var key = baseKey + "012";
        var value = baseBuilder.Clone()
            .WithOperationType(OperationType.View)
            .WithLogContent("访问页面")
            .Build();
        map.Add(key, value);

        key = baseKey + "014";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Read)
            .WithLogContent("下载{发票文件类型}发票：{数电票号码}")
            .Build();
        map.Add(key, value);

        key = baseKey + "016";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Read)
            .WithLogContent("导出")
            .Build();
        map.Add(key, value);

        key = baseKey + "017";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Read)
            .WithLogContent("打印")
            .Build();
        map.Add(key, value);

        key = baseKey + "018";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Read)
            .WithLogContent("批量下载{发票文件类型}发票")
            .Build();
        map.Add(key, value);
    }

    /// <summary>
    /// 构建客户管理相关的操作日志映射
    /// </summary>
    /// <param name="map">操作代码映射字典</param>
    private void BuildCustomerManagementMap(Dictionary<string, OperationCodeMapValue> map)
    {
        var menuName = CustomClaims.CustomerManagement;

        var baseBuilder = OperationCodeMapValueBuilder.Empty()
            .WithMenuName(menuName)
            .WithFirstLevel(menuName);

        string baseKey = "104";

        /*
        104001		访问	客户信息	访问页面
        104002		更新	客户信息	编辑客户：{客户名称}
        104003		删除	客户信息	删除客户：{客户名称}
        1040051		更新	客户信息	从数电平台新增同步成功
        1040052		更新	客户信息	从数电平台新增同步失败
        1040061		更新	客户信息	从数电平台覆盖同步成功
        1040062		更新	客户信息	从数电平台覆盖同步失败
        1040071		更新	客户信息	从历史发票同步成功
        1040072		更新	客户信息	从历史发票同步失败
        104008		读取	客户信息	下载客户导入错误文件
        104009		创建	客户信息	客户导入成功
        104010		读取	客户信息	导出客户
        104011		删除	客户信息	批量删除{数量}个客户: {客户名称}
        104012		创建	客户信息	新增客户：{客户名称}
        104013		创建	客户信息	新增客户类别：{客户类别}
        104014		更新	客户信息	编辑客户类别：{旧客户类别}>{新客户类别}
        104015		更新	客户信息	移动客户类别：{客户类别}
        104016		删除	客户信息	删除客户类别：{客户类别}
        */

        string key;
        OperationCodeMapValue value;

        key = baseKey + "001";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Read)
            .WithLogContent("访问页面")
            .Build();
        map.Add(key, value);

        key = baseKey + "002";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Update)
            .WithLogContent("编辑客户：{客户名称}")
            .Build();
        map.Add(key, value);

        key = baseKey + "003";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Delete)
            .WithLogContent("删除客户：{客户名称}")
            .Build();
        map.Add(key, value);

        key = baseKey + "0051";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Update)
            .WithLogContent("从数电平台新增同步成功")
            .Build();
        map.Add(key, value);

        key = baseKey + "0052";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Update)
            .WithLogContent("从数电平台新增同步失败")
            .Build();
        map.Add(key, value);

        key = baseKey + "0061";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Update)
            .WithLogContent("从数电平台覆盖同步成功")
            .Build();
        map.Add(key, value);

        key = baseKey + "0062";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Update)
            .WithLogContent("从数电平台覆盖同步失败")
            .Build();
        map.Add(key, value);

        key = baseKey + "0071";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Update)
            .WithLogContent("从历史发票同步成功")
            .Build();
        map.Add(key, value);

        key = baseKey + "0072";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Update)
            .WithLogContent("从历史发票同步失败")
            .Build();
        map.Add(key, value);

        key = baseKey + "008";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Read)
            .WithLogContent("下载客户导入错误文件")
            .Build();
        map.Add(key, value);

        key = baseKey + "009";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Create)
            .WithLogContent("客户导入成功")
            .Build();
        map.Add(key, value);

        key = baseKey + "010";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Read)
            .WithLogContent("导出客户")
            .Build();
        map.Add(key, value);

        key = baseKey + "011";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Delete)
            .WithLogContent("批量删除{数量}个客户：{客户名称}")
            .Build();
        map.Add(key, value);

        key = baseKey + "012";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Create)
            .WithLogContent("新增客户：{客户名称}")
            .Build();
        map.Add(key, value);

        key = baseKey + "013";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Create)
            .WithLogContent("新增客户类别：{客户类别}")
            .Build();
        map.Add(key, value);

        key = baseKey + "014";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Update)
            .WithLogContent("编辑客户类别：{旧客户类别}>{新客户类别}")
            .Build();
        map.Add(key, value);

        key = baseKey + "015";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Update)
            .WithLogContent("移动客户类别：{客户类别}")
            .Build();
        map.Add(key, value);

        key = baseKey + "016";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Delete)
            .WithLogContent("删除客户类别：{客户类别}")
            .Build();
        map.Add(key, value);
    }

    /// <summary>
    /// 构建项目管理相关的操作日志映射
    /// </summary>
    /// <param name="map">操作代码映射字典</param>
    private void BuildInvoiceItemManagementMap(Dictionary<string, OperationCodeMapValue> map)
    {
        var menuName = CustomClaims.InvoiceItemManagement;

        var baseBuilder = OperationCodeMapValueBuilder.Empty()
            .WithMenuName(menuName)
            .WithFirstLevel(menuName);

        string baseKey = "105";
        /*
        105001		访问	项目信息	访问页面
        105002		更新	项目信息	编辑项目：{项目名称}
        105003		删除	项目信息	删除项目：{项目名称}
        1050051		更新	项目信息	从数电平台同步成功
        1050052		更新	项目信息	从数电平台同步失败
        1050061		更新	项目信息	从历史发票同步成功
        1050062		更新	项目信息	从历史发票同步失败
        105007		读取	项目信息	下载项目导入错误文件
        105008		创建	项目信息	项目导入成功
        105009		读取	项目信息	导出项目
        105010		删除	项目信息	批量删除{数量}个项目：{项目名称}
        105011		创建	项目信息	新增项目：{项目名称}
        105012		创建	项目信息	新增项目类别：{项目类别}
        105013		更新	项目信息	编辑项目类别：{项目类别}
        105014		更新	项目信息	移动项目类别：{项目类别}
        105015		删除	项目信息	删除项目类别：{项目类别}
        */

        var key = baseKey + "001";
        var value = baseBuilder.Clone()
            .WithOperationType(OperationType.View)
            .WithLogContent("访问页面")
            .Build();
        map.Add(key, value);

        key = baseKey + "002";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Update)
            .WithLogContent("编辑项目：{项目名称}")
            .Build();
        map.Add(key, value);

        key = baseKey + "003";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Delete)
            .WithLogContent("删除项目：{项目名称}")
            .Build();
        map.Add(key, value);

        key = baseKey + "0051";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Update)
            .WithLogContent("从数电平台同步成功")
            .Build();
        map.Add(key, value);

        key = baseKey + "0052";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Update)
            .WithLogContent("从数电平台同步失败")
            .Build();
        map.Add(key, value);

        key = baseKey + "0061";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Update)
            .WithLogContent("从历史发票同步成功")
            .Build();
        map.Add(key, value);

        key = baseKey + "0062";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Update)
            .WithLogContent("从历史发票同步失败")
            .Build();
        map.Add(key, value);

        key = baseKey + "007";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Read)
            .WithLogContent("下载项目导入错误文件")
            .Build();
        map.Add(key, value);

        key = baseKey + "008";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Create)
            .WithLogContent("项目导入成功")
            .Build();
        map.Add(key, value);

        key = baseKey + "009";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Read)
            .WithLogContent("导出项目")
            .Build();
        map.Add(key, value);

        key = baseKey + "010";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Delete)
            .WithLogContent("批量删除{数量}个项目：{项目名称}")
            .Build();
        map.Add(key, value);

        key = baseKey + "011";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Create)
            .WithLogContent("新增项目：{项目名称}")
            .Build();
        map.Add(key, value);

        key = baseKey + "012";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Create)
            .WithLogContent("新增项目类别：{项目类别}")
            .Build();
        map.Add(key, value);

        key = baseKey + "013";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Update)
            .WithLogContent("编辑项目类别：{项目类别}")
            .Build();
        map.Add(key, value);

        key = baseKey + "014";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Update)
            .WithLogContent("移动项目类别：{项目类别}")
            .Build();
        map.Add(key, value);

        key = baseKey + "015";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Delete)
            .WithLogContent("删除项目类别：{项目类别}")
            .Build();
        map.Add(key, value);
    }

    /// <summary>
    /// 构建附加信息管理相关的操作日志映射
    /// </summary>
    /// <param name="map">操作代码映射字典</param>
    private void BuildAdditionalInformationManagementMap(Dictionary<string, OperationCodeMapValue> map)
    {
        BuildAddonInfoMap(map);
        BuildAddonTemplateMap(map);
    }

    /// <summary>
    /// 构建附加信息相关的操作日志映射
    /// </summary>
    /// <param name="map">操作代码映射字典</param>
    private void BuildAddonInfoMap(Dictionary<string, OperationCodeMapValue> map)
    {
        var menuName = CustomClaims.AdditionalInformationManagement;
        var secondLevel = "附加信息";

        var baseBuilder = OperationCodeMapValueBuilder.Empty()
            .WithMenuName(menuName)
            .WithFirstLevel(menuName)
            .WithSecondLevel(secondLevel);

        string baseKey = "106";

        /*
        106001		访问	附加信息/附加信息	访问页面
        106002		更新	附加信息/附加信息	编辑附加信息：{附加信息名称}
        106003		删除	附加信息/附加信息	删除附加信息：{附加信息名称}
        1060051		更新	附加信息/附加信息	从数电平台同步成功
        1060052		更新	附加信息/附加信息   从数电平台同步失败"
        106006		删除	附加信息/附加信息	批量删除{数量}个附加信息：{附加信息名称}
        106007		创建	附加信息/附加信息	新增附加信息：{附加信息名称}

        */
        var key = baseKey + "001";
        var value = baseBuilder.Clone()
            .WithOperationType(OperationType.View)
            .WithLogContent("访问页面")
            .Build();
        map.Add(key, value);

        key = baseKey + "002";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Update)
            .WithLogContent("编辑附加信息：{附加信息名称}")
            .Build();
        map.Add(key, value);

        key = baseKey + "003";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Delete)
            .WithLogContent("删除附加信息：{附加信息名称}")
            .Build();
        map.Add(key, value);

        key = baseKey + "0051";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Update)
            .WithLogContent("从数电平台同步成功")
            .Build();
        map.Add(key, value);

        key = baseKey + "0052";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Update)
            .WithLogContent("从数电平台同步失败")
            .Build();
        map.Add(key, value);

        key = baseKey + "006";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Delete)
            .WithLogContent("批量删除{数量}个附加信息：{附加信息名称}")
            .Build();
        map.Add(key, value);

        key = baseKey + "007";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Create)
            .WithLogContent("新增附加信息：{附加信息名称}")
            .Build();
        map.Add(key, value);
    }

    /// <summary>
    /// 构建附加信息场景模板相关的操作日志映射
    /// </summary>
    /// <param name="map">操作代码映射字典</param>
    private void BuildAddonTemplateMap(Dictionary<string, OperationCodeMapValue> map)
    {
        var menuName = CustomClaims.AdditionalInformationManagement;
        var secondLevel = "场景模板";

        var baseBuilder = OperationCodeMapValueBuilder.Empty()
            .WithMenuName(menuName)
            .WithFirstLevel(menuName)
            .WithSecondLevel(secondLevel);

        string baseKey = "106";

        /*
        106008		访问	附加信息/场景模板	访问页面
        106009		更新	附加信息/场景模板	编辑场景模板：{场景模板名称}
        106010		删除	附加信息/场景模板	删除场景模板：{场景模板名称}
        106012		删除	附加信息/场景模板	批量删除{数量}个场景模板
        106013		创建	附加信息/场景模板	新增场景模板：{场景模板名称}
        */

        var key = baseKey + "008";
        var value = baseBuilder.Clone()
            .WithOperationType(OperationType.View)
            .WithLogContent("访问页面")
            .Build();
        map.Add(key, value);

        key = baseKey + "009";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Update)
            .WithLogContent("编辑场景模板：{场景模板名称}")
            .Build();
        map.Add(key, value);

        key = baseKey + "010";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Delete)
            .WithLogContent("删除场景模板：{场景模板名称}")
            .Build();
        map.Add(key, value);

        key = baseKey + "012";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Delete)
            .WithLogContent("批量删除{数量}个场景模板")
            .Build();
        map.Add(key, value);

        key = baseKey + "013";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Create)
            .WithLogContent("新增场景模板：{场景模板名称}")
            .Build();
        map.Add(key, value);
    }

    /// <summary>
    /// 构建设置管理相关的操作日志映射
    /// </summary>
    /// <param name="map">操作代码映射字典</param>
    private void BuildSettingManagementMap(Dictionary<string, OperationCodeMapValue> map)
    {
        BuildCompanyManagementMap(map);
        BuildBasicInformationMap(map);
        BuildPermissionMap(map);
        BuildOperationLogMap(map);
    }

    /// <summary>
    /// 构建企业管理相关的操作日志映射
    /// </summary>
    /// <param name="map">操作代码映射字典</param>
    private void BuildCompanyManagementMap(Dictionary<string, OperationCodeMapValue> map)
    {
        var menuName = CustomClaims.SettingManagement;
        var secondLevel = "企业管理";

        var baseBuilder = OperationCodeMapValueBuilder.Empty()
            .WithMenuName(menuName)
            .WithFirstLevel(menuName)
            .WithSecondLevel(secondLevel);

        string baseKey = "107101";
        //107101001		访问	设置/企业管理	访问页面

        var key = baseKey + "001";
        var value = baseBuilder.Clone()
            .WithOperationType(OperationType.View)
            .WithLogContent("访问页面")
            .Build();
        map.Add(key, value);
    }

    /// <summary>
    /// 构建基础信息相关的操作日志映射
    /// </summary>
    /// <param name="map">操作代码映射字典</param>
    private void BuildBasicInformationMap(Dictionary<string, OperationCodeMapValue> map)
    {
        var menuName = CustomClaims.SettingManagement;
        var secondLevel = "基础信息";

        var baseBuilder = OperationCodeMapValueBuilder.Empty()
            .WithMenuName(menuName)
            .WithFirstLevel(menuName)
            .WithSecondLevel(secondLevel);

        string baseKey = "107102";

        /*
        107102001		访问	设置/基础信息	访问页面
        107102002		更新	设置/基础信息/公司信息	编辑公司信息
        107102003		更新	设置/基础信息/开票信息	编辑开票信息
        */

        var key = baseKey + "001";
        var value = baseBuilder.Clone()
            .WithOperationType(OperationType.View)
            .WithLogContent("访问页面")
            .Build();
        map.Add(key, value);

        key = baseKey + "002";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Update)
            .WithThirdLevel("公司信息")
            .WithLogContent("编辑公司信息")
            .Build();
        map.Add(key, value);

        key = baseKey + "003";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Update)
            .WithThirdLevel("开票信息")
            .WithLogContent("编辑开票信息")
            .Build();
        map.Add(key, value);
    }

    /// <summary>
    /// 构建权限设置相关的操作日志映射
    /// </summary>
    /// <param name="map">操作代码映射字典</param>
    private void BuildPermissionMap(Dictionary<string, OperationCodeMapValue> map)
    {
        var menuName = CustomClaims.SettingManagement;
        var secondLevel = "权限设置";

        var baseBuilder = OperationCodeMapValueBuilder.Empty()
            .WithMenuName(menuName)
            .WithFirstLevel(menuName)
            .WithSecondLevel(secondLevel);

        string baseKey = "107103";
        /*
        107103001		访问	 设置/权限设置	访问页面
        107103002       更新 设置/权限设置   企业管理员由{旧手机号码}移交给{新手机号码}
        107103003		更新	 设置/权限设置	编辑权限：{手机号码}
        107103004		删除	 设置/权限设置	删除权限：{手机号码}
        107103005		创建	 设置/权限设置	新增用户：{手机号码}
        107103006		更新	 设置/权限设置	编辑用户角色：{角色名称}
        107103007		创建	 设置/权限设置	新增用户角色：{角色名称}
        107103008		删除	 设置/权限设置	删除用户角色：{角色名称}
        */

        var key = baseKey + "001";
        var value = baseBuilder.Clone()
            .WithOperationType(OperationType.View)
            .WithLogContent("访问页面")
            .Build();
        map.Add(key, value);

        key = baseKey + "002";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Update)
            .WithLogContent("企业管理员由{旧手机号码}移交给{新手机号码}")
            .Build();
        map.Add(key, value);

        key = baseKey + "003";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Update)
            .WithLogContent("编辑权限：{手机号码}")
            .Build();
        map.Add(key, value);

        key = baseKey + "004";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Delete)
            .WithLogContent("删除权限：{手机号码}")
            .Build();
        map.Add(key, value);

        key = baseKey + "005";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Create)
            .WithLogContent("新增用户：{手机号码}")
            .Build();
        map.Add(key, value);

        key = baseKey + "006";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Update)
            .WithLogContent("编辑用户角色：{角色名称}")
            .Build();
        map.Add(key, value);

        key = baseKey + "007";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Create)
            .WithLogContent("新增用户角色：{角色名称}")
            .Build();
        map.Add(key, value);

        key = baseKey + "008";
        value = baseBuilder.Clone()
            .WithOperationType(OperationType.Delete)
            .WithLogContent("删除用户角色：{角色名称}")
            .Build();
        map.Add(key, value);
    }

    /// <summary>
    /// 构建操作日志相关的操作日志映射
    /// </summary>
    /// <param name="map">操作代码映射字典</param>
    private void BuildOperationLogMap(Dictionary<string, OperationCodeMapValue> map)
    {
        var menuName = CustomClaims.SettingManagement;
        var secondLevel = "操作日志";

        var baseBuilder = OperationCodeMapValueBuilder.Empty()
            .WithMenuName(menuName)
            .WithFirstLevel(menuName)
            .WithSecondLevel(secondLevel);

        string baseKey = "107104";
        /*
        107102001		访问	设置/操作日志	访问页面
        */

        var key = baseKey + "001";
        var value = baseBuilder.Clone()
            .WithOperationType(OperationType.View)
            .WithLogContent("访问页面")
            .Build();
        map.Add(key, value);
    }
}

