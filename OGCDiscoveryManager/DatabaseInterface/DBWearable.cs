using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using IO.Swagger.COPDBContext;

namespace IO.Swagger.DatabaseInterface
{
    public class DBWearable
    {
        public bool AddWearable(int? personid, int? thingid,  ref string errorMessage, ref long newid)
        {
            bool retVal = true;
            try
            {
                using (COPDBContext.monica_cnetContext context = new COPDBContext.monica_cnetContext())
                {

                    //Find type 
                  
                        PersonThings a = new PersonThings();
                        a.Personid = (int) personid;
                        a.Thingid = (int) thingid;
                    a.Timepoint = DateTime.Now;
         


                        context.PersonThings.Add(a);
                        context.SaveChanges();

                        newid = 0;
                   
                }
                return retVal;
            }
            catch (Exception e)
            {
                errorMessage = "Database Excaption:" + e.Message + " " + e.StackTrace;
                return false;
            }
        }

        public bool DeleteWearable(string personid, string thingid, ref string errorMessage)
        {
            bool retVal = true;
            try
            {
                using (COPDBContext.monica_cnetContext context = new COPDBContext.monica_cnetContext())
                {

                    //Find type 
                    var loc = context.PersonThings
                            .Single(b => b.Personid == long.Parse(personid) && b.Thingid == long.Parse(thingid)
                            );

                    if (loc == null)
                    {
                        errorMessage = "Non-Existing wEARABLE";
                        retVal = false;
                    }
                    else
                    {
                        context.PersonThings.Remove(loc);
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

        public bool ListWearables(ref string errorMessage, ref List<IO.Swagger.Models.Wearable> results)
        {
            bool retVal = true;
            try
            {
                using (COPDBContext.monica_cnetContext context = new COPDBContext.monica_cnetContext())
                {

                    

                        foreach (var loc in context.PersonThings)
                        {
                            IO.Swagger.Models.Wearable z = new Models.Wearable();
                            z.PersonId = loc.Personid;
                            z.ThingId = (int?)loc.Thingid;
                            z.Timestamp = loc.Timepoint;
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
    }
}
