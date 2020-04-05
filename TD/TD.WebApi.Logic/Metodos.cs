using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TD.WebApi.Entities;

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

        public int RetornarInt(bool valor)
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

        public Boolean email_bien_escrito(String email)
        {
            String expresion;
            expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, String.Empty).Length == 0)
                {
                    
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public void ContadorEmails(IEnumerable<RowRequest> rowRequests, IEnumerable<HeaderRequest> headerRequests, ref int contEmail)
        {
            int indexH = 0;
            foreach (var itemR in rowRequests)
            {
                foreach (var itemH in headerRequests)
                {
                    int colrow = 0;
                    if (itemH.typeColumn != "0")
                    {
                        foreach (var prop in itemR.GetType().GetProperties())
                        {
                            if (indexH == colrow)
                            {
                                var ValorAtributo = prop.GetValue(itemR, null);
                                if (ValorAtributo != null)
                                {
                                    switch (itemH.typeColumn)
                                    {
                                        case "2":
                                            if (email_bien_escrito(ValorAtributo.ToString()))
                                            {
                                                contEmail += 1;
                                            }
                                            break;
                                    }
                                }
                            }
                            colrow += 1;
                        }
                    }
                    indexH += 1;
                }
                indexH = 0;
            }
        }

    }
}