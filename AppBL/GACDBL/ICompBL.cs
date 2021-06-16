﻿using GACDModels;
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
        /// Adds a competition to the database given the required fields
        /// </summary>
        /// <param name="startDate">Date the Comp started</param>
        /// <param name="endDate">Date the Comp ended</param>
        /// <param name="categoryId">Category for competiton</param>
        /// <param name="competitionName">name for the competition</param>
        /// <param name="user">authId for user starting the competition</param>
        /// <param name="teststring">test string for the competition to be added</param>
        /// <param name="author">test string for the competition to be added</param>
        /// <returns>Tuple with int for comp id and string of code to be competed on</returns>
        Task<int> AddCompetition(DateTime startDate, DateTime endDate, int categoryId, string competitionName, int user, string teststring, string author);
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
        /// <summary>
        /// Method that returns the string for the given competition
        /// </summary>
        /// <param name="compId">competition id to get</param>
        /// <returns>string to be competed upon</returns>
        Task<string> GetCompString(int compId);
        /// <summary>
        /// Method that returns all the competitions in the database
        /// </summary>
        /// <returns>List of Competitions in database</returns>
        Task<List<Competition>> GetAllCompetitions();
        /// <summary>
        /// Gets all the necessary things for a user to participate in a competition
        /// </summary>
        /// <param name="compId">competition id to get things from</param>
        /// <returns>tuple with author, test, category of competition/null on fail</returns>
        Task<Tuple<string, string, int>> GetCompStuff(int compId);
        /// <summary>
        /// Gets a competition by id, null if not found
        /// </summary>
        /// <param name="compId">id of competition to be found</param>
        /// <returns>Competition or null if not found</returns>
        Task<Competition> GetCompetition(int compId);
        /// <summary>
        /// Places a bet, returns null on error
        /// </summary>
        /// <param name="better">string of user authId</param>
        /// <param name="bettee">number id of better</param>
        /// <param name="compId">number id of competition</param>
        /// <param name="betAmount"></param>
        /// <returns></returns>
        Task<Bet> PlaceBet(string better, int bettee, int compId, int betAmount);
    }
}
