using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core.Scripts.Tools.DataLoaders
{
    public interface IDataLoader
    {
        UniTask<string> LoadDataAsync(string path, CancellationToken token);
    }
}