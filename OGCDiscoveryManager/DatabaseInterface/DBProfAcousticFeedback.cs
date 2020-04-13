using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IO.Swagger.COPDBContext;
namespace IO.Swagger.DatabaseInterface
{
    public class DBProfAcousticFeedback
    {
        public bool AddFeedback(string feedbackTypeStr, string phoneid, string textmessage, double reporttype, double lat, double lon, ref string errorMessage, ref long newid)
        {
            bool retVal = true;
            try
            {
                using (COPDBContext.monica_cnetContext context = new COPDBContext.monica_cnetContext())
                {

                    //Find type 
                    var FeedbackType = context.ProAcousticFeedbackTypes
                            .Single(b => b.Name == feedbackTypeStr);

                    if (FeedbackType == null)
                    {
                        errorMessage = "Non-Existing Feedback type";
                        retVal = false;
                    }
                    else
                    {
                        ProAcousticFeedback a = new ProAcousticFeedback();
                        a.ProAcousticFeedbackType = FeedbackType.Id;
                        a.ReportType = reporttype;
                        a.Textmessage = textmessage;
                        a.Phoneid = phoneid;
                        a.Lat = lat;
                        a.Lon = lon;


                        context.ProAcousticFeedback.Add(a);
                        context.SaveChanges();

                        newid = a.Id;
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

        public bool ListProAcousticFeedback(string feedbackType, ref string errorMessage, ref List<IO.Swagger.Models.ProAcousticFeedback> results)
        {
            bool retVal = true;
            try
            {
                using (COPDBContext.monica_cnetContext context = new COPDBContext.monica_cnetContext())
                {

                    //Make query 
                    var feedbacks = (from d in context.ProAcousticFeedback
                                     join f in context.ProAcousticFeedbackTypes
                                     on d.ProAcousticFeedbackType equals f.Id
                                     where f.Name == feedbackType
                                     select new
                                     { id = d.Id, lat = d.Lat, lon = d.Lon, textmessage = d.Textmessage, reportype =d.ReportType }
                                     ).ToList();

                    if (feedbacks == null)
                    {
                        errorMessage = "No feedbacks";
                        retVal = false;
                    }
                    else
                        foreach (var loc in feedbacks)
                        {
                            IO.Swagger.Models.ProAcousticFeedback z = new Models.ProAcousticFeedback();
                            z.FeedbackLat = (decimal) loc.lat;
                            z.FeedbackLon = (decimal) loc.lon;
                            z.FeedbackMessage = loc.textmessage;
                            z.FeedbackType = feedbackType;
                            z.FeedbackValue = (decimal) loc.reportype;
                       
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


        public bool ListAllProAcousticFeedback(ref string errorMessage, ref List<IO.Swagger.Models.ProAcousticFeedback> results)
        {
            bool retVal = true;
            try
            {
                using (COPDBContext.monica_cnetContext context = new COPDBContext.monica_cnetContext())
                {

                    //Make query 
                    var feedbacks = (from d in context.ProAcousticFeedback
                                     join f in context.ProAcousticFeedbackTypes
                                     on d.ProAcousticFeedbackType equals f.Id
                                     select new
                                     { id = d.Id, lat = d.Lat, lon = d.Lon, textmessage = d.Textmessage, reportype = d.ReportType, feedbacktype = f.Name}
                                     ).ToList();

                    if (feedbacks == null)
                    {
                        errorMessage = "No feedbacks";
                        retVal = false;
                    }
                    else
                        foreach (var loc in feedbacks)
                        {
                            IO.Swagger.Models.ProAcousticFeedback z = new Models.ProAcousticFeedback();
                            z.FeedbackLat = (decimal)loc.lat;
                            z.FeedbackLon = (decimal)loc.lon;
                            z.FeedbackMessage = loc.textmessage;
                            z.FeedbackType = loc.feedbacktype;
                            z.FeedbackValue = (decimal)loc.reportype;

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
