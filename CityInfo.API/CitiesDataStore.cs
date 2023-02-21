using CityInfo.API.Model;

namespace CityInfo.API
{
    public class CitiesDataStore
    {

        public static CitiesDataStore current { get; } = new CitiesDataStore();
        public List<CityDto> Cities { get; set; }

        public CitiesDataStore()
        {
            Cities = new List<CityDto>()
            {
                new CityDto()
                {
                    Id= 1,
                    Name = "Ahmedabad",
                    Description = "The one with river front."
                },

                new CityDto()
                {
                    Id= 2,
                    Name = "Anand",
                    Description = "The one with Amul Dairy."
                },

                new CityDto()
                {
                    Id= 3,
                    Name = "Paris",
                    Description = "The one with a tower."
                },


            };
        }
    }
}
