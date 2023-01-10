
namespace BookStore.Domain.Services;

public interface ILog<T>
{
    void Trace(string msg);
    void Trace(Exception exception, string msg);
    void Debug(string msg);
    void Debug(Exception exception, string msg);
    void Info(string msg);
    void Info(Exception exception, string msg);
    void Warn(string msg);
    void Warn(Exception exception, string msg);
    void Error(string msg);
    void Error(Exception exception, string msg);
    void Critical(string msg);
    void Critical(Exception exception, string msg);
}