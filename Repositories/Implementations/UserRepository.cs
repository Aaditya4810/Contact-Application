using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContactApp.Models;
using ContactApp.Repositories.Interfaces;
using Npgsql;

namespace ContactApp.Repositories.Implementations
{
    public class UserRepository : IUserInterface
    {
        private readonly NpgsqlConnection _conn;

        public UserRepository(NpgsqlConnection connection)
        {
            _conn = connection;
        }

        #region For Registeration
        public async Task<int> Add(t_User data)
        {
            try
            {
                await _conn.CloseAsync();
                NpgsqlCommand comcheck = new NpgsqlCommand(@"SELECT * FROM t_user_contact WHERE c_email=@email;", _conn);
                comcheck.Parameters.AddWithValue("@email", data.c_Email);
                await _conn.OpenAsync();

                using (NpgsqlDataReader dr = await comcheck.ExecuteReaderAsync())
                {
                    if (dr.HasRows)
                    {
                        await _conn.CloseAsync();
                        return 0;
                    }
                    else
                    {
                        await _conn.CloseAsync();
                        NpgsqlCommand cm = new NpgsqlCommand(@"INSERT INTO t_user_contact (c_username,c_email,c_password,c_address,c_mobile,c_gender,c_image)
                                                       VALUES
                                                       (@username,@email,@password,@address,@mobile,@gender,@image);", _conn);

                        cm.Parameters.AddWithValue("@username", data.c_UserName);
                        cm.Parameters.AddWithValue("@email", data.c_Email);
                        cm.Parameters.AddWithValue("@password", data.c_Password);
                        cm.Parameters.AddWithValue("@address", data.c_Address);
                        cm.Parameters.AddWithValue("@mobile", data.c_Mobile);
                        cm.Parameters.AddWithValue("@gender", data.c_Gender);
                        cm.Parameters.AddWithValue("@image", data.c_Image);

                        await _conn.CloseAsync();
                        await _conn.OpenAsync();
                        await cm.ExecuteNonQueryAsync();
                        await _conn.CloseAsync();
                        return 1;
                    }
                }

            }
            catch (Exception ex)
            {
              
                Console.WriteLine(ex.Message);
                return -1;
            }
            finally
            {
                await _conn.CloseAsync();
            }
        }
        #endregion

        #region For Login

        public async Task<t_User> Login(vm_Login user)
        {
            t_User user1 = new t_User();
            var q1 = "SELECT * FROM t_user_contact WHERE c_email=@email AND c_password=@password;";

            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand(q1, _conn))
                {
                    cmd.Parameters.AddWithValue("@email", user.c_Email);
                    cmd.Parameters.AddWithValue("@password", user.c_Password);
                    await _conn.OpenAsync();
                    var reader = await cmd.ExecuteReaderAsync();

                    if (reader.Read())
                    {
                        user1.c_UserId = (int)reader["c_userid"];
                        user1.c_UserName = (string)reader["c_username"];
                        user1.c_Email = (string)reader["c_email"];
                        user1.c_Gender = (string)reader["c_gender"];
                        user1.c_Mobile = (string)reader["c_mobile"];
                        user1.c_Address = (string)reader["c_address"];
                        user1.c_Image = (string)reader["c_image"];
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Login Error: " + e.Message);
            }
            finally
            {
                await _conn.CloseAsync();
            }
            return user1;
        }


        public async Task<t_User> GetUser(int userid)
        {
            t_User userData = new t_User();
            var q1 = "SELECT * FROM t_user_contact WHERE c_userid=@userid;";

            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand(q1, _conn))
                {
                    cmd.Parameters.AddWithValue("@userid", userid);
                    await _conn.OpenAsync();
                    var reader = await cmd.ExecuteReaderAsync();
                    if (reader.Read())
                    {
                        userData.c_UserId = (int)reader["c_userid"];
                        userData.c_UserName = (string)reader["c_username"];
                        userData.c_Email = (string)reader["c_email"];
                        userData.c_Gender = (string)reader["c_gender"];
                        userData.c_Mobile = (string)reader["c_mobile"];
                        userData.c_Address = (string)reader["c_address"];
                        userData.c_Image = (string)reader["c_image"];
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("getting user Error: " + e.Message);
            }
            finally
            {
                await _conn.CloseAsync();
            }
            return userData;
        }


        #endregion
    }
}