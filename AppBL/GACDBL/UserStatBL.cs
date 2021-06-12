﻿using System;
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
                userStat = await _repo.AddUpdateStats(categoryId, userId, userStat);
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

        public async Task<List<Tuple<User, double, double, int>>> GetBestUsersForCategory(int categoryId)
        {
            try
            {
                List<User> users = await _repo.GetAllUsers();
                List<Tuple<User, double, double>> userStats = new List<Tuple<User, double, double>>();

                foreach (User u in users)
                {
                    if (await _repo.GetSatUserCat(categoryId, u.Id) != null)
                    {
                        UserStat userStat = await _repo.GetSatUserCat(categoryId, u.Id);
                        Tuple<User, double, double> statTuple = Tuple.Create(u, userStat.AverageWPM, userStat.AverageAccuracy);

                        if (statTuple.Item2 != 0) userStats.Add(statTuple);
                    }
                }
                List<Tuple<User, double, double>> returnUsers = (from tuple in userStats
                                                                orderby tuple.Item2 descending
                                                                select tuple).ToList();
                List<Tuple<User, double, double, int>> usersRanked = new List<Tuple<User, double, double, int>>();
                int i = 0;
                foreach (Tuple<User, double, double> t in returnUsers)
                {
                    i += 1;
                    Tuple<User, double, double, int> tuple = Tuple.Create(t.Item1, t.Item2, t.Item3, i);
                    usersRanked.Add(tuple);
                }
                return usersRanked;
            }
            catch (Exception e)
            {
                Log.Error(e.Message + "Issue getting overallbestusers, returning empty list");
                return new List<Tuple<User, double, double, int>>();
            }
        }

        public async Task<List<Tuple<User, double, double, int>>> GetOverallBestUsers()
        {
            try {
                List<User> users = await _repo.GetAllUsers();
                List<Tuple<User, double, double>> userStats = new List<Tuple<User, double, double>>();
                UserStat userStat;

                foreach(User u in users)
                {
                    userStat = await GetAvgUserStat(u.Id);
                    Tuple<User, double, double> statTuple = Tuple.Create(u, userStat.AverageWPM, userStat.AverageAccuracy);

                    if (statTuple.Item2 != 0) userStats.Add(statTuple);
                }
                List<Tuple<User, double, double>> returnUsers = (from tuple in userStats
                                          orderby tuple.Item2 descending
                                          select tuple).ToList();
                int i = 0;
                List<Tuple<User, double, double, int>> usersRanked = new List<Tuple<User, double, double, int>>();
                foreach (Tuple<User, double, double> t in returnUsers)
                {
                    i += 1;
                    Tuple<User, double, double, int> tuple = Tuple.Create(t.Item1, t.Item2, t.Item3, i);
                    usersRanked.Add(tuple);
                    Log.Information(tuple.Item1.Auth0Id);
                    Console.WriteLine(tuple.Item1.Auth0Id);
                }
                return usersRanked;
            }
            catch (Exception e)
            {
                Log.Error(e.Message + "Issue getting overallbestusers, returning empty list");
                return new List<Tuple<User, double, double, int>>();
            }
        }

        public async Task<List<UserStat>> GetUserStats(int userId)
        {
            return await _repo.GetUserStats(userId);
        }

        public async Task<TypeTest> SaveTypeTest(int errors, int charactersTyped, int timeTaken, int wpm, DateTime date)
        {
            TypeTest test = new TypeTest();
            test.NumberOfErrors = errors;
            test.NumberOfWords = (charactersTyped/5);
            test.TimeTaken = timeTaken;
            test.Date = date;
            //double time = timeTaken / 60000;
            test.WPM = wpm;
            return test;

               

        }
    }
}
