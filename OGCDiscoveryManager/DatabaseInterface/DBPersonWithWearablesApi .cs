using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace IO.Swagger.DatabaseInterface
{
    public class DBPersonWithWearablesApi
    {

        public bool ListPersonWithWearables(string role, ref string errorMessage, ref List<IO.Swagger.Models.PersonWithWearable> results)
        {
            bool retVal = true;
            try
            {
                using (COPDBContext.monica_cnetContext context = new COPDBContext.monica_cnetContext())
                {

                    //Make query 
                    var pp = (from d in context.Person
                              join f in context.PersonRoles
                              on d.Personid equals f.Personid
                              join h in context.Role
                              on f.Roleid equals h.Roleid
                              join g in context.PersonThings
                              on d.Personid equals g.Personid
                              where h.Const == role
                              select d);
                    if (role == null || role == "")
                    {
                        pp = (from d in context.Person
                              join g in context.PersonThings
                              on d.Personid equals g.Personid
                              select d);
                    }
                    var roleset = context.Role.FirstOrDefault(Role => Role.Const == role);
                    if (pp == null || pp.Count() == 0)
                    {
                        if (roleset == null)
                        {
                            errorMessage = "Non existing role:" + role;
                            retVal = false;
                        }
                        else
                            retVal = true;
                    }
                    else
                        foreach (var p in pp)
                        {
                            IO.Swagger.Models.PersonWithWearable z = new Models.PersonWithWearable();
                            var roleid = context.PersonRoles.FirstOrDefault(PersonRoles => PersonRoles.Personid == p.Personid);
                            var thisRole = context.Role.FirstOrDefault(Role => Role.Roleid == roleid.Roleid);
                            z.Name = p.Fullname;
                            z.PersonId = (int)p.Personid;
                            z.Status = p.Isactive;
                            z.Role = thisRole.Const;
                            z.RoleDescription = thisRole.Description;
                            var wearable = (from a in context.PersonThings
                                            join f in context.Thing
                                            on a.Thingid equals f.Thingid
                                            join h in context.LatestObservation
                                            on f.Thingid equals h.Thingid
                                            where a.Personid == z.PersonId && h.Personid == z.PersonId
                                            select h);
                            foreach (var obs in wearable)
                            {
                                string str = obs.Observationresult;
                                dynamic json = JValue.Parse(str);


                                string strLat = json.result.lat;
                                if (strLat == null)
                                    strLat = "0.0";

                                z.Lat = decimal.Parse(strLat, System.Globalization.CultureInfo.InvariantCulture);
                                string strLon = json.result.lon;
                                if (strLon == null)
                                    strLon = "0.0";

                                z.Lon = decimal.Parse(strLon, System.Globalization.CultureInfo.InvariantCulture);

                                string strTime = json.phenomenonTime;
                                z.Timestamp = DateTime.Parse(strTime);

                                results.Add(z);


                            }

                        }

                }
                return retVal;
            }
            catch (Exception e)
            {
                errorMessage = "Database Excaption:" + e.Message + " " + e.StackTrace;
                return false;
            }

        }

        public bool ListPersonByIdWithWearables(int personId, ref string errorMessage, ref IO.Swagger.Models.PersonWithWearable result)
        {
            bool retVal = true;
            try
            {
                using (COPDBContext.monica_cnetContext context = new COPDBContext.monica_cnetContext())
                {

                    //Make query 
                    var pp = (from d in context.Person
                              where d.Personid == personId
                              select d);

                    if (pp == null || pp.Count() == 0)
                    {
                        errorMessage = "Non existing person:" + personId.ToString();
                        retVal = false;
                    }
                    else
                    {
                        
                        foreach (var p in pp)
                        {
                            IO.Swagger.Models.PersonWithWearable z = new Models.PersonWithWearable();
                            var roleid = context.PersonRoles.FirstOrDefault(PersonRoles => PersonRoles.Personid == personId);
                            var roleset = context.Role.FirstOrDefault(Role => Role.Roleid == roleid.Roleid);
                            z.Name = p.Fullname;
                            z.PersonId = (int)p.Personid;
                            z.Status = p.Isactive;
                            z.Role = roleset.Const;
                            z.RoleDescription = roleset.Description;
                            var wearable = (from a in context.PersonThings
                                            join f in context.Thing
                                            on a.Thingid equals f.Thingid
                                            join h in context.LatestObservation
                                            on f.Thingid equals h.Thingid
                                            where a.Personid == z.PersonId && h.Personid == z.PersonId
                                            select h);
                            foreach (var obs in wearable)
                            {

                                string str = obs.Observationresult;
                                dynamic json = JValue.Parse(str);
                                string strLat = json.result.lat;
                                if (strLat == null)
                                    strLat = "0.0";
                                
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

        public bool DBSendPersonMessage(string PersonID, string Message, ref string errormessage)
        {
            bool retVal = false;
            int intPersonID = int.Parse(PersonID);
            using (COPDBContext.monica_cnetContext context = new COPDBContext.monica_cnetContext())
            {
                var wearable = (from a in context.PersonThings
                                join f in context.Thing
                                on a.Thingid equals f.Thingid
                                join h in context.LatestObservation
                                on f.Thingid equals h.Thingid
                                where a.Personid == intPersonID && h.Personid == intPersonID
                                select f);
                foreach (var thing in wearable)
                {

                    string thingId = thing.Ogcid;
                    dynamic jmsg = new JObject();
                    dynamic payload = new JObject();
                    //{
                    //    "phenomenonTime":"2018-11-27T11:31:33.836Z","result":{
                    //            "tagId":"LumiereTestGlass",
                    //    "type":"text",
                    //    "message": "Hello World From CNet"
                    //   }
                    //}
                    jmsg.phenomenonTime = DateTime.UtcNow.ToString("O");
                    payload.tagId = thingId;
                    payload.type = "text";
                    payload.message = Message;
                    jmsg.result = payload;
                    //string mqttPayload = jmsg.ToString();
                    //MQTT.SendMQTTMessage mq = new MQTT.SendMQTTMessage();
                    //mq.sendMqttMessage("GOST_LEEDS/Datastreams(2)/Observations", mqttPayload);
                }
            }


           return retVal;

        }
    }
}
