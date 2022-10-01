using DinnamuSWebApi.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DinnamuSWebApi.Repositories.Grades
{
    public class GradeRepository : IGradeRepository
    {
        private IDbConnection _connection;

        public GradeRepository()
        {
            string _con = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            _connection = new SqlConnection(_con);
        }

        public List<Grade> Get()
        {
            List<Grade> grades = new List<Grade>();

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT chaveunica, descricao FROM grade WHERE descricao IS NOT NULL";
                cmd.Connection = (SqlConnection)_connection;
                _connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while(reader.Read())
                {
                    Grade grade = new Grade()
                    {
                        Chaveunica = (int)reader.GetInt32(0),
                        Descricao = reader.GetString(1)
                    };

                    grades.Add(grade);
                }

                return grades;
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public Grade Get(int chaveunica)
        {
            throw new NotImplementedException();
        }

        public void Insert(Grade grade)
        {
            throw new NotImplementedException();
        }

        public void Update(Grade grade)
        {
            throw new NotImplementedException();
        }

        public void Delete(int chaveunica)
        {
            throw new NotImplementedException();
        }
    }
}