namespace NbaDbInitialiser
{
    /// <summary>
    /// 
    /// </summary>
    public class Field
    {
        public string Name { get; }
        public int Index { get; }

        public Field(string name, int index)
        {
            this.Name = name;
            this.Index = index;
        }
    }
}
