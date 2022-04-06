using MediaPPP.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace MediaPPP.Repositories
{
    public class ProdutRepo
    {
        public static string connectionString
        {
            get
            {
                return WebConfigurationManager
                    .ConnectionStrings["DBConnection"]
                    .ConnectionString;
            }
        }

        public IList<Product> GetAll()
        {
            IList<Product> productsList = new List<Product>();
            using (DbConnection conn = new SqlConnection(connectionString))
            {
                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT p.[ProductID] 
                                              ,p.[ProductName]
                                              ,p.[ProductPrice]
                                              ,p.[ProductDescription]
                                              ,c.CategoryName
                                              
                                        FROM [dbo].[Product] p LEFT JOIN Category c on e.CategoryID=c.CategoryID"; ;
                    conn.Open();
                    using (DbDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Product p = new Product()
                            {
                                ProductID = rdr.GetInt32(rdr.GetOrdinal("ProductID")),
                                CategoryID = rdr.GetInt32(rdr.GetOrdinal("CategoryID")),
                                ProductName = rdr.GetString(rdr.GetOrdinal("ProductName")),
                                ProductPrice = rdr.GetDecimal(rdr.GetOrdinal("ProductPrice")),
                                ProductDescription = rdr.GetString(rdr.GetOrdinal("ProductDescription"))
                            };
                            productsList.Add(p);
                        }
                    }
                }
            }
            return productsList;
        }

        public IList<Product> Filter(string prodName, string desc)
        {
            IList<Product> productsList = new List<Product>();
            using (var conn = new SqlConnection(connectionString))
            {
                using (var cmd = conn.CreateCommand())
                {
                    string sql = @"SELECT p.[ProductID] 
                                              
                                              ,p.[ProductName]
                                              ,p.[ProductPrice]
                                              ,p.[ProductDescription]
                                              ,p.CategoryID
                                              ,c.CategoryName
                                              
                                        FROM [dbo].[Product] p LEFT JOIN Category c on p.CategoryID=c.CategoryID ";
                    string whereSql = "";
                    if (!string.IsNullOrEmpty(prodName))
                    {
                        whereSql += (whereSql.Length==0 ? "" : " AND ")
                            + " p.ProductName like @ProductName + '%' ";
                        cmd.Parameters.AddWithValue("@ProductName", prodName);
                    }
                    
                    if (!string.IsNullOrEmpty(desc))
                    {
                        whereSql += (whereSql.Length==0 ? "" : " AND ")
                            + " p.ProductDescription like @ProductDescription + '%' ";
                        cmd.Parameters.AddWithValue("@ProductDescription", desc);
                    }
                   /*
                    if(category.ToString().Length > 0)
                    {
                        whereSql += (whereSql.Length == 0 ? "" : " AND ")
                            + " p.CategoryID like @CategoryID + '%' ";
                        cmd.Parameters.AddWithValue("@CategoryID", category);
                    }
                   */
                    if (!string.IsNullOrEmpty(whereSql))
                    {
                        whereSql = " WHERE " + whereSql;
                    }
                    /*
                    string pageSql = " OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY ";
                    cmd.Parameters.AddWithValue("@offset", (page -1)*pageSize);
                    cmd.Parameters.AddWithValue("@pageSize", pageSize);

                    cmd.CommandText = sql + whereSql + " ORDER BY ProductID " + pageSql;
                    */

                    cmd.CommandText = sql + whereSql;
                    Console.WriteLine(cmd.CommandText);
                    conn.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Product p = new Product()
                            {
                                ProductID = rdr.GetInt32(rdr.GetOrdinal("ProductID")),
                                CategoryID = rdr.GetInt32(rdr.GetOrdinal("CategoryID")),
                                ProductName = rdr.GetString(rdr.GetOrdinal("ProductName")),
                                ProductPrice = rdr.GetDecimal(rdr.GetOrdinal("ProductPrice")),
                                ProductDescription = rdr.GetString(rdr.GetOrdinal("ProductDescription"))
                            };
                            productsList.Add(p);
                        }
                    }
                }
            }
            return productsList;
        }
        public void Create(Product product)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO [dbo].[Product] 
                                                    ([CategoryID],
                                                     [ProductName],
                                                     [ProductPrice],
                                                     [ProductImage],
                                                     [ProductDescription]) 
                                        
                                        VALUES      (@CategoryID,
                                                     @ProductName,
                                                     @ProductPrice,
                                                     @ProductImage,
                                                     @ProductDescription
                                                     )";
                   
                    cmd.Parameters.AddWithValue("@CategoryID",product.CategoryID);
                    cmd.Parameters.AddWithValue("@ProductName",product.ProductName);
                    cmd.Parameters.AddWithValue("@ProductPrice",product.ProductPrice);
                    cmd.Parameters.AddWithValue("@ProductDescription",product.ProductDescription);

                    
                    cmd.Parameters.AddWithValue("@ProductImage",(object)product.ProductImage??SqlBinary.Null);

                    
                    conn.Open();
                    cmd.ExecuteNonQuery();

                }
            }
        }
        public void Update(Product product)
        {
            using (DbConnection conn = new SqlConnection(connectionString))
            {
                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE [dbo].[Product]
                                          SET [CategoryID]			= @CategoryID
                                             ,[ProductName]		= @ProductName
                                             ,[ProductPrice]		= @ProductPrice
                                             
                                             ,[ProductDescription] = @ProductDescription
                                             
                                        WHERE ProductID=@ProductID";

                    DbParameter pCategoryID = cmd.CreateParameter();
                    pCategoryID.ParameterName = "@CategoryID";
                    pCategoryID.Value = product.CategoryID;
                    pCategoryID.DbType = System.Data.DbType.Int32;
                    cmd.Parameters.Add(pCategoryID);

                    DbParameter pProductID = cmd.CreateParameter();
                    pProductID.ParameterName = "@ProductID";
                    pProductID.Value = product.ProductID;
                    pProductID.DbType = System.Data.DbType.Int32;
                    cmd.Parameters.Add(pProductID);

                    DbParameter pProductName = cmd.CreateParameter();
                    pProductName.ParameterName = "@ProductName";
                    pProductName.Value = product.ProductName;
                    pProductName.DbType = System.Data.DbType.String;
                    cmd.Parameters.Add(pProductName);

                    DbParameter pProductPrice = cmd.CreateParameter();
                    pProductPrice.ParameterName = "@ProductPrice";
                    pProductPrice.Value = product.ProductPrice;
                    pProductPrice.DbType = System.Data.DbType.Decimal;
                    cmd.Parameters.Add(pProductPrice);

                    DbParameter pProductDescription = cmd.CreateParameter();
                    pProductDescription.ParameterName = "@ProductDescription";
                    pProductDescription.Value = product.ProductDescription;
                    pProductDescription.DbType = System.Data.DbType.String;
                    cmd.Parameters.Add(pProductDescription);
                    /*
                    DbParameter pProductImage = cmd.CreateParameter();
                    pProductImage.DbType = System.Data.DbType.Binary;
                    pProductImage.ParameterName = "@ProductImage";
                    pProductImage.Value = product.ProductImage;
                    cmd.Parameters.Add(pProductImage);
                    if (pProductImage.Value == null)
                    {
                        pProductImage.Value = DBNull.Value;
                    }*/
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public Product GetById(int id)
        {
            Product product = null;
            using (var conn = new SqlConnection(connectionString))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT ProductName, CategoryID, ProductPrice, ProductImage, ProductDescription FROM [dbo].[Product] WHERE ProductID=@ProductID";
                    cmd.Parameters.AddWithValue("ProductID", id);
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            product = new Product()
                            {
                                ProductID = id,
                                CategoryID = reader.GetInt32(reader.GetOrdinal("CategoryID")),
                                ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                ProductPrice = reader.GetDecimal(reader.GetOrdinal("ProductPrice")),
                                ProductDescription = reader.GetString(reader.GetOrdinal("ProductDescription")),
                                ProductImage = reader.IsDBNull(reader.GetOrdinal("ProductImage"))
                                ? null
                                : (byte[])reader["ProductImage"]
                            };
                        }
                    }
                }
            }
            return product;
        }
        public void Delete(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM [dbo].[Product] WHERE ProductID=@ProductID";
                    command.Parameters.AddWithValue("ProductID", id);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}