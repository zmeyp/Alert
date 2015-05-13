using System.Collections.Generic;

namespace Alert.Common
{
    public interface IAction
    {
        void PerformAction(IEnumerable<Alert> messages);
    }
}
