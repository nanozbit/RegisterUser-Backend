using LoginApi.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Threading.Tasks;

namespace LoginApi.Infraestructure
{
    public class RepositoryUser : IRepositoryUser
    {
        private string _connectionString;
        public RepositoryUser(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("AccessDb");
        }

        public async Task<Response> CreateUser(LoginUserEntiy userRequest)
        {
            Response rp = new Response();
            using (OleDbConnection connection = new OleDbConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    List<string> user = new List<string>();
                    string queryString = $"INSERT INTO TblUser (UserName, Passwords, FirstName, LastName, EmailAddress, Address, BirdDate)" +
                        $" VALUES('{userRequest.UserName}'," +
                        $"'{userRequest.Password}'," +
                        $"'{userRequest.FirstName}'," +
                        $"'{userRequest.LastName}'," +
                        $"'{userRequest.EmailAddress}'," +
                        $"'{userRequest.Address}'," +
                        $"'{userRequest.BirdDate.ToString()}');";
           
                    OleDbCommand cmd = new OleDbCommand(queryString, connection);
                    int result = await cmd.ExecuteNonQueryAsync();
                    if (result == 1)
                    {
                        rp.Status = "Ok";
                        rp.Message = "Create New User";
                    }
                    else
                    {
                        rp.Status = "Bad";
                        rp.Message = "User Is Not Create";
                    }
                    await connection.CloseAsync();
                }
                catch (Exception ex)
                {
                    rp.Message = ex.Message;
                    rp.Status = "Something Went Wrong";
                    await connection.CloseAsync();
                }
            }
            return rp;
        }

        public async Task<Response> DeleteUser(int Id)
        {
            Response rp = new Response();
            using (OleDbConnection connection = new OleDbConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    List<string> user = new List<string>();
                    string queryString = $"DELETE FROM TblUser WHERE ID={Id};";

                    OleDbCommand cmd = new OleDbCommand(queryString, connection);
                    int result = await cmd.ExecuteNonQueryAsync();
                    if (result == 1)
                    {
                        rp.Status = "Ok";
                        rp.Message = "Delete User";
                    }
                    else
                    {
                        rp.Status = "Bad";
                        rp.Message = "Delete Is Not Update";
                    }
                    await connection.CloseAsync();
                }
                catch (Exception ex)
                {
                    rp.Message = ex.Message;
                    rp.Status = "Something Went Wrong";
                    await connection.CloseAsync();
                }
            }
            return rp;
        }

        public  async Task<LoginUserEntiy> GetUser(int Id)
        {
            LoginUserEntiy  rp = new LoginUserEntiy();
            using (OleDbConnection connection = new OleDbConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();;
                    string queryString = $"SELECT * FROM TblUser WHERE ID={Id}";
                    OleDbCommand cmd = new OleDbCommand(queryString, connection);
                    OleDbDataReader reader = cmd.ExecuteReader();

                    while (await reader.ReadAsync() && reader.HasRows)
                    {
                        rp = new LoginUserEntiy()
                        {
                            Id = Int32.Parse(reader["ID"].ToString()),
                            UserName = reader["UserName"].ToString(),
                            Password = reader["Passwords"].ToString(),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            EmailAddress = reader["EmailAddress"].ToString(),
                            Address = reader["Address"].ToString(),
                            BirdDate = DateTime.Parse(reader["BirdDate"].ToString())
                        };
                        
                    }

                    if (rp.Id > 0)
                    {
                        rp.Status = "Ok";
                        rp.Message = "Search successful";
                    }
                    else
                    {
                        rp.Status = "Bad";
                        rp.Message = "Search is not found";
                    }
                    await connection.CloseAsync();
                }
                catch (Exception ex)
                {
                    rp.Message = ex.Message;
                    rp.Status = "Something went wrong";
                    await connection.CloseAsync();
                }
            }
            return rp;
        }

        public async Task<ListUsers> GetUsers()
        {
            ListUsers rp = new ListUsers();
            using (OleDbConnection connection = new OleDbConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    rp.ListUser = new List<LoginUserEntiy>();
                    string queryString = $"SELECT * FROM TblUser;";
                    OleDbCommand cmd = new OleDbCommand(queryString, connection);
                    OleDbDataReader reader = cmd.ExecuteReader();

                    while (await reader.ReadAsync() && reader.HasRows)
                    {
                        LoginUserEntiy userEntity = new LoginUserEntiy()
                        {
                            Id = Int32.Parse(reader["Id"].ToString()),
                            UserName = reader["UserName"].ToString(),
                            Password = reader["Passwords"].ToString(),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            EmailAddress = reader["EmailAddress"].ToString(),
                            Address = reader["Address"].ToString(),
                            BirdDate = DateTime.Parse(reader["BirdDate"].ToString())
                        };
                        
                        rp.ListUser.Add(userEntity);
                    }

                    if (rp.ListUser.Count > 0)
                    {
                        rp.Status = "Ok";
                        rp.Message = "Search successful";
                    }
                    else
                    {
                        rp.Status = "Bad";
                        rp.Message = "Search is not found";
                    }
                    await connection.CloseAsync();
                }
                catch (Exception ex)
                {
                    rp.Message = ex.Message;
                    rp.Status = "Something went wrong";
                    await connection.CloseAsync();
                }
            }
            return rp;
        }

        public async Task<Response> LoginValidation(LoginUserEntiy userRequest)
        {
            Response rp = new Response();
            using (OleDbConnection connection = new OleDbConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    List<string> user = new List<string>();
                    string queryString = $"SELECT * FROM TblUser WHERE UserName='{userRequest.UserName}' AND Passwords='{userRequest.Password}';";
                    OleDbCommand cmd = new OleDbCommand(queryString,connection);
                    OleDbDataReader reader = cmd.ExecuteReader();

                    while(await reader.ReadAsync() && reader.HasRows)
                    {
                       user.Add(reader["ID"].ToString());
                    }

                    if (user.Count == 1)
                    {
                        rp.Status = "Ok";
                        rp.Message = "Login successful";
                    }
                    else if (user.Count >= 1)
                    {
                        rp.Status = "Bad";
                        rp.Message = "User Repeat";
                    }
                    else if (user.Count == 0)
                    {
                        rp.Status = "Bad";
                        rp.Message = "User not found";
                    }
                    await connection.CloseAsync();
                } catch(Exception ex)
                {
                    rp.Message  = ex.Message;
                    rp.Status = "Something went wrong";
                    await connection.CloseAsync();
                }
            }
            return rp;
        }

        public async Task<Response> Updateuser(LoginUserEntiy userRequest)
        {
            Response rp = new Response();
            using (OleDbConnection connection = new OleDbConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    List<string> user = new List<string>();
                    string queryString = $"Update TblUser " +
                        $"SET UserName='{userRequest.UserName}', " +
                        $"Passwords ='{userRequest.Password}'," +
                        $" FirstName='{userRequest.FirstName}'," +
                        $" LastName='{userRequest.LastName}'," +
                        $" EmailAddress='{userRequest.EmailAddress}'," +
                        $" Address='{userRequest.Address}'," +
                        $" BirdDate='{userRequest.BirdDate.ToString()}'" +
                        $"WHERE ID={userRequest.Id};";

                    OleDbCommand cmd = new OleDbCommand(queryString, connection);
                    int result = await cmd.ExecuteNonQueryAsync();
                    if (result == 1)
                    {
                        rp.Status = "Ok";
                        rp.Message = "Update User";
                    }
                    else
                    {
                        rp.Status = "Bad";
                        rp.Message = "User Is Not Update";
                    }
                    await connection.CloseAsync();
                }
                catch (Exception ex)
                {
                    rp.Message = ex.Message;
                    rp.Status = "Something Went Wrong";
                    await connection.CloseAsync();
                }
            }
            return rp;
        }
    }
}
