using MagicVilla_VillaAPI.Models.Dto;

namespace MagicVilla_VillaAPI.Data
{
    public static class VillaStore
    {
        public static List<VillaDto> villalist = new List<VillaDto> {
            new VillaDto{Id=1, Name= "Pool View", Sqft=100, Occupancy=4},
            new VillaDto{Id=1, Name= "Beach View", Sqft= 330, Occupancy=2}
        };
        

    }
}
