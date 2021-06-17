using System;
using System.Collections.Generic;
using GACDModels;
using System.Threading.Tasks;

namespace GACDDL
{
    public interface IRepo
    {
        /// <summary>
        /// Get a user based on username and email
        /// </summary>
        /// <param name="userName">username of user</param>
        /// <param name="email">email of user</param>
        /// /// <returns>User with the given username and email</returns>
        Task<User> GetUser(string auth0id);
        /// <summary>
        /// Return a user based on id
        /// </summary>
        /// <param name="id">Id of requested yser</param>
        /// <returns>User Associated Id</returns>
        Task<User> GetUser(int id);
        /// <summary>
        /// Gets all users in the database
        /// </summary>
        /// <returns>List of Users</returns>
        Task<List<User>> GetAllUsers();
        /// <summary>
        /// Adds a user to the database
        /// </summary>
        /// <param name="user">user to add</param>
        /// <returns>user aded</returns>
        Task<User> AddUser(User user);
        /// <summary>
        /// Adds a category to the database
        /// </summary>
        /// <param name="cat">category to be added</param>
        /// <returns>category added to the database</returns>
        Task<Category> AddCategory(Category cat);
        /// <summary>
        /// Retrieves all categories currently in the database
        /// </summary>
        /// <returns>List of categories found</returns>
        Task<List<Category>> GetAllCategories();
        /// <summary>
        /// Gets a category by it's Octokit.Language int name
        /// </summary>
        /// <param name="name">name of category requested</param>
        /// <returns>Category requested</returns>
        Task<Category> GetCategoryByName(int name);
        /// <summary>
        /// Versatile method to update a user's stats for a given category
        /// </summary>
        /// <param name="categoryid">category user participated in</param>
        /// <param name="userid">user id related to user</param>
        /// <returns>userstat updated</returns>
        Task<UserStat> AddUpdateStats(int categoryid, int userid, UserStat userStat);
        /// <summary>
        /// Method that returns a user statistics for a given category, null if not found
        /// </summary>
        /// <param name="categoryId">category id for stat</param>
        /// <param name="userId">user id for stat</param>
        /// <returns>Userstat if found null otherwise</returns>
        Task<UserStat> GetSatUserCat(int categoryId, int userId);
        /// <summary>
        /// Method that adds a test to the database
        /// </summary>
        /// <param name="typeTest">TypeTest to add</param>
        /// <returns>test added</returns>
        Task<TypeTest> AddTest(TypeTest typeTest);
        /// <summary>
        /// Method that returns all stats for a given user
        /// </summary>
        /// <param name="userId">Id for user whose stats are being requested</param>
        /// <returns>List of stats if found, null otherwise</returns>
        Task<List<UserStatCatJoin>> GetUserStats(int userId);
        /// <summary>
        /// Creates a competition and then sends back the ID of the competition for
        /// front end processing
        /// </summary>
        /// <param name="comp">Competition to be added to the database</param>
        /// <returns>int of the competitionId, -1 if failed</returns>
        Task<int> AddCompetition(Competition comp);
        /// <summary>
        /// Gets a competition string based on competition id sent into the method
        /// </summary>
        /// <param name="compId">id of competition to participate in</param>
        /// <returns>string of competition</returns>
        Task<string> GetCompetitionString(int compId);
        /// <summary>
        /// Returns rankings of every User participating in the given competition
        /// </summary>
        /// <param name="compId">Id of competition to get stats from</param>
        /// <returns>List of revelant user scores from competition</returns>
        Task<List<CompetitionStat>> GetCompStats(int compId);
        /// <summary>
        /// Adds the competitionStat to the db, null on fail
        /// </summary>
        /// <param name="c">CompetitionStat to be added</param>
        /// <returns>null on fail, competitionstat on success</returns>
        Task<CompetitionStat> AddCompStat(CompetitionStat c);
        /// <summary>
        /// Updates the given competition stat in the database
        /// </summary>
        /// <param name="c">competition stat to be added</param>
        /// <returns>null on fail, competitionstat on success</returns>
        Task<CompetitionStat> UpdateCompStat(CompetitionStat c);
        /// <summary>
        /// Returns all competitions in db
        /// </summary>
        /// <returns>Returns all competitions in the database</returns>
        Task<List<Competition>> GetAllCompetitions();
        /// <summary>
        /// Gets a category by its id in the database
        /// </summary>
        /// <param name="id">category id to get category from </param>
        /// <returns>Category with id</returns>
        Task<Category> GetCategoryById(int id);
        /// <summary>
        /// Gets a userstat by the user stat id
        /// </summary>
        /// <param name="id">id of userstat to get</param>
        /// <returns>userstat if found null otherwise</returns>
        Task<UserStat> GetUserStatById(int id);
        /// <summary>
        /// Gets all the necessary things for a user to participate in a competition
        /// </summary>
        /// <param name="compId">competition Id for user to get stuff from</param>
        /// <returns>tuple with author, test, category of competition/null on fail</returns>
        Task<Tuple<string, string, int>> GetCompStuff(int compId);
        /// <summary>
        /// Gets a competition by its comp id
        /// </summary>
        /// <param name="id">Id of competition to get</param>
        /// <returns>Competition or null on fail</returns>
        Task<Competition> GetCompetition(int id);
        /// <summary>
        /// Gets the relevant type tests for a given user Id
        /// </summary>
        /// <param name="id">User Id to get tests for</param>
        /// <returns>List of type tests user has taken</returns>
        Task<List<TypeTest>> GetTypeTestsForUser(int userId);
        /// <summary>
        /// Method which allows users to place bet on a user in a competition 
        /// </summary>
        /// <param name="better">id of person betting</param>
        /// <param name="bettee">id of personing being bet on</param>
        /// <param name="compId">id of competition bettee is participating in</param>
        /// <paran name="betAmount">amount that better is betting on bettee</paran>
        /// <returns></returns>
        Task<Bet> PlaceBetOnCompUser(int better, int bettee, int compId, int betAmount);
        /// <summary>
        /// Clains all the bets for a given user
        /// </summary>
        /// <param name="userId">userId to claim bets from</param>
        /// <returns>Bets claimed empty if none found </returns>
        Task<List<Bet>> ClaimBets(int userId);
    }
}
