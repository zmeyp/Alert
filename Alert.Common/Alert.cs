using System;

namespace Alert.Common
{
    [Serializable]
    public class Alert
    {
        public string Message { get; set; }
        public string Source { get; set; }
        public string StackTrace { get; set; }
        public string Target { get; set; }
        public string Status { get; set; }
    }
}
