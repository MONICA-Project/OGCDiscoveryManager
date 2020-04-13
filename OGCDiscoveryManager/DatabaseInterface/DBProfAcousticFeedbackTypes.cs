using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IO.Swagger.COPDBContext;
namespace IO.Swagger.DatabaseInterface
{
    public class DBProfAcousticFeedbackTypes
    {
        public bool AddFeedbackType(string feedbackTypeName, string description, ref string errorMessage, ref long newid)
        {
            bool retVal = true;
            try
            {
                using (COPDBContext.monica_cnetContext context = new COPDBContext.monica_cnetContext())
                {

                    
                        ProAcousticFeedbackTypes a = new ProAcousticFeedbackTypes();
                    a.Name = feedbackTypeName;
                    a.Description = description;
                     
                        context.ProAcousticFeedbackTypes.Add(a);
                        context.SaveChanges();

                        newid = a.Id;
                   
                }
                return retVal;
            }
            catch (Exception e)
            {
                errorMessage = "Database Exception:" + e.Message + " " + e.StackTrace;
                return false;
            }
        }

        public bool ListProAcousticFeedbackTypes( ref string errorMessage, ref List<IO.Swagger.Models.ProAcousticFeedbackType> results)
        {
            bool retVal = true;
            try
            {
                using (COPDBContext.monica_cnetContext context = new COPDBContext.monica_cnetContext())
                {

                    //Make query 
                    var feedbackt = (from d in context.ProAcousticFeedbackTypes
                                     select new
                                     { id = d.Id, typeName = d.Name, description = d.Description}
                                     ).ToList();

                    if (feedbackt == null)
                    {
                        errorMessage = "No feedbacks";
                        retVal = false;
                    }
                    else
                        foreach (var loc in feedbackt)
                        {
                            IO.Swagger.Models.ProAcousticFeedbackType z = new Models.ProAcousticFeedbackType();
                            z.Typeid = loc.id;
                            z.FeedbackTypeName = loc.typeName;
                            z.FeedbackTypeDescription = loc.description;
                       

                            results.Add(z);

                        }



                    //Insert role connection;
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
