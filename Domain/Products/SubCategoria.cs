namespace Domain.Products
{
    public class SubCategoria
    {
        
        private int SubCategoria_ID { get; }
        private int FK_Categoria_ID { get; }
        private string Nombre, Descripcion;
        
        public SubCategoria() {}

        public SubCategoria(int subCategoriaId, int fkCategoriaId, string nombre, string descripcion)
        {
            this.SubCategoria_ID = subCategoriaId;
            this.FK_Categoria_ID = fkCategoriaId;
            this.Nombre = nombre;
            this.Descripcion = descripcion;
        }

        public override string ToString()
        {
            return $"SubCategoria: [ID: {SubCategoria_ID} / Categoria ID: {FK_Categoria_ID} / Nombre: {Nombre} / Descripcion: {Descripcion}]";
        }

    }
}