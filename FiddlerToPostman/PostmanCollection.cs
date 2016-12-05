using System;
using System.Collections.Generic;
using System.Text;

namespace FiddlerToPostman
{
    public class PostmanCollection
    {
        public string id { get; set; }

        public string name { get; set; }

        public List<string> order { get; set; }

        public long timestamp { get; set; }

        public int owner { get; set; }

        public bool __public { get; set; }

        public List<Request> requests { get; set; }
    }

    public class Request
    {
        public string id { get; set; }

        public string headers { get; set; }

        public string url { get; set; }

        public string method { get; set; }

        public string dataMode { get; set; }

        public string currentHelper { get; set; }

        public long time { get; set; }

        public string name { get; set; }

        public string collectionId { get; set; }

        public string rawModeData { get; set; }
    }
}
