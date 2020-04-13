using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace IO.Swagger.DatabaseInterface
{
    public class DBThingToPhoneid
    {

        public bool ListWearableByWearablesid(string wearablePhysicalId, ref string errorMessage, ref IO.Swagger.Models.PersonWithWearable result)
        {
            bool retVal = true;
            try
            {
                using (COPDBContext.monica_cnetContext context = new COPDBContext.monica_cnetContext())
                {

                    //Make query 
                    var pp = (from d in context.Thing
                              where d.Ogcid == wearablePhysicalId
                              select d);

                    if (pp == null || pp.Count() == 0)
                    {
                        errorMessage = "Non existing wearable:" + wearablePhysicalId;
                        retVal = false;
                    }
                    else
                        foreach (var p in pp)
                        {
                            IO.Swagger.Models.PersonWithWearable z = new Models.PersonWithWearable();
                            z.Name = p.Ogcid;
                            z.PersonId = (int)0;
                            z.Status = true;
                            var wearable = (from a in  context.Thing
                                            join h in context.LatestObservation
                                            on a.Thingid equals h.Thingid
                                            where a.Thingid == p.Thingid 
                                            select h);
                            foreach (var obs in wearable)
                            {

                                string str = obs.Observationresult;
                                dynamic json = JValue.Parse(str);


                                string strLat = json.result.lat;
                                if (strLat == null)
                                    strLat = "0.0";
                                //strLat = strLat.Substring(0, strLat.IndexOf(","));
                                z.Lat = decimal.Parse(strLat, System.Globalization.CultureInfo.InvariantCulture);
                                string strLon = json.result.lon;
                                if (strLon == null)
                                    strLon = "0.0";
                                //strLon = strLon.Substring(0, strLon.IndexOf(","));
                                z.Lon = decimal.Parse(strLon, System.Globalization.CultureInfo.InvariantCulture);
                                //string strLat = str.Substring(str.IndexOf("\"lat\":") + 6);
                                //strLat = strLat.Substring(0, strLat.IndexOf(","));
                                //z.Lat = decimal.Parse(strLat, System.Globalization.CultureInfo.InvariantCulture);
                                //string strLon = str.Substring(str.IndexOf("\"lon\":") + 6);
                                //strLon = strLon.Substring(0, strLon.IndexOf(","));
                                //z.Lon = decimal.Parse(strLon, System.Globalization.CultureInfo.InvariantCulture);
                                string strTime = json.phenomenonTime;
                                z.Timestamp = DateTime.Parse(strTime);
                                result = z;


                            }

                        }



                    //Insert role connection;
                }
                return retVal;
            }
            catch (Exception e)
            {
                errorMessage = "Database Excaption:" + e.Message + " " + e.StackTrace;
                return false;
            }

        }
    }
}
