using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

namespace ps3g.Models
{
    public class DbModel
    {

        static List<UserData> objUserData = new List<UserData>();
        private readonly SessionUtility objSession;
        public DbModel()
        {
            objSession = new SessionUtility();

        }
        public bool ValidateLogin(UserData objuser)
        {
            try
            {
                bool IsValidlogin = false;
                foreach (var item in objUserData.Where(item => objuser.UserName == item.UserName && objuser.Password == item.Password))
                {
                    IsValidlogin = true;
                    
                    if (item.UserName != null)
                    {
                        objuser.UserName = item.UserName;
                        objSession.SetSession("UserName", item.UserName);

                    }
                    if (item.Password != null)
                    {
                        objuser.Password = item.Password;
                        objSession.SetSession("Password", item.Password);
                    }

                }

                return IsValidlogin;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public Response Signup(UserData model)
        {
            Response objres = new Response();
            try
            {
                bool check = false;
                foreach (var item in objUserData.Where(item => model.UserName == item.UserName))
                {
                    check = true;
                }
                if (check == false)
                {
                    objUserData.Add(model);
                    objres.message = "Successfull";
                    objres.title = "Success";
                    objres.type = "success";
                }
                else
                {
                    objres.message = "Username Already Exist, Use Another one!!";
                    objres.title = "Warning";
                    objres.type = "warning";
                }

            }
            catch (Exception ex)
            {
                objres.message = ex.Message;
                objres.title = "Failed";
                objres.type = "error";
            }
            return objres;
        }
    }

    public class UserData
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class Response
    {
        public string type { get; set; }
        public string title { get; set; }
        public string message { get; set; }
        public bool status { get; set; }
        public int DataId { get; set; }
        public int DataId2 { get; set; }
    }



}
