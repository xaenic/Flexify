using Flexify.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using System.Data.SqlClient;

namespace Flexify.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ViewResult Register(UserModel user)
        {
            if(!ModelState.IsValid) {
                return View();
            }
            string? connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

            if(connectionString == null ) { 
                return View("Error");
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            lblError.Text = "Registration successful!";
                            lblError.ForeColor = System.Drawing.Color.Green;
                            lblError.Visible = true;
                        }
                        else
                        {
                            lblError.Text = "Failed to register user.";
                            lblError.ForeColor = System.Drawing.Color.Red;
                            lblError.Visible = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = "An error occurred: " + ex.Message;
                        lblError.ForeColor = System.Drawing.Color.Red;
                        lblError.Visible = true;
                    }
                }
            }
            return View();

            
           
        }
    }
}
