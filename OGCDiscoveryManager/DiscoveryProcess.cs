using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml;
using System.Net;
using System.Globalization;

namespace OGCDiscoveryManager
{
    public class DiscoveryProcess
    {
        private Thread workThread = null;
        public bool serviceStopped = false;
        public bool safelyStopped = false;

        private string OGCServerBaseURL = "http://monappdwp3.monica-cloud.eu:5050/gost_kff/v1.0/";
        private string OGCServerUID = "";
        private string OGCServerPwd = "";
        private string OGCMQTTPrefix = "GOST_KFF";
        //private string OGCSearchString = "Smart-Glasses/Incident-Reporting";
        //private string thingType = "incidentreporter";
        //private bool wearableSearch = false;

        private string OGCSearchString = "Localization-Smart-Glasses";
        private string thingType = "wearables UWB";
        private bool wearableSearch = true;
        private string latestObsExtraValue = "";
        public bool start()
        {
            string tmp = Environment.GetEnvironmentVariable("OGCServerBaseURL");
            if (tmp == null || tmp == "")
            {
                System.Console.WriteLine("Warning:Missing OGCServerBaseURL env variable.");
            }
            else OGCServerBaseURL = tmp;

            tmp = Environment.GetEnvironmentVariable("OGCServerUID");
            if (tmp == null || tmp == "")
            {
                System.Console.WriteLine("Warning:Missing OGCServerUID env variable.");
            }
            else OGCServerUID = tmp;

            tmp = Environment.GetEnvironmentVariable("OGCServerPwd");
            if (tmp == null || tmp == "")
            {
                System.Console.WriteLine("Warning:Missing OGCServerPwd env variable.");
            }
            else OGCServerPwd = tmp;

            tmp = Environment.GetEnvironmentVariable("OGCMQTTPrefix");
            if (tmp == null || tmp == "")
            {
                System.Console.WriteLine("Warning:Missing OGCMQTTPrefix env variable.");
            }
            else OGCMQTTPrefix = tmp;

            tmp = Environment.GetEnvironmentVariable("OGCSearchString");
            if (tmp == null || tmp == "")
            {
                System.Console.WriteLine("Warning:Missing OGCSearchString env variable.");
            }
            else OGCSearchString = tmp;


            tmp = Environment.GetEnvironmentVariable("thingType");
            if (tmp == null || tmp == "")
            {
                System.Console.WriteLine("Warning:Missing thingType env variable.");
            }
            else thingType = tmp;

            tmp = Environment.GetEnvironmentVariable("wearableSearch");
            if (tmp == null || tmp == "")
            {
                System.Console.WriteLine("Warning:Missing wearableSearch env variable.");
            }
            else wearableSearch = bool.Parse(tmp);

            tmp = Environment.GetEnvironmentVariable("latestObsExtraValue");
            if (tmp == null || tmp == "")
            {
                System.Console.WriteLine("Warning:Missing latestObsExtraValue env variable.");
            }
            else latestObsExtraValue = tmp;

            //CreateDatastreamsForSound();
            //return true;
            ThreadStart myThreadStart = new ThreadStart(this.ThreadGuard);
            workThread = new Thread(myThreadStart);
            workThread.Start();
            System.Console.WriteLine("Discovery started");
            return true;
        }

        public bool stop()
        {

            this.serviceStopped = true;

            while (!safelyStopped)
            {
                System.Threading.Thread.Sleep(1000);
            }
            return true;
        }

        /// <summary>
        /// Catches all strange exceptions, loggs them and restarts work
        /// </summary>
        public void ThreadGuard()
        {
            while (!serviceStopped)
            {
                try
                {
                    DoTheDiscovery();
                }
                catch (Exception e)
                {
                    System.Console.WriteLine("Thread Exception:" + e.Message);
                }
            }
            safelyStopped = true;
            System.Console.WriteLine("Service Stopped");
        }


        private void DoTheDiscovery()
        {
            while (!serviceStopped)
            {
                XmlDocument xDoc = null;
                string JsonResult = "";
                WebClient client = new WebClient();
                try
                {
                    string url = OGCServerBaseURL + "Datastreams?$filter=substringof('" + OGCSearchString + "',name)";
                    //string url = OGCServerBaseURL + "Datastreams?$filter=substringof('DSS',name) and substringof('Id',name) and not(substringof('ALERT',name))";

                    client.Encoding = System.Text.Encoding.UTF8;
                    client.Headers["Accept"] = "application/json";
                    client.Headers["Content-Type"] = "application/json";
                    NetworkCredential myCreds = new NetworkCredential(OGCServerUID, OGCServerPwd);
                    client.Credentials = myCreds;

                    JsonResult = client.DownloadString(url);

                    xDoc = JsonConvert.DeserializeXmlNode(JsonResult, "Root");

                }
                catch (WebException exception)
                {
                    System.Console.WriteLine("Datastreams?$filter failed:" + exception.Message);


                }
                if (xDoc != null)
                {
                    XmlNodeList foundOGCThings = xDoc.SelectNodes(".//value");
                    foreach (XmlNode foundOGCThing in foundOGCThings)
                    {
                        //Temp fix for DSS
                        string iotid = foundOGCThing.SelectSingleNode("@iot.id").Value;
                        //if (int.Parse(iotid) > 80)
                        //{
                            //Extract id and Observation link
                            string name = foundOGCThing.SelectSingleNode("name").InnerText;
                            string observationUrl = foundOGCThing.SelectSingleNode("Observations_x0040_iot.navigationLink").InnerText;

                            //Id is the last part of the name 
                            string[] parsedName = name.Split("/");
                            string id = "";
                            if (thingType == "SoundHeatmap") //SoundHeatmap in a diffrent place
                                id = parsedName[1];
                            else if (thingType == "SoundmeterAggregate") //SoundHeatmap in a diffrent place
                                id = name;
                            else
                                id = parsedName[parsedName.GetUpperBound(0)];
                            decimal? lat = 0;
                            decimal? lon = 0;
                            XmlNode xPoint = foundOGCThing.SelectSingleNode(".//observedArea");
                            if (xPoint != null)
                            {
                                XmlNodeList xCoord = xPoint.SelectNodes(".//coordinates");
                                lat = decimal.Parse(xCoord[1].InnerText, CultureInfo.InvariantCulture);
                                lon = decimal.Parse(xCoord[0].InnerText, CultureInfo.InvariantCulture);
                            }
                       
                        //Observation stream is the last part of the observationUrl http://monappdwp3.monica-cloud.eu:5050/gost_leeds/v1.0/Datastreams(3)/Observations
                        string[] parsedUrl = observationUrl.Split("/");
                        string observationTopic = OGCMQTTPrefix + "/" + parsedUrl[parsedUrl.GetUpperBound(0) - 1] + "/" + parsedUrl[parsedUrl.GetUpperBound(0)];
                        string tmpObsTop = observationTopic;
                        if (latestObsExtraValue == "AGGREGATE")
                            tmpObsTop += ":AGGREGATE";
                        if (!exist(tmpObsTop))
                            {
                                if (wearableSearch)
                                {
                                    CreateNewWearableAndPerson(id, observationTopic);
                                }
                                else if (!wearableSearch)
                                    CreateNewThing(id, observationTopic, lat, lon);
                            }
                        //}
                    }
                    // 5 minutes
                    Thread.Sleep(5 * 60 * 1000);
                }
            }
            }
            private bool exist(string topic)
            {
                string error = "";
                IO.Swagger.DatabaseInterface.DBObservation obs = new IO.Swagger.DatabaseInterface.DBObservation();
                return (obs.ObservationStreamKnown(topic, ref error));
            }

            private void CreateNewWearableAndPerson(string id, string observationTopic)
            {
                string error = "";
                int personID = 0;
                long thingID = 0;
                long wearableID = 0;
                //First Create a person
                IO.Swagger.DatabaseInterface.DBPerson dP = new IO.Swagger.DatabaseInterface.DBPerson();
                if (dP.AddPerson(0, "MONICA", "Wearer of " + id, "", "", 1, true, ref error, ref personID))
                {
                    //Create a thing 
                    IO.Swagger.DatabaseInterface.DBThing dT = new IO.Swagger.DatabaseInterface.DBThing();
                    if (dT.AddThing("Wearable" + id, thingType, "Autogenerated Thing by OGC Discovery Manager", 1, 0, 0, id, ref error, ref thingID))
                    {
                        //Connect thing and person
                        IO.Swagger.DatabaseInterface.DBWearable dW = new IO.Swagger.DatabaseInterface.DBWearable();
                        if (dW.AddWearable(personID, (int)thingID, ref error, ref wearableID))
                        {
                            long obsID = 0;
                            //Add observation
                            IO.Swagger.DatabaseInterface.DBObservation dO = new IO.Swagger.DatabaseInterface.DBObservation();
                            dO.AddUpdateObservation((int)thingID, observationTopic + ":" + id + ":", DateTime.Now, "", "", personID, null, ref error, ref obsID);
                        }
                    }

                }
            }


            private void CreateNewThing(string id, string observationTopic, decimal? lat, decimal? lon)
            {
                string error = "";

                long thingID = 0;

                //Create a thing 
                IO.Swagger.DatabaseInterface.DBThing dT = new IO.Swagger.DatabaseInterface.DBThing();
                if (dT.AddThing("Thing" + id, thingType, "Autogenerated Thing by OGC Discovery Manager", 1, lat, lon, id, ref error, ref thingID))
                {

                    long obsID = 0;
                    //Add observation
                    IO.Swagger.DatabaseInterface.DBObservation dO = new IO.Swagger.DatabaseInterface.DBObservation();
                if (thingType == "SoundmeterAggregate") id = "";
                //Tmp fix
                if(latestObsExtraValue == "AGGREGATE")  id = "AGGREGATE";
                dO.AddUpdateObservation((int)thingID, observationTopic + ":" + id + ":", DateTime.Now, "", latestObsExtraValue, null, null, ref error, ref obsID);

                }


            }

            public void CreateDatastreamsForSound()
            {
                List<IO.Swagger.Models.Thing> results = new List<IO.Swagger.Models.Thing>();
                IO.Swagger.DatabaseInterface.DBThing dbt = new IO.Swagger.DatabaseInterface.DBThing();
                string err = "";
                if (dbt.ListThings(thingType, ref err, ref results))
                {
                    foreach (IO.Swagger.Models.Thing th in results)
                    {
                        XmlDocument xDoc = null;
                        string JsonResult = "";
                        WebClient client = new WebClient();
                        try
                        {
                            string url = OGCServerBaseURL + "Datastreams?$filter=substringof('" + th.Name.Replace("Thing", "") + "',name)";

                            client.Encoding = System.Text.Encoding.UTF8;
                            client.Headers["Accept"] = "application/json";
                            client.Headers["Content-Type"] = "application/json";
                            NetworkCredential myCreds = new NetworkCredential(OGCServerUID, OGCServerPwd);
                            client.Credentials = myCreds;

                            JsonResult = client.DownloadString(url);

                            xDoc = JsonConvert.DeserializeXmlNode(JsonResult, "Root");

                        }
                        catch (WebException exception)
                        {
                            System.Console.WriteLine("Datastreams?$filter failed:" + exception.Message);


                        }
                        if (xDoc != null)
                        {
                            XmlNodeList foundOGCThings = xDoc.SelectNodes(".//value");
                            foreach (XmlNode foundOGCThing in foundOGCThings)
                            {
                                //Extract id and Observation link
                                string name = foundOGCThing.SelectSingleNode("name").InnerText;
                                string observationUrl = foundOGCThing.SelectSingleNode("Observations_x0040_iot.navigationLink").InnerText;

                                //Id is the last part of the name 
                                string[] parsedName = name.Split("/");
                                //string id = parsedName[parsedName.GetUpperBound(0)];
                                string id = "";
                                if (thingType == "SoundHeatmap") //SoundHeatmap in a diffrent place
                                    id = parsedName[1];
                                else
                                    id = parsedName[parsedName.GetUpperBound(0)];
                                string type = "";
                                if (thingType == "SoundHeatmap") //SoundHeatmap in a diffrent place
                                {
                                    string tmptype = parsedName[parsedName.GetUpperBound(0)];
                                    switch (tmptype)
                                    {
                                    case "SPL dB Cfull":
                                        type = "dbCheatmapfull";
                                        break;
                                    case "SPL dB Afull":
                                        type = "dbAheatmapfull";
                                        break;
                                    case "SPL dB Zfull":
                                        type = "dbZheatmapfull";
                                        break;
                                    case "LCeq":
                                            type = "dbCheatmap";
                                            break;
                                        case "LZeq":
                                            type = "dbZheatmap";
                                            break;
                                        case "LAeq":
                                            type = "dbAheatmap";
                                            break;
                                        default:
                                            type = "no match";
                                            break;
                                    }
                                }
                                else
                                    type = parsedName[parsedName.GetUpperBound(0) - 1]; ;

                                //Observation stream is the last part of the observationUrl http://monappdwp3.monica-cloud.eu:5050/gost_leeds/v1.0/Datastreams(3)/Observations
                                string[] parsedUrl = observationUrl.Split("/");
                                string observationTopic = OGCMQTTPrefix + "/" + parsedUrl[parsedUrl.GetUpperBound(0) - 1] + "/" + parsedUrl[parsedUrl.GetUpperBound(0)];
                                if (!exist(observationTopic))
                                {
                                    string error = "";
                                    long obsID = 0;
                                    IO.Swagger.DatabaseInterface.DBObservation dO = new IO.Swagger.DatabaseInterface.DBObservation();
                                    dO.AddUpdateObservation((int)th.Id, observationTopic + ":" + type + ":", DateTime.Now, "", latestObsExtraValue, null, null, ref error, ref obsID);
                                }

                            }
                        }

                    }
                }
            }
        }
    }
