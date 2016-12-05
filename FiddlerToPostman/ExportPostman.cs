using Fiddler;
using Newtonsoft.Json;
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
                    int addedSessions = 0;
                    int numSessions = oSessions.Length;

                    PostmanCollection postmanCollection = new PostmanCollection();
                    postmanCollection.id = Guid.NewGuid().ToString();
                    postmanCollection.name = string.Format("Collection_{0}", DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss"));
                    postmanCollection.order = new List<string>();
                    postmanCollection.timestamp = DateTime.UtcNow.Ticks;
                    postmanCollection.owner = 0;
                    postmanCollection.__public = false;
                    postmanCollection.requests = new List<Request>();

                    foreach (Session session in oSessions)
                    {
                        addedSessions++;
                        if (session != null)
                        {
                            Request r = new Request();
                            r.id = Guid.NewGuid().ToString();
                            r.collectionId = postmanCollection.id;
                            r.method = session.RequestMethod;
                            r.url = session.fullUrl;
                            r.name = session.fullUrl;
                            r.currentHelper = "normal";
                            r.dataMode = "raw";
                            r.rawModeData = Encoding.Default.GetString(session.RequestBody);
                            r.time = DateTime.UtcNow.Ticks;
                            r.headers = "";

                            foreach (HTTPHeaderItem httphi in session.RequestHeaders)
                            {
                                r.headers += string.Format("{0}: {1}\n", httphi.Name, httphi.Value);
                            }

                            postmanCollection.requests.Add(r);
                            postmanCollection.order.Add(r.id);
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

                    JsonSerializerSettings jss = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };

                    sw.Write(JsonConvert.SerializeObject(postmanCollection).Replace("__", ""));
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