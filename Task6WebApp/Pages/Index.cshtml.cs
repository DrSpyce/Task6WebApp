using Bogus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Task6WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IWebHostEnvironment _env;

        public IndexModel(IWebHostEnvironment env)
        {
            _env = env;
            WebRootPath = _env.WebRootPath;
        }

        private string WebRootPath;

        private Faker Fak;

        private Faker<Person> PersonGenerator;

        [BindProperty]
        public int Seed { get; set; } = 0;

        [BindProperty]
        public string Locale { get; set; } = "ru";

        private const int NumberOfPersons = 20;

        public void OnGet()
        {
            Persons = RandomizePersons(Seed, Locale, NumberOfPersons);
        }

        public IEnumerable<Person> Persons { get; set; } = default!;

        public async Task<IActionResult> OnGetScroll(int seed, string locale, int numberOfPersons, double errors)
        {
            Persons = RandomizePersons(seed, locale, numberOfPersons);
            Fak = new Faker(locale);
            GenerateErrors(errors);
            return new JsonResult(Persons);
        }

        public IEnumerable<Person> RandomizePersons(int seed, string locale, int numberOfPersons)
        {
            Randomizer.Seed = new Random(seed);
            int personId = 1;
            PersonGenerator = new Faker<Person>(locale)
                .CustomInstantiator(f => new Person(personId++))
                .RuleFor(p => p.Gender, f => f.Person.Gender.ToString())
                .RuleFor(p => p.FullName, f => f.FullNameWithMiddle(f.Person.Gender, WebRootPath))
                .RuleFor(p => p.Phone, f => f.Phone.PhoneNumber())
                .RuleFor(p => p.Adress, f => f.AddressWithoutCountry())
                .RuleFor(p => p.Guid, f =>
                {
                    var str = Convert.ToString(f.Random.Uuid());
                    return str;
                });
            return PersonGenerator.Generate(numberOfPersons);
        }

        public void GenerateErrors(double errors)
        {
            for (double i = errors; i > 0; i--)
            {
                if (i < 1)
                {
                    if (Fak.Random.Bool()) { break; }
                }
                foreach (var item in Persons)
                {
                    SelectStringAndSave(item);
                }
            }
        }

        private void SelectStringAndSave(Person item)
        {
            switch(Fak.Random.Number(3))
            {
                case 0:
                    item.Phone = SelectTypeOfError(item.Phone);
                    break;
                case 1:
                    item.FullName = SelectTypeOfError(item.FullName);
                    break;
                case 2:
                    item.Adress = SelectTypeOfError(item.Adress);
                    break;
                case 3:
                    item.Guid = SelectTypeOfError(item.Guid);
                    break;
            }
        }

        private string SelectTypeOfError(string str)
        {
            int num = IsValid(str);
            switch (num)
            {
                case 0:
                    return DeleteSymbols(str);
                case 1:
                    return ChangePlaces(str);
                case 2:
                    return AddSymbol(str);
            }
            return "error";
        }

        private int IsValid(string str)
        {
            if(str.Length < 3)
            {
                return Fak.Random.Number(1,2);
            }
            if(str.Length > 40)
            {
                return Fak.Random.Number(0, 1);
            }
            return Fak.Random.Number(0, 2);
        }

        private string DeleteSymbols(string str)
        {
            return str.Remove(Fak.Random.Number(str.Length - 1), 1);
        }

        private string AddSymbol(string str)
        {
            return str.Insert(Fak.Random.Number(str.Length), Fak.StringWithLocale());        
        }

        private string ChangePlaces(string str)
        {
            var tmp = str.ToCharArray();
            int index = Fak.Random.Number(tmp.Length - 2);
            char tmp1 = tmp[index];
            tmp[index] = tmp[index + 1];
            tmp[index + 1] = tmp1;
            return String.Concat(tmp);
        }
    }
}