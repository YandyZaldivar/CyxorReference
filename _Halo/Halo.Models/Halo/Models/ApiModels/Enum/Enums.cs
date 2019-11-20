/*
  { Halo.Server } - Sistema Nacional de Vigilancia a la Morbilidad Materna Extremadamente Grave
  Copyright (C) 2017 Halo
  Authors:  Mayli Sanchez
            Yandy Zaldivar
*/

using System;
using System.ComponentModel.DataAnnotations;

namespace Halo.Models
{
    public enum TipoArea
    {
        Rural,
        Urbana,
    }

    public enum TipoOcupacion
    {
        Estudiante,
        Trabajadora,
        AmaDeCasa,
    }

    public enum TipoEscolaridad
    {
        Primaria,
        Secundaria,
        Preuniversitaria,
        Universitaria,
    }

    [Flags]
    public enum CondicionesIdentificadas
    {
        No = 0,
        EdadExtrema = 1,
        Asma = 2,
        DiabetesMellitus = 4,
        Anemia = 8,
        Malnutricion = 16,
        HTA = 32,
        Preeclampsia = 64,
        Prematuridad = 128,
        Gemelaridad = 256,
        InfeccionUrinaria = 512,
        InfeccionVaginal = 1024,
        ITS = 2048,
        HabitosToxicos = 4096,
        Otras = 8192,
    }

    [Flags]
    public enum LugarIngreso
    {
        CuidadosPerinatales = 1,
        UnidadCuidadosIntensivos = 2,
    }

    [Flags]
    public enum CausasMorbilidad
    {
        HtaPee = 1,
        HtaCronica = 2,
        ComplicacionesHemorragicas = 4,
        ComplicacionesAborto = 8,
        SepsisOrigenObstetrico = 16,
        SepsisOrigenNoObstetrico = 32,
        SepsisOrigenPulmonar = 64,
        ComplicacionEnfermedadExistente = 128,
        Otra = 256,
    }

    public enum CausaHemorragia
    {
        [Display(Name = "Aborto")]
        Aborto,
        [Display(Name = "Atonía")]
        Atonia,
        [Display(Name = "Trauma")]
        Trauma,
        [Display(Name = "Coagulopatía")]
        Coagulopatia,
        [Display(Name = "Placenta previa")]
        PlacentaPrevia,
        [Display(Name = "Retención de restos")]
        RetencionRestos,
        [Display(Name = "Hematoma retroplacent.")]
        HematomaRetroplacentario,
        [Display(Name = "Acretismo placentario")]
        AcretismoPlacentario,
        [Display(Name = "Embarazo ectópico")]
        EmbarazoEctópico,
        [Display(Name = "Otra")]
        Otra,
    }

    [Flags]
    public enum EnfermedadEspecifica
    {
        ShockSeptico = 1,
        ShockHipovolemico = 2,
        Eclampsia = 4,
    }

    [Flags]
    public enum FallaOrganica
    {
        Cerebral = 1,
        Cardiaca = 2,
        Hepatica = 4,
        Vascular = 8,
        Renal = 16,
        Coagulacion = 32,
        Metabolica = 64,
        Respiratoria = 128,
    }

    [Flags]
    public enum TipoManejo
    {
        Cirugia = 1,
        Transfusion = 2,
    }

    [Flags]
    public enum IntervencionQuirurgica
    {
        HisterectomiaTotal = 1,
        HisterectomiaSubTotal = 2,
        SalpingectomiaTotal = 4,
        SuturasCompresivas = 8,
        LigadurasArterialesSelectivas = 16,
        LigadurasArteriasHipogastricas = 32,
        Otra = 64,
    }

    [Flags]
    public enum UsoOcitocicos
    {
        Ocitocina = 1,
        Ergonovina = 2,
        Misoprostol = 4,
        AcidoTranexamico = 8,
    }

    public enum PeriodoHemorragia
    {
        PrimeraMitad = 1,
        SegundaMitad = 2,
        Posparto = 4,
    }

    public enum TipoParto
    {
        Eutocico,
        Distocico,
        Instrumentado,
    }

    public enum TipoMorbilidadParto
    {
        Antes,
        Durante,
        Despues,
    }

    public enum DopplerArteriaUterina
    {
        Positivo,
        Negativo,
        NoRealizado,
    }

    public enum TipoIndiceMasaCorporal
    {
        Obesa,
        SobrePeso,
        PesoAdecuado,
        PesoDeficiente,
    }

    public enum TipoCausaMuerteDirecta
    {
        [Display(Name = "Embarazo ectópico")]
        EmbarazoEctopico,

        [Display(Name = "Aborto complicado -Sepsis")]
        AbortoComplicadoSepsis,
        [Display(Name = "Aborto complicado -Hemorragia")]
        AbortoComplicadoHemorragia,

        [Display(Name = "Mola hidatiforme")]
        MolaHidatiforme,

        [Display(Name = "Hemorragia -Atonía uterina")]
        HemorragiaAtoniaUterina,
        [Display(Name = "Hemorragia -Placenta previa")]
        HemorragiaPlacentaPrevia,
        [Display(Name = "Hemorragia -Acretismo placentario")]
        HemorragiaAcretismoPlacentario,
        [Display(Name = "Hemorragia -Hematoma retroplacentario")]
        HemorragiaHematomaRetroplacentario,
        [Display(Name = "Hemorragia -Rotura uterina")]
        HemorragiaRoturaUterina,
        [Display(Name = "Hemorragia -Retención de restos placentarios")]
        HemorragiaRetencionRestosPlacentarios,

        [Display(Name = "Sepsis -Puerperal")]
        SepsisPuerperal,
        [Display(Name = "Sepsis -Ovular")]
        SepsisOvular,
        [Display(Name = "Sepsis -Corioamnionitis")]
        SepsisCorioamnionitis,
        [Display(Name = "Sepsis -Urinaria")]
        SepsisUrinaria,
        [Display(Name = "Sepsis -De herida quirúrgica")]
        SepsisHeridaQuirurgica,
        [Display(Name = "Sepsis -De la mama")]
        SepsisMama,

        [Display(Name = "Trastorno hipertensivo -Preeclampsia agravada")]
        TrastornoHipertensivoPreeclampsiaAgravada,
        [Display(Name = "Trastorno hipertensivo -Eclampsia")]
        TrastornoHipertensivoEclampsia,
        [Display(Name = "Trastorno hipertensivo -HTA crónica")]
        TrastornoHipertensivoHTACronica,
        [Display(Name = "Trastorno hipertensivo -HTA crónica + preeclampsia")]
        TrastornoHipertensivoHTACronicaPreeclampsia,

        [Display(Name = "Síndrome de HELLP")]
        SindromeHellp,

        [Display(Name = "Embolismo del líquido amniótico")]
        EmbolismoLiquidoAmniotico,

        [Display(Name = "Tromboembolismo pulmonar")]
        TromboembolismoPulmonar,

        [Display(Name = "Complicaciones anestésicas")]
        ComplicacionesAnestesicas,

        [Display(Name = "Hiperemesis gravídica")]
        HiperemesisGravidica,

        [Display(Name = "Trombosis venosa")]
        TrombosisVenosa,

        [Display(Name = "Coriocarnicoma")]
        Coriocarnicoma,

        [Display(Name = "Muerte de causa desconocida")]
        MuerteCausaDesconocida,
    }

    public enum TipoCausaMuerteIndirecta
    {
        [Display(Name = "Enf. del Sistema Circulatorio -Cardiaca reumática")]
        EnfermedadSistemaCirculatorioCardiacaReumatica,
        [Display(Name = "Enf. del Sistema Circulatorio -HTA")]
        EnfermedadSistemaCirculatorioHTA,
        [Display(Name = "Enf. del Sistema Circulatorio -Isquémica del corazón")]
        EnfermedadSistemaCirculatorioIsquemicaCorazon,
        [Display(Name = "Enf. del Sistema Circulatorio -Cerebro vascular")]
        EnfermedadSistemaCirculatorioCerebroVascular,
        [Display(Name = "Enf. del Sistema Circulatorio -Aneurisma")]
        EnfermedadSistemaCirculatorioAneurisma,

        [Display(Name = "Enf. del Sistema Respiratorio -Infección aguda de las vías respiratorias superiores")]
        EnfermedadSistemaRespiratorioViasRespiratoriasSuperiores,
        [Display(Name = "Enf. del Sistema Respiratorio -Influenza y Neumonía")]
        EnfermedadSistemaRespiratorio,
        [Display(Name = "Enf. del Sistema Respiratorio -Otras infecciones de las vías respiratorias inferiores")]
        EnfermedadSistemaRespiratorioViasRespiratoriasInferiores,

        [Display(Name = "Anemia")]
        Anemia,

        [Display(Name = "Defectos en la coagulación")]
        DefectosCoagulacion,

        [Display(Name = "Leucemia")]
        Leucemia,

        [Display(Name = "Enfermedades infecciosas y parasitarias")]
        EnfermedadesInfecciosasParasitarias,

        [Display(Name = "Otras")]
        Otras,
    }
}
/* { Halo.Server } */
