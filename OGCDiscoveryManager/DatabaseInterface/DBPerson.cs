using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;

namespace IO.Swagger.DatabaseInterface
{
    public class DBPerson
    {

        public bool AddPerson(int personid, string role, string fullname, string email,string phone,int organizationid,bool? active , ref string errorMessage, ref int newid)
        {

            var connString = settings.ConnectionString;
            try
            {
                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    //Find the role id
                    int roleID = 0;
                    int newPersonId = -1;
                    using (var cmd = new NpgsqlCommand("SELECT roleid FROM monica.role where Const=@role", conn))
                    {
                        cmd.Parameters.AddWithValue("role", role);
                        using (var reader = cmd.ExecuteReader())
                            while (reader.Read())
                               roleID = reader.GetInt32(0);
                    }
                    if (roleID == 0)
                    {
                        errorMessage = "Non-Existing Role";
                        return false;
                    }
                    // Insert person data
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "INSERT INTO monica.person (FullName,Email,Phone,OrganizationId,IsActive) VALUES (@fullname,@email,@phone,@OrganizationId,@active);SELECT currval(pg_get_serial_sequence('monica.person','personid'));";
                       // cmd.Parameters.AddWithValue("personid", personid);
                        cmd.Parameters.AddWithValue("fullname", fullname);
                        cmd.Parameters.AddWithValue("email", email);
                        cmd.Parameters.AddWithValue("phone", phone);
                        cmd.Parameters.AddWithValue("OrganizationId", organizationid);
                        cmd.Parameters.AddWithValue("active", active);
                        //cmd.ExecuteNonQuery();
                        using (var reader = cmd.ExecuteReader())
                            while (reader.Read())
                                newPersonId = reader.GetInt32(0);
                    }
                    newid = newPersonId;
                    //Insert role connection
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "INSERT INTO monica.person_roles (PersonId, RoleId) VALUES (@personid,@roleid)";
                        cmd.Parameters.AddWithValue("personid", newPersonId);
                        cmd.Parameters.AddWithValue("roleid", roleID);
                        cmd.ExecuteNonQuery();
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

        public bool UpdatePerson(int personid, string role, string fullname, string email, string phone, int organizationid, bool? active, ref string errorMessage)
        {
            bool retVal = true;

            var connString = settings.ConnectionString;
            try
            {
                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    //Find the role id
                    int roleID = 0;
                    int newPersonId = -1;
                    using (var cmd = new NpgsqlCommand("SELECT roleid FROM monica.role where Const=@role", conn))
                    {
                        cmd.Parameters.AddWithValue("role", role);
                        using (var reader = cmd.ExecuteReader())
                            while (reader.Read())
                                roleID = reader.GetInt32(0);
                    }
                    if (roleID == 0)
                    {
                        errorMessage = "Non-Existing Role";
                        return false;
                    }
                    // Insert person data
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "UPDATE monica.person SET FullName=@fullname,Email=@email,Phone=@phone,OrganizationId=@OrganizationId,IsActive=@active where personid=@personid ;";
                        cmd.Parameters.AddWithValue("personid", personid);
                        cmd.Parameters.AddWithValue("fullname", fullname);
                        cmd.Parameters.AddWithValue("email", email);
                        cmd.Parameters.AddWithValue("phone", phone);
                        cmd.Parameters.AddWithValue("OrganizationId", organizationid);
                        cmd.Parameters.AddWithValue("active", active);
                        //cmd.ExecuteNonQuery();
                        using (var reader = cmd.ExecuteReader()) ;
                        
                    }
                    //Insert role connection
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "UPDATE monica.person_roles SET RoleId=@roleid WHERE personid=@personid";
                        cmd.Parameters.AddWithValue("personid", personid);
                        cmd.Parameters.AddWithValue("roleid", roleID);
                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                errorMessage = "Database Excaption:" + e.Message + " " + e.StackTrace;
                return false;
            }
            try
            {
                using (COPDBContext.monica_cnetContext context = new COPDBContext.monica_cnetContext())
                {


                    var a = context.Person.FirstOrDefault(Person => Person.Personid == personid);
                    if (a != null)
                    {
                        a.Fullname = fullname;
                        a.Email = email;
                        a.Phone = phone;


                        context.Person.Update(a);
                        context.SaveChanges();
                    }
                    else
                    {
                        errorMessage = "Person with ID :" + personid + "  does not exist";
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

        public bool DeletePerson(int personid, ref string errorMessage)
        {


            var connString = settings.ConnectionString;
            try
            {
                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    //Find the role id
            
                    
                    // Delete person role relations
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "DELETE FROM monica.person_roles where personid=@personid";
                        cmd.Parameters.AddWithValue("personid", personid);
                        cmd.ExecuteNonQuery();
   
                    }
                    //Insert role connection
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "DELETE FROM monica.person  where personid=@personid";
                        cmd.Parameters.AddWithValue("personid", personid);
                        cmd.ExecuteNonQuery();
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



        public bool FindPersonFromId(int personid, ref string errorMessage, ref Models.Person foundPerson)
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
                        cmd.CommandText = "select  monica.person.personid, monica.person.fullname,monica.person.email,monica.person.phone,monica.person.isactive,monica.role.const, monica.role.description from monica.person, monica.role,monica.person_roles  where person.personid = person_roles.personid and person_roles.roleid = role.roleid and person.personid=@personid";
                        cmd.Parameters.AddWithValue("personid", personid);
                        //cmd.Prepare();
                        //cmd.ExecuteNonQuery();
                        using (var reader = cmd.ExecuteReader())
                            while (reader.Read())
                            {
                                foundPerson = new Models.Person();
                                foundPerson.Id = reader.GetInt32(0);
                                foundPerson.FullName = reader.GetString(1);
                                foundPerson.Email = reader.GetString(2);
                                foundPerson.Phone = reader.GetString(3);
                                foundPerson.Active = reader.GetBoolean(4);
                                foundPerson.Role = reader.GetString(5);
                                foundPerson.RoleDescription = reader.GetString(5);
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

        public bool AllPersons( ref string errorMessage, ref List<Models.Person> foundPersons)
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
                        cmd.CommandText = "select monica.person.personid, monica.person.fullname,monica.person.email,monica.person.phone,monica.person.isactive,monica.role.const, monica.role.description from monica.person, monica.role,monica.person_roles  where person.personid = person_roles.personid and person_roles.roleid = role.roleid";
                        cmd.Prepare();
                        foundPersons = new List<Models.Person>();
                        //cmd.ExecuteNonQuery();
                        using (var reader = cmd.ExecuteReader())
                            while (reader.Read())
                            {
                                Models.Person foundPerson = new Models.Person();
                                foundPerson.Id = reader.GetInt32(0);
                                foundPerson.FullName = reader.GetString(1);
                                foundPerson.Email = reader.GetString(2);
                                foundPerson.Phone = reader.GetString(3);
                                foundPerson.Active = reader.GetBoolean(4);
                                foundPerson.Role = reader.GetString(5);
                                foundPerson.RoleDescription = reader.GetString(6);
                                foundPersons.Add(foundPerson);
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


        public bool AllPersonsWithRole(string role,ref string errorMessage, ref List<Models.Person> foundPersons)
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
                        cmd.CommandText = "select monica.person.personid, monica.person.fullname,monica.person.email,monica.person.phone,monica.person.isactive,monica.role.const, monica.role.description from monica.person, monica.role,monica.person_roles  where person.personid = person_roles.personid and person_roles.roleid = role.roleid and role.const=@role";
                        cmd.Parameters.AddWithValue("role", role);
                        //cmd.Prepare();
                        foundPersons = new List<Models.Person>();
                        //cmd.ExecuteNonQuery();
                        using (var reader = cmd.ExecuteReader())
                            while (reader.Read())
                            {
                                Models.Person foundPerson = new Models.Person();
                                foundPerson.Id = reader.GetInt32(0);
                                foundPerson.FullName = reader.GetString(1);
                                foundPerson.Email = reader.GetString(2);
                                foundPerson.Phone = reader.GetString(3);
                                foundPerson.Active = reader.GetBoolean(4);
                                foundPerson.Role = reader.GetString(5);
                                foundPerson.RoleDescription = reader.GetString(6);
                                foundPersons.Add(foundPerson);
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
