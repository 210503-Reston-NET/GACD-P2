﻿using GACDModels;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GACDDL
{
    public class Repo : IRepo
    {
        private GACDDBContext _context;
        public Repo(GACDDBContext context)
        {
            _context = context;
            Log.Debug("Repo instantiated");
        }

        public async Task<Category> AddCategory(Category cat)
        {
            try
            {
                //Make sure category doesn't already exists
                Category category = await (from c in _context.Categories
                                     where c.Name == cat.Name
                                     select c).SingleAsync();
                return null;
            }catch (Exception)
            {
               await _context.Categories.AddAsync(cat);
               await _context.SaveChangesAsync();
                return cat;
            }
        }

        public async Task<TypeTest> AddTest(TypeTest typeTest)
        {
            try
            {
                await _context.TypeTests.AddAsync(typeTest);
                await _context.SaveChangesAsync();
                Log.Information("Test added");
                return typeTest;
            } catch(Exception)
            {
                Log.Error("Issue adding test");
                return null;
            }
        }

        public async Task<UserStat> AddUpdateStats(int categoryid, int userid, UserStat userStat)
        {
            //Assuming these categories and users exist
            try 
            {
                int userStatId = await (from uscj in _context.UserStatCatJoins
                                  where uscj.CategoryId == categoryid &&
                                  uscj.UserId == userid
                                  select uscj.UserStatId).SingleAsync();
                UserStat uStatInDB = await (from uS in _context.UserStats
                                      where uS.Id == userStatId
                                      select uS).SingleAsync();
                uStatInDB.AverageWPM = userStat.AverageWPM;
                uStatInDB.AverageAccuracy = userStat.AverageAccuracy;
                uStatInDB.NumberOfTests = userStat.NumberOfTests;
                uStatInDB.TotalTestTime = userStat.TotalTestTime;
                await _context.SaveChangesAsync();
                return uStatInDB;
            } catch (Exception)
            {
                await _context.UserStats.AddAsync(userStat);
                await _context.SaveChangesAsync();
                //this might miss timing just call me if you have an issue
                UserStatCatJoin uscj = new UserStatCatJoin();
                uscj.CategoryId = categoryid;
                uscj.UserId = userid;
                uscj.UserStatId = userStat.Id;
                await _context.UserStatCatJoins.AddAsync(uscj);
                await _context.SaveChangesAsync();
            }
            return userStat;
        }

        public async Task<User> AddUser(User user)
        {
            try
            {
                if (await GetUser(user.UserName, user.Email) != null) return null;
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch(Exception e)
            {
                Log.Error(e.Message);
                return null;
            }
        }

        public async Task<List<Category>> GetAllCategories()
        {
            try
            {
                return await (from c in _context.Categories
                        select c).ToListAsync();
            }
            catch(Exception e)
            {
                Log.Error(e.Message);
                return null;
            }
        }

        public async Task<List<User>> GetAllUsers()
        {
            try {
                return await (from u in _context.Users
                        select u).ToListAsync();
            }
            catch(Exception e)
            {
                Log.Error(e.Message);
                return new List<User>();
            }
        }

        public async Task<UserStat> GetSatUserCat(int categoryId, int userId)
        {
            try
            {
                int userStatId = await (from uscj in _context.UserStatCatJoins
                                  where uscj.CategoryId == categoryId &&
                                  uscj.UserId == userId
                                  select uscj.UserStatId).SingleAsync();
                UserStat uStatInDB = await (from uS in _context.UserStats
                                      where uS.Id == userStatId
                                      select uS).SingleAsync();
                return uStatInDB;
            } catch (Exception)
            {
                Log.Debug("Stat not found, returning null");
                return null;
            }
        }

        public async  Task<User> GetUser(int id)
        {
            try
            {
                return await (from u in _context.Users
                        where u.Id == id
                        select u).SingleAsync();
            }
            catch (Exception)
            {
                Log.Error("User Not Found");
                return null;
            }
        }

        public async Task<User> GetUser(string userName, string email)
        {
            try
            {
                return await (from u in _context.Users
                        where u.UserName == userName &&
                        u.Email == email
                        select u).SingleAsync();
            }
            catch(Exception)
            {
                Log.Error("User not found");
                return null;
            }
        }

        public async Task<User> UpdateUser(User user)
        {
            try
            {
               _context.Users.Update(user);
               await _context.SaveChangesAsync();
                return user;
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return null;
            }
        }
    }
}
