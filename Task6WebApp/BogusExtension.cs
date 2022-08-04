using Bogus;
using Microsoft.Extensions.Hosting.Internal;
using System.Runtime.CompilerServices;

namespace Task6WebApp
{
	public static class BogusExtension
	{
		private static List<string> MiddleNames;

        public static string FullNameWithMiddle(this Bogus.Faker f, Bogus.DataSets.Name.Gender gender, string WebRootPath)
		{
			switch (f.Locale)
			{
				case "ru":
					return FullRuName(f, gender, WebRootPath);
				case "en_US":
					return FullUSName(f, gender);
				case "es":
					return FullEsName(f, gender);
				default:
					return "Error: Unknown locale";
			}
		}

		public static string AddressWithoutCountry(this Bogus.Faker f)
        {
			string value = f.Address.StreetAddress();
			string value2 = f.Address.City();
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 3);
			defaultInterpolatedStringHandler.AppendFormatted(value);
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted(value2);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		public static string StringWithLocale(this Bogus.Faker f)
        {
			switch (f.Locale)
            {
				case "ru":
					string ruAlphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя1234567890";
					return Convert.ToString(ruAlphabet[f.Random.Number(ruAlphabet.Length-1)]);
				case "en_US":
					string enAlphabet = "abcdefghijklmnopqrstuvwxyz1234567890";
					return Convert.ToString(enAlphabet[f.Random.Number(enAlphabet.Length-1)]);
				case "es":
					string esAlphabet = "abcdefghijklmnñopqrstuvwxyz1234567890";
					return Convert.ToString(esAlphabet[f.Random.Number(esAlphabet.Length-1)]);
				default:
					return "Error: Unknown locale";
			}
        }

        private static string FullRuName(this Bogus.Faker f, Bogus.DataSets.Name.Gender gender, string WebRootPath)
        {
            if (gender == Bogus.DataSets.Name.Gender.Male)
            {
                GetMiddleManNames("Man", WebRootPath);
            }
            else
            {
                GetMiddleManNames("Woman", WebRootPath);
            }
            return f.Name.LastName(gender) + " " + f.Name.FirstName(gender) + " " + f.PickRandom(MiddleNames);
        }

        private static string FullUSName(this Bogus.Faker f, Bogus.DataSets.Name.Gender gender)
        {
			if(gender == Bogus.DataSets.Name.Gender.Male)
            {
				return f.PickRandom(new[] {"Mr.", "Dr." }) + " " + f.Name.FirstName(gender) + " " + f.Name.LastName(gender);
            }
			return f.PickRandom(new[] { "Miss", "Ms.", "Mrs." }) + " " + f.Name.FirstName(gender) + " " + f.Name.LastName(gender);
		}

		private static string FullEsName(this Bogus.Faker f, Bogus.DataSets.Name.Gender gender)
		{
			return f.Name.FirstName(gender) + " " + f.Name.LastName(gender) + " " + f.Name.LastName(gender);
		}

        private static void GetMiddleManNames(string gender, string WebRootPath)
        {
            MiddleNames = new List<string>();
            string line;
            try
            {
				string path1 = "data/" + gender + "Middlenames.txt";
				string path = Path.Combine(WebRootPath, path1);
				using (StreamReader reader = new StreamReader(path))
				{
					line = reader.ReadLine();
					while (line != null)
					{
						MiddleNames.Add(line);
						line = reader.ReadLine();
					}
					reader.Close();
				}
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}