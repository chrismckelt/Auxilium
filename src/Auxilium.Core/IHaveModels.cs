using System.Collections.Generic;

namespace Auxilium.Core
{
    public interface IHaveModels<T>
    {
        IList<T> Value { get; set; }
    }
}