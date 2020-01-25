using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Domain.DB
{
    public class FilterBuilder
    {

        public enum Operador
        {
            Y,
            O
        } 
        
        private static readonly Dictionary<Operador, string> operadores = new Dictionary<Operador, string>
        {
            { Operador.Y, "and" },
            { Operador.O, "or" }
        };

        private Type clase;
        private Operador op;
        private List<Criteria> criterias;
        
        public FilterBuilder()
        {
            this.clase = null;
            this.criterias = new List<Criteria>();
        }

        public Criteria AgregarCriteria(string atributo, Criteria.Operador operador)
        {

            if (this.clase == null)
                throw new InvalidOperationException("No se ha establecido una clase para este builder");

            if (clase.GetRuntimeFields().All(field => !field.Name.Equals(atributo)))
                throw new ArgumentException("Atributo no es propiedad de la clase proporcionada.");

            var nuevaCriteria = new Criteria(atributo, operador);
            this.criterias.Add(nuevaCriteria);
            return nuevaCriteria;

        }

        public string Build()
        {
            
            if(this.criterias.Count < 1)
                throw new InvalidOperationException("No se ha asignado ninguna Criteria a este Builder.");

            if (this.clase == null)
                throw new InvalidOperationException("No se ha establecido una clase para este builder");

            var filtros = (this.criterias.Count == 1) ?
                this.criterias[0].GetExpresion() :
                string.Join(" " + operadores[this.op] + " ", this.criterias.Select(c => c.GetExpresion()));

            return $"Select * From {this.clase.Name} Where {filtros}";
            
        }

        public void SetClase(Type tipoClase)
        {
            if(this.clase != null && this.criterias.Count > 0)
                throw new InvalidOperationException("No se puede asignar un tipo de clase al filtro habiendo criterias. Cree un nuevo Builder.");
            this.clase = tipoClase;
        }

        public void SetOperador(Operador operador)
        {
            this.op = operador;
        }

        public List<Criteria> GetCriterias()
        {
            return this.criterias;
        }

    }

    public class Criteria
    {
        
        public enum Operador
        {
            IGUAL,
            DISTINTO,
            MAYOR,
            MENOR,
            MAYOR_O_IGUAL,
            MENOR_O_IGUAL
        }
        
        private static readonly Dictionary<Operador, string> operadores = new Dictionary<Operador, string>
        {
            { Operador.IGUAL, "=" },
            { Operador.DISTINTO, "!=" },
            { Operador.MAYOR, ">" },
            { Operador.MENOR, "<" },
            { Operador.MAYOR_O_IGUAL, ">=" },
            { Operador.MENOR_O_IGUAL, "<=" },
        };
        
        private readonly string op;
        private readonly string atributo;

        internal Criteria(string atributo, Operador op)
        {
            this.atributo = atributo;
            this.op = operadores[op];
        }

        public string GetExpresion()
        {
            return $"{this.atributo} {this.op} @{this.atributo}";
        }
    }
}