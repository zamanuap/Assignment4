using CareerCloud.DataAccessLayer;
using CareerCloud.Pocos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace CareerCloud.ADODataAccessLayer
{
    public class ApplicantProfileRepository : IDataRepository<ApplicantProfilePoco>
    {
        private readonly string _connStr;
        public ApplicantProfileRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }
        public void Add(params ApplicantProfilePoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                foreach (ApplicantProfilePoco item in items)
                {
                   cmd.CommandText = @"INSERT INTO [dbo].[Applicant_Profiles]
                                       ([Id]
                                       ,[Login]
                                       ,[Current_Salary]
                                       ,[Current_Rate]
                                       ,[Currency]
                                       ,[Country_Code]
                                       ,[State_Province_Code]
                                       ,[Street_Address]
                                       ,[City_Town]
                                       ,[Zip_Postal_Code])
                                 VALUES
                                       (@Id,
                                        @Login,
                                        @Current_Salary,
                                        @Current_Rate,
                                        @Currency,
                                        @Country_Code,
                                        @State_Province_Code,
                                        @Street_Address,
                                        @City_Town,
                                        @Zip_Postal_Code)";

                    cmd.Parameters.AddWithValue("@Id", item.Id);
                    cmd.Parameters.AddWithValue("@Login", item.Login);
                    cmd.Parameters.AddWithValue("@Current_Salary", item.CurrentSalary);
                    cmd.Parameters.AddWithValue("@Current_Rate", item.CurrentRate);
                    cmd.Parameters.AddWithValue("@Currency", item.Currency);
                    cmd.Parameters.AddWithValue("@Country_Code", item.Country);
                    cmd.Parameters.AddWithValue("@State_Province_Code", item.Province);
                    cmd.Parameters.AddWithValue("@Street_Address", item.Street);
                    cmd.Parameters.AddWithValue("@City_Town", item.City);
                    cmd.Parameters.AddWithValue("@Zip_Postal_Code", item.PostalCode);

                    connection.Open();
                    int rowEffected = cmd.ExecuteNonQuery();
                    connection.Close();
                }

            }
        }

        public void CallStoredProc(string name, params Tuple<string, string>[] parameters)
        {
            throw new NotImplementedException();
        }

        public IList<ApplicantProfilePoco> GetAll(params Expression<Func<ApplicantProfilePoco, object>>[] navigationProperties)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = @"SELECT
                                    [Id]
                                    ,[Login]
                                    ,[Current_Salary]
                                    ,[Current_Rate]
                                    ,[Currency]
                                    ,[Country_Code]
                                    ,[State_Province_Code]
                                    ,[Street_Address]
                                    ,[City_Town]
                                    ,[Zip_Postal_Code]
                                    ,[Time_Stamp]
                                    FROM [dbo].[Applicant_Profiles]";

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                ApplicantProfilePoco[] pocos = new ApplicantProfilePoco[100];
                int index = 0;
                while (reader.Read())
                {
                    ApplicantProfilePoco poco = new ApplicantProfilePoco();
                    poco.Id = reader.GetGuid(0);
                    poco.Login = reader.GetGuid(1);
                    poco.CurrentSalary = reader.IsDBNull(2) ? (decimal?)null : reader.GetDecimal(2);
                    poco.CurrentRate = reader.IsDBNull(3) ? (decimal?)null : reader.GetDecimal(3);
                    poco.Currency = reader.IsDBNull(4) ? null : reader.GetString(4);
                    poco.Country = reader.IsDBNull(5) ? null : reader.GetString(5);
                    poco.Province = reader.IsDBNull(6) ? null : reader.GetString(6);
                    poco.Street = reader.IsDBNull(7) ? null : reader.GetString(7);
                    poco.City = reader.IsDBNull(8) ? null : reader.GetString(8);
                    poco.PostalCode = reader.IsDBNull(9) ? null : reader.GetString(9);
                    poco.TimeStamp = (byte[])reader[10];

                    pocos[index] = poco;
                    index++;
                }
                return pocos.Where(a => a != null).ToList();
            }
        }

        public IList<ApplicantProfilePoco> GetList(Expression<Func<ApplicantProfilePoco, bool>> where, params Expression<Func<ApplicantProfilePoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public ApplicantProfilePoco GetSingle(Expression<Func<ApplicantProfilePoco, bool>> where, params Expression<Func<ApplicantProfilePoco, object>>[] navigationProperties)
        {
            IQueryable<ApplicantProfilePoco> pocos = GetAll().AsQueryable();
            return pocos.Where(where).FirstOrDefault();
        }

        public void Remove(params ApplicantProfilePoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                foreach (ApplicantProfilePoco item in items)
                {
                    cmd.CommandText = @"DELETE [dbo].[Applicant_Profiles] WHERE [Id] = @Id";

                    cmd.Parameters.AddWithValue("@Id", item.Id);

                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void Update(params ApplicantProfilePoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                foreach (ApplicantProfilePoco item in items)
                {
                    cmd.CommandText = @"UPDATE [dbo].[Applicant_Profiles]
                                       SET
                                        [Login] = @Login
                                       ,[Current_Salary] = @Current_Salary
                                       ,[Current_Rate] = @Current_Rate
                                       ,[Currency] = @Currency
                                       ,[Country_Code] = @Country_Code
                                       ,[State_Province_Code] = @State_Province_Code
                                       ,[Street_Address] = @Street_Address
                                       ,[City_Town] = @City_Town
                                       ,[Zip_Postal_Code] =  @Zip_Postal_Code
                                 WHERE [Id] = @Id";

                    cmd.Parameters.AddWithValue("@Id", item.Id);
                    cmd.Parameters.AddWithValue("@Login", item.Login);
                    cmd.Parameters.AddWithValue("@Current_Salary", item.CurrentSalary);
                    cmd.Parameters.AddWithValue("@Current_Rate", item.CurrentRate);
                    cmd.Parameters.AddWithValue("@Currency", item.Currency);
                    cmd.Parameters.AddWithValue("@Country_Code", item.Country);
                    cmd.Parameters.AddWithValue("@State_Province_Code", item.Province);
                    cmd.Parameters.AddWithValue("@Street_Address", item.Street);
                    cmd.Parameters.AddWithValue("@City_Town", item.City);
                    cmd.Parameters.AddWithValue("@Zip_Postal_Code", item.PostalCode);

                    connection.Open();
                    int count = cmd.ExecuteNonQuery();
                    if (count != 1)
                    {
                        throw new Exception();
                    }
                    connection.Close();
                }

            }
        }
    }
}
