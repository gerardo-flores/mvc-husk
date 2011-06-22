using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.IO;
using MvcMiniProfiler;
using MvcMiniProfiler.Data;
using System.Data.Objects;

namespace MVC_Husk.Models
{
    public class SeasonalProduct
    {
        public int id { get; set; }
        public string Week { get; set; }
        public string Category { get; set; }
        public double IDX { get; set; }

        public SeasonalProduct(string[] entities)
        {
            Week = entities[0];
            Category = entities[1];
            IDX = Convert.ToDouble(entities[2]);
        }

        public SeasonalProduct()
        {
        }

        public static void LoadFileData(string path)
        {
            SeasonalProductDBContext db = new SeasonalProductDBContext();

            string line;
            using (StreamReader sFile = new StreamReader(path.ToString()))
            {
                sFile.ReadLine();
                while ((line = sFile.ReadLine()) != null)
                {
                    char[] delimiters = new char[] { '\t' };
                    string[] parts = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    db.SeasonalProducts.Add(new SeasonalProduct(parts));
                }
                db.SaveChanges();
                sFile.Close();
            }

        }
    }


    public class SeasonalProductDBContext : DbContext
    {
        public DbSet<SeasonalProduct> SeasonalProducts { get; set; }
    }
}