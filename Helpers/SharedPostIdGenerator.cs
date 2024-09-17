using Microsoft.EntityFrameworkCore;
using System;
using api_screenvault.Data;

namespace api_screenvault.Helpers
{
    public interface ISharedPostIdGenerator { 
    public string GenerateUniqueLinkId();
    }
    public class SharedPostIdGenerator: ISharedPostIdGenerator
    {
        const string charSet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private readonly ApplicationDbContext _dbContext;
        public SharedPostIdGenerator(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public string GenerateUniqueLinkId()
        {
            string newId;
            do
            {
                newId = GenerateRandomId();
            }
            while (_dbContext.Posts.Any(post => post.LinkId == newId)); // Ensure the ID is not a duplicate


            return newId;
        }

        private static string GenerateRandomId()
        {
            Random random = new Random();
            var length = 6;
            char[] id = new char[length];
            for (int i = 0; i < length; i++)
            {
                id[i] = charSet[random.Next(charSet.Length)];
            }
            return new string(id);
        }
    }
}
