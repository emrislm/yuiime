using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using yuiime.Models;

namespace yuiime.Repo
{
    public class FirebaseRepo : IUserRepo<Users>
    {
        private const string BaseUrl = "https://yuiimedb-default-rtdb.europe-west1.firebasedatabase.app/";
        private static ChildQuery _query;

        public FirebaseRepo()
        {
            string path = "users";
            _query = new FirebaseClient(BaseUrl).Child(path);
        }

        public async Task<bool> AddUserAsync(Users user)
        {
            try
            {
                var addedUser = await _query.PostAsync(user);
                user.Id = addedUser.Key;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }

            return true;
        }

        public async Task<bool> DeleteUserAsync(string username)
        {
            try
            {
                await _query.Child(username).DeleteAsync();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public async Task<Users> GetUserAsync(string username)
        {
            try
            {
                var allUsers = await GetUsersAsync();
                await _query.Child("users").OnceAsync<Users>();

                return allUsers.Where(a => a.Username == username).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }

        public async Task<IEnumerable<Users>> GetUsersAsync(bool forceRefresh = false)
        {
            try
            {
                var firebaseObjects = await _query.OnceAsync<Users>();

                return firebaseObjects.Select(x => new Users
                {
                    Id = x.Key,
                    Username = x.Object.Username,
                    Password = x.Object.Password
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }

        public async Task<bool> UpdateUserAsync(Users user)
        {
            try
            {
                Users copy = new Users()
                {
                    Username = user.Username,
                    Password = user.Password
                };

                await _query.Child(user.Username).PutAsync(copy);
            }
            catch (Exception)
            {

                return false;
            }

            return true;
        }


        ////Read All    
        //public static async Task<List<Users>> GetAllUser()
        //{
        //    try
        //    {
        //        var userlist = (await firebase
        //        .Child("Users")
        //        .OnceAsync<Users>()).Select(item =>
        //        new Users
        //        {
        //            Username = item.Object.Username,
        //            Password = item.Object.Password
        //        }).ToList();
        //        return userlist;
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.WriteLine($"Error:{e}");
        //        return null;
        //    }
        //}

        ////Read     
        //public static async Task<Users> GetUser(string username)
        //{
        //    try
        //    {
        //        var allUsers = await GetAllUser();
        //        await firebase
        //        .Child("Users")
        //        .OnceAsync<Users>();
        //        return allUsers.Where(a => a.Username == username).FirstOrDefault();
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.WriteLine($"Error:{e}");
        //        return null;
        //    }
        //}

        ////Inser a user    
        //public static async Task<bool> AddUser(string username, string password)
        //{
        //    try
        //    {
        //        await firebase
        //        .Child("Users")
        //        .PostAsync(new Users() { Username = username, Password = password });
        //        return true;
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.WriteLine($"Error:{e}");
        //        return false;
        //    }
        //}

        ////Update     
        //public static async Task<bool> UpdateUser(string username, string password)
        //{
        //    try
        //    {
        //        var toUpdateUser = (await firebase
        //        .Child("Users")
        //        .OnceAsync<Users>()).Where(a => a.Object.Username == username).FirstOrDefault();
        //        await firebase
        //        .Child("Users")
        //        .Child(toUpdateUser.Key)
        //        .PutAsync(new Users() { Username = username, Password = password });
        //        return true;
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.WriteLine($"Error:{e}");
        //        return false;
        //    }
        //}

        ////Delete User    
        //public static async Task<bool> DeleteUser(string username)
        //{
        //    try
        //    {
        //        var toDeletePerson = (await firebase
        //        .Child("Users")
        //        .OnceAsync<Users>()).Where(a => a.Object.Username == username).FirstOrDefault();
        //        await firebase.Child("Users").Child(toDeletePerson.Key).DeleteAsync();
        //        return true;
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.WriteLine($"Error:{e}");
        //        return false;
        //    }
        //}
    }
}
