using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GACDModels;

namespace GACDBL
{
    public interface IUserBL
    {
        /// <summary>
        /// Method to get users from the database
        /// </summary>
        /// <returns>List of Users in the database</returns>
        List<User> GetUsers();
        /// <summary>
        /// Method that adds a user to the db if able
        /// </summary>
        /// <param name="u">User to be added to the db</param>
        /// <returns>user added, null otherwise</returns>
        User AddUser(User u);
        /// <summary>
        /// Get a user by his or her ID
        /// </summary>
        /// <param name="id">ID of requested user</param>
        /// <returns></returns>
        User GetUser(int id);
    }
}
