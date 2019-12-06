using System.Dynamic;

namespace Domain.Locations
{
    public class Storage
    {
        private int id { get; }
        private string Name { get; }
        private string Code { get; }

        public Storage(string name, string code)
        {
            this.Name = name;
            this.Code = code;
        }
    }
}