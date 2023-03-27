namespace pxtools.Models
{
    public class Table
    {
        public string Name { get; set; } = "";

        public IList<Field> Fields { get; set; } = new List<Field>();

        public IList<IList<object?>> Data { get; set; } = new List<IList<object?>>(); 
    }
}
