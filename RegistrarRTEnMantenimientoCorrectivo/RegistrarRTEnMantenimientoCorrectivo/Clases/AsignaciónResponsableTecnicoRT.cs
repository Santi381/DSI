using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RegistrarRTEnMantenimientoCorrectivo.Clases;

namespace RegistrarRTEnMantenimientoCorrectivo.Clases
{
    public class AsignaciónResponsableTecnicoRT
    {

        private DateTime fechaDesde;
        private DateTime fechaHasta;
        private PersonalCientifico personalCientifico;
        private List<RecursoTecnologico> recursoTecnologicos;

        public AsignaciónResponsableTecnicoRT(DateTime fechaDesde, DateTime fechaHasta, PersonalCientifico personalCientifico, List<RecursoTecnologico> recursoTecnologicos)
        {
            this.fechaDesde = fechaDesde;
            this.fechaHasta = fechaHasta;
            this.personalCientifico = personalCientifico;
            this.recursoTecnologicos = recursoTecnologicos;
        }

        public AsignaciónResponsableTecnicoRT()
        {

        }

        public DateTime FechaDesde { get => fechaDesde; set => fechaDesde = value; }
        public DateTime FechaHasta { get => fechaHasta; set => fechaHasta = value; }
        public PersonalCientifico PersonalCientifico { get => personalCientifico; set => personalCientifico = value; }
        public List<RecursoTecnologico> RecursoTecnologicos { get => recursoTecnologicos; set => recursoTecnologicos = value; }

        public AsignaciónResponsableTecnicoRT esVigente(PersonalCientifico personalCientifico)
        {
            var sentenciaSql = $"SELECT p.*,r.numeroRT FROM AsignacionRespTecnicoRT p, RecursoTecnologico r WHERE p.fechaHasta IS NULL and p.legajo = {personalCientifico.Legajo}";
            var tablaResultado = DBHelper.GetDBHelper().ConsultaSQL(sentenciaSql);
             
            RecursoTecnologico recursoTecnologico = new RecursoTecnologico();
            AsignaciónResponsableTecnicoRT asig = new AsignaciónResponsableTecnicoRT();
            asig.FechaDesde = Convert.ToDateTime(tablaResultado.Rows[0]["fechaDesde"]);
            if (tablaResultado.Rows[0]["fechaHasta"] is DBNull)
            {
                asig.FechaHasta = DateTime.MinValue;
            }
            else
            {
                asig.FechaHasta = Convert.ToDateTime(tablaResultado.Rows[0]["fechaHasta"]);
            }
            
            asig.PersonalCientifico = personalCientifico;
            var recurso = recursoTecnologico.getRecursoTecnologicoEspecifico(personalCientifico.Legajo);
            asig.RecursoTecnologicos = recurso;

            return asig;
        }


        public AsignaciónResponsableTecnicoRT esAsignacionVigenteCientif(PersonalCientifico persCientifico)
        {
            var asignacion = esVigente(persCientifico);
            return asignacion;
        }

        public List<List<string>> getRTDisponibless(List<RecursoTecnologico> listaRTD)
        {
            List<List<string>> listaGrande = new List<List<string>>();
            foreach (RecursoTecnologico rec in listaRTD)
            {
                RecursoTecnologico resul = new RecursoTecnologico();
                var resultado = resul.getDatos(rec);
                listaGrande.Add(resultado);
            }
            return listaGrande;
        }

        public List<RecursoTecnologico> getRTDisponibles(AsignaciónResponsableTecnicoRT asig)
        {
            var rts = asig.RecursoTecnologicos;
            if (rts != null)
            {
                List<RecursoTecnologico> listaRT = new List<RecursoTecnologico>();
                foreach (RecursoTecnologico rec in rts)
                {
                    RecursoTecnologico resul = new RecursoTecnologico();
                    var resultado = resul.esDisponible(rec);
                    if (resultado == true)
                    {
                        listaRT.Add(rec);
                    }
                }
                return listaRT;
            }
            else
            {
                return null;
            }
            
        }

        


    }
}
