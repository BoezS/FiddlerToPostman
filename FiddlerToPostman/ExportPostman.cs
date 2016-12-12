using Fiddler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
[assembly: Fiddler.RequiredVersion("2.6.3.44034")]

namespace FiddlerToPostman
{
    [ProfferFormat("Postman-Collection", "Export session list as Postman collection")]
    public class ExportPostman : ISessionExporter
    {
        public bool ExportSessions(string sExportFormat,
            Session[] oSessions,
            Dictionary<string, object> dictOptions,
            EventHandler<ProgressCallbackEventArgs> evtProgressNotifications)
        {
            bool result = false;

            string filename = null;
            if (dictOptions != null && dictOptions.ContainsKey("Filename"))
            {
                filename = (dictOptions["Filename"] as string);
            }

            if (sExportFormat == "Postman-Collection")
            {
                if (string.IsNullOrEmpty(filename))
                {
                    filename = Fiddler.Utilities.ObtainSaveFilename(string.Format("Export as {0}", sExportFormat), "Postman Collection (*.postman_collection.json)|*.postman_collection.json");
                }
            }

            if (string.IsNullOrEmpty(filename)) { return false; }

            try
            {
                using (StreamWriter sw = new StreamWriter(filename, false, Encoding.UTF8))
                {
                    Request request = new Request();
                    Response response = new Response();
                    List<string> order = new List<string>();
                    List<Request> requests = new List<Request>();
                    List<Response> responses = new List<Response>();
                    List<ResponseHeaderInfo> responseHeaderInfos = new List<ResponseHeaderInfo>();
                    int addedSessions = 0;
                    int numSessions = oSessions.Length;

                    PostmanCollection postmanCollection = new PostmanCollection();
                    postmanCollection.id = Guid.NewGuid().ToString();
                    postmanCollection.name = string.Format("Collection_{0}", DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss"));
                    postmanCollection.timestamp = DateTime.UtcNow.Ticks;
                    postmanCollection.owner = 0;
                    postmanCollection.__public = false;

                    foreach (Session session in oSessions)
                    {
                        addedSessions++;
                        if (session != null)
                        {
                            request.id = Guid.NewGuid().ToString();
                            request.collectionId = postmanCollection.id;
                            request.method = session.RequestMethod;
                            request.url = session.fullUrl;
                            request.name = session.fullUrl;
                            request.currentHelper = "normal";
                            request.dataMode = "raw";
                            request.rawModeData = Encoding.Default.GetString(session.RequestBody);
                            request.time = DateTime.UtcNow.Ticks;
                            request.headers = "";

                            List<RequestHeaderInfo> requestHeaderInfoList = new List<RequestHeaderInfo>();

                            foreach (HTTPHeaderItem httphi in session.RequestHeaders)
                            {
                                request.headers += string.Format("{0}: {1}\n", httphi.Name, httphi.Value);

                                requestHeaderInfoList.Add(new RequestHeaderInfo() { key = httphi.Name, value = httphi.Value, enabled = true });
                            }

                            response.responseCode = new ResponseCodeInfo()
                            {
                                code = session.responseCode
                            };
                            response.text = Encoding.Default.GetString(session.ResponseBody);
                            response.language = "json";
                            response.rawDataType = "text";
                            response.previewType = "text";
                            response.searchResultScrolledTo = -1;
                            response.forceNoPretty = false;
                            response.write = true;
                            response.empty = false;
                            response.failed = false;
                            response.state = new ResponseState()
                            {
                                size = "normal"
                            };
                            response.id = Guid.NewGuid().ToString();
                            response.name = "response";
                            response.request = new RequestInfo()
                            {
                                url = session.fullUrl,
                                headers = requestHeaderInfoList.ToArray(),
                                data = "",
                                method = session.RequestMethod,
                                dataMode = "raw"
                            };

                            foreach (HTTPHeaderItem httphi in session.ResponseHeaders)
                            {
                                responseHeaderInfos.Add(new ResponseHeaderInfo() { name = httphi.Name, key = httphi.Name, value = httphi.Value, description = httphi.Name });
                            }

                            responses.Add(response);
                            request.responses = responses.ToArray();
                            response = new Response();

                            requests.Add(request);
                            order.Add(request.id);
                            request = new Request();
                        }
                        if (evtProgressNotifications != null)
                        {
                            ProgressCallbackEventArgs pcea = new ProgressCallbackEventArgs((addedSessions / (float)numSessions), string.Format("{0} records added", addedSessions));
                            evtProgressNotifications(null, pcea);
                            if (pcea.Cancel)
                            {
                                sw.Close();
                                return false;
                            }
                        }
                    }

                    postmanCollection.requests = requests.ToArray();
                    postmanCollection.order = order.ToArray();

                    sw.Write(Utilities.ToJSONString(postmanCollection).Replace("__", ""));
                    sw.Close();
                }

                result = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Failed to export");
                result = false;
            }

            return result;
        }

        public void Dispose() { }
    }
}