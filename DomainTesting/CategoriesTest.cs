using System;
using System.Data;
using Domain.Products;
using NUnit.Framework;

namespace DomainTesting
{
    public class CategoriesTest : Setup
    {

        public void PopulateCategorias()
        {
            var categoria = Categoria.AgregarCategoria("Llantas", "Descripcion de Llantas.");
            categoria.AgregarSubCategoria("Llantas de Invierno", "Descripcion de Llantas de Invierno");
        }

        [Test]
        public void CrearCategorias()
        {
            Assert.DoesNotThrow(() =>
            {
                this.PopulateCategorias();
                var categoria = Categoria.GetCategoria(1);
                categoria.GetSubCategorias();
            });
        }

        [Test]
        public void DuplicarCategorias()
        {
            
            this.PopulateCategorias();
            var categoria = Categoria.GetCategoria(1);
            Assert.Multiple(() =>
            {
                Assert.Throws<DuplicateNameException>(
                    () => Categoria.AgregarCategoria("Llantas", "Descripcion"),
                    "[ERROR]: Una excepcion DuplicateNameException deberia ser lanzada en Categoria."
                );
                
                Assert.Throws<DuplicateNameException>(
                    () => categoria.AgregarSubCategoria("Llantas de Invierno", "Descripcion"),
                    "[ERROR]: Una excepcion DuplicateNameException deberia ser lanzada en creacion de SubCategoria."
                );
            });
            
        }

        [Test]
        public void CrearCategoriasParametrosNulos()
        {
            
            this.PopulateCategorias();
            var categoria = Categoria.GetCategoria(1);
            Assert.Multiple(() =>
            {
                Assert.Throws<ArgumentNullException>(
                    () => Categoria.AgregarCategoria(null, null),
                    "[ERROR]: Una excepcion ArgumentNullException deberia ser lanzada en creacion de Categoria."
                );
                
                Assert.Throws<ArgumentNullException>(
                    () => categoria.AgregarSubCategoria(null, null),
                    "[ERROR]: Una excepcion ArgumentNullException deberia ser lanzada en creacion de SubCategoria."
                );
            });
            
        }

        [Test]
        public void CrearCategoriasParametrosFueraRango()
        {
            
            this.PopulateCategorias();
            var categoria = Categoria.GetCategoria(1);
            
            Assert.Throws<ArgumentOutOfRangeException>(
                () => Categoria.AgregarCategoria("Motores", "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam vel placerat dui, in interdum felis. Mauris eleifend cursus volutpat. Nulla ante tellus, faucibus quis leo nec, blandit vehicula ante. Aliquam gravida elementum sollicitudin. Praesent non volutpat sem. Maecenas lacus ligula, finibus sed accumsan quis, aliquam non tellus. Aenean nec commodo risus. Donec lacinia auctor congue. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Etiam quis euismod ligula, nec eleifend sapien. Integer quam magna, maximus eu dignissim vitae, blandit a enim. Nulla rutrum fringilla luctus. Integer laoreet consequat dapibus. Vivamus rhoncus lectus ut scelerisque pellentesque. Duis luctus ligula arcu, at efficitur nisi mollis sit amet. Nulla molestie ultricies odio, sed scelerisque erat dictum nec. Phasellus pellentesque sit amet mi non consectetur. Maecenas luctus erat id mauris ultrices, porta suscipit purus accumsan. Proin semper risus eu nulla efficitur scelerisque. Vivamus metus velit, congue id magna et, mattis luctus magna. Quisque at sagittis enim, quis iaculis nisl. Donec ut tempor nisi. "),
                "[ERROR]: Una excepcion ArgumentOutOfRangeException deberia ser lanzada en Categoria."
            );
            
            Assert.Throws<ArgumentOutOfRangeException>(
                () => categoria.AgregarSubCategoria("Motores", "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam vel placerat dui, in interdum felis. Mauris eleifend cursus volutpat. Nulla ante tellus, faucibus quis leo nec, blandit vehicula ante. Aliquam gravida elementum sollicitudin. Praesent non volutpat sem. Maecenas lacus ligula, finibus sed accumsan quis, aliquam non tellus. Aenean nec commodo risus. Donec lacinia auctor congue. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Etiam quis euismod ligula, nec eleifend sapien. Integer quam magna, maximus eu dignissim vitae, blandit a enim. Nulla rutrum fringilla luctus. Integer laoreet consequat dapibus. Vivamus rhoncus lectus ut scelerisque pellentesque. Duis luctus ligula arcu, at efficitur nisi mollis sit amet. Nulla molestie ultricies odio, sed scelerisque erat dictum nec. Phasellus pellentesque sit amet mi non consectetur. Maecenas luctus erat id mauris ultrices, porta suscipit purus accumsan. Proin semper risus eu nulla efficitur scelerisque. Vivamus metus velit, congue id magna et, mattis luctus magna. Quisque at sagittis enim, quis iaculis nisl. Donec ut tempor nisi. "),
                "[ERROR]: Una excepcion ArgumentOutOfRangeException deberia ser lanzada en SubCategoria."
            );
            
        }
    }
}