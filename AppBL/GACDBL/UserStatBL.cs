using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GACDDL;
using GACDModels;
using Serilog;

namespace GACDBL
{
    public class UserStatBL : IUserStatBL
    {
        private Repo _repo;
        public UserStatBL(GACDDBContext context)
        {
            _repo = new Repo(context);
        }

        public async Task<UserStat> AddTestUpdateStat(int userId, int categoryId, TypeTest typeTest)
        {
           UserStat userStat;
            try
            {
                if(await _repo.GetSatUserCat(userId, categoryId) != null) userStat = await _repo.GetSatUserCat(userId, categoryId);
                else
                {
                    userStat = new UserStat();
                    userStat.AverageAccuracy = 0;
                    userStat.AverageWPM = 0;
                    userStat.NumberOfTests = 0;
                    userStat.TotalTestTime = 0;
                }
                
                userStat.TotalTestTime += typeTest.TimeTaken;
                userStat.AverageAccuracy = ((userStat.AverageAccuracy * userStat.NumberOfTests) + ((typeTest.NumberOfWords - typeTest.NumberOfErrors) / typeTest.NumberOfWords)) / (userStat.NumberOfTests + 1);
                userStat.AverageWPM = ((userStat.AverageWPM * userStat.NumberOfTests) + typeTest.WPM) / (userStat.NumberOfTests + 1);
                userStat.NumberOfTests += 1;
                userStat = await _repo.AddUpdateStats(userId, categoryId, userStat);
                typeTest.UserStatId = userStat.Id;
                await _repo.AddTest(typeTest);
                return userStat;


            }catch (Exception e)
            {
                Log.Error(e.Message);
                Log.Error("Error occured in AddTest BL");
                return null;
            }
        }

        public async Task<UserStat> GetAvgUserStat(int userId)
        {
            List <UserStat> userStats = await _repo.GetUserStats(userId);
            if (userStats.Count == 0) return new UserStat();
            else
            {
                UserStat userStat = new UserStat();
                userStat.TotalTestTime = 0;
                userStat.NumberOfTests = 0;
                userStat.AverageAccuracy = 0;
                userStat.AverageWPM = 0;
                foreach (UserStat u in userStats) userStat.TotalTestTime += u.TotalTestTime;
                foreach (UserStat us in userStats)
                {
                    userStat.NumberOfTests += us.NumberOfTests;
                    userStat.AverageAccuracy += (us.AverageAccuracy * us.TotalTestTime) / userStat.TotalTestTime;
                    userStat.AverageWPM += (us.AverageWPM * us.TotalTestTime) / us.TotalTestTime;
                }
                return userStat;
            }
            
        }

        public async Task<List<User>> GetBestUsersForCategory(int categoryId)
        {
            try
            {
                List<User> users = await _repo.GetAllUsers();
                List<Tuple<UserStat, User>> userStats = new List<Tuple<UserStat, User>>();

                foreach (User u in users)
                {
                    if (await _repo.GetSatUserCat(categoryId, u.Id) != null)
                    {
                        Tuple<UserStat, User> statTuple = Tuple.Create(await _repo.GetSatUserCat(categoryId, u.Id), u);

                        if (statTuple.Item1.AverageWPM != 0) userStats.Add(statTuple);
                    }
                }
                List<User> returnUsers = (from tuple in userStats
                                          orderby tuple.Item1.AverageWPM descending
                                          select tuple.Item2).ToList();
                return returnUsers;
            }
            catch (Exception e)
            {
                Log.Error(e.Message + "Issue getting overallbestusers, returning empty list");
                return new List<User>();
            }
        }

        public async Task<List<User>> GetOverallBestUsers()
        {
            try {
                List<User> users = await _repo.GetAllUsers();
                List<Tuple<UserStat,User>> userStats = new List<Tuple<UserStat, User>>();

                foreach(User u in users)
                {
                    Tuple<UserStat, User> statTuple = Tuple.Create(await GetAvgUserStat(u.Id), u);

                    if (statTuple.Item1.AverageWPM != 0) userStats.Add(statTuple);
                }
                List<User> returnUsers = (from tuple in userStats
                                          orderby tuple.Item1.AverageWPM descending
                                          select tuple.Item2).ToList();
                return returnUsers;
            }
            catch (Exception e)
            {
                Log.Error(e.Message + "Issue getting overallbestusers, returning empty list");
                return new List<User>();
            }
        }

        public async Task<List<UserStat>> GetUserStats(int userId)
        {
            return await _repo.GetUserStats(userId);
        }

        public async Task<TypeTest> SaveTypeTest(UserStat userStat, int errors, int charactersTyped, int timeTaken, DateTime date)
        {
            TypeTest test = new TypeTest();
                test.UserStat = userStat;
            test.UserStatId = userStat.Id;
            test.NumberOfErrors = errors;
            test.NumberOfWords = (charactersTyped/5);
            test.TimeTaken = timeTaken/60;
            test.Date = date;
            test.WPM = ((test.NumberOfWords - test.NumberOfErrors) / (test.TimeTaken/60));
            return test;

               

        }
    }
}
