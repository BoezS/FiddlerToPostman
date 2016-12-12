using System;
using System.Collections.Generic;
using System.Text;

namespace FiddlerToPostman
{
    public class PostmanCollection
    {
        public string id { get; set; }

        public string name { get; set; }

        public string[] order { get; set; }

        public long timestamp { get; set; }

        public int owner { get; set; }

        public bool __public { get; set; }

        public Request[] requests { get; set; }
    }

    public class Request
    {
        public string id { get; set; }

        public string headers { get; set; }

        public string url { get; set; }

        public string method { get; set; }

        public string dataMode { get; set; }

        public string currentHelper { get; set; }

        public string name { get; set; }

        public long time { get; set; }

        public string collectionId { get; set; }

        public Response[] responses { get; set; }

        public string rawModeData { get; set; }
    }

    public class RequestInfo
    {
        public string url { get; set; }

        public RequestHeaderInfo[] headers { get; set; }

        public string data { get; set; }

        public string method { get; set; }

        public string dataMode { get; set; }
    }

    public class RequestHeaderInfo
    {
        public string key { get; set; }

        public string value { get; set; }

        public bool enabled { get; set; } 
    }

    public class Response
    {
        public ResponseCodeInfo responseCode { get; set; }

        public ResponseHeaderInfo[] headers { get; set; }

        public string text { get; set; }

        public string language { get; set; }

        public string rawDataType { get; set; }

        public string previewType { get; set; }

        public int searchResultScrolledTo { get; set; }

        public bool forceNoPretty { get; set; }

        public bool write { get; set; }

        public bool empty { get; set; }

        public bool failed { get; set; }

        public ResponseState state { get; set; }

        public string id { get; set; }

        public string name { get; set; }

        public RequestInfo request { get; set; }
    }

    public class ResponseCodeInfo
    {
        public int code { get; set; }
    }

    public class ResponseHeaderInfo
    {
        public string name { get; set; }

        public string key { get; set; }

        public string value { get; set; }

        public string description { get; set; }
    }

    public class ResponseState
    {
        public string size { get; set; }
    }
}
