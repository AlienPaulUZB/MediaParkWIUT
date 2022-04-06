using MediaPPP.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace MediaPPP.Repositories
{
    public class CategoryRepo
    {
        public static string conn
        {
            get
            {
                return WebConfigurationManager
                    .ConnectionStrings["DBConnection"]
                    .ConnectionString;
            }
        }

        public IList<Category> GetAll()
        {
            IList<Category> list = new List<Category>();
            using(DbConnection con = new SqlConnection(conn))
            {
                using(DbCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SELECT [CategoryID], [CategoryName] FROM [dbo].[Category]";
                    con.Open();
                    using(DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Category c = new Category()
                            {
                                CategoryID=reader.GetInt32(0),
                                CategoryName=reader.GetString(1)
                            };
                            list.Add(c);
                        }
                    }
                }
            }
            return list;
        }
        
        public void Create(Category category)
        {
            using (DbConnection con = new SqlConnection(conn))
            {
                using (DbCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO [dbo].[Category] ([CategoryName]) VALUES (@CategoryName)";
                    DbParameter pCategoryName=cmd.CreateParameter();
                    pCategoryName.Value = category.CategoryName;
                    pCategoryName.ParameterName = "@CategoryName";
                    pCategoryName.DbType = System.Data.DbType.String;
                    cmd.Parameters.Add(pCategoryName);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void Update(Category category)
        {
            using (DbConnection connection = new SqlConnection(conn))
            {
                using(DbCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "UPDATE [dbo].[Category] SET [CategoryName] = @CategoryName WHERE [CategoryID] = @CategoryID";
                    
                    DbParameter pCategoryName = cmd.CreateParameter();
                    pCategoryName.ParameterName = "@CategoryName";
                    pCategoryName.Value=category.CategoryName;
                    pCategoryName.DbType=System.Data.DbType.String;
                    cmd.Parameters.Add(pCategoryName);
                    
                    DbParameter pCategoryID = cmd.CreateParameter();
                    pCategoryID.ParameterName = "@CategoryID";
                    pCategoryID.Value=category.CategoryID;
                    pCategoryID.DbType=System.Data.DbType.String;
                    cmd.Parameters.Add(pCategoryID);

                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public Category GetById(int id)
        {
            Category category = null;
            using(var connection = new SqlConnection(conn))
            {
                using(var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT [CategoryID],[CategoryName] FROM [dbo].[Category] WHERE [CategoryID]=@CategoryID";
                    cmd.Parameters.AddWithValue("CategoryID",id);
                    connection.Open();
                    using(var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            category= new Category()
                            { CategoryID=id,CategoryName=reader.GetString(reader.GetOrdinal("CategoryName"))};
                        }
                    }
                }
            }
            return category;
        }
        public void Delete(int id)
        {
            using(var connection = new SqlConnection(conn))
            {
                using(var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM [dbo].[Category] WHERE CategoryID=@CategoryID";
                    command.Parameters.AddWithValue("CategoryID", id);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}