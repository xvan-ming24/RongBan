using log4net;
using System;
using System.Reflection;

namespace Common;

/// <summary>
/// Log4net 日志工具类
/// </summary>
public static class LogHelper
{
    private static bool _isInitialized = false;

    /// <summary>
    /// 初始化日志配置
    /// </summary>
    /// <param name="configFilePath">配置文件路径，默认 log4net.config</param>
    public static void Init(string configFilePath = "log4net.config")
    {
        if (!_isInitialized)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            log4net.Config.XmlConfigurator.Configure(logRepository, new FileInfo(configFilePath));
            _isInitialized = true;

            // 记录初始化信息
            Info(typeof(LogHelper), "Log4net 日志系统初始化完成");
        }
    }

    /// <summary>
    /// 获取日志实例
    /// </summary>
    private static ILog GetLogger(Type type)
    {
        if (!_isInitialized)
        {
            Init(); // 自动初始化
        }
        return LogManager.GetLogger(type);
    }

    /// <summary>
    /// 调试日志
    /// </summary>
    public static void Debug(Type type, string message)
    {
        GetLogger(type).Debug(message);
    }

    /// <summary>
    /// 信息日志
    /// </summary>
    public static void Info(Type type, string message)
    {
        GetLogger(type).Info(message);
    }

    /// <summary>
    /// 信息日志（带格式化）
    /// </summary>
    public static void InfoFormat(Type type, string format, params object[] args)
    {
        GetLogger(type).InfoFormat(format, args);
    }

    /// <summary>
    /// 警告日志
    /// </summary>
    public static void Warn(Type type, string message)
    {
        GetLogger(type).Warn(message);
    }

    /// <summary>
    /// 错误日志
    /// </summary>
    public static void Error(Type type, string message, Exception ex = null)
    {
        if (ex == null)
            GetLogger(type).Error(message);
        else
            GetLogger(type).Error(message, ex);
    }

    /// <summary>
    /// 致命错误日志
    /// </summary>
    public static void Fatal(Type type, string message, Exception ex = null)
    {
        if (ex == null)
            GetLogger(type).Fatal(message);
        else
            GetLogger(type).Fatal(message, ex);
    }

    // 简化调用的泛型方法（更常用）
    public static void Debug<T>(string message) => Debug(typeof(T), message);
    public static void Info<T>(string message) => Info(typeof(T), message);
    public static void InfoFormat<T>(string format, params object[] args) => InfoFormat(typeof(T), format, args);
    public static void Warn<T>(string message) => Warn(typeof(T), message);
    public static void Error<T>(string message, Exception ex = null) => Error(typeof(T), message, ex);
    public static void Fatal<T>(string message, Exception ex = null) => Fatal(typeof(T), message, ex);
}
