using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using DotNetCoreAPI.Models;

namespace DotNetCoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select DepartmentId, Department as DepartmentName from department";
            DataTable dt = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("AppDBConnection");
            SqlDataReader DR;
            using (SqlConnection conn=new SqlConnection(sqlDataSource))
            {
                conn.Open();
                using(SqlCommand cmd=new SqlCommand(query, conn))
                {
                    DR = cmd.ExecuteReader();
                    dt.Load(DR);
                    DR.Close();
                    conn.Close();
                }
            }

            return new JsonResult(dt);
        }

        [HttpPost]
        public JsonResult Post(Department dep)
        {
            string query = @"insert into Department values(@DepartmentName)";
            DataTable dt = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("AppDBConnection");
            SqlDataReader DR;
            using (SqlConnection conn = new SqlConnection(sqlDataSource))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@DepartmentName", dep.DepartmentName);
                    DR = cmd.ExecuteReader();
                    dt.Load(DR);
                    DR.Close();
                    conn.Close();
                }
            }

            return new JsonResult("Added Successfully");
        }

        [HttpPut]
        public JsonResult Put(Department dep)
        {
            string query = @"update Department set Department = @DepartmentName where DepartmentId=@DepartmentId";
            DataTable dt = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("AppDBConnection");
            SqlDataReader DR;
            using (SqlConnection conn = new SqlConnection(sqlDataSource))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@DepartmentName", dep.DepartmentName);
                    cmd.Parameters.AddWithValue("@DepartmentId", dep.DepartmentId);
                    DR = cmd.ExecuteReader();
                    dt.Load(DR);
                    DR.Close();
                    conn.Close();
                }
            }

            return new JsonResult("Updated Successfully");
        }

        [HttpDelete("{Id}")]
        public JsonResult Delete(int Id)
        {
            string query = @"delete from Department where DepartmentId=@DepartmentId";
            DataTable dt = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("AppDBConnection");
            SqlDataReader DR;
            using (SqlConnection conn = new SqlConnection(sqlDataSource))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@DepartmentId", Id);
                    DR = cmd.ExecuteReader();
                    dt.Load(DR);
                    DR.Close();
                    conn.Close();
                }
            }

            return new JsonResult("Deleted Successfully");
        }
    }
}
