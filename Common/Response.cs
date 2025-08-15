

/// <summary>
/// 响应模型
/// </summary>
/// <typeparam name="T"></typeparam>
public class Response<T>
{
    public int StatusCode { get; set; }

    public string Message { get; set; }
    public T Data { get; set; }

    // 定义常用的状态码常量
    public const int StatusOk = 200;
    public const int StatusNotFound = 404;
    public const int StatusInternalServerError = 500;

    // 构造函数方便初始化
    public Response(int statusCode, string message, T data)
    {
        StatusCode = statusCode;
        Message = message;
        Data = data;
    }

    // 成功响应的静态方法
    public static Response<T> Success(T data, string message = "操作成功")
    {
        return new Response<T>(StatusOk, message, data);
    }

    // 失败响应的静态方法
    public static Response<T> Fail(string message, int statusCode = StatusInternalServerError)
    {
        return new Response<T>(statusCode, message, default(T));
    }

    // 无数据的成功响应
    public static Response<T> SuccessWithoutData(string message = "操作成功")
    {
        return new Response<T>(StatusOk, message, default(T));
    }

    // 无数据的失败响应
    public static Response<T> FailWithoutData(string message, int statusCode = StatusInternalServerError)
    {
        return new Response<T>(statusCode, message, default(T));
    }
}




