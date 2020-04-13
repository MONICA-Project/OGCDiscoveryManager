using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;

namespace IO.Swagger.DatabaseInterface
{
    public class DBPublicFeedback
    {
        public bool AddOrUpdatePublicFeedback(string phoneid, string feedbacktype, double feedbackvalue, ref string errorMessage)
        {
            bool retVal = true;

            using (COPDBContext.monica_cnetContext context = new COPDBContext.monica_cnetContext())
            {

                //Find type
                try
                {
                    var obs = context.PublicFeedback
                           .Single(b => b.Phoneid == phoneid && b.FeedbackType == feedbacktype);


                    obs.Phoneid = phoneid;
                    obs.FeedbackType = feedbacktype;
                    obs.FeedbackValue = feedbackvalue;

                    context.SaveChanges();

                }
                catch (Exception f)
                {
                    COPDBContext.PublicFeedback lo = new COPDBContext.PublicFeedback();
                    lo.Phoneid = phoneid;
                    lo.FeedbackType = feedbacktype;
                    lo.FeedbackValue = feedbackvalue;
                    context.PublicFeedback.Add(lo);
                    context.SaveChanges();
                }
            }
            return retVal;
        }

        public bool GetAvg(ref string errorMessage, ref List<Models.PublicFeedbackResult> results)
        {


            var connString = settings.ConnectionString;
            try
            {
                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "select count(*),monica.public_feedback.feedback_type, AVG(monica.public_feedback.feedback_value) from monica.public_feedback group by monica.public_feedback.feedback_type";
                    
                        using (var reader = cmd.ExecuteReader())
                            while (reader.Read())
                            {
                                Models.PublicFeedbackResult pf = new Models.PublicFeedbackResult();
                                pf.Count = reader.GetInt32(0);
                                pf.FeedbackType = reader.GetString(1);
                                pf.FeedbackMeanvalue = (decimal) reader.GetDouble(2);
                                results.Add(pf);
                            }
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                errorMessage = "Database Excaption:" + e.Message + " " + e.StackTrace;
                return false;
            }
        }
    }
}
