using Biz1BookPOS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Biz1PosApi.Services
{
    public static class DbContextFactory
    {
        public static Dictionary<string, string> ConnStrings {  get; set; }
        public static void SetConnString(Dictionary<string, string> connStrings)
        {
            ConnStrings = connStrings;
        }
        public static TempDbContext Create(string connId)
        {
            if(!string.IsNullOrEmpty(connId))
            {
                string connString = ConnStrings[connId];
                var optionsBuilder = new DbContextOptionsBuilder<TempDbContext>();
                optionsBuilder.UseSqlServer(connString);
                return new TempDbContext(optionsBuilder.Options);
            }
            else
            {
                throw new ArgumentNullException("ConnectionId");
            }
        }
    }
}
