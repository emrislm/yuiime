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
    }
}
