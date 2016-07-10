using System;

namespace Asteroids.Utilities
{
  public static class TypeExtensions
  {
    public static T GetInstance<T>(this Type type, object param)
    {
      if (type.FullName == null) return default(T);
      return (T) Activator.CreateInstance(Type.GetType(type.FullName), param);
    }
  }
}