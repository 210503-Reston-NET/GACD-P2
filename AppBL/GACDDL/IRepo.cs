﻿using System;
using System.Collections.Generic;
using GACDModels;

namespace GACDDL
{
    interface IRepo
    {
        /// <summary>
        /// Get a user based on username and email
        /// </summary>
        /// <param name="userName">username of user</param>
        /// <param name="email">email of user</param>
        /// <returns>User with the given username and email</returns>
        User GetUser(string userName, string email);
        /// <summary>
        /// Return a user based on id
        /// </summary>
        /// <param name="id">Id of requested yser</param>
        /// <returns>User Associated Id</returns>
        User GetUser(int id);
        /// <summary>
        /// Gets all users in the database
        /// </summary>
        /// <returns>List of Users</returns>
        List<User> GetAllUsers();
        /// <summary>
        /// Adds a user to the database
        /// </summary>
        /// <param name="user">user to add</param>
        /// <returns>user aded</returns>
        User AddUser(User user);
        /// <summary>
        /// Updates a give user in the database
        /// </summary>
        /// <param name="user">user to update</param>
        /// <returns>User updated</returns>
        User UpdateUser(User user);
        /// <summary>
        /// Adds a category to the database
        /// </summary>
        /// <param name="cat">category to be added</param>
        /// <returns>category added to the database</returns>
        Category AddCategory(Category cat);
        /// <summary>
        /// Retrieves all categories currently in the database
        /// </summary>
        /// <returns>List of categories found</returns>
        List<Category> GetAllCategories();
        /// <summary>
        /// Versatile method to update a user's stats for a given category
        /// </summary>
        /// <param name="categoryid">category user participated in</param>
        /// <param name="userid">user id related to user</param>
        /// <returns>userstat updated</returns>
        UserStat AddUpdateStats(int categoryid, int userid, UserStat userStat);
        /// <summary>
        /// Method that returns a user statistics for a given category, null if not found
        /// </summary>
        /// <param name="categoryId">category id for stat</param>
        /// <param name="userId">user id for stat</param>
        /// <returns>Userstat if found null otherwise</returns>
        UserStat GetSatUserCat(int categoryId, int userId);
        /// <summary>
        /// Method that adds a test to the database
        /// </summary>
        /// <param name="typeTest">TypeTest to add</param>
        /// <returns>test added</returns>
        TypeTest AddTest(TypeTest typeTest);
    }
}
