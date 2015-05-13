using System.Collections.Generic;

namespace Alert.Common
{
    public interface ICheck
    {
        IEnumerable<Alert> Inspect();
    }
}
