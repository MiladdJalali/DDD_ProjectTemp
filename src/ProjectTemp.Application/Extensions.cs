using System.Security.Cryptography;
using System.Text;

namespace ProjectTemp.Application;

public static class Extensions
{
    public static IEnumerable<T> ApplyPaging<T>(this IEnumerable<T> queryResult, int pageSize, int pageIndex)
    {
        if (pageSize == -1 && pageIndex == -1)
            return queryResult;

        if (pageSize == 0)
            pageSize = 100;

        if (pageIndex == 0)
            pageIndex = 1;

        return queryResult
            .Skip(pageSize * (pageIndex - 1))
            .Take(pageSize);
    }

    public static string GetHash(this string phrase)
    {
        using var sha256 = SHA256.Create();

        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(phrase));
        var builder = new StringBuilder();

        foreach (var @byte in bytes)
            builder.Append(@byte.ToString("x2"));

        return builder.ToString();
    }
}