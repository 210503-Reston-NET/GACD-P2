using GACDModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GACDBL
{
    public interface ICompBL
    {
        /// <summary>
        /// Adds a competition to the database given the required fields
        /// </summary>
        /// <param name="startDate">Date the Comp started</param>
        /// <param name="endDate">Date the Comp ended</param>
        /// <param name="categoryId">Category for competiton</param>
        /// <param name="competitionName">name for the competition</param>
        /// <param name="user">authId for user starting the competition</param>
        /// <param name="teststring">test string for the competition to be added</param>
        /// <returns>Tuple with int for comp id and string of code to be competed on</returns>
        Task<int> AddCompetition(DateTime startDate, DateTime endDate, int categoryId, string competitionName, int user, string teststring);
        /// <summary>
        /// Method which returns the users that participated in a given competition
        /// </summary>
        /// <param name="competitionId"></param>
        /// <returns></returns>
        Task <List<CompetitionStat>> GetCompetitionStats (int competitionId);
        /// <summary>
        /// Adds a competition to the database and updates the rankings
        /// </summary>
        /// <param name="competitionStat">Competition stat to be added</param>
        /// <param name="numberWords">number of words in test</param>
        /// <param name="numberErrors">number of errors in test</param>
        /// <returns>rank in the competition, -1 on error</returns>
        Task<int> InsertCompStatUpdate(CompetitionStat competitionStat, int numberWords, int numberErrors);
    }
}
