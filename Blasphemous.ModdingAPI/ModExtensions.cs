using System;
using System.Linq;

namespace Blasphemous.ModdingAPI;

internal static class ModExtensions
{
    public static string CleanStackTrace(this Exception exception)
    {
        var split = exception.StackTrace?.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

        if (split == null || split.Length < 2)
            return exception.StackTrace;

        split = split.Take(split.Length - 2).ToArray();
        return string.Join(Environment.NewLine, split);
    }
}
