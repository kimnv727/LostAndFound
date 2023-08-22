using System;
using System.Collections.Generic;

namespace LostAndFound.Core.Entities.Common
{
    public interface ITextSearchableEntity
    {
        // Texts to search along with their relative weights
        public IReadOnlyDictionary<Func<string>, double> SearchTextsWithWeights { get; }
    }
}
