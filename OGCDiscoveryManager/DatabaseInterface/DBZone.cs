using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using IO.Swagger.COPDBContext;
using Newtonsoft.Json.Linq;

namespace IO.Swagger.DatabaseInterface
{
    public class DBZone
    {

        public bool AddZone(string zoneName, string type, string description, string metadata, string boundingpolygon, int? capacity, ref string errorMessage, ref long newid)
        {
            bool retVal = true;
            try
            {
                using (COPDBContext.monica_cnetContext context = new COPDBContext.monica_cnetContext())
                {

                    //Find type 
                    var LocationType = context.LocationTemplates
                            .Single(b => b.Name == type);

                    if (LocationType == null)
                    {
                        errorMessage = "Non-Existing Role";
                        retVal = false;
                    }
                    else
                    { 
                        Location a = new Location();
                        a.Name = zoneName;
                        a.Locationtemplateid = LocationType.Locationtemplateid;
                        a.Description = description;
                        a.Metadata = metadata;
                        a.Boundingpolygon = boundingpolygon;
                        if(capacity != null)
                            a.Capacity = capacity;

                        context.Location.Add(a);
                        context.SaveChanges();

                        newid = a.Locationid;
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

        public bool DeleteZone(string zonid, ref string errorMessage)
        {
            bool retVal = true;
            try
            {
                using (COPDBContext.monica_cnetContext context = new COPDBContext.monica_cnetContext())
                {

                    //Find type 
                    var loc = context.Location
                            .Single(b => b.Locationid == long.Parse(zonid));

                    if (loc == null)
                    {
                        errorMessage = "Non-Existing Location";
                        retVal = false;
                    }
                    else
                    {
                        context.Location.Remove(loc);
                        context.SaveChanges();
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

        public bool ListZones(string zoneType, ref string errorMessage, ref List<IO.Swagger.Models.Zone> results)
        {
            bool retVal = true;
            try
            {
                using (COPDBContext.monica_cnetContext context = new COPDBContext.monica_cnetContext())
                {

                    //Make query 
                    var locations = (from d in context.Location
                                     join f in context.LocationTemplates
                                     on d.Locationtemplateid equals f.Locationtemplateid
                                     where f.Name == zoneType
                                     select new
                                     { id= d.Locationid, name = d.Name, descripton = d.Description, metadata=d.Metadata, bpol = d.Boundingpolygon, capacity = d.Capacity, type = f.Name}
                                     ).ToList();

                    if (locations == null)
                    {
                        errorMessage = "No locations";
                        retVal = false;
                    }
                    else
                        foreach (var loc in locations)
                        {
                            int? count = null;
                            try
                            {
                                var things = context.LocationThings
                                .Single(b => b.Locationid == loc.id);

                                var values = (from d in context.LatestObservation
                                              where d.Locationid == loc.id && d.Type == "PEOPLE_DENSITY"
                                              select new
                                              { type = d.Type, obs = d.Observationresult }
                                  ).ToList();
                                if (values.Count > 0)
                                {
                                    string str = values[0].obs;
                                    dynamic json = JValue.Parse(str);
                                    count = json.result.density_count;

                                }
                            }
                            catch (Exception exx)
                            {
                                System.Console.WriteLine(exx.Message);
                            }
                            IO.Swagger.Models.Zone z = new Models.Zone();
                            z.Name = loc.name;
                            z.Id = (int) loc.id;
                            z.Description = loc.descripton;
                            z.Metadata = loc.metadata;
                            z.Type = loc.type;
                            z.Capacity = loc.capacity;
                            z.Peoplecount = count;
                            z.BoundingPolygon = CreateBoundingPol(loc.bpol);
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


        public bool ListAllZones( ref string errorMessage, ref List<IO.Swagger.Models.Zone> results)
        {
            bool retVal = true;
            try
            {
                using (COPDBContext.monica_cnetContext context = new COPDBContext.monica_cnetContext())
                {

                    //Make query 
                    var locations = (from d in context.Location
                                     join f in context.LocationTemplates
                                     on d.Locationtemplateid equals f.Locationtemplateid
                        
                                     select new
                                     { id = d.Locationid, name = d.Name, descripton = d.Description, metadata = d.Metadata, bpol = d.Boundingpolygon, type = f.Name, capacity =d.Capacity }
                                     ).ToList();

                    if (locations == null)
                    {
                        errorMessage = "No locations";
                        retVal = false;
                    }
                    else
                        foreach (var loc in locations)
                        {
                            int? count = null;
                            try
                            {
                                var things = context.LocationThings
                                .Single(b => b.Locationid == loc.id);

                                var values = (from d in context.LatestObservation
                                              where d.Locationid == loc.id && d.Type == "PEOPLE_DENSITY"
                                              select new
                                              { type = d.Type, obs = d.Observationresult }
                                  ).ToList();
                                if (values.Count > 0)
                                {
                                    string str = values[0].obs;
                                    dynamic json = JValue.Parse(str);
                                    count = json.result.density_count;

                                }
                            }
                            catch (Exception exx)
                            {
                                System.Console.WriteLine(exx.Message);
                            }
                            IO.Swagger.Models.Zone z = new Models.Zone();
                            z.Name = loc.name;
                            z.Id = (int)loc.id;
                            z.Description = loc.descripton;
                            z.Metadata = loc.metadata;
                            z.Type = loc.type;
                            z.BoundingPolygon = CreateBoundingPol(loc.bpol);
                            z.Capacity = loc.capacity;
                            if (count != null)
                                z.Peoplecount = count;
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
        public bool FindZoneFromId(string zoneId, ref string errorMessage, ref IO.Swagger.Models.Zone results)
        {
            bool retVal = true;
            try
            {
                using (COPDBContext.monica_cnetContext context = new COPDBContext.monica_cnetContext())
                {

                    //Make query 
                    var locations = (from d in context.Location
                                     join f in context.LocationTemplates
                                     on d.Locationtemplateid equals f.Locationtemplateid
                                     where d.Locationid == long.Parse(zoneId)
                                     select new
                                     { id = d.Locationid, name = d.Name, descripton = d.Description, metadata = d.Metadata, bpol = d.Boundingpolygon, capacity = d.Capacity, type = f.Name }
                                     ).ToList();

                    if (locations == null)
                    {
                        errorMessage = "No location";
                        retVal = false;
                    }
                    else
                    {
                        results = new Models.Zone();
                        var loc = locations[0];
                        int? count = null;
                        try
                        {
                            var things = context.LocationThings
                            .Single(b => b.Locationid == loc.id);

                            var values = (from d in context.LatestObservation
                                          where d.Locationid == loc.id && d.Type == "PEOPLE_DENSITY"
                                          select new
                                          { type = d.Type, obs = d.Observationresult }
                              ).ToList();
                            if (values.Count > 0)
                            {
                                string str = values[0].obs;
                                dynamic json = JValue.Parse(str);
                                count = json.result.density_count;

                            }
                        }
                        catch(Exception exx)
                        {
                            System.Console.WriteLine(exx.Message);
                        }
                        results.Id = (int)loc.id;
                        results.Name = loc.name;
                        results.Description = loc.descripton;
                        results.Metadata = loc.metadata;
                        results.Type = loc.type;
                        results.Capacity = loc.capacity;
                        results.Peoplecount = count;
                        //results.Peoplecount
                        results.BoundingPolygon = CreateBoundingPol(loc.bpol);
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

        List<List<decimal?>> CreateBoundingPol(string bpol)
        {
            List<List<decimal?>> retval = new List<List<decimal?>>();

            bpol = bpol.Replace(" ","");
            bpol = bpol.Replace("\t", "");
            bpol = bpol.Replace("\r", "");
            bpol = bpol.Replace("\n", "");

            string[] parts = bpol.Split("],[");
            foreach (string p in parts)
            {
                string tmp = p;
                tmp = tmp.Replace("[", "");
                tmp = tmp.Replace("]", "");
                string[] vals = tmp.Split(",");
                if (vals.GetLength(0) == 2)
                {
                    List<decimal?> tmpArr = new List<decimal?>();
                    tmpArr.Add(decimal.Parse(vals[0], System.Globalization.CultureInfo.InvariantCulture));
                    tmpArr.Add(decimal.Parse(vals[1], System.Globalization.CultureInfo.InvariantCulture));
                    retval.Add(tmpArr);
                }
            }


            return retval;

        }

     

    }
}

