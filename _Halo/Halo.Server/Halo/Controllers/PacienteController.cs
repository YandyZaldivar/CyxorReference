/*
  { Halo.Server } - Sistema Nacional de Vigilancia a la Morbilidad Materna Extremadamente Grave
  Copyright (C) 2017 Halo
  Authors:  Mayli Sanchez
            Yandy Zaldivar
*/

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;

namespace Halo.Controllers
{
    using Models;

    using Cyxor.Models;
    using Cyxor.Controllers;

    class PacienteController : BaseController
    {
        /*
        Paciente NewPaciente(PacienteApiModel paciente) =>
            new Paciente
            {
                Id = paciente.Id,
                Edad = paciente.Edad,
                Nombre = paciente.Nombre,
                HospitalId = paciente.HospitalId,
                OcupacionId = paciente.OcupacionId,
                FechaIngreso = paciente.FechaIngreso,
                EscolaridadId = paciente.EscolaridadId,
                //HistoriaClinica = paciente.HistoriaClinica,
                Traslado1HospitalId = paciente.Traslado1HospitalId,
                Traslado2HospitalId = paciente.Traslado2HospitalId,

                Direccion = paciente.Direccion == null ? null : new Direccion
                {
                    AreaId = paciente.Direccion.AreaId,
                    MunicipioId = paciente.Direccion.MunicipioId,
                },

                HistorialMedico = paciente.HistorialMedico == null ? null : new HistorialMedico
                {
                    AntecedenteGinecoObstetrico = paciente.HistorialMedico.AntecedenteGinecoObstetrico == null ? null : new AntecedenteGinecoObstetrico
                    {
                        Vivos = paciente.HistorialMedico.AntecedenteGinecoObstetrico.Vivos,
                        Molas = paciente.HistorialMedico.AntecedenteGinecoObstetrico.Molas,
                        Muertos = paciente.HistorialMedico.AntecedenteGinecoObstetrico.Muertos,
                        Abortos = paciente.HistorialMedico.AntecedenteGinecoObstetrico.Abortos,
                        Cesarias = paciente.HistorialMedico.AntecedenteGinecoObstetrico.Cesarias,
                        Ectopicos = paciente.HistorialMedico.AntecedenteGinecoObstetrico.Ectopicos,
                        Gestaciones = paciente.HistorialMedico.AntecedenteGinecoObstetrico.Gestaciones,
                        PartosVaginales = paciente.HistorialMedico.AntecedenteGinecoObstetrico.PartosVaginales,
                        UltimaGestacion = paciente.HistorialMedico.AntecedenteGinecoObstetrico.UltimaGestacion,
                    },
                    AtencionPrenatal = paciente.HistorialMedico.AtencionPrenatal == null ? null : new AtencionPrenatal
                    {
                        Reevaluacion = paciente.HistorialMedico.AtencionPrenatal.Reevaluacion,
                        SemanasCaptacion = paciente.HistorialMedico.AtencionPrenatal.SemanasCaptacion,
                        EvaluadoComoRiesgo = paciente.HistorialMedico.AtencionPrenatal.EvaluadoComoRiesgo,
                        ControlesPrenatales = paciente.HistorialMedico.AtencionPrenatal.ControlesPrenatales,
                        IndiceMasaCorporalId = paciente.HistorialMedico.AtencionPrenatal.IndiceMasaCorporalId,

                        Orina = paciente.HistorialMedico.AtencionPrenatal.Orina == null ? null : new Orina
                        {
                            PrimerTrimestre = paciente.HistorialMedico.AtencionPrenatal.Orina.PrimerTrimestre,
                            SegundoTrimestre = paciente.HistorialMedico.AtencionPrenatal.Orina.SegundoTrimestre,
                            TercerTrimestre = paciente.HistorialMedico.AtencionPrenatal.Orina.TercerTrimestre,
                        },

                        Riesgo = paciente.HistorialMedico.AtencionPrenatal.Riesgo == null ? null : new Riesgo
                        {
                            Asma = paciente.HistorialMedico.AtencionPrenatal.Riesgo.Asma,
                            Otros = paciente.HistorialMedico.AtencionPrenatal.Riesgo.Otros,
                            Anemia = paciente.HistorialMedico.AtencionPrenatal.Riesgo.Anemia,
                            Eclampsia = paciente.HistorialMedico.AtencionPrenatal.Riesgo.Eclampsia,
                            EdadExtrema = paciente.HistorialMedico.AtencionPrenatal.Riesgo.EdadExtrema,
                            Gemelaridad = paciente.HistorialMedico.AtencionPrenatal.Riesgo.Gemelaridad,
                            Malnutricion = paciente.HistorialMedico.AtencionPrenatal.Riesgo.Malnutricion,
                            PreEclampsia = paciente.HistorialMedico.AtencionPrenatal.Riesgo.PreEclampsia,
                            HabitosToxicos = paciente.HistorialMedico.AtencionPrenatal.Riesgo.HabitosToxicos,
                            DiabetesMellitus = paciente.HistorialMedico.AtencionPrenatal.Riesgo.DiabetesMellitus,
                            InfeccionVaginal = paciente.HistorialMedico.AtencionPrenatal.Riesgo.InfeccionVaginal,
                            InfeccionUrinaria = paciente.HistorialMedico.AtencionPrenatal.Riesgo.InfeccionUrinaria,
                            HipertensionArterial = paciente.HistorialMedico.AtencionPrenatal.Riesgo.HipertensionArterial,
                            InfeccionTransmisionSexual = paciente.HistorialMedico.AtencionPrenatal.Riesgo.InfeccionTransmisionSexual,
                        },

                        Hemoglobina = paciente.HistorialMedico.AtencionPrenatal.Hemoglobina == null ? null : new Hemoglobina
                        {
                            PrimerTrimestre = paciente.HistorialMedico.AtencionPrenatal.Hemoglobina.PrimerTrimestre,
                            SegundoTrimestre = paciente.HistorialMedico.AtencionPrenatal.Hemoglobina.SegundoTrimestre,
                            TercerTrimestre = paciente.HistorialMedico.AtencionPrenatal.Hemoglobina.TercerTrimestre,
                        },

                        UltrasonidoGenetico = paciente.HistorialMedico.AtencionPrenatal.UltrasonidoGenetico == null ? null : new UltrasonidoGenetico
                        {
                            Semana13 = paciente.HistorialMedico.AtencionPrenatal.UltrasonidoGenetico.Semana13,
                            Semana22 = paciente.HistorialMedico.AtencionPrenatal.UltrasonidoGenetico.Semana22,
                            Semana26 = paciente.HistorialMedico.AtencionPrenatal.UltrasonidoGenetico.Semana26,
                        },
                    },

                    AtencionHospitalaria = paciente.HistorialMedico.AtencionHospitalaria == null ? null : new AtencionHospitalaria
                    {
                        PartoId = paciente.HistorialMedico.AtencionHospitalaria.PartoId,
                        MorbilidadPartoId = paciente.HistorialMedico.AtencionHospitalaria.MorbilidadPartoId,
                        UsoSulfatoMagnesio = paciente.HistorialMedico.AtencionHospitalaria.UsoSulfatoMagnesio,

                        Ocitocico = paciente.HistorialMedico.AtencionHospitalaria.Ocitocico == null ? null : new Ocitocico
                        {
                            Ocitocina = paciente.HistorialMedico.AtencionHospitalaria.Ocitocico.Ocitocina,
                            Ergonovina = paciente.HistorialMedico.AtencionHospitalaria.Ocitocico.Ergonovina,
                            Misoprostol = paciente.HistorialMedico.AtencionHospitalaria.Ocitocico.Misoprostol,
                            AcidoTranexamico = paciente.HistorialMedico.AtencionHospitalaria.Ocitocico.AcidoTranexamico,
                        },

                        Hemorragia = paciente.HistorialMedico.AtencionHospitalaria.Hemorragia == null ? null : new Hemorragia
                        {
                            Aborto = paciente.HistorialMedico.AtencionHospitalaria.Hemorragia.Aborto,
                            Posparto = paciente.HistorialMedico.AtencionHospitalaria.Hemorragia.Posparto,
                            SegundaMitad = paciente.HistorialMedico.AtencionHospitalaria.Hemorragia.SegundaMitad,
                        },

                        LugarIngreso = paciente.HistorialMedico.AtencionHospitalaria.LugarIngreso == null ? null : new LugarIngreso
                        {
                            CuidadosPerinatales = paciente.HistorialMedico.AtencionHospitalaria.LugarIngreso.CuidadosPerinatales,
                            UnidadCuidadosIntensivos = paciente.HistorialMedico.AtencionHospitalaria.LugarIngreso.UnidadCuidadosIntensivos,
                        },

                        CausaMorbilidad = paciente.HistorialMedico.AtencionHospitalaria.CausaMorbilidad == null ? null : new CausaMorbilidad
                        {
                            OtraCausa = paciente.HistorialMedico.AtencionHospitalaria.CausaMorbilidad.OtraCausa,
                            ComplicacionesAborto = paciente.HistorialMedico.AtencionHospitalaria.CausaMorbilidad.ComplicacionesAborto,
                            SepsisOrigenPulmonar = paciente.HistorialMedico.AtencionHospitalaria.CausaMorbilidad.SepsisOrigenPulmonar,
                            SepsisOrigenObstetrico = paciente.HistorialMedico.AtencionHospitalaria.CausaMorbilidad.SepsisOrigenObstetrico,
                            TrastornosHipertensivos = paciente.HistorialMedico.AtencionHospitalaria.CausaMorbilidad.TrastornosHipertensivos,
                            SepsisOrigenNoObstetrico = paciente.HistorialMedico.AtencionHospitalaria.CausaMorbilidad.SepsisOrigenNoObstetrico,
                            ComplicacionesHemorragicas = paciente.HistorialMedico.AtencionHospitalaria.CausaMorbilidad.ComplicacionesHemorragicas,
                            ComplicacionEnfermedadExistente = paciente.HistorialMedico.AtencionHospitalaria.CausaMorbilidad.ComplicacionEnfermedadExistente,
                        },

                        IntervencionQuirurgica = paciente.HistorialMedico.AtencionHospitalaria.IntervencionQuirurgica == null ? null : new IntervencionQuirurgica
                        {
                            HisterectomiaTotal = paciente.HistorialMedico.AtencionHospitalaria.IntervencionQuirurgica.HisterectomiaTotal,
                            SuturasCompresivas = paciente.HistorialMedico.AtencionHospitalaria.IntervencionQuirurgica.SuturasCompresivas,
                            HisterectomiaSubTotal = paciente.HistorialMedico.AtencionHospitalaria.IntervencionQuirurgica.HisterectomiaSubTotal,
                            SalpingectomiaTotalBilateral = paciente.HistorialMedico.AtencionHospitalaria.IntervencionQuirurgica.SalpingectomiaTotalBilateral,
                            LigadurasArterialesSelectivas = paciente.HistorialMedico.AtencionHospitalaria.IntervencionQuirurgica.LigadurasArterialesSelectivas,
                            SalpingectomiaTotalUnilateral = paciente.HistorialMedico.AtencionHospitalaria.IntervencionQuirurgica.SalpingectomiaTotalUnilateral,
                            LigadurasArteriasHipogastricas = paciente.HistorialMedico.AtencionHospitalaria.IntervencionQuirurgica.LigadurasArteriasHipogastricas,
                        },

                        CriterioMorbilidad = paciente.HistorialMedico.AtencionHospitalaria.CriterioMorbilidad == null ? null : new CriterioMorbilidad
                        {
                            Manejo = paciente.HistorialMedico.AtencionHospitalaria.CriterioMorbilidad.Manejo == null ? null : new Manejo
                            {
                                Cirugia = paciente.HistorialMedico.AtencionHospitalaria.CriterioMorbilidad.Manejo.Cirugia,
                                Transfusion = paciente.HistorialMedico.AtencionHospitalaria.CriterioMorbilidad.Manejo.Transfusion,
                            },

                            FallaOrganica = paciente.HistorialMedico.AtencionHospitalaria.CriterioMorbilidad.FallaOrganica == null ? null : new FallaOrganica
                            {
                                Renal = paciente.HistorialMedico.AtencionHospitalaria.CriterioMorbilidad.FallaOrganica.Renal,
                                Cardiaca = paciente.HistorialMedico.AtencionHospitalaria.CriterioMorbilidad.FallaOrganica.Cardiaca,
                                Cerebral = paciente.HistorialMedico.AtencionHospitalaria.CriterioMorbilidad.FallaOrganica.Cerebral,
                                Vascular = paciente.HistorialMedico.AtencionHospitalaria.CriterioMorbilidad.FallaOrganica.Vascular,
                                Hepatica = paciente.HistorialMedico.AtencionHospitalaria.CriterioMorbilidad.FallaOrganica.Hepatica,
                                Metabolica = paciente.HistorialMedico.AtencionHospitalaria.CriterioMorbilidad.FallaOrganica.Metabolica,
                                Coagulacion = paciente.HistorialMedico.AtencionHospitalaria.CriterioMorbilidad.FallaOrganica.Coagulacion,
                                Respiratoria = paciente.HistorialMedico.AtencionHospitalaria.CriterioMorbilidad.FallaOrganica.Respiratoria,
                            },

                            EnfermedadEspecifica = paciente.HistorialMedico.AtencionHospitalaria.CriterioMorbilidad.EnfermedadEspecifica == null ? null : new EnfermedadEspecifica
                            {
                                Eclampsia = paciente.HistorialMedico.AtencionHospitalaria.CriterioMorbilidad.EnfermedadEspecifica.Eclampsia,
                                ShockSeptico = paciente.HistorialMedico.AtencionHospitalaria.CriterioMorbilidad.EnfermedadEspecifica.ShockSeptico,
                                ShockHipovolemico = paciente.HistorialMedico.AtencionHospitalaria.CriterioMorbilidad.EnfermedadEspecifica.ShockHipovolemico,
                            },
                        },
                    },

                    Egreso = paciente.HistorialMedico.Egreso == null ? null : new Egreso
                    {
                        Fecha = paciente.HistorialMedico.Egreso.Fecha,
                        Fallecida = paciente.HistorialMedico.Egreso.Fallecida,
                        CausaMuerteDirectaId = paciente.HistorialMedico.Egreso.CausaMuerteDirectaId,
                        CausaMuerteIndirectaId = paciente.HistorialMedico.Egreso.CausaMuerteIndirectaId,

                        RecienNacido = paciente.HistorialMedico.Egreso.RecienNacido == null ? null : new RecienNacido
                        {
                            Peso = paciente.HistorialMedico.Egreso.RecienNacido.Peso,
                            Peso2 = paciente.HistorialMedico.Egreso.RecienNacido.Peso2,
                            Apgar = paciente.HistorialMedico.Egreso.RecienNacido.Apgar,
                            Apgar2 = paciente.HistorialMedico.Egreso.RecienNacido.Apgar2,
                            Fallecido = paciente.HistorialMedico.Egreso.RecienNacido.Fallecido,
                            Multiplicidad = paciente.HistorialMedico.Egreso.RecienNacido.Multiplicidad,
                        },
                    },
                },
            };

        PacienteApiModel NewPacienteApiModel(Paciente paciente) =>
            new PacienteApiModel
            {
                Id = paciente.Id,
                Edad = paciente.Edad,
                Nombre = paciente.Nombre,
                HospitalId = paciente.HospitalId,
                OcupacionId = paciente.OcupacionId,
                FechaIngreso = paciente.FechaIngreso,
                EscolaridadId = paciente.EscolaridadId,
                //HistoriaClinica = paciente.HistoriaClinica,
                Traslado1HospitalId = paciente.Traslado1HospitalId,
                Traslado2HospitalId = paciente.Traslado2HospitalId,

                Direccion = paciente.Direccion == null ? null : new DireccionApiModel
                {
                    AreaId = paciente.Direccion.AreaId,
                    MunicipioId = paciente.Direccion.MunicipioId,
                },

                HistorialMedico = paciente.HistorialMedico == null ? null : new HistorialMedicoApiModel
                {
                    AntecedenteGinecoObstetrico = paciente.HistorialMedico.AntecedenteGinecoObstetrico == null ? null : new AntecedenteGinecoObstetricoApiModel
                    {
                        Vivos = paciente.HistorialMedico.AntecedenteGinecoObstetrico.Vivos,
                        Molas = paciente.HistorialMedico.AntecedenteGinecoObstetrico.Molas,
                        Muertos = paciente.HistorialMedico.AntecedenteGinecoObstetrico.Muertos,
                        Abortos = paciente.HistorialMedico.AntecedenteGinecoObstetrico.Abortos,
                        Cesarias = paciente.HistorialMedico.AntecedenteGinecoObstetrico.Cesarias,
                        Ectopicos = paciente.HistorialMedico.AntecedenteGinecoObstetrico.Ectopicos,
                        Gestaciones = paciente.HistorialMedico.AntecedenteGinecoObstetrico.Gestaciones,
                        PartosVaginales = paciente.HistorialMedico.AntecedenteGinecoObstetrico.PartosVaginales,
                        UltimaGestacion = paciente.HistorialMedico.AntecedenteGinecoObstetrico.UltimaGestacion,
                    },
                    AtencionPrenatal = paciente.HistorialMedico.AtencionPrenatal == null ? null : new AtencionPrenatalApiModel
                    {
                        Reevaluacion = paciente.HistorialMedico.AtencionPrenatal.Reevaluacion,
                        SemanasCaptacion = paciente.HistorialMedico.AtencionPrenatal.SemanasCaptacion,
                        EvaluadoComoRiesgo = paciente.HistorialMedico.AtencionPrenatal.EvaluadoComoRiesgo,
                        ControlesPrenatales = paciente.HistorialMedico.AtencionPrenatal.ControlesPrenatales,
                        IndiceMasaCorporalId = paciente.HistorialMedico.AtencionPrenatal.IndiceMasaCorporalId,
                        
                        Orina = paciente.HistorialMedico.AtencionPrenatal.Orina == null ? null : new OrinaApiModel
                        {
                            PrimerTrimestre = paciente.HistorialMedico.AtencionPrenatal.Orina.PrimerTrimestre,
                            SegundoTrimestre = paciente.HistorialMedico.AtencionPrenatal.Orina.SegundoTrimestre,
                            TercerTrimestre = paciente.HistorialMedico.AtencionPrenatal.Orina.TercerTrimestre,
                        },

                        Riesgo = paciente.HistorialMedico.AtencionPrenatal.Riesgo == null ? null : new RiesgoApiModel
                        {
                            Asma = paciente.HistorialMedico.AtencionPrenatal.Riesgo.Asma,
                            Otros = paciente.HistorialMedico.AtencionPrenatal.Riesgo.Otros,
                            Anemia = paciente.HistorialMedico.AtencionPrenatal.Riesgo.Anemia,
                            Eclampsia = paciente.HistorialMedico.AtencionPrenatal.Riesgo.Eclampsia,
                            EdadExtrema = paciente.HistorialMedico.AtencionPrenatal.Riesgo.EdadExtrema,
                            Gemelaridad = paciente.HistorialMedico.AtencionPrenatal.Riesgo.Gemelaridad,
                            Malnutricion = paciente.HistorialMedico.AtencionPrenatal.Riesgo.Malnutricion,
                            PreEclampsia = paciente.HistorialMedico.AtencionPrenatal.Riesgo.PreEclampsia,
                            HabitosToxicos = paciente.HistorialMedico.AtencionPrenatal.Riesgo.HabitosToxicos,
                            DiabetesMellitus = paciente.HistorialMedico.AtencionPrenatal.Riesgo.DiabetesMellitus,
                            InfeccionVaginal = paciente.HistorialMedico.AtencionPrenatal.Riesgo.InfeccionVaginal,
                            InfeccionUrinaria = paciente.HistorialMedico.AtencionPrenatal.Riesgo.InfeccionUrinaria,
                            HipertensionArterial = paciente.HistorialMedico.AtencionPrenatal.Riesgo.HipertensionArterial,
                            InfeccionTransmisionSexual = paciente.HistorialMedico.AtencionPrenatal.Riesgo.InfeccionTransmisionSexual,
                        },

                        Hemoglobina = paciente.HistorialMedico.AtencionPrenatal.Hemoglobina == null ? null : new HemoglobinaApiModel
                        {
                            PrimerTrimestre = paciente.HistorialMedico.AtencionPrenatal.Hemoglobina.PrimerTrimestre,
                            SegundoTrimestre = paciente.HistorialMedico.AtencionPrenatal.Hemoglobina.SegundoTrimestre,
                            TercerTrimestre = paciente.HistorialMedico.AtencionPrenatal.Hemoglobina.TercerTrimestre,
                        },

                        UltrasonidoGenetico = paciente.HistorialMedico.AtencionPrenatal.UltrasonidoGenetico == null ? null : new UltrasonidoGeneticoApiModel
                        {
                            Semana13 = paciente.HistorialMedico.AtencionPrenatal.UltrasonidoGenetico.Semana13,
                            Semana22 = paciente.HistorialMedico.AtencionPrenatal.UltrasonidoGenetico.Semana22,
                            Semana26 = paciente.HistorialMedico.AtencionPrenatal.UltrasonidoGenetico.Semana26,
                        },
                    },

                    AtencionHospitalaria = paciente.HistorialMedico.AtencionHospitalaria == null ? null : new AtencionHospitalariaApiModel
                    {
                        PartoId = paciente.HistorialMedico.AtencionHospitalaria.PartoId,
                        MorbilidadPartoId = paciente.HistorialMedico.AtencionHospitalaria.MorbilidadPartoId,
                        UsoSulfatoMagnesio = paciente.HistorialMedico.AtencionHospitalaria.UsoSulfatoMagnesio,

                        Ocitocico = paciente.HistorialMedico.AtencionHospitalaria.Ocitocico == null ? null : new OcitocicoApiModel
                        {
                            Ocitocina = paciente.HistorialMedico.AtencionHospitalaria.Ocitocico.Ocitocina,
                            Ergonovina = paciente.HistorialMedico.AtencionHospitalaria.Ocitocico.Ergonovina,
                            Misoprostol = paciente.HistorialMedico.AtencionHospitalaria.Ocitocico.Misoprostol,
                            AcidoTranexamico = paciente.HistorialMedico.AtencionHospitalaria.Ocitocico.AcidoTranexamico,
                        },

                        Hemorragia = paciente.HistorialMedico.AtencionHospitalaria.Hemorragia == null ? null : new HemorragiaApiModel
                        {
                            Aborto = paciente.HistorialMedico.AtencionHospitalaria.Hemorragia.Aborto,
                            Posparto = paciente.HistorialMedico.AtencionHospitalaria.Hemorragia.Posparto,
                            SegundaMitad = paciente.HistorialMedico.AtencionHospitalaria.Hemorragia.SegundaMitad,
                        },

                        LugarIngreso = paciente.HistorialMedico.AtencionHospitalaria.LugarIngreso == null ? null : new LugarIngresoApiModel
                        {
                            CuidadosPerinatales = paciente.HistorialMedico.AtencionHospitalaria.LugarIngreso.CuidadosPerinatales,
                            UnidadCuidadosIntensivos = paciente.HistorialMedico.AtencionHospitalaria.LugarIngreso.UnidadCuidadosIntensivos,
                        },

                        CausaMorbilidad = paciente.HistorialMedico.AtencionHospitalaria.CausaMorbilidad == null ? null : new CausaMorbilidadApiModel
                        {
                            OtraCausa = paciente.HistorialMedico.AtencionHospitalaria.CausaMorbilidad.OtraCausa,
                            ComplicacionesAborto = paciente.HistorialMedico.AtencionHospitalaria.CausaMorbilidad.ComplicacionesAborto,
                            SepsisOrigenPulmonar = paciente.HistorialMedico.AtencionHospitalaria.CausaMorbilidad.SepsisOrigenPulmonar,
                            SepsisOrigenObstetrico = paciente.HistorialMedico.AtencionHospitalaria.CausaMorbilidad.SepsisOrigenObstetrico,
                            TrastornosHipertensivos = paciente.HistorialMedico.AtencionHospitalaria.CausaMorbilidad.TrastornosHipertensivos,
                            SepsisOrigenNoObstetrico = paciente.HistorialMedico.AtencionHospitalaria.CausaMorbilidad.SepsisOrigenNoObstetrico,
                            ComplicacionesHemorragicas = paciente.HistorialMedico.AtencionHospitalaria.CausaMorbilidad.ComplicacionesHemorragicas,
                            ComplicacionEnfermedadExistente = paciente.HistorialMedico.AtencionHospitalaria.CausaMorbilidad.ComplicacionEnfermedadExistente,
                        },

                        IntervencionQuirurgica = paciente.HistorialMedico.AtencionHospitalaria.IntervencionQuirurgica == null ? null : new IntervencionQuirurgicaApiModel
                        {
                            HisterectomiaTotal = paciente.HistorialMedico.AtencionHospitalaria.IntervencionQuirurgica.HisterectomiaTotal,
                            SuturasCompresivas = paciente.HistorialMedico.AtencionHospitalaria.IntervencionQuirurgica.SuturasCompresivas,
                            HisterectomiaSubTotal = paciente.HistorialMedico.AtencionHospitalaria.IntervencionQuirurgica.HisterectomiaSubTotal,
                            SalpingectomiaTotalBilateral = paciente.HistorialMedico.AtencionHospitalaria.IntervencionQuirurgica.SalpingectomiaTotalBilateral,
                            LigadurasArterialesSelectivas = paciente.HistorialMedico.AtencionHospitalaria.IntervencionQuirurgica.LigadurasArterialesSelectivas,
                            SalpingectomiaTotalUnilateral = paciente.HistorialMedico.AtencionHospitalaria.IntervencionQuirurgica.SalpingectomiaTotalUnilateral,
                            LigadurasArteriasHipogastricas = paciente.HistorialMedico.AtencionHospitalaria.IntervencionQuirurgica.LigadurasArteriasHipogastricas,
                        },

                        CriterioMorbilidad = paciente.HistorialMedico.AtencionHospitalaria.CriterioMorbilidad == null ? null : new CriterioMorbilidadApiModel
                        {
                            Manejo = paciente.HistorialMedico.AtencionHospitalaria.CriterioMorbilidad.Manejo == null ? null : new ManejoApiModel
                            {
                                Cirugia = paciente.HistorialMedico.AtencionHospitalaria.CriterioMorbilidad.Manejo.Cirugia,
                                Transfusion = paciente.HistorialMedico.AtencionHospitalaria.CriterioMorbilidad.Manejo.Transfusion,
                            },

                            FallaOrganica = paciente.HistorialMedico.AtencionHospitalaria.CriterioMorbilidad.FallaOrganica == null ? null : new FallaOrganicaApiModel
                            {
                                Renal = paciente.HistorialMedico.AtencionHospitalaria.CriterioMorbilidad.FallaOrganica.Renal,
                                Cardiaca = paciente.HistorialMedico.AtencionHospitalaria.CriterioMorbilidad.FallaOrganica.Cardiaca,
                                Cerebral = paciente.HistorialMedico.AtencionHospitalaria.CriterioMorbilidad.FallaOrganica.Cerebral,
                                Vascular = paciente.HistorialMedico.AtencionHospitalaria.CriterioMorbilidad.FallaOrganica.Vascular,
                                Hepatica = paciente.HistorialMedico.AtencionHospitalaria.CriterioMorbilidad.FallaOrganica.Hepatica,
                                Metabolica = paciente.HistorialMedico.AtencionHospitalaria.CriterioMorbilidad.FallaOrganica.Metabolica,
                                Coagulacion = paciente.HistorialMedico.AtencionHospitalaria.CriterioMorbilidad.FallaOrganica.Coagulacion,
                                Respiratoria = paciente.HistorialMedico.AtencionHospitalaria.CriterioMorbilidad.FallaOrganica.Respiratoria,
                            },

                            EnfermedadEspecifica = paciente.HistorialMedico.AtencionHospitalaria.CriterioMorbilidad.EnfermedadEspecifica == null ? null : new EnfermedadEspecificaApiModel
                            {
                                Eclampsia = paciente.HistorialMedico.AtencionHospitalaria.CriterioMorbilidad.EnfermedadEspecifica.Eclampsia,
                                ShockSeptico = paciente.HistorialMedico.AtencionHospitalaria.CriterioMorbilidad.EnfermedadEspecifica.ShockSeptico,
                                ShockHipovolemico = paciente.HistorialMedico.AtencionHospitalaria.CriterioMorbilidad.EnfermedadEspecifica.ShockHipovolemico,
                            },
                        },
                    },

                    Egreso = paciente.HistorialMedico.Egreso == null ? null : new EgresoApiModel
                    {
                        Fecha = paciente.HistorialMedico.Egreso.Fecha,
                        Fallecida = paciente.HistorialMedico.Egreso.Fallecida,
                        CausaMuerteDirectaId = paciente.HistorialMedico.Egreso.CausaMuerteDirectaId,
                        CausaMuerteIndirectaId = paciente.HistorialMedico.Egreso.CausaMuerteIndirectaId,

                        RecienNacido = paciente.HistorialMedico.Egreso.RecienNacido == null ? null : new RecienNacidoApiModel
                        {
                            Peso = paciente.HistorialMedico.Egreso.RecienNacido.Peso,
                            Peso2 = paciente.HistorialMedico.Egreso.RecienNacido.Peso2,
                            Apgar = paciente.HistorialMedico.Egreso.RecienNacido.Apgar,
                            Apgar2 = paciente.HistorialMedico.Egreso.RecienNacido.Apgar2,
                            Fallecido = paciente.HistorialMedico.Egreso.RecienNacido.Fallecido,
                            Multiplicidad = paciente.HistorialMedico.Egreso.RecienNacido.Multiplicidad,
                        },
                    },
                },
            };

        async Task<Paciente> GetPaciente(int id)
            => await HaloDbContext.Pacientes

                //.Include(p => p.Ocupacion)
                //.Include(p => p.Escolaridad)

                .Include(p => p.Direccion)
                    //.ThenInclude(p => p.Area)
                //.Include(p => p.Direccion)
                    //.ThenInclude(p => p.Municipio)
                        //.ThenInclude(p => p.Provincia)

                .Include(p => p.Hospital)
                    //.ThenInclude(p => p.Provincia)

                .Include(p => p.HistorialMedico)
                    .ThenInclude(p => p.AntecedenteGinecoObstetrico)

                .Include(p => p.HistorialMedico)
                    .ThenInclude(p => p.AtencionPrenatal)
                        .ThenInclude(p => p.Orina)
                .Include(p => p.HistorialMedico)
                    .ThenInclude(p => p.AtencionPrenatal)
                        .ThenInclude(p => p.Riesgo)
                .Include(p => p.HistorialMedico)
                    .ThenInclude(p => p.AtencionPrenatal)
                        .ThenInclude(p => p.Hemoglobina)
                //.Include(p => p.HistorialMedico)
                //    .ThenInclude(p => p.AtencionPrenatal)
                //        .ThenInclude(p => p.IndiceMasaCorporal)
                .Include(p => p.HistorialMedico)
                    .ThenInclude(p => p.AtencionPrenatal)
                        .ThenInclude(p => p.UltrasonidoGenetico)

                .Include(p => p.HistorialMedico)
                    .ThenInclude(p => p.Egreso)
                        .ThenInclude(p => p.RecienNacido)
                //.Include(p => p.HistorialMedico)
                //    .ThenInclude(p => p.Egreso)
                //        .ThenInclude(p => p.CausaMuerteDirecta)
                //.Include(p => p.HistorialMedico)
                //    .ThenInclude(p => p.Egreso)
                //        .ThenInclude(p => p.CausaMuerteIndirecta)

                //.Include(p => p.HistorialMedico)
                //    .ThenInclude(p => p.AtencionHospitalaria)
                //        .ThenInclude(p => p.Parto)
                .Include(p => p.HistorialMedico)
                    .ThenInclude(p => p.AtencionHospitalaria)
                        .ThenInclude(p => p.Ocitocico)
                .Include(p => p.HistorialMedico)
                    .ThenInclude(p => p.AtencionHospitalaria)
                        .ThenInclude(p => p.Hemorragia)
                .Include(p => p.HistorialMedico)
                    .ThenInclude(p => p.AtencionHospitalaria)
                .Include(p => p.HistorialMedico)
                    .ThenInclude(p => p.AtencionHospitalaria)
                        .ThenInclude(p => p.LugarIngreso)
                //.Include(p => p.HistorialMedico)
                //    .ThenInclude(p => p.AtencionHospitalaria)
                //        .ThenInclude(p => p.MorbilidadParto)
                .Include(p => p.HistorialMedico)
                    .ThenInclude(p => p.AtencionHospitalaria)
                        .ThenInclude(p => p.CausaMorbilidad)
                .Include(p => p.HistorialMedico)
                    .ThenInclude(p => p.AtencionHospitalaria)
                        .ThenInclude(p => p.IntervencionQuirurgica)

                .Include(p => p.HistorialMedico)
                    .ThenInclude(p => p.AtencionHospitalaria)
                        .ThenInclude(p => p.CriterioMorbilidad)
                            .ThenInclude(p => p.Manejo)
                .Include(p => p.HistorialMedico)
                    .ThenInclude(p => p.AtencionHospitalaria)
                        .ThenInclude(p => p.CriterioMorbilidad)
                            .ThenInclude(p => p.FallaOrganica)
                .Include(p => p.HistorialMedico)
                    .ThenInclude(p => p.AtencionHospitalaria)
                        .ThenInclude(p => p.CriterioMorbilidad)
                            .ThenInclude(p => p.EnfermedadEspecifica)

                //.FindAsync(model.Id);
                .SingleOrDefaultAsync(p => p.Id == id);

        //public async Task<IEnumerable<Paciente>> List(PacienteListApiModel model)
        //    => await Query(HaloDbContext.Pacientes

        //        .Include(p => p.Ocupacion)
        //        .Include(p => p.Escolaridad)

        //        //.Include(p => p.Traslados)
        //        //    .ThenInclude(p => p.Hospital)
        //        //        .ThenInclude(p => p.Municipio)
        //        //            .ThenInclude(p => p.Provincia)

        //        .Include(p => p.Direccion)
        //            .ThenInclude(p => p.Area)
        //        .Include(p => p.Direccion)
        //            .ThenInclude(p => p.Municipio)
        //                .ThenInclude(p => p.Provincia)

        //        .Include(p => p.Hospital)
        //            .ThenInclude(p => p.Provincia)
        //                //.ThenInclude(p => p.Provincia)

        //        .Include(p => p.HistorialMedico)
        //            .ThenInclude(p => p.AntecedenteGinecoObstetrico)

        //        .Include(p => p.HistorialMedico)
        //            .ThenInclude(p => p.AtencionPrenatal)
        //                .ThenInclude(p => p.Orina)
        //        .Include(p => p.HistorialMedico)
        //            .ThenInclude(p => p.AtencionPrenatal)
        //                .ThenInclude(p => p.Riesgo)
        //        .Include(p => p.HistorialMedico)
        //            .ThenInclude(p => p.AtencionPrenatal)
        //                .ThenInclude(p => p.Hemoglobina)
        //        .Include(p => p.HistorialMedico)
        //            .ThenInclude(p => p.AtencionPrenatal)
        //                .ThenInclude(p => p.IndiceMasaCorporal)
        //        .Include(p => p.HistorialMedico)
        //            .ThenInclude(p => p.AtencionPrenatal)
        //                .ThenInclude(p => p.UltrasonidoGenetico)

        //        .Include(p => p.HistorialMedico)
        //            .ThenInclude(p => p.Egreso)
        //                .ThenInclude(p => p.RecienNacido)
        //        .Include(p => p.HistorialMedico)
        //            .ThenInclude(p => p.Egreso)
        //                .ThenInclude(p => p.CausaMuerteDirecta)
        //        .Include(p => p.HistorialMedico)
        //            .ThenInclude(p => p.Egreso)
        //                .ThenInclude(p => p.CausaMuerteIndirecta)

        //        .Include(p => p.HistorialMedico)
        //            .ThenInclude(p => p.AtencionHospitalaria)
        //                .ThenInclude(p => p.Parto)
        //        .Include(p => p.HistorialMedico)
        //            .ThenInclude(p => p.AtencionHospitalaria)
        //                .ThenInclude(p => p.Ocitocico)
        //        .Include(p => p.HistorialMedico)
        //            .ThenInclude(p => p.AtencionHospitalaria)
        //                .ThenInclude(p => p.Hemorragia)
        //        //.Include(p => p.HistorialMedico)
        //        //    .ThenInclude(p => p.AtencionHospitalaria)
        //        //        .ThenInclude(p => p.Seguimiento)
        //        .Include(p => p.HistorialMedico)
        //            .ThenInclude(p => p.AtencionHospitalaria)
        //                .ThenInclude(p => p.LugarIngreso)
        //        .Include(p => p.HistorialMedico)
        //            .ThenInclude(p => p.AtencionHospitalaria)
        //                .ThenInclude(p => p.MorbilidadParto)
        //        .Include(p => p.HistorialMedico)
        //            .ThenInclude(p => p.AtencionHospitalaria)
        //                .ThenInclude(p => p.CausaMorbilidad)
        //        .Include(p => p.HistorialMedico)
        //            .ThenInclude(p => p.AtencionHospitalaria)
        //                .ThenInclude(p => p.IntervencionQuirurgica)

        //        .Include(p => p.HistorialMedico)
        //            .ThenInclude(p => p.AtencionHospitalaria)
        //                .ThenInclude(p => p.CriterioMorbilidad)
        //                    .ThenInclude(p => p.Manejo)
        //        .Include(p => p.HistorialMedico)
        //            .ThenInclude(p => p.AtencionHospitalaria)
        //                .ThenInclude(p => p.CriterioMorbilidad)
        //                    .ThenInclude(p => p.FallaOrganica)
        //        .Include(p => p.HistorialMedico)
        //            .ThenInclude(p => p.AtencionHospitalaria)
        //                .ThenInclude(p => p.CriterioMorbilidad)
        //                    .ThenInclude(p => p.EnfermedadEspecifica)

        //        .OrderBy(p => p.Id), model).ToListAsync();

        public async Task<IEnumerable<PacienteApiModel>> List(PacienteListApiModel model)
        {
            var pacientes = await Query(HaloDbContext.Pacientes
                .Include(p => p.Direccion)
                .Include(p => p.Hospital)
                .Include(p => p.HistorialMedico)
                    .ThenInclude(p => p.AntecedenteGinecoObstetrico)
                .Include(p => p.HistorialMedico)
                    .ThenInclude(p => p.AtencionPrenatal)
                        .ThenInclude(p => p.Orina)
                .Include(p => p.HistorialMedico)
                    .ThenInclude(p => p.AtencionPrenatal)
                        .ThenInclude(p => p.Riesgo)
                .Include(p => p.HistorialMedico)
                    .ThenInclude(p => p.AtencionPrenatal)
                        .ThenInclude(p => p.Hemoglobina)
                .Include(p => p.HistorialMedico)
                    .ThenInclude(p => p.AtencionPrenatal)
                        .ThenInclude(p => p.UltrasonidoGenetico)
                .Include(p => p.HistorialMedico)
                    .ThenInclude(p => p.Egreso)
                        .ThenInclude(p => p.RecienNacido)
                .Include(p => p.HistorialMedico)
                    .ThenInclude(p => p.AtencionHospitalaria)
                        .ThenInclude(p => p.Ocitocico)
                .Include(p => p.HistorialMedico)
                    .ThenInclude(p => p.AtencionHospitalaria)
                        .ThenInclude(p => p.Hemorragia)
                .Include(p => p.HistorialMedico)
                    .ThenInclude(p => p.AtencionHospitalaria)
                .Include(p => p.HistorialMedico)
                    .ThenInclude(p => p.AtencionHospitalaria)
                        .ThenInclude(p => p.LugarIngreso)
                .Include(p => p.HistorialMedico)
                    .ThenInclude(p => p.AtencionHospitalaria)
                        .ThenInclude(p => p.CausaMorbilidad)
                .Include(p => p.HistorialMedico)
                    .ThenInclude(p => p.AtencionHospitalaria)
                        .ThenInclude(p => p.IntervencionQuirurgica)
                .Include(p => p.HistorialMedico)
                    .ThenInclude(p => p.AtencionHospitalaria)
                        .ThenInclude(p => p.CriterioMorbilidad)
                            .ThenInclude(p => p.Manejo)
                .Include(p => p.HistorialMedico)
                    .ThenInclude(p => p.AtencionHospitalaria)
                        .ThenInclude(p => p.CriterioMorbilidad)
                            .ThenInclude(p => p.FallaOrganica)
                .Include(p => p.HistorialMedico)
                    .ThenInclude(p => p.AtencionHospitalaria)
                        .ThenInclude(p => p.CriterioMorbilidad)
                            .ThenInclude(p => p.EnfermedadEspecifica)

                .OrderBy(p => p.Id), model).ToListAsync();

            var entries = new List<PacienteApiModel>(pacientes.Count);

            foreach (var paciente in pacientes)
                entries.Add(NewPacienteApiModel(paciente));

            return entries;
        }

        public async Task<PacienteApiModel> Get(PacienteGetApiModel model)
            => NewPacienteApiModel(await GetPaciente(model.Id).ConfigureAwait(false));

        public async Task<IdApiModel> Add(PacienteAddApiModel model)
        {
            model.Id = 0;
            var paciente = HaloDbContext.Pacientes.Add(NewPaciente(model)).Entity;
            await HaloDbContext.SaveChangesAsync();
            return new IdApiModel { Id = paciente.Id };
        }

        public async Task<IdNombreApiModel> Del(PacienteDelApiModel model)
        {
            var paciente = HaloDbContext.Remove(new Paciente { Id = model.Id }).Entity;
            await HaloDbContext.SaveChangesAsync();
            return new IdNombreApiModel { Id = paciente.Id, Nombre = paciente.Nombre };
        }

        public async Task<IdNombreApiModel> Upd(PacienteUpdApiModel model)
        {
            var paciente = HaloDbContext.Update(NewPaciente(model)).Entity;
            await HaloDbContext.SaveChangesAsync();
            return new IdNombreApiModel { Id = paciente.Id, Nombre = paciente.Nombre };
        }
        */
    }
}
/* { Halo.Server } - Sistema Nacional de Vigilancia a la Morbilidad Materna Extremadamente Grave */
