using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using Npgsql.EntityFrameworkCore;
using IO.Swagger.COPDBContext;

namespace IO.Swagger.DatabaseInterface
{
    public class DBObservation
    {
        public bool AddUpdateObservation(int? thingid, string streamid, DateTime? phenomentime, string Observationresult, string type, int? personid, int? zoneid, ref string errorMessage, ref long newid)
        {
            bool retVal = true;

            using (COPDBContext.monica_cnetContext context = new COPDBContext.monica_cnetContext())
            {

                //Find type
                try
                {
                    var obs = context.LatestObservation
                           .Single(b => b.Thingid == thingid && b.Datastreamid == streamid);


                    obs.Thingid = (long)thingid;
                    obs.Datastreamid = streamid;
                    obs.Phenomentime = phenomentime;
                    obs.Personid = personid;
                    obs.Locationid = zoneid;
                    obs.Observationresult = Observationresult;
                    obs.Type = type;
                    context.SaveChanges();

                }
                catch (Exception f)
                {
                    LatestObservation lo = new LatestObservation();
                    lo.Thingid = (long)thingid;
                    lo.Datastreamid = streamid;
                    lo.Phenomentime = phenomentime;
                    lo.Observationresult = Observationresult;
                    lo.Personid = personid;
                    lo.Locationid = zoneid;
                    lo.Type = type;
                    context.LatestObservation.Add(lo);
                    context.SaveChanges();
                }
            }
            return retVal;
        }

        public bool ListObs(int thingId, ref string errorMessage, ref List<IO.Swagger.Models.Observation> results)
        {
            bool retVal = true;
            try
            {
                using (COPDBContext.monica_cnetContext context = new COPDBContext.monica_cnetContext())
                {

                    //Make query 
                    var obs = (from d in context.LatestObservation  where d.Thingid == (long) thingId select d
                                     ).ToList();

                    if (obs == null || obs.Count() == 0)
                    {
                        errorMessage = "No things";
                        retVal = false;
                    }
                    else
                        foreach (var th in obs)
                        {
                            IO.Swagger.Models.Observation z = new Models.Observation();
                            z.ThingId = (int) th.Thingid;
                            z.DatastreamId = th.Datastreamid;
                            z.Personid = th.Personid;
                            z.Zoneid = th.Locationid;
                            z.PhenomenTime = th.Phenomentime;
                            z.ObservationResult = th.Observationresult;
                            results.Add(z);

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

        public bool ObservationStreamKnown(string ObservationStream, ref string errorMessage)
        {
            bool retVal = false;
          
                var connString = settings.ConnectionString;
                try
                {
                    using (var conn = new NpgsqlConnection(connString))
                    {
                        conn.Open();
                        //Find the role id
                        int obsID = 0;
                        int newPersonId = -1;
                        using (var cmd = new NpgsqlCommand("SELECT thingid FROM monica.latest_observation where datastreamid LIKE @observationStream", conn))
                        {
                            ObservationStream = "%" + ObservationStream + "%";
                            cmd.Parameters.AddWithValue("observationStream", ObservationStream);
                            using (var reader = cmd.ExecuteReader())
                                while (reader.Read())
                                    obsID = reader.GetInt32(0);
                        if (obsID > 0)
                            retVal = true;
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
          
    }
}

