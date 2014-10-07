using System;

namespace VsEmacs
{
    internal static class IServiceProviderExtensions
    {
        internal static T GetService<T>(this IServiceProvider serviceProvider)
        {
            return (T) serviceProvider.GetService(typeof (T));
        }

        internal static T GetService<S, T>(this IServiceProvider serviceProvider)
        {
            return (T) serviceProvider.GetService(typeof (S));
        }
    }
}