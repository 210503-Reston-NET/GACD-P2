using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GACDModels;


namespace GACDBL
{
    public interface IUserStatBL
    {
        /// <summary>
        /// Method that Adds test and updates the user's stats
        /// </summary>
        /// <param name="userId">userId of test taker</param>
        /// <param name="categoryId">id of the category</param>
        /// <param name="typeTest">Test user has taken</param>
        /// <returns>user stat of test taker</returns>
        Task<UserStat> AddTestUpdateStat(int userId, int categoryId, TypeTest typeTest);

        /// <summary>
        /// Method that gets the user's stats and averages them out into overall stat
        /// </summary>
        /// <param name="userId">Id of user to find average stat for</param>
        /// <returns>User Stat with average stats</returns>
        Task<UserStat> GetAvgUserStat(int userId);

        /// <summary>
        /// Method that gets all the stats associated with a given user in order to return them through rest api
        /// </summary>
        /// <param name="userId">Id of User whose stats you are looking for</param>
        /// <returns>List of user stats associated with the user</returns>
        Task<List<UserStat>> GetUserStats(int userId);
        /// <summary>
        /// Leaderboard method that returns a list of Users with the best WPM
        /// </summary>
        /// <returns>List of users ranked by WPM and their WPM, acuraccies and ranking</returns>
        Task<List<Tuple<User, double, double, int>>> GetOverallBestUsers();
        /// <summary>
        /// Leaderboard method that returns list of Users with the best WPM in the given category 
        /// </summary>
        /// <param name="categoryId">Id of category that users participated in/param>
        /// <returns>List of Users and their WPM, acuraccies and ranking ordered by their performance in a category</returns>
        Task<List<Tuple<User, double, double, int>>> GetBestUsersForCategory(int categoryId);
    }
}
