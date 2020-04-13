using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IO.Swagger.DatabaseInterface
{
    public class DBEvent
    {


        public bool UpdateEvent(string name, string city, decimal? lat, decimal? lon, int? zoomlevel, DateTime start, DateTime end, ref string errorMessage, ref long newid)
        {
            bool retVal = true;
            try
            {
                using (COPDBContext.monica_cnetContext context = new COPDBContext.monica_cnetContext())
                {


                    var a = context.Event.FirstOrDefault();
                    if (a != null)
                    {
                        a.Name = name;
                        a.Description = city;
                        a.Lat = (double?)lat;
                        a.Lon = (double?)lon;
                        a.Zoom = zoomlevel;
                        a.Fromdate = start;
                        a.Todate = end;
                    

                        context.Event.Update(a);
                        context.SaveChanges();
                    }
                    else
                    {
                        errorMessage = "Event info  does not exist";
                        return false;
                    }
                }
                return retVal;
            }
            catch (Exception e)
            {
                errorMessage = "Database Exception:" + e.Message + " " + e.StackTrace;
                return false;
            }
        }

        public bool GetEvent( ref string errorMessage, ref IO.Swagger.Models.Event ev)
        {
            bool retVal = true;
            try
            {
                using (COPDBContext.monica_cnetContext context = new COPDBContext.monica_cnetContext())
                {


                    var a = context.Event.FirstOrDefault();
                    if (a != null)
                    {
                        ev = new IO.Swagger.Models.Event();
                        ev.Name= a.Name;
                        ev.City = a.Description;
                        ev.Lat = (decimal) a.Lat;
                        ev.Lon = (decimal)a.Lon;
                        ev.Zoom = a.Zoom;
                        ev.Start = a.Fromdate;
                        ev.End = a.Todate;
                    }
                    else
                    {
                        errorMessage = "Event info missing";
                        return false;
                    }
                }
                return retVal;
            }
            catch (Exception e)
            {
                errorMessage = "Database Exception:" + e.Message + " " + e.StackTrace;
                return false;
            }
        }
    }
}
