using System;
using System.Linq;

namespace Pro4Soft.MobileDevice.Plumbing.Infrastructure
{
	public class Singleton<T> where T : class, new()
	{
		private static readonly Lazy<T> _instance = new Lazy<T>(() => Factory.Create<T>());
		public static readonly T Instance = _instance.Value;
    }

	public class Factory
	{
		public static T Create<T>(params object[] pars) where T : class
        {
            return Create(typeof(T), pars) as T;
        }

        public static object Create(Type type, params object[] pars)
        {
            var constr = type.GetConstructors();

            return constr.SingleOrDefault(c => c.GetParameters().Length == pars.Length)?.Invoke(pars);
        }
	}
}
