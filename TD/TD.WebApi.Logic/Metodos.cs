using System.Collections.Generic;

namespace TD.WebApi.Logic
{
    public class Metodos
    {

        public List<string> CargaOpciones(string sOpciones)
        {
            List<string> lstOpciones = new List<string>();
            if (sOpciones.Length > 0)
            {
                string[] opciones = sOpciones.Split('|');
                foreach (var item in opciones)
                {
                    lstOpciones.Add(item.ToString());
                }
            }
            return lstOpciones;
        }

        public string LeerOpciones(List<string> lstOpciones)
        {
            string sOpciones = string.Empty;
            if (lstOpciones.Count > 0)
            {
                foreach (var item in lstOpciones)
                {
                    sOpciones = sOpciones + item + "|";
                }
                sOpciones = sOpciones.Substring(0, sOpciones.Length - 1);
            }
            return sOpciones;
        }

        public string RetornarString(bool? valor)
        {
            string svalor = "0";
            if (valor == true)
            {
                svalor = "1";
            }
            else
            {
                svalor = "0";
            }
            return svalor;
        }

        public int RetornarInt(bool? valor)
        {
            int svalor = 0;
            if (valor == true)
            {
                svalor = 1;
            }
            else
            {
                svalor = 0;
            }
            return svalor;
        }

        public bool? RetornarBoolean(string valor)
        {
            bool? bvalor = null;
            if (!string.IsNullOrEmpty(valor))
            {
                if (valor == "1")
                {
                    bvalor = true;
                }
                else
                {
                    bvalor = false;
                }
            }
            return bvalor;
        }

    }
}