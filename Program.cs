using System;
using System.Collections.Generic;
using CoreEscuela.App;
using CoreEscuela.Entidades;
using CoreEscuela.Util;
using static System.Console;

namespace CoreEscuela
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += AccionDelEvento;
            AppDomain.CurrentDomain.ProcessExit += (o, s) => Printer.Beep(2000, 1000, 1);
            var engine = new EscuelaEngine();
            engine.Inicializar();
            Printer.WriteTitle("BIENVENIDOS A LA ESCUELA");

            var reporteador = new Reporteador(engine.GetDiccionarioObjetos());
            var evalList = reporteador.GetListaEvaluaciones();
            var listaAsg = reporteador.GetListaAsignaturas();
            var listaEvalXAsig = reporteador.GetDicEvaluXAsig();
            var listaPromXAsig = reporteador.GetPromeAlumnoXAsignatura();
            //New feature
            var listTopXAlumnos = reporteador.GetTopXAlumnos(10);

            Printer.WriteTitle("Captura de una Evaluación por Consola");
            var newEval = new Evaluación();
            string nombre, notaString;
            float nota;
            //
            WriteLine("Ingrese el nombre de la evaluación >>");
            Printer.PresioneEnter();
            nombre = ReadLine();
            if (string.IsNullOrWhiteSpace(nombre))
            {
                Printer.WriteTitle("El valor del nombre no puede ser vacio");
                WriteLine("Saliendo del programa");
            }
            else
            {
                newEval.Nombre = nombre.ToLower();
                WriteLine("El nombre de la evaluación ha sido ingresado correctamente");
            }

            WriteLine("Ingrese la nota de la evaluación >>");
            Printer.PresioneEnter();
            notaString = ReadLine();
            if (string.IsNullOrWhiteSpace(notaString))
            {
                Printer.WriteTitle("El valor de la nota no puede ser vacio");
                WriteLine("Saliendo del programa");
                //throw new ArgumentException("El valor de la nota no puede ser vacío");
            }
            else
            {
                try
                {
                    newEval.Nota = float.Parse(notaString);
                    if(newEval.Nota<0 || newEval.Nota>20){
                        throw new ArgumentOutOfRangeException("La nota debe estar entre 0 y 20");
                    }
                    WriteLine("La nota de la evaluación ha sido ingresado correctamente");
                }
                catch(ArgumentOutOfRangeException arge){
                    Printer.WriteTitle(arge.Message);
                    WriteLine("Saliendo del programa");
                }
                catch (Exception)
                {
                    Printer.WriteTitle("El valor de la nota no es un numero válido");
                    WriteLine("Saliendo del programa");
                }
                finally{
                    Printer.WriteTitle("FINALLY");
                    Printer.Beep(2500,500,3);
                }
            }

        }

        private static void AccionDelEvento(object? sender, EventArgs e)
        {
            Printer.WriteTitle("SALIENDO");
            Printer.Beep(3000, 1000, 1);
            Printer.WriteTitle("SALIÓ");
        }

        private static void ImpimirCursosEscuela(Escuela escuela)
        {

            Printer.WriteTitle("Cursos de la Escuela");


            if (escuela?.Cursos != null)
            {
                foreach (var curso in escuela.Cursos)
                {
                    WriteLine($"Nombre {curso.Nombre}, Id  {curso.UniqueId}");
                }
            }
        }
    }
}