using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ps3g.Models
{
    public class DbModel
    {
        public bool ValidateLogin(UserData objuser)
        {
            try
            {
                objdb = new DbModel();
                bool IsValidlogin = false;
                cmd = new SqlCommand();
                objdb.OpenConn();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = objdb.conn;
                cmd.CommandTimeout = 0;
                cmd.CommandText = "USP_Login";
                cmd.Parameters.AddWithValue("@Username", objuser.UserName);
                cmd.Parameters.AddWithValue("@Password", objuser.Password);
                dt = new DataTable();
                dt = objdb.RetDt(cmd);
                if (dt.Rows.Count > 0)
                {
                    IsValidlogin = true;
                    int Temp;
                    if (int.TryParse(dt.Rows[0]["UserID"].ToString(), out Temp))
                    {
                        objuser.UserId = Temp;
                        sessionobj.SetSession("UserId", Temp.ToString());
                    }
                    if (int.TryParse(dt.Rows[0]["CompanyID"].ToString(), out Temp))
                    {
                        sessionobj.SetSession("CompanyId", Temp.ToString());
                        objuser.CompanyId = Temp;
                    }
                    if (int.TryParse(dt.Rows[0]["RoleID"].ToString(), out Temp))
                    {
                        objuser.RoleId = Temp;
                        sessionobj.SetSession("RoleId", Temp.ToString());
                    }
                    if (int.TryParse(dt.Rows[0]["DepartmentID"].ToString(), out Temp))
                    {
                        objuser.DepartmentId = Temp;
                        sessionobj.SetSession("DepartmentId", Temp.ToString());
                    }
                    if (int.TryParse(dt.Rows[0]["RoleCode"].ToString(), out Temp))
                    {
                        objuser.RoleCode = Temp;
                        sessionobj.SetSession("RoleCode", Temp.ToString());
                    }

                    //System.Web.HttpContext.Current.Session["SchoolType"] = "0";
                    objuser.Email = dt.Rows[0]["Email"].ToString();
                }

                return IsValidlogin;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objdb.CloseConn();
            }
        }
    }

    public class UserData
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }



}
