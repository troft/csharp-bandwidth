using System;
using System.Threading.Tasks;

namespace Bandwidth.Net
{
  /// <summary>
  /// Allow to get Id of object without loading it and get oject instance on demand
  /// </summary>
  /// <typeparam name="T">Type of object which instance will be loaded on demand</typeparam>
  public interface ILazyInstance<out T>
  {
    /// <summary>
    /// Id of object
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Instance of object (it will be loaded on demand)
    /// </summary>
    T Instance { get; }
  }

  internal class LazyInstance<T> : ILazyInstance<T>
  {
    private readonly Lazy<T> _instance;

    public LazyInstance(string id, Func<Task<T>> getInstance)
    {
      if (id == null) throw new ArgumentNullException(nameof(id));
      if (getInstance == null) throw new ArgumentNullException(nameof(getInstance));
      Id = id;
      _instance = new Lazy<T>(() => getInstance().Result);
    }

    public string Id { get; }

    public T Instance => _instance.Value;
  }
}
