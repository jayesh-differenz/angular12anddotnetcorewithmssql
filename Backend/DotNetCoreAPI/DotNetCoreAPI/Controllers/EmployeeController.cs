using DotNetCoreAPI.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public EmployeeController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string connStr = _configuration.GetConnectionString("AppDBConnection");
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(connStr);
            string query = @"select EmployeeId, EmployeeName, Department, CONVERT(varchar,DateOfJoining,23) as DateOfJoining, PhotoFileName from employee";
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            dt.Load(dr);
            conn.Close();
            dr.Close();

            return new JsonResult(dt);
        }

        [HttpPost]
        public JsonResult Post(Employee emp)
        {
            string connStr = _configuration.GetConnectionString("AppDBConnection");
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(connStr);
            string query = @"insert into Employee (EmployeeName,Department,DateOfJoining,PhotoFileName)
                                values(@employeeName, @departmentName, @dateOfJoining, @photoFileName)";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@employeeName", emp.EmployeeName);
            cmd.Parameters.AddWithValue("@departmentName", emp.Department);
            cmd.Parameters.AddWithValue("@dateOfJoining", emp.DateOfJoining);
            cmd.Parameters.AddWithValue("@photoFileName", emp.PhotoFilename);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            dt.Load(dr);
            conn.Close();
            dr.Close();

            return new JsonResult("Added Successfully");
        }

        [HttpPut]
        public JsonResult Put(Employee emp)
        {
            string connStr = _configuration.GetConnectionString("AppDBConnection");
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(connStr);
            string query = @"update Employee 
                                set EmployeeName=@employeeName,
                                    Department=@departmentName,
                                    DateOfJoining=@dateOfJoining,
                                    PhotoFileName=@photoFileName
                                where EmployeeId=@employeeId";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@employeeName", emp.EmployeeName);
            cmd.Parameters.AddWithValue("@departmentName", emp.Department);
            cmd.Parameters.AddWithValue("@dateOfJoining", emp.DateOfJoining);
            cmd.Parameters.AddWithValue("@photoFileName", emp.PhotoFilename);
            cmd.Parameters.AddWithValue("@employeeId", emp.EmployeeId);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            dt.Load(dr);
            conn.Close();
            dr.Close();

            return new JsonResult("Updated Successfully");
        }

        [HttpDelete("{Id}")]
        public JsonResult Delete(int Id)
        {
            string connStr = _configuration.GetConnectionString("AppDBConnection");
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(connStr);
            string query = @"delete from Employee where EmployeeId=@employeeId";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@employeeId", Id);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            dt.Load(dr);
            conn.Close();
            dr.Close();

            return new JsonResult("Deleted Successfully");
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string fileName = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + fileName;
                using (var stream=new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
                return new JsonResult(fileName);
            }
            catch (Exception)
            {
                return new JsonResult("anonymous.png");
            }
        }
    }
}
