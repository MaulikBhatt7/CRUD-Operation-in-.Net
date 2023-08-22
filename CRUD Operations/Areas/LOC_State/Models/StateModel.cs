namespace CRUD_Operations.Areas.LOC_State.Models
{
    public class StateModel
    {
        public int StateID { get; set; }

        public string StateName { get; set; } = string.Empty;

        public string StateCode{ get; set; } = string.Empty;

        public int CountryID { get; set; }

        public string CountryName { get; set; }
    }
}
