using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IO.Swagger.COPDBContext;

namespace IO.Swagger.DatabaseInterface
{
    public class DBIncident
    {

        public bool AddIncident(string description, string itype, string position, int prio, string status, double probability, string iplan, DateTime itime, string wid, string phoneno, string AdditionalMedia, string MediaType, ref string errorMessage, ref long newid)
        {
            bool retVal = true;
            try
            {
                using (COPDBContext.monica_cnetContext context = new COPDBContext.monica_cnetContext())
                {


                    Incident a = new Incident();
                    a.Description = description;
                    a.Incidenttime = itime;
                    a.Interventionplan = iplan;
                    a.Position = position;
                    a.Prio = prio;
                    a.Probability = probability;
                    a.Status = status;
                    a.Type = itype;
                    a.WearablePhysicalId = wid;
                    a.PhoneNumber = phoneno;
                    a.AdditionalMedia = AdditionalMedia;
                    a.AdditionalMediaType = MediaType;
                    context.Incident.Add(a);
                    context.SaveChanges();

                    newid = a.Incidentid;

                }
                return retVal;
            }
            catch (Exception e)
            {
                errorMessage = "Database Exception:" + e.Message + " " + e.StackTrace;
                return false;
            }
        }

        public bool UpdateIncident(int id, string description, string itype, string position, int prio, string status, double probability, string iplan, DateTime itime, string wid, string phoneno, string AdditionalMedia, string MediaType, ref string errorMessage, ref long newid)
        {
            bool retVal = true;
            try
            {
                using (COPDBContext.monica_cnetContext context = new COPDBContext.monica_cnetContext())
                {


                    var a = context.Incident.FirstOrDefault(Incident => Incident.Incidentid == id);
                   if(a != null)
                    {
                        a.Description = description;
                        a.Incidenttime = itime;
                        a.Interventionplan = iplan;
                        a.Position = position;
                        a.Prio = prio;
                        a.Probability = probability;
                        a.Status = status;
                        a.Type = itype;
                        a.WearablePhysicalId = wid;
                        a.PhoneNumber = phoneno;
                        a.AdditionalMedia = AdditionalMedia;
                        a.AdditionalMediaType = MediaType;
                        context.Incident.Update(a);
                        context.SaveChanges();
                    }
                   else
                    {
                        errorMessage = "Incident with ID :" + id + "  does not exist";
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

        public bool GetIncident(int id, ref string errorMessage, ref IO.Swagger.Models.Incident inc)
        {
            bool retVal = true;
            try
            {
                using (COPDBContext.monica_cnetContext context = new COPDBContext.monica_cnetContext())
                {


                    var a = context.Incident.FirstOrDefault(Incident => Incident.Incidentid == id);
                    if (a != null)
                    {
                        inc = new IO.Swagger.Models.Incident();
                        inc.Incidentid = a.Incidentid;
                        inc.Description =a.Description;
                        inc.Incidenttime = a.Incidenttime;
                        inc.Interventionplan = a.Interventionplan;
                        inc.Position = a.Position;
                        inc.Prio = a.Prio;
                        inc.Probability = (decimal) a.Probability;
                        inc.Status =a.Status;
                        inc.Type = a.Type;
                        inc.Wbid = a.WearablePhysicalId ;
                        inc.Telephone = a.PhoneNumber;
                        inc.AdditionalMedia = a.AdditionalMedia;
                        inc.MediaType = a.AdditionalMediaType;
                        context.Incident.Update(a);
                        context.SaveChanges();
                    }
                    else
                    {
                        errorMessage = "Incident with ID :" + id + "  does not exist";
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

        public bool ListIncidents(string itype, string istatus, ref string errorMessage, ref List<IO.Swagger.Models.Incident> results)
        {
            bool retVal = true;
            try
            {
                using (COPDBContext.monica_cnetContext context = new COPDBContext.monica_cnetContext())
                {

                    //Make query 
                    if (itype != null && istatus != null)
                    {
                        var incidents = (from d in context.Incident
                                         where d.Status == istatus && d.Type == itype
                                         select new
                                         { id = d.Incidentid, description = d.Description, itime = d.Incidenttime, iplan = d.Interventionplan, position = d.Position, prio = d.Prio, probability = d.Probability, status = d.Status, itype = d.Type, wid = d.WearablePhysicalId, phone = d.PhoneNumber, additionalMedia = d.AdditionalMedia, mediaType = d.AdditionalMediaType }
                                        ).ToList();

                        if (incidents == null)
                        {
                            errorMessage = "No incidents";
                            retVal = false;
                        }
                        else
                            foreach (var loc in incidents)
                            {
                                IO.Swagger.Models.Incident z = new Models.Incident();
                                z.Description = loc.description;
                                z.Incidentid = loc.id;
                                z.Incidenttime = loc.itime;
                                z.Interventionplan = loc.iplan;
                                z.Position = loc.position;
                                z.Prio = loc.prio;
                                z.Probability = (decimal)loc.probability;
                                z.Status = loc.status;
                                z.Type = loc.itype;
                                z.Wbid = loc.wid;
                                z.Telephone = loc.phone;
                                z.AdditionalMedia = loc.additionalMedia;
                                z.MediaType = loc.mediaType;
                                results.Add(z);

                            }
                    }

                    //Make query 
                    else if (istatus != null)
                    {
                        var incidents = (from d in context.Incident
                                         where d.Status == istatus
                                         select new
                                         { id = d.Incidentid, description = d.Description, itime = d.Incidenttime, iplan = d.Interventionplan, position = d.Position, prio = d.Prio, probability = d.Probability, status = d.Status, itype = d.Type, wid = d.WearablePhysicalId, phone = d.PhoneNumber, additionalMedia = d.AdditionalMedia, mediaType = d.AdditionalMediaType }
                                        ).ToList();

                        if (incidents == null)
                        {
                            errorMessage = "No incidents";
                            retVal = false;
                        }
                        else
                            foreach (var loc in incidents)
                            {
                                IO.Swagger.Models.Incident z = new Models.Incident();
                                z.Description = loc.description;
                                z.Incidentid = loc.id;
                                z.Incidenttime = loc.itime;
                                z.Interventionplan = loc.iplan;
                                z.Position = loc.position;
                                z.Prio = loc.prio;
                                z.Probability = (decimal)loc.probability;
                                z.Status = loc.status;
                                z.Type = loc.itype;
                                z.Wbid = loc.wid;
                                z.Telephone = loc.phone;
                                z.AdditionalMedia = loc.additionalMedia;
                                z.MediaType = loc.mediaType;
                                results.Add(z);

                            }
                    }
                   
                    else if (itype != null)
                    {
                        var incidents = (from d in context.Incident
                                         where d.Type == itype
                                         select new
                                         { id = d.Incidentid, description = d.Description, itime = d.Incidenttime, iplan = d.Interventionplan, position = d.Position, prio = d.Prio, probability = d.Probability, status = d.Status, itype = d.Type, wid = d.WearablePhysicalId, phone = d.PhoneNumber, additionalMedia = d.AdditionalMedia, mediaType = d.AdditionalMediaType }
                                        ).ToList();

                        if (incidents == null)
                        {
                            errorMessage = "No incidents";
                            retVal = false;
                        }
                        else
                            foreach (var loc in incidents)
                            {
                                IO.Swagger.Models.Incident z = new Models.Incident();
                                z.Description = loc.description;
                                z.Incidentid = loc.id;
                                z.Incidenttime = loc.itime;
                                z.Interventionplan = loc.iplan;
                                z.Position = loc.position;
                                z.Prio = loc.prio;
                                z.Probability = (decimal)loc.probability;
                                z.Status = loc.status;
                                z.Type = loc.itype;
                                z.Wbid = loc.wid;
                                z.Telephone = loc.phone;
                                z.AdditionalMedia = loc.additionalMedia;
                                z.MediaType = loc.mediaType;
                                results.Add(z);

                            }
                    }
                    else
                    {
                        var incidents = (from d in context.Incident
                                         select new
                                         { id = d.Incidentid, description = d.Description, itime = d.Incidenttime, iplan = d.Interventionplan, position = d.Position, prio = d.Prio, probability = d.Probability, status = d.Status, itype = d.Type, wid = d.WearablePhysicalId, phone = d.PhoneNumber, additionalMedia = d.AdditionalMedia, mediaType = d.AdditionalMediaType }
                                        ).ToList();

                        if (incidents == null)
                        {
                            errorMessage = "No incidents";
                            retVal = false;
                        }
                        else
                            foreach (var loc in incidents)
                            {
                                IO.Swagger.Models.Incident z = new Models.Incident();
                                z.Description = loc.description;
                                z.Incidentid = loc.id;
                                z.Incidenttime = loc.itime;
                                z.Interventionplan = loc.iplan;
                                z.Position = loc.position;
                                z.Prio = loc.prio;
                                z.Probability = (decimal)loc.probability;
                                z.Status = loc.status;
                                z.Type = loc.itype;
                                z.Wbid = loc.wid;
                                z.Telephone = loc.phone;
                                z.AdditionalMedia = loc.additionalMedia;
                                z.MediaType = loc.mediaType;
                                results.Add(z);

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
