//
//  Copyright Info
//

using System;
using System.Dynamic;
using System.IO;
using MVC_Husk.Data;

namespace MVC_Husk.Model
{
    public class SeasonalProducts : DynamicModel
    {
        public SeasonalProducts()
            : base("Template")
        {
            PrimaryKeyField = "ID";
        }

        /// <summary>
        /// Adds a Seasonal Product Index
        /// </summary>
        /// <param name="week">The Week for the Index</param>
        /// <param name="category"></param>
        /// <param name="seasonalityIndex"></param>
        /// <returns></returns>
        public dynamic CreateSeasonalProduct(string week, string category, string seasonalityIndex)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;

            if (ValidateWeek(week) && !string.IsNullOrEmpty(category) && ValidateIndex(seasonalityIndex))
            {
                try
                {
                    result.SeasonalProductId = this.Insert(new { Week = Convert.ToDateTime(week), Category = category, SeasonalityIndex = Convert.ToDouble(seasonalityIndex) });
                    result.Success = true;
                    result.Message = "Seasonal Product added";
                }
                catch (Exception ex)
                {
                    result.Message = "An entry for " + category + " for the week of " + week + " already exists";  
                }
            }
            else
            {
                result.Message = "The Week, Category or Seasonal Index was invalid";
            }


            return result;
        }

        /// <summary>
        /// Adds a Seasonal Product Index
        /// </summary>
        /// <param name="week">The Week for the Index</param>
        /// <param name="category"></param>
        /// <param name="seasonalIndex"></param>
        /// <returns></returns>
        public dynamic UpdateSeasonalProduct(Int64 id, string week, string category, string seasonalIndex)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;

            if (ValidateWeek(week) && !string.IsNullOrEmpty(category) && ValidateIndex(seasonalIndex))
            {
                try
                {
                    result.SeasonalProductId = this.Update(new { Week = Convert.ToDateTime(week), Category = category, SeasonalityIndex = Convert.ToDouble(seasonalIndex) }, id);
                    result.Success = true;
                    result.Message = "Seasonal Product Updated";
                }
                catch (Exception ex)
                {
                    result.Message = "An entry for " + category + " for the week of " + week + " already exists";
                }
            }
            else
            {
                result.Message = "The Week, Category or Seasonal Index was invalid";
            }


            return result;
        }


        /// <summary>
        /// Load File Data into the database
        /// </summary>
        /// <param name="path">Path of the File</param>
        public void LoadFileData(string path)
        {
            string line;
            using (StreamReader sFile = new StreamReader(path.ToString()))
            {
                sFile.ReadLine();
                while ((line = sFile.ReadLine()) != null)
                {
                    char[] delimiters = new char[] { '\t' };
                    string[] parts = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                    this.Insert(new { Week = Convert.ToDateTime(parts[0]), Category = parts[1], SeasonalityIndex = Convert.ToDouble(parts[2]) });
                }

                sFile.Close();
            }
        }

        /// <summary>
        /// Simple validation for Weekly data
        /// </summary>
        /// <param name="week">The Week of a Seasonal Index</param>
        /// <returns>Bool indicating if it is valid or not</returns>
        private bool ValidateWeek(string week)
        {
            // If the string isn't a real date, it fails validation
            DateTime temp;
            if (!DateTime.TryParse(week, out temp))
                return false;

            // If week is so far in the past that it's the Min Date, then it's wrong
            if (DateTime.MinValue == temp)
                return false;

            // If Week is in the future, it's wrong.
            if (DateTime.Today < temp)
                return false;

            // Otherwise return true
            return true;
        }

        /// <summary>
        /// Simple validation for Seasonality Index
        /// </summary>
        /// <param name="week">The Week of a Seasonal Index</param>
        /// <returns>Bool indicating if it is valid or not</returns>
        private bool ValidateIndex(string seasonalityIndex)
        {
            if (string.IsNullOrEmpty(seasonalityIndex))
                return false;

            // If the string isn't a real date, it fails validation
            Double temp;
            if (!Double.TryParse(seasonalityIndex, out temp))
                return false;

            // Otherwise return true
            return true;
        }
    }
}