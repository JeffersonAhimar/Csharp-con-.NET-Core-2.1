using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreEscuela.Entidades;

namespace CoreEscuela.App
{
    public class Reporteador
    {
        Dictionary<LlavesDiccionario, IEnumerable<ObjetoEscuelaBase>> _diccionario;

        public Reporteador(Dictionary<LlavesDiccionario, IEnumerable<ObjetoEscuelaBase>> dicObsEsc)
        {
            if (dicObsEsc == null)
                throw new ArgumentNullException(nameof(dicObsEsc));
            _diccionario = dicObsEsc;
        }

        public IEnumerable<Evaluación> GetListaEvaluaciones()
        {
            if (_diccionario.TryGetValue(LlavesDiccionario.Evaluacion, out IEnumerable<ObjetoEscuelaBase> lista))
            {
                return lista.Cast<Evaluación>();
            }
            else
            {
                return new List<Evaluación>();
                // Escribir en el log de auditoria
            }
        }

        //
        public IEnumerable<string> GetListaAsignaturas()
        {
            return GetListaAsignaturas(out var dummy);
        }

        public IEnumerable<string> GetListaAsignaturas(out IEnumerable<Evaluación> listaEvaluaciones)
        {
            listaEvaluaciones = GetListaEvaluaciones();

            return (from Evaluación ev in listaEvaluaciones
                    where ev.Nota >= 5.0f
                    select ev.Asignatura.Nombre).Distinct();
        }

        public Dictionary<string, IEnumerable<Evaluación>> GetDicEvaluXAsig()
        {
            var dicRta = new Dictionary<string, IEnumerable<Evaluación>>();

            var listaAsig = GetListaAsignaturas(out var listaEval);

            foreach (var asig in listaAsig)
            {
                var evalAsig = from eval in listaEval
                               where eval.Asignatura.Nombre == asig
                               select eval;
                dicRta.Add(asig, evalAsig);
            }
            return dicRta;
        }
        public Dictionary<string, IEnumerable<Object>> GetPromeAlumnoXAsignatura()
        {
            var rta = new Dictionary<string, IEnumerable<Object>>();
            var dicEvalXAsig = GetDicEvaluXAsig();

            foreach (var asigConEval in dicEvalXAsig)
            {
                var promAlumnos = from eval in asigConEval.Value
                            group eval by new{
                                eval.Alumno.UniqueId,
                                eval.Alumno.Nombre}
                            into grupoEvalsAlumno
                            select new AlumnoPromedio
                            {
                                alumnoId = grupoEvalsAlumno.Key.UniqueId,
                                alumnoNombre = grupoEvalsAlumno.Key.Nombre,
                                promedio = grupoEvalsAlumno.Average(evaluacion => evaluacion.Nota)
                            };
                rta.Add(asigConEval.Key,promAlumnos);
            }
            return rta;
        }
    }
}