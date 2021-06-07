using GACDModels;
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

        public Category AddCategory(Category cat)
        {
            try
            {
                //Make sure category doesn't already exists
                Category category = (from c in _context.Categories
                                     where c.Name == cat.Name
                                     select c).Single();
                return null;
            }catch (Exception)
            {
                _context.Categories.Add(cat);
                _context.SaveChanges();
                return cat;
            }
        }

        public TypeTest AddTest(TypeTest typeTest)
        {
            try
            {
                _context.TypeTests.Add(typeTest);
                _context.SaveChanges();
                Log.Information("Test added");
                return typeTest;
            } catch(Exception)
            {
                Log.Error("Issue adding test");
                return null;
            }
        }

        public UserStat AddUpdateStats(int categoryid, int userid, UserStat userStat)
        {
            //Assuming these categories and users exist
            try 
            {
                int userStatId = (from uscj in _context.UserStatCatJoins
                                  where uscj.CategoryId == categoryid &&
                                  uscj.UserId == userid
                                  select uscj.UserStatId).Single();
                UserStat uStatInDB = (from uS in _context.UserStats
                                      where uS.Id == userStatId
                                      select uS).Single();
                uStatInDB.AverageWPM = userStat.AverageWPM;
                uStatInDB.AverageAccuracy = userStat.AverageAccuracy;
                uStatInDB.NumberOfTests = userStat.NumberOfTests;
                uStatInDB.TotalTestTime = userStat.TotalTestTime;
                _context.SaveChanges();
                return uStatInDB;
            } catch (Exception)
            {
                _context.UserStats.Add(userStat);
                _context.SaveChanges();
                //this might miss timing just call me if you have an issue
                UserStatCatJoin uscj = new UserStatCatJoin();
                uscj.CategoryId = categoryid;
                uscj.UserId = userid;
                uscj.UserStatId = userStat.Id;
                _context.UserStatCatJoins.Add(uscj);
                _context.SaveChanges();
            }
            return userStat;
        }

        public User AddUser(User user)
        {
            try
            {
                if (GetUser(user.UserName, user.Email) != null) return null;
                _context.Users.Add(user);
                _context.SaveChanges();
                return user;
            }
            catch(Exception e)
            {
                Log.Error(e.Message);
                return null;
            }
        }

        public List<Category> GetAllCategories()
        {
            try
            {
                return (from c in _context.Categories
                        select c).ToList();
            }
            catch(Exception e)
            {
                Log.Error(e.Message);
                return null;
            }
        }

        public List<User> GetAllUsers()
        {
            try {
                return (from u in _context.Users
                        select u).ToList();
            }
            catch(Exception e)
            {
                Log.Error(e.Message);
                return new List<User>();
            }
        }

        public UserStat GetSatUserCat(int categoryId, int userId)
        {
            try
            {
                int userStatId = (from uscj in _context.UserStatCatJoins
                                  where uscj.CategoryId == categoryId &&
                                  uscj.UserId == userId
                                  select uscj.UserStatId).Single();
                UserStat uStatInDB = (from uS in _context.UserStats
                                      where uS.Id == userStatId
                                      select uS).Single();
                return uStatInDB;
            } catch (Exception)
            {
                Log.Debug("Stat not found, returning null");
                return null;
            }
        }

        public User GetUser(int id)
        {
            try
            {
                return (from u in _context.Users
                        where u.Id == id
                        select u).Single();
            }
            catch (Exception)
            {
                Log.Error("User Not Found");
                return null;
            }
        }

        public User GetUser(string userName, string email)
        {
            try
            {
                return (from u in _context.Users
                        where u.UserName == userName &&
                        u.Email == email
                        select u).Single();
            }
            catch(Exception)
            {
                Log.Error("User not found");
                return null;
            }
        }

        public User UpdateUser(User user)
        {
            try
            {
                _context.Users.Update(user);
                _context.SaveChanges();
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
