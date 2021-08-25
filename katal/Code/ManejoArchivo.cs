using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace katal.Code
{
    public class ManejoArchivo
    {
        

        // para crear el archivo
        public void GenerarTXT(string texto)
        {
            string rutaCompleta = @" C:\Users\cyper\Desktop\miarchivo.txt";
           // string texto = "HOLA MUNDO ";

            using (StreamWriter mylogs = File.AppendText(rutaCompleta))         //se crea el archivo
            {

                //se adiciona alguna información y la fecha


                DateTime dateTime = new DateTime();
                dateTime = DateTime.Now;
                string strDate = Convert.ToDateTime(dateTime).ToString("yyMMdd");

                mylogs.WriteLine(texto + strDate);

                mylogs.Close();


            }
        }

            // para escribir en el archivo
        public void AdicionarInfoAlTxt(string texto)
        {
            string rutaCompleta = @" C:\miarchivo.txt";
            //string texto = "HOLA DE NUEVO";

            using (StreamWriter file = new StreamWriter(rutaCompleta, true))
            {
                file.WriteLine(texto); //se agrega información al documento

                file.Close();
            }
        }

            // para leer la información el archivo
        public void LeerInfoTxt()
        {
            string rutaCompleta = @"C:\miarchivo.txt";

            string line = "";
            using (StreamReader file = new StreamReader(rutaCompleta))
            {
                while ((line = file.ReadLine()) != null)                //Leer linea por linea
                {
                    Console.WriteLine(line);
                }

                // OTRA FORMA DE LEER TODO EL ARCHIVO

                line = file.ReadToEnd();

                Console.WriteLine(line);

                file.Close();


            }

        }
    }
}