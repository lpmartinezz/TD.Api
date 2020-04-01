using System.Collections.Generic;

namespace TD.WebApi.Entities
{
	public class Control
	{
		//PK en la tabla. Nuevo campo sugerido.
		//public long PKControl { get; set; }
		//Valor del ID HTML5 del control que será pintado, esto no es la PK en la BD.
		public string id { get; set; }
		//Define el tipo del control o tipo pregunta asociado a la pregunta, definido en la línea 142 de este documento
		public string tipo { get; set; }
		//Texto que se visualiza en la vista del formulario.
		public string pregunta { get; set; }
		//Valores usados para las opciones del tipo (Tipo Control) de valor 1 (opción)
		public List<string> opciones { get; set; }
		//Grupo de Valores definido en la línea 136 de este documento.
		public string tipo_respuesta { get; set; }
		//Valor que determina la obligatoriedad de recibir una respuesta por parte del usuario.
		public string respuesta_obligatoria { get; set; }
		//Activa un control HTML5 para una respuesta multilínea que solo se considera cuando la propiedad tipo_respuesta es 1.
		public string respuesta_larga { get; set; }
		//Valor usado para los controles de tipo: (Calificación) y es el ícono asociado a esta calificación.
		public string tipo_simbolo { get; set; }
		//Valor usado para los controles de tipo: (Calificación). Valor máximo: 10.
		public string niveles { get; set; }
		//Tipo de restricción definida para la pregunta, los valores están definidos en la línea 157 de este documento.
		public string restriccion { get; set; }
		//Columna asociada para los datos a ingresar cuando la propiedad restricción tenga valor.
		//Ejemplo cuando el valor restriccion sea 8o 9 es el menor valor del intervalo.
		public string valor1 { get; set; }
		//Columna asociada para los datos a ingresar cuando la propiedad restricción tenga valor.
		//Ejemplo cuando el valor restriccion sea 8o 9 es el mayor valor del intervalo.
		public string valor2 { get; set; }
		//Valor del NAME HTML5 del control que será pintado, esto no es la PK en la BD.
		public string controlid { get; set; }
	}
}