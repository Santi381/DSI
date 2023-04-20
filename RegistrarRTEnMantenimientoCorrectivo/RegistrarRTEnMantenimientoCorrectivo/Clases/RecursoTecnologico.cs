using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;

namespace RegistrarRTEnMantenimientoCorrectivo.Clases
{
    public class RecursoTecnologico
    {
        private int numeroRT;
        private DateTime fechaAlta;
        private string imagen;
        private int periocidadMantenimientoPrev;
        private int duracionMantenimientoPrev;
        private int fraccionHorariaTurnos;
        private List<HorarioRT> disponibilidad;
        private List<Turno> turnos; 
        private TipoRecursoTecnológico tipoDeRT;
        private List<CaracteristicaRecurso> caracteristicaRecursos;
        private Modelo modelo;
        private List<CambioEstadoRT> cambioEstadoRT;
        private List<Mantenimiento> mantenimientos;
        public RecursoTecnologico()
        {

        }

        public RecursoTecnologico(int numeroRT, DateTime fechaAlta, string imagen, int periocidadMantenimientoPrev, int duracionMantenimientoPrev, int fraccionHorariaTurnos, List<HorarioRT> disponibilidad, TipoRecursoTecnológico tipoDeRT, List<Turno> turnos, List<CaracteristicaRecurso> caracteristicaRecursos, Modelo modelo, List<CambioEstadoRT> cambioEstadoRT, List<Mantenimiento> mantenimientos)
        {
            this.numeroRT = numeroRT;
            this.fechaAlta = fechaAlta;
            this.imagen = imagen;
            this.periocidadMantenimientoPrev = periocidadMantenimientoPrev;
            this.duracionMantenimientoPrev = duracionMantenimientoPrev;
            this.fraccionHorariaTurnos = fraccionHorariaTurnos;
            this.disponibilidad = disponibilidad;
            this.tipoDeRT = tipoDeRT;
            this.turnos = turnos;
            this.caracteristicaRecursos = caracteristicaRecursos;
            this.modelo = modelo;
            this.cambioEstadoRT = cambioEstadoRT;
            this.mantenimientos = mantenimientos;
        }

        public int NumeroRT { get => numeroRT; set => numeroRT = value; }
        public DateTime FechaAlta { get => fechaAlta; set => fechaAlta = value; }
        public string Imagen { get => imagen; set => imagen = value; }
        public int PeriocidadMantenimientoPrev { get => periocidadMantenimientoPrev; set => periocidadMantenimientoPrev = value; }
        public int DuracionMantenimientoPrev { get => duracionMantenimientoPrev; set => duracionMantenimientoPrev = value; }
        public int FraccionHorariaTurnos { get => fraccionHorariaTurnos; set => fraccionHorariaTurnos = value; }
        public List<HorarioRT> Disponibilidad { get => disponibilidad; set => disponibilidad = value; }
        public TipoRecursoTecnológico TipoDeRT { get => tipoDeRT; set => tipoDeRT = value; }
        public List<Turno> Turnos { get => turnos; set => turnos = value; }
        public List<CaracteristicaRecurso> CaracteristicaRecursos { get => caracteristicaRecursos; set => caracteristicaRecursos = value; }
        public Modelo Modelo { get => modelo; set => modelo = value; }
        public List<CambioEstadoRT> CambioEstadoRTT { get => cambioEstadoRT; set => cambioEstadoRT = value; }
        public List<Mantenimiento> Mantenimientos { get => mantenimientos; set => mantenimientos = value; }

        ////public List<RecursoTecnologico> getRecursoTecnologico()
        //{
        //}

        public List<RecursoTecnologico> getRecursoTecnologicoEspecifico(int legajo)
        {
            List<RecursoTecnologico> listaRT = new List<RecursoTecnologico>();
            var sentenciaSql = $"SELECT r.* FROM RecursoTecnologico r JOIN AsignacionRespTecnicoRT a ON (r.idAsignacionRespTecRT = a.idAsignacion) WHERE a.legajo = {legajo}";
            var tablaResultado = DBHelper.GetDBHelper().ConsultaSQL(sentenciaSql);
            for (int i = 0; i < tablaResultado.Rows.Count; i++)
            {
                Mantenimiento mantenimiento= new Mantenimiento();
                CambioEstadoRT cambioRTT = new CambioEstadoRT();
                Modelo modelo = new Modelo();
                CaracteristicaRecurso caracRT = new CaracteristicaRecurso();
                Turno turno = new Turno();
                TipoRecursoTecnológico tipoRT = new TipoRecursoTecnológico();
                HorarioRT disponibilidad = new HorarioRT();
                RecursoTecnologico rt = new RecursoTecnologico();
                var numero = Convert.ToInt32(tablaResultado.Rows[i]["numeroRT"]);
                rt.NumeroRT = numero;
                rt.FechaAlta = Convert.ToDateTime(tablaResultado.Rows[i]["fechaAlta"]);
                rt.Imagen = tablaResultado.Rows[i]["imagenes"].ToString();
                rt.PeriocidadMantenimientoPrev = Convert.ToInt32(tablaResultado.Rows[i]["periodicidadMantPrev"]);
                rt.DuracionMantenimientoPrev = Convert.ToInt32(tablaResultado.Rows[i]["duracionMantPrev"]);
                rt.FraccionHorariaTurnos = Convert.ToInt32(tablaResultado.Rows[i]["fraccionHorarioTurnos"]);
                var disp = disponibilidad.getDisponibilidadEspecifica(numero);
                rt.Disponibilidad = disp;
                var tipo = Convert.ToInt32(tablaResultado.Rows[i]["idTipoRT"]);
                rt.TipoDeRT = tipoRT.getTipoRTEspecifico(tipo);
                var turnos = turno.getTurnosEspecificos(numero);
                rt.Turnos = turnos;
                var carac = caracRT.getCaracteristicaEspecifica(numero);
                rt.CaracteristicaRecursos = carac;
                var m = Convert.ToInt32(tablaResultado.Rows[i]["idModelo"]);
                rt.Modelo = modelo.getModeloEspecifico(m);
                var cambioRT = cambioRTT.getCambioEstadoRTEspecifico(numero);
                rt.CambioEstadoRTT = cambioRT;
                var mantenimientos = mantenimiento.getMantenimientoEspecifico(numero);
                rt.Mantenimientos = mantenimientos;
                listaRT.Add(rt);
            }
            return listaRT;
        }
        public bool esDisponible(RecursoTecnologico rt)
        {
            CambioEstadoRT cambioRT = new CambioEstadoRT();
            var cambios = cambioRT.esActual(rt);

            
            return cambios;
        }
        public List<string> getDatos(RecursoTecnologico rec)
        {
            List<string> listaString = new List<string>();
            listaString.Add(rec.numeroRT.ToString());
            TipoRecursoTecnológico tipoRec = new TipoRecursoTecnológico();
            var resultado = tipoRec.getNombre(rec);
            listaString.Add(resultado.ToString());
            Modelo modelo1 = new Modelo();
            var (stringModelo, stringMarca) = modelo1.getDatos(rec);
            listaString.Add(stringMarca);
            listaString.Add(stringModelo);
            //var listaOrdenada = listaString.OrderByDescending(x => resultado);
            return listaString;
        }

        public RecursoTecnologico obtenerRT(int numero, List<RecursoTecnologico> listaRT) 
        {
            var recursos = listaRT;
            var recurso = new RecursoTecnologico();
            foreach (RecursoTecnologico item in recursos)
            {
                if (item.NumeroRT == numero)
                {
                    recurso = item;
                    break;
                }
            }
            return recurso;
        }

        public List<Turno> getTurnosCancelablesEnPeriodo(RecursoTecnologico rec1, DateTime fechaIngresada)
        {

            var turnosRT = rec1.Turnos;
            var turno = new Turno();

            var turnosRTCancelables = turno.esCancelableEnPeriodo(turnosRT, fechaIngresada);

            return turnosRTCancelables;

        }

        public void newCambioEstadoRT(RecursoTecnologico rec, Estado estado)
        {
            var nuevoCambio = new CambioEstadoRT();
            var sentenciaSQL = $"SELECT idEstado FROM Estado WHERE nombre = '{estado.Nombre}'";
            var tabla = DBHelper.GetDBHelper().ConsultaSQL(sentenciaSQL);
            var idEstado = Convert.ToInt32(tabla.Rows[0]["idEstado"]);

            nuevoCambio.newCambioEstadoRT(rec.numeroRT, idEstado);
        }

        public void newMantenimiento(RecursoTecnologico rec, string motivo,DateTime fechaFin)
        {
            var mant = new Mantenimiento();
            mant.newMantenimiento(rec, motivo, fechaFin);

        }

        

    }
}
