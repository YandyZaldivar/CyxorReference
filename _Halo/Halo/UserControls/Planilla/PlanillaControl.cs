using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace Halo
{
    using Models;

    using Cyxor.Models;
    using Cyxor.Networking;

    public partial class PlanillaControl : UserControl
    {
        public const decimal HemoglobinaNRCheckedConstant = -10;

        public static readonly string DateFormat = "dd/MM/yyyy";
        static readonly string EnabledDateFormat = DateFormat;
        static readonly string DisabledDateFormat = "__ /__ /____";

        static PlanillaControl()
        {

        }

        IdNombreApiModel[] InstitucionesArray;
        Dictionary<int, IdNombreApiModel> Instituciones;

        Dictionary<Control, Control> ControlFocus = new Dictionary<Control, Control>();

        List<RadioButton> AreaRadioButtonGroup;
        List<RadioButton> OcupacionRadioButtonGroup;
        List<RadioButton> EscolaridadRadioButtonGroup;

        List<RadioButton> RiesgoRadioButtonGroup;
        List<RadioButton> DopplerRadioButtonGroup;
        List<RadioButton> ReevaluacionRadioButtonGroup;
        List<RadioButton> ImcRadioButtonGroup;

        List<RadioButton> Urocultivo1TRadioButtonGroup;
        List<RadioButton> Urocultivo2TRadioButtonGroup;
        List<RadioButton> Urocultivo3TRadioButtonGroup;

        List<RadioButton> PartoRadioButtonGroup;
        List<RadioButton> HemorragiaRadioButtonGroup;
        List<RadioButton> DiagnosticoRadioButtonGroup;
        List<RadioButton> SulfatoMagnesioRadioButtonGroup;

        List<RadioButton> EgresoRadioButtonGroup;
        List<RadioButton> RecienNacidoRadioButtonGroup;

        List<List<RadioButton>> RadioButtonGroup;

        public Control FocusedControl { get; private set; }
        public bool RadioButtonCheckDisabled { get; private set; }

        public PacienteApiModel Paciente
        {
            get => GetPaciente();
            set => SetPaciente(value);
        }

        public PlanillaControl()
        {
            InitializeComponent();

            InitializePlanilla();
            InitializeControlFocus();
        }

        public void SelectFocusedControl()
        {
            if (FocusedControl is NudControl nudControl)
                nudControl.SetFocus();
            else
            {
                RadioButtonCheckDisabled = true;
                FocusedControl?.Focus();
                RadioButtonCheckDisabled = false;
            }
        }

        void InitializeControlFocus()
        {
            ControlFocus = new Dictionary<Control, Control>()
            {
                #region Datos Generales
                [NombreTextBox] = NombreLabel,
                [EdadNud] = EdadLabel,
                [FechaIngresoDateTimePicker] = FechaIngresoLabel,
                [ProvinciaComboBox] = ProvinciaLabel,
                [MunicipioComboBox] = MunicipioLabel,
                [RuralRadioButton] = AreaLabel,
                [UrbanaRadioButton] = AreaLabel,
                [AmaDeCasaRadioButton] = OcupacionLabel,
                [EstudianteRadioButton] = OcupacionLabel,
                [TrabajadoraRadioButton] = OcupacionLabel,
                [PrimariaRadioButton] = EscolaridadLabel,
                [SecundariaRadioButton] = EscolaridadLabel,
                [PreuniversitariaRadioButton] = EscolaridadLabel,
                [UniversitariaRadioButton] = EscolaridadLabel,
                [HospitalProvinciaComboBox] = HospitalProvinciaLabel,
                [HospitalComboBox] = HospitalLabel,
                [RemitidaCheckBox] = RemitidaCheckBox,
                [Institucion1ComboBox] = Institucion1Label,
                [Institucion2ComboBox] = Institucion2Label,
                #endregion

                #region Antecedentes Gineco-Obstétricos
                [PartosVaginalesNud] = PartosVaginalesLabel,
                [GestacionesNud] = GestacionesLabel,
                [EctopicosNud] = EctopicosLabel,
                [CesareasNud] = CesareasLabel,
                [AbortosNud] = AbortosLabel,
                [MuertosNud] = MuertosLabel,
                [VivosNud] = VivosLabel,
                [MolasNud] = MolasLabel,
                [FechaGestacionDateTimePicker] = FechaGestacionLabel,
                #endregion

                #region Atención Prenatal
                [SemanasCaptacionNud] = SemanasCaptacionLabel,
                [ControlesPrenatalesNud] = ControlesPrenatalesLabel,
                [EvaluadoRiesgoSiRadioButton] = EvaluadoRiesgoLabel,
                [EvaluadoRiesgoNoRadioButton] = EvaluadoRiesgoLabel,
                [ReevaluacionSiRadioButton] = ReevaluacionLabel,
                [ReevaluacionNoRadioButton] = ReevaluacionLabel,

                [HTACheckBox] = CondicionesGroupBox,
                [AsmaCheckBox] = CondicionesGroupBox,
                [PrematuridadCheckBox] = CondicionesGroupBox,
                [PreeclampsiaCheckBox] = CondicionesGroupBox,
                [EdadExtremaCheckBox] = CondicionesGroupBox,
                [DiabetesMellitusCheckBox] = CondicionesGroupBox,
                [HabitosToxicoxCheckBox] = CondicionesGroupBox,
                [ITSCheckBox] = CondicionesGroupBox,
                [AnemiaCheckBox] = CondicionesGroupBox,
                [GemelaridadCheckBox] = CondicionesGroupBox,
                [MalnutricionCheckBox] = CondicionesGroupBox,
                [InfeccionVaginalCheckBox] = CondicionesGroupBox,
                [InfeccionUrinariaCheckBox] = CondicionesGroupBox,
                [OtrasCondicionesCheckBox] = CondicionesGroupBox,
                [NoCondicionesCheckBox] = CondicionesGroupBox,

                [ObesaRadioButton] = IMCGroupBox,
                [SobrePesoRadioButton] = IMCGroupBox,
                [PesoAdecuadoRadioButton] = IMCGroupBox,
                [PesoDeficienteRadioButton] = IMCGroupBox,

                [Hemoglobina1TNud] = SeguimientoGroupBox,
                [Hemoglobina2TNud] = SeguimientoGroupBox,
                [Hemoglobina3TNud] = SeguimientoGroupBox,

                [HemoglobinaNR1TCheckBox] = SeguimientoGroupBox,
                [HemoglobinaNR2TCheckBox] = SeguimientoGroupBox,
                [HemoglobinaNR3TCheckBox] = SeguimientoGroupBox,

                [UrocultivoNegativo1TRadioButton] = SeguimientoGroupBox,
                [UrocultivoPositivo1TRadioButton] = SeguimientoGroupBox,
                [UrocultivoNR1TRadioButton] = SeguimientoGroupBox,

                [UrocultivoNegativo2TRadioButton] = SeguimientoGroupBox,
                [UrocultivoPositivo2TRadioButton] = SeguimientoGroupBox,
                [UrocultivoNR2TRadioButton] = SeguimientoGroupBox,

                [UrocultivoNegativo3TRadioButton] = SeguimientoGroupBox,
                [UrocultivoPositivo3TRadioButton] = SeguimientoGroupBox,
                [UrocultivoNR3TRadioButton] = SeguimientoGroupBox,
                #endregion

                #region Atención Hospitalaria
                [CuidadosPerinatalesCheckBox] = LugarIngresoLabel,
                [UCICheckBox] = LugarIngresoLabel,

                [EutocicoRadioButton] = TipoPartoLabel,
                [CesareaRadioButton] = TipoPartoLabel,
                [InstrumentadoRadioButton] = TipoPartoLabel,

                [DopplerPositivoRadioButton] = DopplerLabel,
                [DopplerNegativoRadioButton] = DopplerLabel,
                [DopplerNoRealizadoRadioButton] = DopplerLabel,

                [DiagnosticoAntesRadioButton] = DiagnosticoLabel,
                [DiagnosticoDuranteRadioButton] = DiagnosticoLabel,
                [DiagnosticoDespuesRadioButton] = DiagnosticoLabel,

                [HtaPeeCheckBox] = CausasMorbilidadGroupBox,
                [HtaCronicaCheckBox] = CausasMorbilidadGroupBox,
                [ComplicacionesHemorragicasCheckBox] = CausasMorbilidadGroupBox,
                [ComplicacionesAbortoCheckBox] = CausasMorbilidadGroupBox,
                [SepsisOrigenPulmonarCheckBox] = CausasMorbilidadGroupBox,
                [SepsisOrigenObstetricoCheckBox] = CausasMorbilidadGroupBox,
                [SepsisOrigenNoObstetricoCheckBox] = CausasMorbilidadGroupBox,
                [EnfermedadExistenteCheckBox] = CausasMorbilidadGroupBox,
                [OtraCausaMorbilidadCheckBox] = CausasMorbilidadGroupBox,
                [OtraCausaMorbilidadTextBox] = CausasMorbilidadGroupBox,

                [ShockSepticoCheckBox] = CriteriosMorbilidadGroupBox,
                [ShockHipovolemicoCheckBox] = CriteriosMorbilidadGroupBox,
                [EclampsiaCheckBox] = CriteriosMorbilidadGroupBox,

                [CerebralCheckBox] = CriteriosMorbilidadGroupBox,
                [CardiacaCheckBox] = CriteriosMorbilidadGroupBox,
                [HepaticaCheckBox] = CriteriosMorbilidadGroupBox,
                [VascularCheckBox] = CriteriosMorbilidadGroupBox,
                [RenalCheckBox] = CriteriosMorbilidadGroupBox,
                [CoagulacionCheckBox] = CriteriosMorbilidadGroupBox,
                [MetabolicaCheckBox] = CriteriosMorbilidadGroupBox,
                [RespiratoriaCheckBox] = CriteriosMorbilidadGroupBox,

                [CirugiaCheckBox] = CriteriosMorbilidadGroupBox,
                [TransfusionCheckBox] = CriteriosMorbilidadGroupBox,

                [NoCriteriosMorbilidadCheckBox] = CriteriosMorbilidadGroupBox,

                [HisterectomiaTotalCheckBox] = IntervencionQuirurgicaGroupBox,
                [HisterectomiaSubtotalCheckBox] = IntervencionQuirurgicaGroupBox,
                [SalpingectomiaTotalCheckBox] = IntervencionQuirurgicaGroupBox,
                [SuturasCompBLynchCheckBox] = IntervencionQuirurgicaGroupBox,
                [LigadurasArterialesSelectivasCheckBox] = IntervencionQuirurgicaGroupBox,
                [LigaduraArteriasHipogastricasCheckBox] = IntervencionQuirurgicaGroupBox,
                [OtraIntervencionQuirurgicaCheckBox] = IntervencionQuirurgicaGroupBox,
                [OtraIntervencionQuirurgicaTextBox] = IntervencionQuirurgicaGroupBox,
                [NoIntervencionQuirurgicaCheckBox] = IntervencionQuirurgicaGroupBox,

                [CausaHemorragiaComboBox] = HemorragiaLabel,
                [Hemorragia1raMitadRadioButton] = HemorragiaLabel,
                [Hemorragia2daMitadRadioButton] = HemorragiaLabel,
                [HemorragiaPospartoRadioButton] = HemorragiaLabel,

                [AcidoTranexamicoCheckBox] = OcitocicosLabel,
                [OcitocinaCheckBox] = OcitocicosLabel,
                [ErgonovinaCheckBox] = OcitocicosLabel,
                [MisoprostolCheckBox] = OcitocicosLabel,

                [SulfatoMagnesioSiRadioButton] = SulfatoMagnesioLabel,
                [SulfatoMagnesioNoRadioButton] = SulfatoMagnesioLabel,
                #endregion

                #region Egreso
                [EgresoVivaRadioButton] = EgresoVivaRadioButton,
                [EgresoFallecidaRadioButton] = EgresoFallecidaRadioButton,

                [FechaEgresoDateTimePicker] = FechaEgresoLabel,

                [RecienNacidoVivoRadioButton] = RecienNacidoLabel,
                [RecienNacidoFallecidoRadioButton] = RecienNacidoLabel,

                [EGNud] = EdadGestacionalLabel,

                [PesoNud] = PesoLabel,

                [Apgar1Nud] = ApgarLabel,
                [Apgar2Nud] = ApgarLabel,

                [CausaMuerteDirectaComboBox] = CausaMuerteDirectaGroupBox,
                [CausaMuerteIndirectaComboBox] = CausaMuerteIndirectaGroupBox,
                #endregion
            };

            foreach (var item in ControlFocus)
            {
                item.Key.Enter += ControlEnter;
                item.Key.Leave += ControlLeave;
            }

            void ControlEnter(object sender, EventArgs e) =>
                ControlFocus[sender as Control].ForeColor = Color.Blue;

            void ControlLeave(object sender, EventArgs e) =>
                ControlFocus[(sender as Control)].ForeColor = Color.Black;
        }

        void InitializePlanilla()
        {
            Instituciones = new Dictionary<int, IdNombreApiModel>(HospitalApiModel.Hospitales.Length);

            for (var i = 0; i < HospitalApiModel.Hospitales.Length; i++)
                Instituciones.Add(HospitalApiModel.Hospitales[i].Id, new IdNombreApiModel
                {
                    Id = HospitalApiModel.Hospitales[i].Id,
                    Nombre = ProvinciaApiModel.Provincias.Single(p => p.Id ==
                        HospitalApiModel.Hospitales[i].ProvinciaId).NombreCorto +
                        "- " + HospitalApiModel.Hospitales[i].Nombre,
                });

            InstitucionesArray = Instituciones.Values.ToArray();

            RuralRadioButton.Tag = (int)TipoArea.Rural;
            UrbanaRadioButton.Tag = (int)TipoArea.Urbana;

            AmaDeCasaRadioButton.Tag = (int)TipoOcupacion.AmaDeCasa;
            EstudianteRadioButton.Tag = (int)TipoOcupacion.Estudiante;
            TrabajadoraRadioButton.Tag = (int)TipoOcupacion.Trabajadora;

            PrimariaRadioButton.Tag = (int)TipoEscolaridad.Primaria;
            SecundariaRadioButton.Tag = (int)TipoEscolaridad.Secundaria;
            PreuniversitariaRadioButton.Tag = (int)TipoEscolaridad.Preuniversitaria;
            UniversitariaRadioButton.Tag = (int)TipoEscolaridad.Universitaria;

            ObesaRadioButton.Tag = (int)TipoIndiceMasaCorporal.Obesa;
            SobrePesoRadioButton.Tag = (int)TipoIndiceMasaCorporal.SobrePeso;
            PesoAdecuadoRadioButton.Tag = (int)TipoIndiceMasaCorporal.PesoAdecuado;
            PesoDeficienteRadioButton.Tag = (int)TipoIndiceMasaCorporal.PesoDeficiente;

            EutocicoRadioButton.Tag = (int)TipoParto.Eutocico;
            CesareaRadioButton.Tag = (int)TipoParto.Distocico;
            InstrumentadoRadioButton.Tag = (int)TipoParto.Instrumentado;

            DopplerPositivoRadioButton.Tag = (int)DopplerArteriaUterina.Positivo;
            DopplerNegativoRadioButton.Tag = (int)DopplerArteriaUterina.Negativo;
            DopplerNoRealizadoRadioButton.Tag = (int)DopplerArteriaUterina.NoRealizado;

            DiagnosticoAntesRadioButton.Tag = (int)TipoMorbilidadParto.Antes;
            DiagnosticoDuranteRadioButton.Tag = (int)TipoMorbilidadParto.Durante;
            DiagnosticoDespuesRadioButton.Tag = (int)TipoMorbilidadParto.Despues;

            Hemorragia1raMitadRadioButton.Tag = (int)PeriodoHemorragia.PrimeraMitad;
            Hemorragia2daMitadRadioButton.Tag = (int)PeriodoHemorragia.SegundaMitad;
            HemorragiaPospartoRadioButton.Tag = (int)PeriodoHemorragia.Posparto;

            AreaRadioButtonGroup = new List<RadioButton> { RuralRadioButton, UrbanaRadioButton };
            OcupacionRadioButtonGroup = new List<RadioButton> { EstudianteRadioButton, TrabajadoraRadioButton, AmaDeCasaRadioButton };
            EscolaridadRadioButtonGroup = new List<RadioButton> { PrimariaRadioButton, SecundariaRadioButton, PreuniversitariaRadioButton, UniversitariaRadioButton };

            RiesgoRadioButtonGroup = new List<RadioButton> { EvaluadoRiesgoSiRadioButton, EvaluadoRiesgoNoRadioButton };
            ReevaluacionRadioButtonGroup = new List<RadioButton> { ReevaluacionSiRadioButton, ReevaluacionNoRadioButton };
            DopplerRadioButtonGroup = new List<RadioButton> { DopplerPositivoRadioButton, DopplerNegativoRadioButton, DopplerNoRealizadoRadioButton };
            ImcRadioButtonGroup = new List<RadioButton> { ObesaRadioButton, SobrePesoRadioButton, PesoAdecuadoRadioButton, PesoDeficienteRadioButton };

            Urocultivo1TRadioButtonGroup = new List<RadioButton> { UrocultivoPositivo1TRadioButton, UrocultivoNegativo1TRadioButton, UrocultivoNR1TRadioButton };
            Urocultivo2TRadioButtonGroup = new List<RadioButton> { UrocultivoPositivo2TRadioButton, UrocultivoNegativo2TRadioButton, UrocultivoNR2TRadioButton };
            Urocultivo3TRadioButtonGroup = new List<RadioButton> { UrocultivoPositivo3TRadioButton, UrocultivoNegativo3TRadioButton, UrocultivoNR3TRadioButton };

            PartoRadioButtonGroup = new List<RadioButton> { EutocicoRadioButton, CesareaRadioButton, InstrumentadoRadioButton };
            HemorragiaRadioButtonGroup = new List<RadioButton> { Hemorragia1raMitadRadioButton, Hemorragia2daMitadRadioButton, HemorragiaPospartoRadioButton };
            DiagnosticoRadioButtonGroup = new List<RadioButton> { DiagnosticoAntesRadioButton, DiagnosticoDuranteRadioButton, DiagnosticoDespuesRadioButton };
            SulfatoMagnesioRadioButtonGroup = new List<RadioButton> { SulfatoMagnesioSiRadioButton, SulfatoMagnesioNoRadioButton };

            EgresoRadioButtonGroup = new List<RadioButton> { EgresoVivaRadioButton, EgresoFallecidaRadioButton };
            RecienNacidoRadioButtonGroup = new List<RadioButton> { RecienNacidoVivoRadioButton, RecienNacidoFallecidoRadioButton };

            RadioButtonGroup = new List<List<RadioButton>>
            {
                AreaRadioButtonGroup, OcupacionRadioButtonGroup, EscolaridadRadioButtonGroup,

                RiesgoRadioButtonGroup, DopplerRadioButtonGroup, ReevaluacionRadioButtonGroup, ImcRadioButtonGroup,
                Urocultivo1TRadioButtonGroup, Urocultivo2TRadioButtonGroup, Urocultivo3TRadioButtonGroup,

                PartoRadioButtonGroup, HemorragiaRadioButtonGroup, DiagnosticoRadioButtonGroup, SulfatoMagnesioRadioButtonGroup,

                EgresoRadioButtonGroup, RecienNacidoRadioButtonGroup,
            };

            foreach (var group in RadioButtonGroup)
                foreach (var item in group)
                    item.Click += RadioButton_Click;

            void RadioButton_Click(object sender, EventArgs e)
            {
                if (RadioButtonCheckDisabled)
                    return;

                var radioButton = sender as RadioButton;

                foreach (var group in RadioButtonGroup)
                    foreach (var item in group)
                        if (item == radioButton)
                        {
                            group.ForEach(p => p.Checked = p == item ? !p.Checked : false);
                            return;
                        }
            }

            EdadExtremaCheckBox.Tag = (int)CondicionesIdentificadas.EdadExtrema;
            AsmaCheckBox.Tag = (int)CondicionesIdentificadas.Asma;
            DiabetesMellitusCheckBox.Tag = (int)CondicionesIdentificadas.DiabetesMellitus;
            AnemiaCheckBox.Tag = (int)CondicionesIdentificadas.Anemia;
            MalnutricionCheckBox.Tag = (int)CondicionesIdentificadas.Malnutricion;
            HTACheckBox.Tag = (int)CondicionesIdentificadas.HTA;
            PreeclampsiaCheckBox.Tag = (int)CondicionesIdentificadas.Preeclampsia;
            PrematuridadCheckBox.Tag = (int)CondicionesIdentificadas.Prematuridad;
            GemelaridadCheckBox.Tag = (int)CondicionesIdentificadas.Gemelaridad;
            InfeccionUrinariaCheckBox.Tag = (int)CondicionesIdentificadas.InfeccionUrinaria;
            InfeccionVaginalCheckBox.Tag = (int)CondicionesIdentificadas.InfeccionVaginal;
            ITSCheckBox.Tag = (int)CondicionesIdentificadas.ITS;
            HabitosToxicoxCheckBox.Tag = (int)CondicionesIdentificadas.HabitosToxicos;
            OtrasCondicionesCheckBox.Tag = (int)CondicionesIdentificadas.Otras;
            NoCondicionesCheckBox.Tag = (int)CondicionesIdentificadas.No;

            CuidadosPerinatalesCheckBox.Tag = (int)LugarIngreso.CuidadosPerinatales;
            UCICheckBox.Tag = (int)LugarIngreso.UnidadCuidadosIntensivos;

            HtaPeeCheckBox.Tag = (int)CausasMorbilidad.HtaPee;
            HtaCronicaCheckBox.Tag = (int)CausasMorbilidad.HtaCronica;
            ComplicacionesHemorragicasCheckBox.Tag = (int)CausasMorbilidad.ComplicacionesHemorragicas;
            ComplicacionesAbortoCheckBox.Tag = (int)CausasMorbilidad.ComplicacionesAborto;
            SepsisOrigenObstetricoCheckBox.Tag = (int)CausasMorbilidad.SepsisOrigenObstetrico;
            SepsisOrigenNoObstetricoCheckBox.Tag = (int)CausasMorbilidad.SepsisOrigenNoObstetrico;
            SepsisOrigenPulmonarCheckBox.Tag = (int)CausasMorbilidad.SepsisOrigenPulmonar;
            EnfermedadExistenteCheckBox.Tag = (int)CausasMorbilidad.ComplicacionEnfermedadExistente;
            OtraCausaMorbilidadCheckBox.Tag = (int)CausasMorbilidad.Otra;

            ShockSepticoCheckBox.Tag = (int)EnfermedadEspecifica.ShockSeptico;
            ShockHipovolemicoCheckBox.Tag = (int)EnfermedadEspecifica.ShockHipovolemico;
            EclampsiaCheckBox.Tag = (int)EnfermedadEspecifica.Eclampsia;

            CerebralCheckBox.Tag = (int)FallaOrganica.Cerebral;
            CardiacaCheckBox.Tag = (int)FallaOrganica.Cardiaca;
            HepaticaCheckBox.Tag = (int)FallaOrganica.Hepatica;
            VascularCheckBox.Tag = (int)FallaOrganica.Vascular;
            RenalCheckBox.Tag = (int)FallaOrganica.Renal;
            CoagulacionCheckBox.Tag = (int)FallaOrganica.Coagulacion;
            MetabolicaCheckBox.Tag = (int)FallaOrganica.Metabolica;
            RespiratoriaCheckBox.Tag = (int)FallaOrganica.Respiratoria;

            CirugiaCheckBox.Tag = (int)TipoManejo.Cirugia;
            TransfusionCheckBox.Tag = (int)TipoManejo.Transfusion;

            HisterectomiaTotalCheckBox.Tag = (int)IntervencionQuirurgica.HisterectomiaTotal;
            HisterectomiaSubtotalCheckBox.Tag = (int)IntervencionQuirurgica.HisterectomiaSubTotal;
            SalpingectomiaTotalCheckBox.Tag = (int)IntervencionQuirurgica.SalpingectomiaTotal;
            SuturasCompBLynchCheckBox.Tag = (int)IntervencionQuirurgica.SuturasCompresivas;
            LigadurasArterialesSelectivasCheckBox.Tag = (int)IntervencionQuirurgica.LigadurasArterialesSelectivas;
            LigaduraArteriasHipogastricasCheckBox.Tag = (int)IntervencionQuirurgica.LigadurasArteriasHipogastricas;
            OtraIntervencionQuirurgicaCheckBox.Tag = (int)IntervencionQuirurgica.Otra;
            
            AcidoTranexamicoCheckBox.Tag = (int)UsoOcitocicos.AcidoTranexamico;
            OcitocinaCheckBox.Tag = (int)UsoOcitocicos.Ocitocina;
            ErgonovinaCheckBox.Tag = (int)UsoOcitocicos.Ergonovina;
            MisoprostolCheckBox.Tag = (int)UsoOcitocicos.Misoprostol;

            ProvinciaComboBox.Items.Clear();
            HospitalProvinciaComboBox.Items.Clear();

            for (var i = 0; i < ProvinciaApiModel.Provincias.Length; i++)
                ProvinciaComboBox.Items.Add(ProvinciaApiModel.Provincias[i]);

            for (var i = 0; i < ProvinciaApiModel.Provincias.Length; i++)
                HospitalProvinciaComboBox.Items.Add(ProvinciaApiModel.Provincias[i]);

            ProvinciaComboBox.SelectedIndexChanged += (s, e) =>
            {
                MunicipioComboBox.Items.Clear();
                MunicipioComboBox.Text = null;

                if (ProvinciaComboBox.SelectedItem is ProvinciaApiModel provincia)
                    for (var i = 0; i < MunicipioApiModel.Municipios.Length; i++)
                        if (MunicipioApiModel.Municipios[i].ProvinciaId == provincia.Id)
                            MunicipioComboBox.Items.Add(MunicipioApiModel.Municipios[i]);
            };

            ProvinciaComboBox.TextChanged += (s, e) =>
            {
                if (string.IsNullOrEmpty(ProvinciaComboBox.Text))
                {
                    MunicipioComboBox.Items.Clear();
                    MunicipioComboBox.Text = null;
                }
            };

            HospitalProvinciaComboBox.SelectedIndexChanged += (s, e) =>
            {
                HospitalComboBox.Items.Clear();
                HospitalComboBox.Text = null;

                if (HospitalProvinciaComboBox.SelectedItem is ProvinciaApiModel provincia)
                    for (var i = 0; i < HospitalApiModel.Hospitales.Length; i++)
                        if (HospitalApiModel.Hospitales[i].ProvinciaId == provincia.Id)
                            HospitalComboBox.Items.Add(HospitalApiModel.Hospitales[i]);
            };

            HospitalProvinciaComboBox.TextChanged += (s, e) =>
            {
                if (string.IsNullOrEmpty(HospitalProvinciaComboBox.Text))
                {
                    HospitalComboBox.Items.Clear();
                    HospitalComboBox.Text = null;
                }
            };

            RemitidaCheckBox.CheckedChanged += (s, e) =>
            {
                if (!RemitidaCheckBox.Checked)
                {
                    Institucion1ComboBox.Items.Clear();
                    Institucion1ComboBox.Text = null;

                    Institucion2ComboBox.Items.Clear();
                    Institucion2ComboBox.Text = null;
                }
                else
                    Institucion1ComboBox.Items.AddRange(InstitucionesArray);
            };

            Institucion1ComboBox.TextChanged += (s, e) =>
            {
                if (!RemitidaCheckBox.Checked)
                    return;

                if (string.IsNullOrEmpty(Institucion1ComboBox.Text))
                {
                    Institucion2ComboBox.Items.Clear();
                    Institucion2ComboBox.Text = null;
                }
                else if (Institucion2ComboBox.Items.Count == 0)
                    Institucion2ComboBox.Items.AddRange(InstitucionesArray);
            };

            ComplicacionesHemorragicasCheckBox.CheckedChanged += (s, e) =>
            {
                CausaHemorragiaComboBox.Items.Clear();
                CausaHemorragiaComboBox.Text = null;

                if (ComplicacionesHemorragicasCheckBox.Checked)
                {
                    foreach (var value in Enum.GetValues(typeof(CausaHemorragia)))
                        CausaHemorragiaComboBox.Items.Add(new IdNombreApiModel
                        {
                            Id = (int)value,
                            Nombre = (Attribute.GetCustomAttribute(typeof(CausaHemorragia).GetMember(value.ToString()).Single(), typeof(DisplayAttribute)) as DisplayAttribute).Name,
                        });
                }
            };

            EgresoFallecidaRadioButton.CheckedChanged += (s, e) =>
            {
                CausaMuerteDirectaComboBox.Items.Clear();
                CausaMuerteDirectaComboBox.Text = null;

                CausaMuerteIndirectaComboBox.Items.Clear();
                CausaMuerteIndirectaComboBox.Text = null;

                if (EgresoFallecidaRadioButton.Checked)
                {
                    foreach (var value in Enum.GetValues(typeof(TipoCausaMuerteDirecta)))
                        CausaMuerteDirectaComboBox.Items.Add(new IdNombreApiModel
                        {
                            Id = (int)value,
                            Nombre = (Attribute.GetCustomAttribute(typeof(TipoCausaMuerteDirecta).GetMember(value.ToString()).Single(), typeof(DisplayAttribute)) as DisplayAttribute).Name,
                        });

                    foreach (var value in Enum.GetValues(typeof(TipoCausaMuerteIndirecta)))
                        CausaMuerteIndirectaComboBox.Items.Add(new IdNombreApiModel
                        {
                            Id = (int)value,
                            Nombre = (Attribute.GetCustomAttribute(typeof(TipoCausaMuerteIndirecta).GetMember(value.ToString()).Single(), typeof(DisplayAttribute)) as DisplayAttribute).Name,
                        });
                }
            };

            CausaMuerteDirectaComboBox.SelectedIndexChanged += (s, e) =>
            {
                if (CausaMuerteDirectaComboBox.SelectedItem != null)
                    CausaMuerteIndirectaComboBox.Text = null;
            };

            CausaMuerteIndirectaComboBox.SelectedIndexChanged += (s, e) =>
            {
                if (CausaMuerteIndirectaComboBox.SelectedItem != null)
                    CausaMuerteDirectaComboBox.Text = null;
            };

            ProvinciaComboBox.TextUpdate += ComboBoxTextUpdate;
            MunicipioComboBox.TextUpdate += ComboBoxTextUpdate;
            HospitalComboBox.TextUpdate += ComboBoxTextUpdate;
            HospitalProvinciaComboBox.TextUpdate += ComboBoxTextUpdate;
            Institucion1ComboBox.TextUpdate += ComboBoxTextUpdate;
            Institucion2ComboBox.TextUpdate += ComboBoxTextUpdate;
            CausaHemorragiaComboBox.TextUpdate += ComboBoxTextUpdate;
            CausaMuerteDirectaComboBox.TextUpdate += ComboBoxTextUpdate;
            CausaMuerteIndirectaComboBox.TextUpdate += ComboBoxTextUpdate;

            ProvinciaComboBox.Leave += ComboBoxLeave;
            MunicipioComboBox.Leave += ComboBoxLeave;
            HospitalComboBox.Leave += ComboBoxLeave;
            HospitalProvinciaComboBox.Leave += ComboBoxLeave;
            Institucion1ComboBox.Leave += ComboBoxLeave;
            Institucion2ComboBox.Leave += ComboBoxLeave;
            CausaHemorragiaComboBox.Leave += ComboBoxLeave;
            CausaMuerteDirectaComboBox.Leave += ComboBoxLeave;
            CausaMuerteIndirectaComboBox.Leave += ComboBoxLeave;

            void ComboBoxTextUpdate(object sender, EventArgs e)
            {
                var match = false;
                var comboBox = sender as ComboBox;

                foreach (IdNombreApiModel item in comboBox.Items)
                    if (item.Nombre.StartsWith(comboBox.Text, StringComparison.InvariantCultureIgnoreCase))
                    {
                        match = true;
                        break;
                    }

                comboBox.ForeColor = match ? Color.Black : Color.Red;
            };

            void ComboBoxLeave(object sender, EventArgs e)
            {
                var comboBox = sender as ComboBox;
                comboBox.ForeColor = Color.Black;

                foreach (IdNombreApiModel item in comboBox.Items)
                    if (item.Nombre.Equals(comboBox.Text, StringComparison.InvariantCultureIgnoreCase))
                    {
                        comboBox.SelectedItem = item;
                        return;
                    }

                comboBox.Text = null;
            };

            FechaIngresoDateTimePicker.ValueChanged += DateTimePicker_ValueChanged;
            FechaEgresoDateTimePicker.ValueChanged += DateTimePicker_ValueChanged;

            void DateTimePicker_ValueChanged(object sender, EventArgs e)
            {
                if (!FechaIngresoDateTimePicker.Checked)
                    EstadiaHospitalariaValueLabel.Text = null;
                else
                {
                    var fechaEgreso = FechaEgresoDateTimePicker.Checked ? (DateTime?)FechaEgresoDateTimePicker.Value.Date : default;
                    var dias = (int)((fechaEgreso ?? DateTime.Now.Date) - FechaIngresoDateTimePicker.Value.Date).TotalDays;

                    EstadiaHospitalariaValueLabel.Text = dias.ToString();
                    EstadiaHospitalariaValueLabel.ForeColor = dias < 0 ? Color.Red : Color.Black;
                }
            }

            #region Hemoglobina
            var hemoglobinaTrigger = false;

            HemoglobinaNR1TCheckBox.CheckedChanged += (s, e) => HemoglobinaNRCheckBox_CheckedChanged(s, e);
            HemoglobinaNR2TCheckBox.CheckedChanged += (s, e) => HemoglobinaNRCheckBox_CheckedChanged(s, e);
            HemoglobinaNR3TCheckBox.CheckedChanged += (s, e) => HemoglobinaNRCheckBox_CheckedChanged(s, e);

            void HemoglobinaNRCheckBox_CheckedChanged(object sender, EventArgs e)
            {
                if (!hemoglobinaTrigger)
                {
                    hemoglobinaTrigger = true;

                    if (sender == HemoglobinaNR1TCheckBox)
                        Hemoglobina1TNud.Value = Hemoglobina1TNud.Minimum;
                    else if (sender == HemoglobinaNR2TCheckBox)
                        Hemoglobina2TNud.Value = Hemoglobina2TNud.Minimum;
                    else if (sender == HemoglobinaNR3TCheckBox)
                        Hemoglobina3TNud.Value = Hemoglobina3TNud.Minimum;

                    hemoglobinaTrigger = false;
                }
            }

            Hemoglobina1TNud.NumericUpDownControl.ValueChanged += (s, e) => HemoglobinaNUD_ValueChanged(s, e);
            Hemoglobina2TNud.NumericUpDownControl.ValueChanged += (s, e) => HemoglobinaNUD_ValueChanged(s, e);
            Hemoglobina3TNud.NumericUpDownControl.ValueChanged += (s, e) => HemoglobinaNUD_ValueChanged(s, e);

            void HemoglobinaNUD_ValueChanged(object sender, EventArgs e)
            {
                if (!hemoglobinaTrigger)
                {
                    hemoglobinaTrigger = true;

                    if (sender == Hemoglobina1TNud.NumericUpDownControl)
                        HemoglobinaNR1TCheckBox.Checked = false;
                    else if (sender == Hemoglobina2TNud.NumericUpDownControl)
                        HemoglobinaNR2TCheckBox.Checked = false;
                    else if (sender == Hemoglobina3TNud.NumericUpDownControl)
                        HemoglobinaNR3TCheckBox.Checked = false;

                    hemoglobinaTrigger = false;
                }
            }

            HemoglobinaNR1TCheckBox.Enter += (s, e) => Hemoglobina_ControlEnter(s, e);
            HemoglobinaNR2TCheckBox.Enter += (s, e) => Hemoglobina_ControlEnter(s, e);
            HemoglobinaNR3TCheckBox.Enter += (s, e) => Hemoglobina_ControlEnter(s, e);

            HemoglobinaNR1TCheckBox.Leave += (s, e) => Hemoglobina_ControlLeave(s, e);
            HemoglobinaNR2TCheckBox.Leave += (s, e) => Hemoglobina_ControlLeave(s, e);
            HemoglobinaNR3TCheckBox.Leave += (s, e) => Hemoglobina_ControlLeave(s, e);

            UrocultivoPositivo1TRadioButton.Enter += (s, e) => Hemoglobina_ControlEnter(s, e);
            UrocultivoNegativo1TRadioButton.Enter += (s, e) => Hemoglobina_ControlEnter(s, e);
            UrocultivoNR1TRadioButton.Enter += (s, e) => Hemoglobina_ControlEnter(s, e);
            UrocultivoPositivo2TRadioButton.Enter += (s, e) => Hemoglobina_ControlEnter(s, e);
            UrocultivoNegativo2TRadioButton.Enter += (s, e) => Hemoglobina_ControlEnter(s, e);
            UrocultivoNR2TRadioButton.Enter += (s, e) => Hemoglobina_ControlEnter(s, e);
            UrocultivoPositivo3TRadioButton.Enter += (s, e) => Hemoglobina_ControlEnter(s, e);
            UrocultivoNegativo3TRadioButton.Enter += (s, e) => Hemoglobina_ControlEnter(s, e);
            UrocultivoNR3TRadioButton.Enter += (s, e) => Hemoglobina_ControlEnter(s, e);

            UrocultivoPositivo1TRadioButton.Leave += (s, e) => Hemoglobina_ControlLeave(s, e);
            UrocultivoNegativo1TRadioButton.Leave += (s, e) => Hemoglobina_ControlLeave(s, e);
            UrocultivoNR1TRadioButton.Leave += (s, e) => Hemoglobina_ControlLeave(s, e);
            UrocultivoPositivo2TRadioButton.Leave += (s, e) => Hemoglobina_ControlLeave(s, e);
            UrocultivoNegativo2TRadioButton.Leave += (s, e) => Hemoglobina_ControlLeave(s, e);
            UrocultivoNR2TRadioButton.Leave += (s, e) => Hemoglobina_ControlLeave(s, e);
            UrocultivoPositivo3TRadioButton.Leave += (s, e) => Hemoglobina_ControlLeave(s, e);
            UrocultivoNegativo3TRadioButton.Leave += (s, e) => Hemoglobina_ControlLeave(s, e);
            UrocultivoNR3TRadioButton.Leave += (s, e) => Hemoglobina_ControlLeave(s, e);

            void Hemoglobina_ControlEnter(object sender, EventArgs e)
                => (sender as Control).BackColor = Color.Blue;

            void Hemoglobina_ControlLeave(object sender, EventArgs e)
                => (sender as Control).BackColor = Color.White;

            #endregion Hemoglobina

            #region Condiciones Identificadas No CheckBox
            var condicionesTrigger = false;

            NoCondicionesCheckBox.CheckedChanged += (s, e) =>
            {
                if (!condicionesTrigger && NoCondicionesCheckBox.Checked)
                {
                    condicionesTrigger = true;

                    HTACheckBox.Checked = false;
                    ITSCheckBox.Checked = false;
                    AsmaCheckBox.Checked = false;
                    AnemiaCheckBox.Checked = false;
                    PrematuridadCheckBox.Checked = false;
                    GemelaridadCheckBox.Checked = false;
                    PreeclampsiaCheckBox.Checked = false;
                    MalnutricionCheckBox.Checked = false;
                    EdadExtremaCheckBox.Checked = false;
                    InfeccionVaginalCheckBox.Checked = false;
                    InfeccionUrinariaCheckBox.Checked = false;
                    DiabetesMellitusCheckBox.Checked = false;
                    HabitosToxicoxCheckBox.Checked = false;
                    OtrasCondicionesCheckBox.Checked = false;

                    condicionesTrigger = false;
                }
            };

            HTACheckBox.CheckedChanged += CondicionCheckBox_CheckedChanged;
            ITSCheckBox.CheckedChanged += CondicionCheckBox_CheckedChanged;
            AsmaCheckBox.CheckedChanged += CondicionCheckBox_CheckedChanged;
            AnemiaCheckBox.CheckedChanged += CondicionCheckBox_CheckedChanged;
            PrematuridadCheckBox.CheckedChanged += CondicionCheckBox_CheckedChanged;
            GemelaridadCheckBox.CheckedChanged += CondicionCheckBox_CheckedChanged;
            PreeclampsiaCheckBox.CheckedChanged += CondicionCheckBox_CheckedChanged;
            MalnutricionCheckBox.CheckedChanged += CondicionCheckBox_CheckedChanged;
            EdadExtremaCheckBox.CheckedChanged += CondicionCheckBox_CheckedChanged;
            InfeccionVaginalCheckBox.CheckedChanged += CondicionCheckBox_CheckedChanged;
            InfeccionUrinariaCheckBox.CheckedChanged += CondicionCheckBox_CheckedChanged;
            DiabetesMellitusCheckBox.CheckedChanged += CondicionCheckBox_CheckedChanged;
            HabitosToxicoxCheckBox.CheckedChanged += CondicionCheckBox_CheckedChanged;
            OtrasCondicionesCheckBox.CheckedChanged += CondicionCheckBox_CheckedChanged;

            void CondicionCheckBox_CheckedChanged(object sender, EventArgs e)
            {
                if (!condicionesTrigger)
                {
                    condicionesTrigger = true;
                    NoCondicionesCheckBox.Checked = false;
                    condicionesTrigger = false;
                }
            }
            #endregion

            #region Criterios de Morbilidad
            var criteriosMorbilidadTrigger = false;

            NoCriteriosMorbilidadCheckBox.CheckedChanged += (s, e) =>
            {
                if (!criteriosMorbilidadTrigger && NoCriteriosMorbilidadCheckBox.Checked)
                {
                    criteriosMorbilidadTrigger = true;

                    ShockSepticoCheckBox.Checked = false;
                    ShockHipovolemicoCheckBox.Checked = false;
                    EclampsiaCheckBox.Checked = false;
                    CerebralCheckBox.Checked = false;
                    CardiacaCheckBox.Checked = false;
                    HepaticaCheckBox.Checked = false;
                    VascularCheckBox.Checked = false;
                    RenalCheckBox.Checked = false;
                    CoagulacionCheckBox.Checked = false;
                    MetabolicaCheckBox.Checked = false;
                    RespiratoriaCheckBox.Checked = false;
                    CirugiaCheckBox.Checked = false;
                    TransfusionCheckBox.Checked = false;

                    criteriosMorbilidadTrigger = false;
                }
            };

            ShockSepticoCheckBox.CheckedChanged += CriteriosMorbilidadCheckBox_CheckedChanged;
            ShockHipovolemicoCheckBox.CheckedChanged += CriteriosMorbilidadCheckBox_CheckedChanged;
            EclampsiaCheckBox.CheckedChanged += CriteriosMorbilidadCheckBox_CheckedChanged;
            CerebralCheckBox.CheckedChanged += CriteriosMorbilidadCheckBox_CheckedChanged;
            CardiacaCheckBox.CheckedChanged += CriteriosMorbilidadCheckBox_CheckedChanged;
            HepaticaCheckBox.CheckedChanged += CriteriosMorbilidadCheckBox_CheckedChanged;
            VascularCheckBox.CheckedChanged += CriteriosMorbilidadCheckBox_CheckedChanged;
            RenalCheckBox.CheckedChanged += CriteriosMorbilidadCheckBox_CheckedChanged;
            CoagulacionCheckBox.CheckedChanged += CriteriosMorbilidadCheckBox_CheckedChanged;
            MetabolicaCheckBox.CheckedChanged += CriteriosMorbilidadCheckBox_CheckedChanged;
            RespiratoriaCheckBox.CheckedChanged += CriteriosMorbilidadCheckBox_CheckedChanged;
            CirugiaCheckBox.CheckedChanged += CriteriosMorbilidadCheckBox_CheckedChanged;
            TransfusionCheckBox.CheckedChanged += CriteriosMorbilidadCheckBox_CheckedChanged;

            void CriteriosMorbilidadCheckBox_CheckedChanged(object sender, EventArgs e)
            {
                if (!criteriosMorbilidadTrigger)
                {
                    criteriosMorbilidadTrigger = true;
                    NoCriteriosMorbilidadCheckBox.Checked = false;
                    criteriosMorbilidadTrigger = false;
                }
            }
            #endregion

            #region Intervención Quirúrgica No CheckBox
            var intervencioneQuirurgicaTrigger = false;

            NoIntervencionQuirurgicaCheckBox.CheckedChanged += (s, e) =>
            {
                if (!intervencioneQuirurgicaTrigger && NoIntervencionQuirurgicaCheckBox.Checked)
                {
                    intervencioneQuirurgicaTrigger = true;

                    HisterectomiaTotalCheckBox.Checked = false;
                    HisterectomiaSubtotalCheckBox.Checked = false;
                    SalpingectomiaTotalCheckBox.Checked = false;
                    SuturasCompBLynchCheckBox.Checked = false;
                    LigadurasArterialesSelectivasCheckBox.Checked = false;
                    LigaduraArteriasHipogastricasCheckBox.Checked = false;
                    OtraIntervencionQuirurgicaCheckBox.Checked = false;
                    OtraIntervencionQuirurgicaTextBox.Text = nameof(IntervencionQuirurgica.Otra);

                    intervencioneQuirurgicaTrigger = false;
                }
            };

            HisterectomiaTotalCheckBox.CheckedChanged += IntervencionQuirurgicaCheckBox_CheckedChanged;
            HisterectomiaSubtotalCheckBox.CheckedChanged += IntervencionQuirurgicaCheckBox_CheckedChanged;
            SalpingectomiaTotalCheckBox.CheckedChanged += IntervencionQuirurgicaCheckBox_CheckedChanged;
            SuturasCompBLynchCheckBox.CheckedChanged += IntervencionQuirurgicaCheckBox_CheckedChanged;
            LigadurasArterialesSelectivasCheckBox.CheckedChanged += IntervencionQuirurgicaCheckBox_CheckedChanged;
            LigaduraArteriasHipogastricasCheckBox.CheckedChanged += IntervencionQuirurgicaCheckBox_CheckedChanged;
            OtraIntervencionQuirurgicaCheckBox.CheckedChanged += IntervencionQuirurgicaCheckBox_CheckedChanged;

            void IntervencionQuirurgicaCheckBox_CheckedChanged(object sender, EventArgs e)
            {
                if (!intervencioneQuirurgicaTrigger)
                {
                    intervencioneQuirurgicaTrigger = true;
                    NoIntervencionQuirurgicaCheckBox.Checked = false;
                    intervencioneQuirurgicaTrigger = false;
                }
            }
            #endregion

            #region Otra (Causa morbilidad / Intervención Quirúrgica)

            OtraCausaMorbilidadTextBox.TextChanged += OtraTextBox_TextChanged;
            OtraIntervencionQuirurgicaTextBox.TextChanged += OtraTextBox_TextChanged;

            //void OtraTextBox_TextChanged(object sender, EventArgs e)
            //{
            //    var textBox = sender as TextBox;

            //    if (string.IsNullOrEmpty(OtraIntervencionQuirurgicaTextBox.Text)
            //        || string.Compare(OtraIntervencionQuirurgicaTextBox.Text, nameof(IntervencionQuirurgica.Otra), ignoreCase: true) == 0)
            //        OtraIntervencionQuirurgicaCheckBox.Checked = false;
            //    else
            //        OtraIntervencionQuirurgicaCheckBox.Checked = true;

            //    if (string.IsNullOrEmpty(OtraIntervencionQuirurgicaTextBox.Text))
            //        OtraIntervencionQuirurgicaCheckBox.Text = nameof(IntervencionQuirurgica.Otra);
            //    else
            //        OtraIntervencionQuirurgicaCheckBox.Text = OtraIntervencionQuirurgicaTextBox.Text;
            //};

            void OtraTextBox_TextChanged(object sender, EventArgs e)
            {
                var textBox = sender as TextBox;
                var comparerTerm = nameof(IntervencionQuirurgica.Otra);
                var checkBox = sender == OtraCausaMorbilidadTextBox ? OtraCausaMorbilidadCheckBox : OtraIntervencionQuirurgicaCheckBox;

                if (string.IsNullOrEmpty(textBox.Text)
                    || string.Compare(textBox.Text, comparerTerm, ignoreCase: true) == 0)
                    checkBox.Checked = false;
                else
                    checkBox.Checked = true;

                if (string.IsNullOrEmpty(textBox.Text))
                    checkBox.Text = comparerTerm;
                else
                    checkBox.Text = textBox.Text;
            };

            OtraCausaMorbilidadTextBox.Enter += OtraTextBox_Enter;
            OtraIntervencionQuirurgicaTextBox.Enter += OtraTextBox_Enter;

            void OtraTextBox_Enter(object sender, EventArgs e) => (sender as TextBox).SelectAll();

            //OtraIntervencionQuirurgicaTextBox.Leave += (s, e) =>
            //{
            //    OtraIntervencionQuirurgicaTextBox.Text = OtraIntervencionQuirurgicaTextBox.Text.Trim();

            //    if (string.IsNullOrEmpty(OtraIntervencionQuirurgicaTextBox.Text))
            //        OtraIntervencionQuirurgicaTextBox.Text = nameof(IntervencionQuirurgica.Otra);
            //    else if (OtraIntervencionQuirurgicaTextBox.Text != nameof(IntervencionQuirurgica.Otra)
            //        && string.Compare(OtraIntervencionQuirurgicaTextBox.Text, nameof(IntervencionQuirurgica.Otra), ignoreCase: true) == 0)
            //        OtraIntervencionQuirurgicaTextBox.Text = nameof(IntervencionQuirurgica.Otra);

            //    if (OtraIntervencionQuirurgicaCheckBox.Checked
            //        && OtraIntervencionQuirurgicaTextBox.Text == nameof(IntervencionQuirurgica.Otra))
            //        OtraIntervencionQuirurgicaCheckBox.Checked = false;
            //};

            OtraCausaMorbilidadTextBox.Leave += OtraTextBox_Leave;
            OtraIntervencionQuirurgicaTextBox.Leave += OtraTextBox_Leave;

            void OtraTextBox_Leave(object sender, EventArgs e)
            {
                var textBox = sender as TextBox;
                var comparerTerm = nameof(IntervencionQuirurgica.Otra);
                var checkBox = sender == OtraCausaMorbilidadTextBox ? OtraCausaMorbilidadCheckBox : OtraIntervencionQuirurgicaCheckBox;

                textBox.Text = textBox.Text.Trim();

                if (string.IsNullOrEmpty(textBox.Text))
                    textBox.Text = comparerTerm;
                else if (textBox.Text != comparerTerm
                    && string.Compare(textBox.Text, comparerTerm, ignoreCase: true) == 0)
                    textBox.Text = comparerTerm;

                if (checkBox.Checked && textBox.Text == comparerTerm)
                    checkBox.Checked = false;
            };

            OtraCausaMorbilidadCheckBox.Enter += OtraCheckBox_Enter;
            OtraIntervencionQuirurgicaCheckBox.Enter += OtraCheckBox_Enter;

            void OtraCheckBox_Enter(object sender, EventArgs e)
            {
                var textBox = sender == OtraCausaMorbilidadCheckBox ? OtraCausaMorbilidadTextBox : OtraIntervencionQuirurgicaTextBox;

                textBox.Focus();
                textBox.SelectAll();
            };

            //OtraIntervencionQuirurgicaCheckBox.Enter += (s, e) =>
            //{
            //    OtraIntervencionQuirurgicaTextBox.Focus();
            //    OtraIntervencionQuirurgicaTextBox.SelectAll();
            //};
            #endregion

            Clear();
        }

        public void Clear()
        {
            var date = DateTime.Now.Date;

            var version = Assembly.GetExecutingAssembly().GetName().Version.ToString(fieldCount: 3);
            VersionLabel.Text = $"Planilla generada por Halo v{version} el {date.ToString("dd/MM/yyyy")}";

            #region Datos Generales
            HistoriaClinicaValueLabel.Tag = null;
            EdadNud.Value = EdadNud.Minimum;
            NombreTextBox.Text = null;
            HistoriaClinicaValueLabel.Text = null;
            FechaIngresoDateTimePicker.Value = FechaIngresoDateTimePicker.MinDate;
            FechaIngresoDateTimePicker.Checked = false;
            DateTimePicker_CheckedChanged(FechaIngresoDateTimePicker);

            MunicipioComboBox.Text = null;
            ProvinciaComboBox.Text = null;
            RuralRadioButton.Checked = false;
            UrbanaRadioButton.Checked = false;

            AmaDeCasaRadioButton.Checked = false;
            EstudianteRadioButton.Checked = false;
            TrabajadoraRadioButton.Checked = false;
            PrimariaRadioButton.Checked = false;
            SecundariaRadioButton.Checked = false;
            UniversitariaRadioButton.Checked = false;
            PreuniversitariaRadioButton.Checked = false;

            HospitalComboBox.Text = null;
            HospitalProvinciaComboBox.Text = null;
            RemitidaCheckBox.Checked = false;
            Institucion1ComboBox.Text = null;
            Institucion2ComboBox.Text = null;
            #endregion

            #region Antecedentes Gineco-Obstétricos
            MolasNud.Value = MolasNud.Minimum;
            VivosNud.Value = VivosNud.Minimum;
            MuertosNud.Value = MuertosNud.Minimum;
            AbortosNud.Value = AbortosNud.Minimum;
            CesareasNud.Value = CesareasNud.Minimum;
            EctopicosNud.Value = EctopicosNud.Minimum;
            GestacionesNud.Value = GestacionesNud.Minimum;
            PartosVaginalesNud.Value = PartosVaginalesNud.Minimum;
            FechaGestacionDateTimePicker.Value = FechaGestacionDateTimePicker.MinDate;
            FechaGestacionDateTimePicker.Checked = false;
            DateTimePicker_CheckedChanged(FechaGestacionDateTimePicker);
            //FechaGestacionDateTimePicker.CustomFormat = DisabledDateFormat;
            #endregion

            #region Atención Prenatal
            SemanasCaptacionNud.Value = SemanasCaptacionNud.Minimum;
            ControlesPrenatalesNud.Value = ControlesPrenatalesNud.Minimum;
            EvaluadoRiesgoSiRadioButton.Checked = false;
            EvaluadoRiesgoNoRadioButton.Checked = false;
            ReevaluacionSiRadioButton.Checked = false;
            ReevaluacionNoRadioButton.Checked = false;

            DopplerPositivoRadioButton.Checked = false;
            DopplerNegativoRadioButton.Checked = false;
            DopplerNoRealizadoRadioButton.Checked = false;

            HTACheckBox.Checked = false;
            ITSCheckBox.Checked = false;
            AsmaCheckBox.Checked = false;
            AnemiaCheckBox.Checked = false;
            PrematuridadCheckBox.Checked = false;
            GemelaridadCheckBox.Checked = false;
            PreeclampsiaCheckBox.Checked = false;
            MalnutricionCheckBox.Checked = false;
            EdadExtremaCheckBox.Checked = false;
            InfeccionVaginalCheckBox.Checked = false;
            InfeccionUrinariaCheckBox.Checked = false;
            DiabetesMellitusCheckBox.Checked = false;
            HabitosToxicoxCheckBox.Checked = false;
            OtrasCondicionesCheckBox.Checked = false;
            NoCondicionesCheckBox.Checked = false;

            ObesaRadioButton.Checked = false;
            SobrePesoRadioButton.Checked = false;
            PesoAdecuadoRadioButton.Checked = false;
            PesoDeficienteRadioButton.Checked = false;

            Hemoglobina1TNud.Value = Hemoglobina1TNud.Minimum;
            Hemoglobina2TNud.Value = Hemoglobina2TNud.Minimum;
            Hemoglobina3TNud.Value = Hemoglobina3TNud.Minimum;
            HemoglobinaNR1TCheckBox.Checked = false;
            HemoglobinaNR2TCheckBox.Checked = false;
            HemoglobinaNR3TCheckBox.Checked = false;
            UrocultivoPositivo1TRadioButton.Checked = false;
            UrocultivoPositivo2TRadioButton.Checked = false;
            UrocultivoPositivo3TRadioButton.Checked = false;
            UrocultivoNegativo1TRadioButton.Checked = false;
            UrocultivoNegativo2TRadioButton.Checked = false;
            UrocultivoNegativo3TRadioButton.Checked = false;
            UrocultivoNR1TRadioButton.Checked = false;
            UrocultivoNR2TRadioButton.Checked = false;
            UrocultivoNR3TRadioButton.Checked = false;
            #endregion

            #region Atención Hospitalaria
            CuidadosPerinatalesCheckBox.Checked = false;
            UCICheckBox.Checked = false;
            EutocicoRadioButton.Checked = false;
            CesareaRadioButton.Checked = false;
            InstrumentadoRadioButton.Checked = false;
            DiagnosticoAntesRadioButton.Checked = false;
            DiagnosticoDuranteRadioButton.Checked = false;
            DiagnosticoDespuesRadioButton.Checked = false;

            HtaPeeCheckBox.Checked = false;
            HtaCronicaCheckBox.Checked = false;
            ComplicacionesHemorragicasCheckBox.Checked = false;
            ComplicacionesAbortoCheckBox.Checked = false;
            SepsisOrigenObstetricoCheckBox.Checked = false;
            SepsisOrigenNoObstetricoCheckBox.Checked = false;
            SepsisOrigenPulmonarCheckBox.Checked = false;
            EnfermedadExistenteCheckBox.Checked = false;
            OtraCausaMorbilidadCheckBox.Checked = false;
            OtraCausaMorbilidadTextBox.Text = nameof(CausasMorbilidad.Otra);

            EclampsiaCheckBox.Checked = false;
            ShockHipovolemicoCheckBox.Checked = false;
            ShockSepticoCheckBox.Checked = false;
            CerebralCheckBox.Checked = false;
            VascularCheckBox.Checked = false;
            MetabolicaCheckBox.Checked = false;
            CardiacaCheckBox.Checked = false;
            RenalCheckBox.Checked = false;
            RespiratoriaCheckBox.Checked = false;
            HepaticaCheckBox.Checked = false;
            CoagulacionCheckBox.Checked = false;
            CirugiaCheckBox.Checked = false;
            TransfusionCheckBox.Checked = false;

            HisterectomiaTotalCheckBox.Checked = false;
            HisterectomiaSubtotalCheckBox.Checked = false;
            SalpingectomiaTotalCheckBox.Checked = false;
            SuturasCompBLynchCheckBox.Checked = false;
            LigadurasArterialesSelectivasCheckBox.Checked = false;
            LigaduraArteriasHipogastricasCheckBox.Checked = false;
            OtraIntervencionQuirurgicaCheckBox.Checked = false;
            OtraIntervencionQuirurgicaTextBox.Text = nameof(IntervencionQuirurgica.Otra);

            CausaHemorragiaComboBox.Text = null;
            CausaHemorragiaComboBox.Items.Clear();
            Hemorragia1raMitadRadioButton.Checked = false;
            Hemorragia2daMitadRadioButton.Checked = false;
            HemorragiaPospartoRadioButton.Checked = false;
            AcidoTranexamicoCheckBox.Checked = false;
            OcitocinaCheckBox.Checked = false;
            ErgonovinaCheckBox.Checked = false;
            MisoprostolCheckBox.Checked = false;
            SulfatoMagnesioSiRadioButton.Checked = false;
            SulfatoMagnesioNoRadioButton.Checked = false;
            #endregion

            #region Egreso
            EgresoVivaRadioButton.Checked = false;
            EgresoFallecidaRadioButton.Checked = false;
            FechaEgresoDateTimePicker.Value = FechaEgresoDateTimePicker.MinDate;
            FechaEgresoDateTimePicker.Checked = false;
            DateTimePicker_CheckedChanged(FechaEgresoDateTimePicker);
            EstadiaHospitalariaValueLabel.Text = null;
            RecienNacidoVivoRadioButton.Checked = false;
            RecienNacidoFallecidoRadioButton.Checked = false;
            EGNud.Value = EGNud.Minimum;
            PesoNud.Value = PesoNud.Minimum;
            Apgar1Nud.Value = Apgar1Nud.Minimum;
            Apgar2Nud.Value = Apgar2Nud.Minimum;

            CausaMuerteDirectaComboBox.Text = null;
            CausaMuerteIndirectaComboBox.Text = null;
            #endregion
        }

        string ValidatePlanilla()
        {
            var nombre = NombreTextBox.Text?.Trim();

            #region Datos Generales
            if (string.IsNullOrEmpty(nombre))
                return "El nombre de la paciente es obligatorio";

            if (EdadNud.Value == 0)
                return "La edad de la paciente es obligatoria";

            if (!FechaIngresoDateTimePicker.Checked)
                return "La fecha de ingreso de la paciente es obligatoria";

            if (RemitidaCheckBox.Checked && Institucion1ComboBox.SelectedItem == null)
                return "Para establecer la paciente como remitida debe especificar la institución de procedencia";
            #endregion

            #region Antecedentes Gineco-Obstétricos
            if (FechaGestacionDateTimePicker.Checked && GestacionesNud.Value <= GestacionesNud.Minimum + 1)
                return "Para establecer la fecha de término de la última gestación debe especificar la cantidad de gestaciones";

            if (GestacionesNud.Value > GestacionesNud.Minimum + 1 && !FechaGestacionDateTimePicker.Checked)
                return "Si especifica la cantidad de gestaciones debe establecer la fecha de término de la última gestación";
            #endregion

            #region Atención Prenatal
            #endregion

            #region Atención Hospitalaria
            if (OtraCausaMorbilidadCheckBox.Checked && string.IsNullOrWhiteSpace(OtraCausaMorbilidadTextBox.Text))
                return "El texto introducido en 'Causa de Morbilidad - Otra' no es válido";

            if ((SulfatoMagnesioSiRadioButton.Checked || SulfatoMagnesioNoRadioButton.Checked) && (!HtaPeeCheckBox.Checked && !HtaCronicaCheckBox.Checked))
                return "Para establecer el uso de sulfato de magnesio debe especificar trastornos hipertensivos como causa de morbilidad";

            if ((HtaPeeCheckBox.Checked || HtaCronicaCheckBox.Checked) && (!SulfatoMagnesioSiRadioButton.Checked && !SulfatoMagnesioNoRadioButton.Checked))
                return "Si especifica trastornos hipertensivos como causa de morbilidad debe establecer el uso o no de sulfato de magnesio";

            if ((Hemorragia1raMitadRadioButton.Checked || Hemorragia2daMitadRadioButton.Checked || HemorragiaPospartoRadioButton.Checked) && !ComplicacionesHemorragicasCheckBox.Checked)
                return "Para establecer el período de ocurrencia de la hemorragia debe especificar complicaciones hemorrágicas como causa de morbilidad";

            if (ComplicacionesHemorragicasCheckBox.Checked && (!Hemorragia1raMitadRadioButton.Checked && !Hemorragia2daMitadRadioButton.Checked && !HemorragiaPospartoRadioButton.Checked))
                return "Si especifica complicaciones hemorrágicas como causa de morbilidad debe establecer el período de ocurrencia de la hemorragia";

            if (ComplicacionesHemorragicasCheckBox.Checked && CausaHemorragiaComboBox.SelectedItem == null)
                return "Si especifica complicaciones hemorrágicas como causa de morbilidad debe establecer la causa de la hemorragia";

            if (HemorragiaPospartoRadioButton.Checked && (!AcidoTranexamicoCheckBox.Checked && !OcitocinaCheckBox.Checked && !ErgonovinaCheckBox.Checked && !MisoprostolCheckBox.Checked))
                return "Si especifica posparto como el período de ocurrencia de hemorragia debe establecer el uso de ocitócicos";

            var isIntervencionQuirurgica = HisterectomiaTotalCheckBox.Checked || HisterectomiaSubtotalCheckBox.Checked ||
                SalpingectomiaTotalCheckBox.Checked || OtraIntervencionQuirurgicaCheckBox.Checked || SuturasCompBLynchCheckBox.Checked ||
                LigadurasArterialesSelectivasCheckBox.Checked || LigaduraArteriasHipogastricasCheckBox.Checked;

            if (CirugiaCheckBox.Checked && !isIntervencionQuirurgica)
                return "Si especifica 'Manejo con Cirugía' dentro de los 'Criterios de Morbilidad' debe establecer el tipo de 'Intervención Quirúrgica'";

            if (isIntervencionQuirurgica && !CirugiaCheckBox.Checked)
                return "Para establecer el tipo de 'Intervención Quirúrgica' debe especificar 'Manejo con Cirugía' dentro de los 'Criterios de Morbilidad'";

            if (OtraIntervencionQuirurgicaCheckBox.Checked && string.IsNullOrWhiteSpace(OtraIntervencionQuirurgicaTextBox.Text))
                return "El texto introducido en 'Intervención Quirúrgica - Otra' no es válido";
            #endregion

            #region Egreso
            if (FechaEgresoDateTimePicker.Checked && (!EgresoVivaRadioButton.Checked && !EgresoFallecidaRadioButton.Checked))
                return "Para establecer la fecha de egreso debe especificar si la paciente egresó viva o fallecida";

            if ((EgresoVivaRadioButton.Checked || EgresoFallecidaRadioButton.Checked) && !FechaEgresoDateTimePicker.Checked)
                return "Para establecer la paciente como egresada debe especificar una fecha de egreso";

            if (EgresoFallecidaRadioButton.Checked && (CausaMuerteDirectaComboBox.SelectedItem == null && CausaMuerteIndirectaComboBox.SelectedItem == null))
                return "Para establecer la paciente como egresada fallecida debe especificar la causa de muerte";

            if (FechaEgresoDateTimePicker.Checked)
                if (FechaEgresoDateTimePicker.Value < FechaIngresoDateTimePicker.Value)
                    return "La fecha de egreso no puede ser anterior a la fecha de ingreso";

            if ((EGNud.Value != EGNud.Minimum || PesoNud.Value != PesoNud.Minimum || Apgar1Nud.Value != Apgar1Nud.Minimum) &&
                (RecienNacidoVivoRadioButton.Checked == false && RecienNacidoFallecidoRadioButton.Checked == false))
                return "Para llenar los datos del recién nacido debe especificar el estado, vivo o fallecido";
            #endregion

            return null;
        }

        PacienteApiModel GetPaciente()
        {
            var paciente = new PacienteApiModel();

            if (ValidatePlanilla() is string value)
            {
                paciente.Account.Error = value;
                return paciente;
            }

            #region Datos Generales

            paciente.Nombre = NombreTextBox.Text;
            paciente.Edad = (int)EdadNud.Value;
            //paciente.Id = (int?)HistoriaClinicaValueLabel.Tag ?? 0;
            paciente.Id = HistoriaClinicaValueLabel.Tag as string;
            paciente.FechaIngreso = FechaIngresoDateTimePicker.Value.Date;

            if (MunicipioComboBox.SelectedItem is MunicipioApiModel municipio)
                paciente.MunicipioId = municipio.Id;

            paciente.AreaId = RuralRadioButton.Checked ? (int?)RuralRadioButton.Tag :
                UrbanaRadioButton.Checked ? (int?)UrbanaRadioButton.Tag : null;

            paciente.OcupacionId = AmaDeCasaRadioButton.Checked ? (int?)AmaDeCasaRadioButton.Tag :
                EstudianteRadioButton.Checked ? (int?)EstudianteRadioButton.Tag :
                TrabajadoraRadioButton.Checked ? (int?)TrabajadoraRadioButton.Tag : null;

            paciente.EscolaridadId = PrimariaRadioButton.Checked ? (int?)PrimariaRadioButton.Tag :
                SecundariaRadioButton.Checked ? (int?)SecundariaRadioButton.Tag :
                PreuniversitariaRadioButton.Checked ? (int?)PreuniversitariaRadioButton.Tag :
                UniversitariaRadioButton.Checked ? (int?)UniversitariaRadioButton.Tag : null;

            if (HospitalComboBox.SelectedItem is HospitalApiModel hospital)
                paciente.HospitalId = hospital.Id;

            if (RemitidaCheckBox.Checked)
            {
                if (Institucion1ComboBox.SelectedItem is IdNombreApiModel institucion1)
                    paciente.Traslado1HospitalId = institucion1.Id;

                if (Institucion2ComboBox.SelectedItem is IdNombreApiModel institucion2)
                    paciente.Traslado2HospitalId = institucion2.Id;
            }

            #endregion

            #region Antecedentes Gineco-Obstétricos

            if (MolasNud.Value != MolasNud.Minimum || VivosNud.Value != VivosNud.Minimum ||
                MuertosNud.Value != MuertosNud.Minimum || AbortosNud.Value != AbortosNud.Minimum ||
                CesareasNud.Value != CesareasNud.Minimum || EctopicosNud.Value != EctopicosNud.Minimum ||
                GestacionesNud.Value != GestacionesNud.Minimum || FechaGestacionDateTimePicker.Checked ||
                PartosVaginalesNud.Value != PartosVaginalesNud.Minimum)
            {
                if (paciente.HistorialMedico == null)
                    paciente.HistorialMedico = new HistorialMedicoApiModel();

                paciente.HistorialMedico.AntecedenteGinecoObstetrico = new AntecedenteGinecoObstetricoApiModel();

                if (MolasNud.Value != MolasNud.Minimum)
                    paciente.HistorialMedico.AntecedenteGinecoObstetrico.Molas = (int)MolasNud.Value;

                if (VivosNud.Value != VivosNud.Minimum)
                    paciente.HistorialMedico.AntecedenteGinecoObstetrico.Vivos = (int)VivosNud.Value;

                if (MuertosNud.Value != MuertosNud.Minimum)
                    paciente.HistorialMedico.AntecedenteGinecoObstetrico.Muertos = (int)MuertosNud.Value;

                if (AbortosNud.Value != AbortosNud.Minimum)
                    paciente.HistorialMedico.AntecedenteGinecoObstetrico.Abortos = (int)AbortosNud.Value;

                if (CesareasNud.Value != CesareasNud.Minimum)
                    paciente.HistorialMedico.AntecedenteGinecoObstetrico.Cesareas = (int)CesareasNud.Value;

                if (EctopicosNud.Value != EctopicosNud.Minimum)
                    paciente.HistorialMedico.AntecedenteGinecoObstetrico.Ectopicos = (int)EctopicosNud.Value;

                if (GestacionesNud.Value != GestacionesNud.Minimum)
                    paciente.HistorialMedico.AntecedenteGinecoObstetrico.Gestaciones = (int)GestacionesNud.Value;

                if (PartosVaginalesNud.Value != PartosVaginalesNud.Minimum)
                    paciente.HistorialMedico.AntecedenteGinecoObstetrico.PartosVaginales = (int)PartosVaginalesNud.Value;

                if (FechaGestacionDateTimePicker.Checked)
                    paciente.HistorialMedico.AntecedenteGinecoObstetrico.UltimaGestacion = FechaGestacionDateTimePicker.Value.Date;
            }

            #endregion

            #region Atención Prenatal

            var isCondicion = EdadExtremaCheckBox.Checked || AsmaCheckBox.Checked || DiabetesMellitusCheckBox.Checked ||
                AnemiaCheckBox.Checked || MalnutricionCheckBox.Checked || HTACheckBox.Checked || PreeclampsiaCheckBox.Checked ||
                PrematuridadCheckBox.Checked || GemelaridadCheckBox.Checked || InfeccionUrinariaCheckBox.Checked ||
                InfeccionVaginalCheckBox.Checked || ITSCheckBox.Checked || HabitosToxicoxCheckBox.Checked || 
                OtrasCondicionesCheckBox.Checked || NoCondicionesCheckBox.Checked;

            var isDoppler = DopplerPositivoRadioButton.Checked || DopplerNegativoRadioButton.Checked || DopplerNoRealizadoRadioButton.Checked;

            var isIMC = ObesaRadioButton.Checked || SobrePesoRadioButton.Checked || PesoAdecuadoRadioButton.Checked || PesoDeficienteRadioButton.Checked;

            var isHemoglobina = Hemoglobina1TNud.Value != Hemoglobina1TNud.Minimum || HemoglobinaNR1TCheckBox.Checked ||
                Hemoglobina2TNud.Value != Hemoglobina2TNud.Minimum || HemoglobinaNR2TCheckBox.Checked ||
                Hemoglobina3TNud.Value != Hemoglobina3TNud.Minimum || HemoglobinaNR3TCheckBox.Checked;

            var isUrocultivo = UrocultivoPositivo1TRadioButton.Checked || UrocultivoNegativo1TRadioButton.Checked || UrocultivoNR1TRadioButton.Checked ||
                UrocultivoPositivo2TRadioButton.Checked || UrocultivoNegativo2TRadioButton.Checked || UrocultivoNR2TRadioButton.Checked ||
                UrocultivoPositivo3TRadioButton.Checked || UrocultivoPositivo3TRadioButton.Checked || UrocultivoNR3TRadioButton.Checked;

            if (SemanasCaptacionNud.Value != SemanasCaptacionNud.Minimum || ControlesPrenatalesNud.Value != ControlesPrenatalesNud.Minimum ||
                EvaluadoRiesgoSiRadioButton.Checked || EvaluadoRiesgoNoRadioButton.Checked || isCondicion || isDoppler || isIMC ||
                ReevaluacionSiRadioButton.Checked || ReevaluacionNoRadioButton.Checked || isHemoglobina || isUrocultivo)
            {
                if (paciente.HistorialMedico == null)
                    paciente.HistorialMedico = new HistorialMedicoApiModel();

                paciente.HistorialMedico.AtencionPrenatal = new AtencionPrenatalApiModel();

                if (SemanasCaptacionNud.Value != SemanasCaptacionNud.Minimum)
                    paciente.HistorialMedico.AtencionPrenatal.SemanasCaptacion = (int)SemanasCaptacionNud.Value;

                if (ControlesPrenatalesNud.Value != ControlesPrenatalesNud.Minimum)
                    paciente.HistorialMedico.AtencionPrenatal.ControlesPrenatales = (int)ControlesPrenatalesNud.Value;

                paciente.HistorialMedico.AtencionPrenatal.EvaluadoComoRiesgo = EvaluadoRiesgoSiRadioButton.Checked ? true : EvaluadoRiesgoNoRadioButton.Checked ? false : default(bool?);

                paciente.HistorialMedico.AtencionPrenatal.Reevaluacion = ReevaluacionSiRadioButton.Checked ? true : ReevaluacionNoRadioButton.Checked ? false : default(bool?);

                if (isCondicion)
                {
                    var flags = (int)NoCondicionesCheckBox.Tag;

                    if (!NoCondicionesCheckBox.Checked)
                    {
                        flags |= EdadExtremaCheckBox.Checked ? (int)EdadExtremaCheckBox.Tag : 0;
                        flags |= AsmaCheckBox.Checked ? (int)AsmaCheckBox.Tag : 0;
                        flags |= DiabetesMellitusCheckBox.Checked ? (int)DiabetesMellitusCheckBox.Tag : 0;
                        flags |= AnemiaCheckBox.Checked ? (int)AnemiaCheckBox.Tag : 0;
                        flags |= MalnutricionCheckBox.Checked ? (int)MalnutricionCheckBox.Tag : 0;
                        flags |= HTACheckBox.Checked ? (int)HTACheckBox.Tag : 0;
                        flags |= PreeclampsiaCheckBox.Checked ? (int)PreeclampsiaCheckBox.Tag : 0;
                        flags |= PrematuridadCheckBox.Checked ? (int)PrematuridadCheckBox.Tag : 0;
                        flags |= GemelaridadCheckBox.Checked ? (int)GemelaridadCheckBox.Tag : 0;
                        flags |= InfeccionUrinariaCheckBox.Checked ? (int)InfeccionUrinariaCheckBox.Tag : 0;
                        flags |= InfeccionVaginalCheckBox.Checked ? (int)InfeccionVaginalCheckBox.Tag : 0;
                        flags |= ITSCheckBox.Checked ? (int)ITSCheckBox.Tag : 0;
                        flags |= HabitosToxicoxCheckBox.Checked ? (int)HabitosToxicoxCheckBox.Tag : 0;
                        flags |= OtrasCondicionesCheckBox.Checked ? (int)OtrasCondicionesCheckBox.Tag : 0;
                    }

                    paciente.HistorialMedico.AtencionPrenatal.CondicionesIdentificadasFlags = flags;
                }

                if (isDoppler)
                {
                    paciente.HistorialMedico.AtencionPrenatal.DopplerArteriaUterina =
                        DopplerPositivoRadioButton.Checked ? (int?)DopplerPositivoRadioButton.Tag :
                        DopplerNegativoRadioButton.Checked ? (int?)DopplerNegativoRadioButton.Tag :
                        DopplerNoRealizadoRadioButton.Checked ? (int?)DopplerNoRealizadoRadioButton.Tag : null;
                }

                if (isIMC)
                {
                    paciente.HistorialMedico.AtencionPrenatal.IndiceMasaCorporalId =
                        ObesaRadioButton.Checked ? (int?)ObesaRadioButton.Tag :
                        SobrePesoRadioButton.Checked ? (int?)SobrePesoRadioButton.Tag :
                        PesoAdecuadoRadioButton.Checked ? (int?)PesoAdecuadoRadioButton.Tag :
                        PesoDeficienteRadioButton.Checked ? (int?)PesoDeficienteRadioButton.Tag : null;
                }

                if (isHemoglobina)
                {
                    paciente.HistorialMedico.AtencionPrenatal.Hemoglobina = new HemoglobinaApiModel();

                    if (Hemoglobina1TNud.Value != Hemoglobina1TNud.Minimum)
                        paciente.HistorialMedico.AtencionPrenatal.Hemoglobina.PrimerTrimestre = (int)Hemoglobina1TNud.Value;
                    else if (HemoglobinaNR1TCheckBox.Checked)
                        paciente.HistorialMedico.AtencionPrenatal.Hemoglobina.PrimerTrimestre = (int)HemoglobinaNRCheckedConstant;

                    if (Hemoglobina2TNud.Value != Hemoglobina2TNud.Minimum)
                        paciente.HistorialMedico.AtencionPrenatal.Hemoglobina.SegundoTrimestre = (int)Hemoglobina2TNud.Value;
                    else if (HemoglobinaNR2TCheckBox.Checked)
                        paciente.HistorialMedico.AtencionPrenatal.Hemoglobina.SegundoTrimestre = (int)HemoglobinaNRCheckedConstant;

                    if (Hemoglobina3TNud.Value != Hemoglobina3TNud.Minimum)
                        paciente.HistorialMedico.AtencionPrenatal.Hemoglobina.TercerTrimestre = (int)Hemoglobina3TNud.Value;
                    else if (HemoglobinaNR3TCheckBox.Checked)
                        paciente.HistorialMedico.AtencionPrenatal.Hemoglobina.TercerTrimestre = (int)HemoglobinaNRCheckedConstant;
                }

                if (isUrocultivo)
                {
                    paciente.HistorialMedico.AtencionPrenatal.Urocultivo = new UrocultivoApiModel
                    {
                        PrimerTrimestre = UrocultivoPositivo1TRadioButton.Checked ? 1 : UrocultivoNegativo1TRadioButton.Checked ? 0 : UrocultivoNR1TRadioButton.Checked ? 2 : default(int?),
                        SegundoTrimestre = UrocultivoPositivo2TRadioButton.Checked ? 1 : UrocultivoNegativo2TRadioButton.Checked ? 0 : UrocultivoNR2TRadioButton.Checked ? 2 : default(int?),
                        TercerTrimestre = UrocultivoPositivo3TRadioButton.Checked ? 1 : UrocultivoNegativo3TRadioButton.Checked ? 0 : UrocultivoNR3TRadioButton.Checked ? 2 : default(int?),
                    };
                }
            }

            #endregion

            #region Atención Hospitalaria

            var isLugarIngreso = CuidadosPerinatalesCheckBox.Checked || UCICheckBox.Checked;

            var isTipoParto = EutocicoRadioButton.Checked || CesareaRadioButton.Checked || InstrumentadoRadioButton.Checked;

            var isDiagnostico = DiagnosticoAntesRadioButton.Checked || DiagnosticoDuranteRadioButton.Checked || DiagnosticoDespuesRadioButton.Checked;

            var isCausasMorbilidad = HtaPeeCheckBox.Checked || HtaCronicaCheckBox.Checked || ComplicacionesHemorragicasCheckBox.Checked ||
                ComplicacionesAbortoCheckBox.Checked || SepsisOrigenObstetricoCheckBox.Checked || SepsisOrigenNoObstetricoCheckBox.Checked ||
                SepsisOrigenPulmonarCheckBox.Checked || EnfermedadExistenteCheckBox.Checked || OtraCausaMorbilidadCheckBox.Checked;

            var isEnfermedadEspecifica = ShockSepticoCheckBox.Checked || ShockHipovolemicoCheckBox.Checked || EclampsiaCheckBox.Checked;

            var isFallaOrganica = CerebralCheckBox.Checked || CardiacaCheckBox.Checked || HepaticaCheckBox.Checked ||
                VascularCheckBox.Checked || RenalCheckBox.Checked || CoagulacionCheckBox.Checked ||
                MetabolicaCheckBox.Checked || RespiratoriaCheckBox.Checked;

            var isManejo = CirugiaCheckBox.Checked || TransfusionCheckBox.Checked;

            var isIntervencionQuirurgica = HisterectomiaTotalCheckBox.Checked || HisterectomiaSubtotalCheckBox.Checked ||
                SalpingectomiaTotalCheckBox.Checked || OtraIntervencionQuirurgicaCheckBox.Checked || SuturasCompBLynchCheckBox.Checked ||
                LigadurasArterialesSelectivasCheckBox.Checked || LigaduraArteriasHipogastricasCheckBox.Checked;

            var isHemorragiaPeriodo = Hemorragia1raMitadRadioButton.Checked || Hemorragia2daMitadRadioButton.Checked || HemorragiaPospartoRadioButton.Checked;

            var isOcitocicos = AcidoTranexamicoCheckBox.Checked || OcitocinaCheckBox.Checked ||
                ErgonovinaCheckBox.Checked || MisoprostolCheckBox.Checked;

            var isSulfatoMagnesio = SulfatoMagnesioSiRadioButton.Checked ? true : SulfatoMagnesioNoRadioButton.Checked ? true : false;

            if (isLugarIngreso || isTipoParto || isDiagnostico || isCausasMorbilidad || isEnfermedadEspecifica ||
                isFallaOrganica || isManejo || isIntervencionQuirurgica || isHemorragiaPeriodo || isOcitocicos || isSulfatoMagnesio)
            {
                if (paciente.HistorialMedico == null)
                    paciente.HistorialMedico = new HistorialMedicoApiModel();

                paciente.HistorialMedico.AtencionHospitalaria = new AtencionHospitalariaApiModel();

                if (isLugarIngreso)
                {
                    var flags = UCICheckBox.Checked ? (int)UCICheckBox.Tag : 0;
                    flags |= CuidadosPerinatalesCheckBox.Checked ? (int)CuidadosPerinatalesCheckBox.Tag : 0;

                    paciente.HistorialMedico.AtencionHospitalaria.LugarIngresoFlags = flags;
                }

                if (isTipoParto)
                {
                    paciente.HistorialMedico.AtencionHospitalaria.PartoId =
                        EutocicoRadioButton.Checked ? (int?)EutocicoRadioButton.Tag :
                        CesareaRadioButton.Checked ? (int?)CesareaRadioButton.Tag :
                        InstrumentadoRadioButton.Checked ? (int?)InstrumentadoRadioButton.Tag : null;
                }

                if (isDiagnostico)
                {
                    paciente.HistorialMedico.AtencionHospitalaria.MorbilidadPartoId =
                        DiagnosticoAntesRadioButton.Checked ? (int?)DiagnosticoAntesRadioButton.Tag :
                        DiagnosticoDuranteRadioButton.Checked ? (int?)DiagnosticoDuranteRadioButton.Tag :
                        DiagnosticoDespuesRadioButton.Checked ? (int?)DiagnosticoDespuesRadioButton.Tag : null;
                }

                if (isCausasMorbilidad)
                {
                    var flags = HtaPeeCheckBox.Checked ? (int)HtaPeeCheckBox.Tag : 0;
                    flags |= HtaCronicaCheckBox.Checked ? (int)HtaCronicaCheckBox.Tag : 0;
                    flags |= ComplicacionesHemorragicasCheckBox.Checked ? (int)ComplicacionesHemorragicasCheckBox.Tag : 0;
                    flags |= ComplicacionesAbortoCheckBox.Checked ? (int)ComplicacionesAbortoCheckBox.Tag : 0;
                    flags |= SepsisOrigenObstetricoCheckBox.Checked ? (int)SepsisOrigenObstetricoCheckBox.Tag : 0;
                    flags |= SepsisOrigenNoObstetricoCheckBox.Checked ? (int)SepsisOrigenNoObstetricoCheckBox.Tag : 0;
                    flags |= SepsisOrigenPulmonarCheckBox.Checked ? (int)SepsisOrigenPulmonarCheckBox.Tag : 0;
                    flags |= EnfermedadExistenteCheckBox.Checked ? (int)EnfermedadExistenteCheckBox.Tag : 0;
                    flags |= OtraCausaMorbilidadCheckBox.Checked ? (int)OtraCausaMorbilidadCheckBox.Tag : 0;

                    paciente.HistorialMedico.AtencionHospitalaria.CausasMorbilidadFlags = flags;

                    if (OtraCausaMorbilidadCheckBox.Checked)
                        paciente.HistorialMedico.AtencionHospitalaria.OtraCausaMorbilidad = OtraCausaMorbilidadTextBox.Text;
                }

                if (isEnfermedadEspecifica || isFallaOrganica || isManejo)
                {
                    paciente.HistorialMedico.AtencionHospitalaria.CriterioMorbilidad = new CriterioMorbilidadApiModel();

                    if (isEnfermedadEspecifica)
                    {
                        var flags = ShockSepticoCheckBox.Checked ? (int)ShockSepticoCheckBox.Tag : 0;
                        flags |= ShockHipovolemicoCheckBox.Checked ? (int)ShockHipovolemicoCheckBox.Tag : 0;
                        flags |= EclampsiaCheckBox.Checked ? (int)EclampsiaCheckBox.Tag : 0;

                        paciente.HistorialMedico.AtencionHospitalaria.CriterioMorbilidad.EnfermedadEspecificaFlags = flags;
                    }

                    if (isFallaOrganica)
                    {
                        var flags = CerebralCheckBox.Checked ? (int)CerebralCheckBox.Tag : 0;
                        flags |= CardiacaCheckBox.Checked ? (int)CardiacaCheckBox.Tag : 0;
                        flags |= HepaticaCheckBox.Checked ? (int)HepaticaCheckBox.Tag : 0;
                        flags |= VascularCheckBox.Checked ? (int)VascularCheckBox.Tag : 0;
                        flags |= RenalCheckBox.Checked ? (int)RenalCheckBox.Tag : 0;
                        flags |= CoagulacionCheckBox.Checked ? (int)CoagulacionCheckBox.Tag : 0;
                        flags |= MetabolicaCheckBox.Checked ? (int)MetabolicaCheckBox.Tag : 0;
                        flags |= RespiratoriaCheckBox.Checked ? (int)RespiratoriaCheckBox.Tag : 0;

                        paciente.HistorialMedico.AtencionHospitalaria.CriterioMorbilidad.FallaOrganicaFlags = flags;
                    }

                    if (isManejo)
                    {
                        var flags = CirugiaCheckBox.Checked ? (int)CirugiaCheckBox.Tag : 0;
                        flags |= TransfusionCheckBox.Checked ? (int)TransfusionCheckBox.Tag : 0;

                        paciente.HistorialMedico.AtencionHospitalaria.CriterioMorbilidad.ManejoFlags = flags;
                    }
                }

                if (isIntervencionQuirurgica)
                {
                    var flags = HisterectomiaTotalCheckBox.Checked ? (int)HisterectomiaTotalCheckBox.Tag : 0;
                    flags |= HisterectomiaSubtotalCheckBox.Checked ? (int)HisterectomiaSubtotalCheckBox.Tag : 0;
                    flags |= SalpingectomiaTotalCheckBox.Checked ? (int)SalpingectomiaTotalCheckBox.Tag : 0;
                    flags |= SuturasCompBLynchCheckBox.Checked ? (int)SuturasCompBLynchCheckBox.Tag : 0;
                    flags |= LigadurasArterialesSelectivasCheckBox.Checked ? (int)LigadurasArterialesSelectivasCheckBox.Tag : 0;
                    flags |= LigaduraArteriasHipogastricasCheckBox.Checked ? (int)LigaduraArteriasHipogastricasCheckBox.Tag : 0;
                    flags |= OtraIntervencionQuirurgicaCheckBox.Checked ? (int)OtraIntervencionQuirurgicaCheckBox.Tag : 0;

                    paciente.HistorialMedico.AtencionHospitalaria.IntervencionQuirurgicaFlags = flags;

                    if (OtraIntervencionQuirurgicaCheckBox.Checked)
                        paciente.HistorialMedico.AtencionHospitalaria.OtraIntervencionQuirurgica = OtraIntervencionQuirurgicaTextBox.Text;
                }

                if (!string.IsNullOrEmpty(paciente.HistorialMedico.AtencionHospitalaria.OtraIntervencionQuirurgica))
                    OtraIntervencionQuirurgicaTextBox.Text = paciente.HistorialMedico.AtencionHospitalaria.OtraIntervencionQuirurgica;
                else
                    OtraIntervencionQuirurgicaTextBox.Text = nameof(IntervencionQuirurgica.Otra);

                if (CausaHemorragiaComboBox.SelectedItem is IdNombreApiModel causaHemorragia)
                    paciente.HistorialMedico.AtencionHospitalaria.CausaHemorragiaId = causaHemorragia.Id;

                if (isHemorragiaPeriodo)
                {
                    paciente.HistorialMedico.AtencionHospitalaria.HemorragiaId =
                        Hemorragia1raMitadRadioButton.Checked ? (int?)Hemorragia1raMitadRadioButton.Tag :
                        Hemorragia2daMitadRadioButton.Checked ? (int?)Hemorragia2daMitadRadioButton.Tag :
                        HemorragiaPospartoRadioButton.Checked ? (int?)HemorragiaPospartoRadioButton.Tag : null;
                }

                if (isOcitocicos)
                {
                    var flags = AcidoTranexamicoCheckBox.Checked ? (int)AcidoTranexamicoCheckBox.Tag : 0;
                    flags |= OcitocinaCheckBox.Checked ? (int)OcitocinaCheckBox.Tag : 0;
                    flags |= ErgonovinaCheckBox.Checked ? (int)ErgonovinaCheckBox.Tag : 0;
                    flags |= MisoprostolCheckBox.Checked ? (int)MisoprostolCheckBox.Tag : 0;

                    paciente.HistorialMedico.AtencionHospitalaria.UsoOcitocicosFlags = flags;
                }

                paciente.HistorialMedico.AtencionHospitalaria.UsoSulfatoMagnesio = SulfatoMagnesioSiRadioButton.Checked ? true : SulfatoMagnesioNoRadioButton.Checked ? false : default(bool?);
            }

            #endregion

            #region Egreso

            if (EgresoVivaRadioButton.Checked || EgresoFallecidaRadioButton.Checked ||
                FechaEgresoDateTimePicker.Checked || RecienNacidoVivoRadioButton.Checked ||
                RecienNacidoFallecidoRadioButton.Checked || EGNud.Value != EGNud.Minimum ||
                PesoNud.Value != PesoNud.Minimum || Apgar1Nud.Value != Apgar1Nud.Minimum ||
                Apgar2Nud.Value != Apgar2Nud.Minimum ||
                CausaMuerteDirectaComboBox.SelectedItem != null ||
                CausaMuerteIndirectaComboBox.SelectedItem != null)
            {
                if (paciente.HistorialMedico == null)
                    paciente.HistorialMedico = new HistorialMedicoApiModel();

                paciente.HistorialMedico.Egreso = new EgresoApiModel();

                if (EgresoVivaRadioButton.Checked || EgresoFallecidaRadioButton.Checked)
                    paciente.HistorialMedico.Egreso.Fallecida = EgresoFallecidaRadioButton.Checked;

                if (FechaEgresoDateTimePicker.Checked)
                    paciente.HistorialMedico.Egreso.Fecha = FechaEgresoDateTimePicker.Value.Date;

                if (RecienNacidoVivoRadioButton.Checked || RecienNacidoFallecidoRadioButton.Checked ||
                    EGNud.Value != EGNud.Minimum || PesoNud.Value != PesoNud.Minimum ||
                    Apgar1Nud.Value != Apgar1Nud.Minimum || Apgar2Nud.Value != Apgar2Nud.Minimum)
                {
                    paciente.HistorialMedico.Egreso.RecienNacido = new RecienNacidoApiModel();

                    if (RecienNacidoVivoRadioButton.Checked || RecienNacidoFallecidoRadioButton.Checked)
                        paciente.HistorialMedico.Egreso.RecienNacido.Fallecido = RecienNacidoFallecidoRadioButton.Checked;

                    if (EGNud.Value != EGNud.Minimum)
                        paciente.HistorialMedico.Egreso.RecienNacido.EdadGestacional = (int)EGNud.Value;

                    if (PesoNud.Value != PesoNud.Minimum)
                        paciente.HistorialMedico.Egreso.RecienNacido.Peso = (int)PesoNud.Value;

                    if (Apgar1Nud.Value != Apgar1Nud.Minimum)
                        paciente.HistorialMedico.Egreso.RecienNacido.Apgar1 = (int)Apgar1Nud.Value;

                    if (Apgar2Nud.Value != Apgar2Nud.Minimum)
                        paciente.HistorialMedico.Egreso.RecienNacido.Apgar2 = (int)Apgar2Nud.Value;
                }

                if (CausaMuerteDirectaComboBox.SelectedItem is IdNombreApiModel causaMuerteDirecta)
                    paciente.HistorialMedico.Egreso.CausaMuerteDirectaId = causaMuerteDirecta.Id;

                if (CausaMuerteIndirectaComboBox.SelectedItem is IdNombreApiModel causaMuerteIndirecta)
                    paciente.HistorialMedico.Egreso.CausaMuerteIndirectaId = causaMuerteIndirecta.Id;
            }

            #endregion

            return paciente;
        }

        void SetPaciente(PacienteApiModel paciente)
        {
            Clear();

            if (paciente == null)
                return;

            if (paciente.Validate() != null)
                return;

            #region Datos Generales

            NombreTextBox.Text = paciente.Nombre;
            EdadNud.Value = paciente.Edad ?? EdadNud.Minimum;

            HistoriaClinicaValueLabel.Tag = paciente.Id;
            HistoriaClinicaValueLabel.Text = paciente.HistoriaClinica;

            if (paciente.FechaIngreso is DateTime fechaIngreso)
            {
                FechaIngresoDateTimePicker.Checked = true;
                FechaIngresoDateTimePicker.Value = fechaIngreso;
                DateTimePicker_CheckedChanged(FechaIngresoDateTimePicker);
            }

            if (paciente.AreaId is int areaId)
            {
                RuralRadioButton.Checked = areaId == (int)RuralRadioButton.Tag ? true : false;
                UrbanaRadioButton.Checked = areaId == (int)UrbanaRadioButton.Tag ? true : false;
            }

            if (paciente.MunicipioId is int municipioId)
            {
                var municipio = MunicipioApiModel.Municipios.Single(p => p.Id == municipioId);
                var provincia = ProvinciaApiModel.Provincias.Single(p => p.Id == municipio.ProvinciaId);

                ProvinciaComboBox.SelectedItem = provincia;
                MunicipioComboBox.SelectedItem = municipio;
            }

            if (paciente.OcupacionId is int ocupacionId)
            {
                AmaDeCasaRadioButton.Checked = ocupacionId == (int)AmaDeCasaRadioButton.Tag ? true : false;
                EstudianteRadioButton.Checked = ocupacionId == (int)EstudianteRadioButton.Tag ? true : false;
                TrabajadoraRadioButton.Checked = ocupacionId == (int)TrabajadoraRadioButton.Tag ? true : false;
            }

            if (paciente.EscolaridadId is int escolaridadId)
            {
                PrimariaRadioButton.Checked = escolaridadId == (int)PrimariaRadioButton.Tag ? true : false;
                SecundariaRadioButton.Checked = escolaridadId == (int)SecundariaRadioButton.Tag ? true : false;
                PreuniversitariaRadioButton.Checked = escolaridadId == (int)PreuniversitariaRadioButton.Tag ? true : false;
                UniversitariaRadioButton.Checked = escolaridadId == (int)UniversitariaRadioButton.Tag ? true : false;
            }

            if (paciente.HospitalId is int hospitalId)
            {
                var hospital = HospitalApiModel.Hospitales.Single(p => p.Id == hospitalId);
                var provincia = ProvinciaApiModel.Provincias.Single(p => p.Id == hospital.ProvinciaId);

                HospitalProvinciaComboBox.SelectedItem = provincia;
                HospitalComboBox.SelectedItem = hospital;
            }

            if (paciente.Traslado1HospitalId is int institucion1Id)
            {
                RemitidaCheckBox.Checked = true;

                Instituciones.TryGetValue(institucion1Id, out var institucion);
                Institucion1ComboBox.SelectedItem = institucion;
            }

            if (paciente.Traslado2HospitalId is int institucion2Id)
            {
                RemitidaCheckBox.Checked = true;

                Instituciones.TryGetValue(institucion2Id, out var institucion);
                Institucion2ComboBox.SelectedItem = institucion;
            }

            #endregion

            #region AntecedentesGinecoObstetricos

            PartosVaginalesNud.Value = paciente.HistorialMedico?.AntecedenteGinecoObstetrico?.PartosVaginales ?? PartosVaginalesNud.Minimum;
            GestacionesNud.Value = paciente.HistorialMedico?.AntecedenteGinecoObstetrico?.Gestaciones ?? GestacionesNud.Minimum;
            EctopicosNud.Value = paciente.HistorialMedico?.AntecedenteGinecoObstetrico?.Ectopicos ?? EctopicosNud.Minimum;
            CesareasNud.Value = paciente.HistorialMedico?.AntecedenteGinecoObstetrico?.Cesareas ?? CesareasNud.Minimum;
            AbortosNud.Value = paciente.HistorialMedico?.AntecedenteGinecoObstetrico?.Abortos ?? AbortosNud.Minimum;
            MuertosNud.Value = paciente.HistorialMedico?.AntecedenteGinecoObstetrico?.Muertos ?? MuertosNud.Minimum;
            VivosNud.Value = paciente.HistorialMedico?.AntecedenteGinecoObstetrico?.Vivos ?? VivosNud.Minimum;
            MolasNud.Value = paciente.HistorialMedico?.AntecedenteGinecoObstetrico?.Molas ?? MolasNud.Minimum;

            if (paciente.HistorialMedico?.AntecedenteGinecoObstetrico?.UltimaGestacion is DateTime fechaUltimaGestacion)
            {
                FechaGestacionDateTimePicker.Checked = true;
                FechaGestacionDateTimePicker.Value = fechaUltimaGestacion;
                DateTimePicker_CheckedChanged(FechaGestacionDateTimePicker);
            }

            #endregion

            #region Atención Prenatal

            SemanasCaptacionNud.Value = paciente.HistorialMedico?.AtencionPrenatal?.SemanasCaptacion ?? SemanasCaptacionNud.Minimum;
            ControlesPrenatalesNud.Value = paciente.HistorialMedico?.AtencionPrenatal?.ControlesPrenatales ?? ControlesPrenatalesNud.Minimum;

            if (paciente.HistorialMedico?.AtencionPrenatal?.EvaluadoComoRiesgo is bool evaluadoComoRiesgo)
            {
                EvaluadoRiesgoSiRadioButton.Checked = evaluadoComoRiesgo;
                EvaluadoRiesgoNoRadioButton.Checked = !evaluadoComoRiesgo;
            }

            //if (paciente.HistorialMedico?.AtencionPrenatal?.UltrasonidoGenetico13Sem is bool ultrasonidoGenetico13)
            //    Ultrazonido13semCheckBox.CheckState = ultrasonidoGenetico13 ? CheckState.Checked : CheckState.Indeterminate;

            //if (paciente.HistorialMedico?.AtencionPrenatal?.UltrasonidoGenetico20Sem is bool ultrasonidoGenetico20)
            //    Ultrazonido20semCheckBox.CheckState = ultrasonidoGenetico20 ? CheckState.Checked : CheckState.Indeterminate;

            //if (paciente.HistorialMedico?.AtencionPrenatal?.UltrasonidoGenetico28Sem is bool ultrasonidoGenetico28)
            //    Ultrazonido28semCheckBox.CheckState = ultrasonidoGenetico28 ? CheckState.Checked : CheckState.Indeterminate;

            if (paciente.HistorialMedico?.AtencionPrenatal?.Reevaluacion is bool reevaluacion)
            {
                ReevaluacionSiRadioButton.Checked = reevaluacion;
                ReevaluacionNoRadioButton.Checked = !reevaluacion;
            }

            if (paciente.HistorialMedico?.AtencionPrenatal?.CondicionesIdentificadasFlags is int condicionesIdentificadas)
            {
                var flags = (CondicionesIdentificadas)condicionesIdentificadas;

                if (flags == 0)
                    NoCondicionesCheckBox.Checked = flags == 0;
                else
                {
                    EdadExtremaCheckBox.Checked = flags.HasFlag((CondicionesIdentificadas)EdadExtremaCheckBox.Tag);
                    AsmaCheckBox.Checked = flags.HasFlag((CondicionesIdentificadas)AsmaCheckBox.Tag);
                    DiabetesMellitusCheckBox.Checked = flags.HasFlag((CondicionesIdentificadas)DiabetesMellitusCheckBox.Tag);
                    AnemiaCheckBox.Checked = flags.HasFlag((CondicionesIdentificadas)AnemiaCheckBox.Tag);
                    MalnutricionCheckBox.Checked = flags.HasFlag((CondicionesIdentificadas)MalnutricionCheckBox.Tag);
                    HTACheckBox.Checked = flags.HasFlag((CondicionesIdentificadas)HTACheckBox.Tag);
                    PreeclampsiaCheckBox.Checked = flags.HasFlag((CondicionesIdentificadas)PreeclampsiaCheckBox.Tag);
                    PrematuridadCheckBox.Checked = flags.HasFlag((CondicionesIdentificadas)PrematuridadCheckBox.Tag);
                    GemelaridadCheckBox.Checked = flags.HasFlag((CondicionesIdentificadas)GemelaridadCheckBox.Tag);
                    InfeccionUrinariaCheckBox.Checked = flags.HasFlag((CondicionesIdentificadas)InfeccionUrinariaCheckBox.Tag);
                    InfeccionVaginalCheckBox.Checked = flags.HasFlag((CondicionesIdentificadas)InfeccionVaginalCheckBox.Tag);
                    ITSCheckBox.Checked = flags.HasFlag((CondicionesIdentificadas)ITSCheckBox.Tag);
                    HabitosToxicoxCheckBox.Checked = flags.HasFlag((CondicionesIdentificadas)HabitosToxicoxCheckBox.Tag);
                    OtrasCondicionesCheckBox.Checked = flags.HasFlag((CondicionesIdentificadas)OtrasCondicionesCheckBox.Tag);
                }
            }

            if (paciente.HistorialMedico?.AtencionPrenatal?.DopplerArteriaUterina is int dopplerId)
            {
                DopplerPositivoRadioButton.Checked = dopplerId == (int)DopplerPositivoRadioButton.Tag ? true : false;
                DopplerNegativoRadioButton.Checked = dopplerId == (int)DopplerNegativoRadioButton.Tag ? true : false;
                DopplerNoRealizadoRadioButton.Checked = dopplerId == (int)DopplerNoRealizadoRadioButton.Tag ? true : false;
            }

            if (paciente.HistorialMedico?.AtencionPrenatal?.IndiceMasaCorporalId is int imcId)
            {
                ObesaRadioButton.Checked = imcId == (int)ObesaRadioButton.Tag ? true : false;
                SobrePesoRadioButton.Checked = imcId == (int)SobrePesoRadioButton.Tag ? true : false;
                PesoAdecuadoRadioButton.Checked = imcId == (int)PesoAdecuadoRadioButton.Tag ? true : false;
                PesoDeficienteRadioButton.Checked = imcId == (int)PesoDeficienteRadioButton.Tag ? true : false;
            }

            if (paciente.HistorialMedico?.AtencionPrenatal?.Hemoglobina != null)
            {
                var hemoglobina1T = paciente.HistorialMedico?.AtencionPrenatal?.Hemoglobina?.PrimerTrimestre ?? Hemoglobina1TNud.Minimum;
                var hemoglobina2T = paciente.HistorialMedico?.AtencionPrenatal?.Hemoglobina?.SegundoTrimestre ?? Hemoglobina2TNud.Minimum;
                var hemoglobina3T = paciente.HistorialMedico?.AtencionPrenatal?.Hemoglobina?.TercerTrimestre ?? Hemoglobina3TNud.Minimum;

                Hemoglobina1TNud.Value = hemoglobina1T >= Hemoglobina1TNud.Minimum ? hemoglobina1T : Hemoglobina1TNud.Minimum;
                Hemoglobina2TNud.Value = hemoglobina2T >= Hemoglobina2TNud.Minimum ? hemoglobina2T : Hemoglobina2TNud.Minimum;
                Hemoglobina3TNud.Value = hemoglobina3T >= Hemoglobina3TNud.Minimum ? hemoglobina3T : Hemoglobina3TNud.Minimum;

                HemoglobinaNR1TCheckBox.Checked = hemoglobina1T == HemoglobinaNRCheckedConstant;
                HemoglobinaNR2TCheckBox.Checked = hemoglobina2T == HemoglobinaNRCheckedConstant;
                HemoglobinaNR3TCheckBox.Checked = hemoglobina3T == HemoglobinaNRCheckedConstant;
            }

            if (paciente.HistorialMedico?.AtencionPrenatal?.Urocultivo != null)
            {
                if (paciente.HistorialMedico?.AtencionPrenatal?.Urocultivo?.PrimerTrimestre is int urocultivoPrimerTrimestre)
                {
                    UrocultivoPositivo1TRadioButton.Checked = urocultivoPrimerTrimestre == 1;
                    UrocultivoNegativo1TRadioButton.Checked = urocultivoPrimerTrimestre == 0;
                    UrocultivoNR1TRadioButton.Checked = urocultivoPrimerTrimestre == 2;
                }

                if (paciente.HistorialMedico?.AtencionPrenatal?.Urocultivo?.SegundoTrimestre is int urocultivoSegundoTrimestre)
                {
                    UrocultivoPositivo2TRadioButton.Checked = urocultivoSegundoTrimestre == 1;
                    UrocultivoNegativo2TRadioButton.Checked = urocultivoSegundoTrimestre == 0;
                    UrocultivoNR2TRadioButton.Checked = urocultivoSegundoTrimestre == 2;
                }

                if (paciente.HistorialMedico?.AtencionPrenatal?.Urocultivo?.TercerTrimestre is int urocultivoTercerTrimestre)
                {
                    UrocultivoPositivo3TRadioButton.Checked = urocultivoTercerTrimestre == 1;
                    UrocultivoNegativo3TRadioButton.Checked = urocultivoTercerTrimestre == 0;
                    UrocultivoNR3TRadioButton.Checked = urocultivoTercerTrimestre == 2;
                }
            }

            #endregion

            #region Atención Hospitalaria

            if (paciente.HistorialMedico?.AtencionHospitalaria?.LugarIngresoFlags is int lugarIngreso)
            {
                var flags = (LugarIngreso)lugarIngreso;

                CuidadosPerinatalesCheckBox.Checked = flags.HasFlag((LugarIngreso)CuidadosPerinatalesCheckBox.Tag);
                UCICheckBox.Checked = flags.HasFlag((LugarIngreso)UCICheckBox.Tag);
            }

            if (paciente.HistorialMedico?.AtencionHospitalaria?.PartoId is int partoId)
            {
                EutocicoRadioButton.Checked = partoId == (int)EutocicoRadioButton.Tag ? true : false;
                CesareaRadioButton.Checked = partoId == (int)CesareaRadioButton.Tag ? true : false;
                InstrumentadoRadioButton.Checked = partoId == (int)InstrumentadoRadioButton.Tag ? true : false;
            }

            if (paciente.HistorialMedico?.AtencionHospitalaria?.MorbilidadPartoId is int morbilidadPartoId)
            {
                DiagnosticoAntesRadioButton.Checked = morbilidadPartoId == (int)DiagnosticoAntesRadioButton.Tag ? true : false;
                DiagnosticoDuranteRadioButton.Checked = morbilidadPartoId == (int)DiagnosticoDuranteRadioButton.Tag ? true : false;
                DiagnosticoDespuesRadioButton.Checked = morbilidadPartoId == (int)DiagnosticoDespuesRadioButton.Tag ? true : false;
            }

            if (paciente.HistorialMedico?.AtencionHospitalaria?.CausasMorbilidadFlags is int causasMorbilidad)
            {
                var flags = (CausasMorbilidad)causasMorbilidad;

                HtaPeeCheckBox.Checked = flags.HasFlag((CausasMorbilidad)HtaPeeCheckBox.Tag);
                HtaCronicaCheckBox.Checked = flags.HasFlag((CausasMorbilidad)HtaCronicaCheckBox.Tag);
                ComplicacionesHemorragicasCheckBox.Checked = flags.HasFlag((CausasMorbilidad)ComplicacionesHemorragicasCheckBox.Tag);
                ComplicacionesAbortoCheckBox.Checked = flags.HasFlag((CausasMorbilidad)ComplicacionesAbortoCheckBox.Tag);
                SepsisOrigenObstetricoCheckBox.Checked = flags.HasFlag((CausasMorbilidad)SepsisOrigenObstetricoCheckBox.Tag);
                SepsisOrigenNoObstetricoCheckBox.Checked = flags.HasFlag((CausasMorbilidad)SepsisOrigenNoObstetricoCheckBox.Tag);
                SepsisOrigenPulmonarCheckBox.Checked = flags.HasFlag((CausasMorbilidad)SepsisOrigenPulmonarCheckBox.Tag);
                EnfermedadExistenteCheckBox.Checked = flags.HasFlag((CausasMorbilidad)EnfermedadExistenteCheckBox.Tag);
                OtraCausaMorbilidadCheckBox.Checked = flags.HasFlag((CausasMorbilidad)OtraCausaMorbilidadCheckBox.Tag);

                if (OtraCausaMorbilidadCheckBox.Checked)
                    OtraCausaMorbilidadTextBox.Text = paciente.HistorialMedico.AtencionHospitalaria.OtraCausaMorbilidad;
            }

            if (paciente.HistorialMedico?.AtencionHospitalaria?.CriterioMorbilidad != null)
            {
                if (paciente.HistorialMedico?.AtencionHospitalaria?.CriterioMorbilidad?.EnfermedadEspecificaFlags is int enfermedadEspecifica)
                {
                    var flags = (EnfermedadEspecifica)enfermedadEspecifica;

                    ShockSepticoCheckBox.Checked = flags.HasFlag((EnfermedadEspecifica)ShockSepticoCheckBox.Tag);
                    ShockHipovolemicoCheckBox.Checked = flags.HasFlag((EnfermedadEspecifica)ShockHipovolemicoCheckBox.Tag);
                    EclampsiaCheckBox.Checked = flags.HasFlag((EnfermedadEspecifica)EclampsiaCheckBox.Tag);
                }

                if (paciente.HistorialMedico?.AtencionHospitalaria?.CriterioMorbilidad?.FallaOrganicaFlags is int fallaOrganica)
                {
                    var flags = (FallaOrganica)fallaOrganica;

                    CerebralCheckBox.Checked = flags.HasFlag((FallaOrganica)CerebralCheckBox.Tag);
                    CardiacaCheckBox.Checked = flags.HasFlag((FallaOrganica)CardiacaCheckBox.Tag);
                    HepaticaCheckBox.Checked = flags.HasFlag((FallaOrganica)HepaticaCheckBox.Tag);
                    VascularCheckBox.Checked = flags.HasFlag((FallaOrganica)VascularCheckBox.Tag);
                    RenalCheckBox.Checked = flags.HasFlag((FallaOrganica)RenalCheckBox.Tag);
                    CoagulacionCheckBox.Checked = flags.HasFlag((FallaOrganica)CoagulacionCheckBox.Tag);
                    MetabolicaCheckBox.Checked = flags.HasFlag((FallaOrganica)MetabolicaCheckBox.Tag);
                    RespiratoriaCheckBox.Checked = flags.HasFlag((FallaOrganica)RespiratoriaCheckBox.Tag);
                }

                if (paciente.HistorialMedico?.AtencionHospitalaria?.CriterioMorbilidad?.ManejoFlags is int manejo)
                {
                    var flags = (TipoManejo)manejo;

                    CirugiaCheckBox.Checked = flags.HasFlag((TipoManejo)CirugiaCheckBox.Tag);
                    TransfusionCheckBox.Checked = flags.HasFlag((TipoManejo)TransfusionCheckBox.Tag);
                }
            }

            if (paciente.HistorialMedico?.AtencionHospitalaria?.IntervencionQuirurgicaFlags is int intervencionQuirurgica)
            {
                var flags = (IntervencionQuirurgica)intervencionQuirurgica;

                HisterectomiaTotalCheckBox.Checked = flags.HasFlag((IntervencionQuirurgica)HisterectomiaTotalCheckBox.Tag);
                HisterectomiaSubtotalCheckBox.Checked = flags.HasFlag((IntervencionQuirurgica)HisterectomiaSubtotalCheckBox.Tag);
                SalpingectomiaTotalCheckBox.Checked = flags.HasFlag((IntervencionQuirurgica)SalpingectomiaTotalCheckBox.Tag);
                SuturasCompBLynchCheckBox.Checked = flags.HasFlag((IntervencionQuirurgica)SuturasCompBLynchCheckBox.Tag);
                LigadurasArterialesSelectivasCheckBox.Checked = flags.HasFlag((IntervencionQuirurgica)LigadurasArterialesSelectivasCheckBox.Tag);
                LigaduraArteriasHipogastricasCheckBox.Checked = flags.HasFlag((IntervencionQuirurgica)LigaduraArteriasHipogastricasCheckBox.Tag);
                OtraIntervencionQuirurgicaCheckBox.Checked = flags.HasFlag((IntervencionQuirurgica)OtraIntervencionQuirurgicaCheckBox.Tag);

                if (OtraIntervencionQuirurgicaCheckBox.Checked)
                    OtraIntervencionQuirurgicaTextBox.Text = paciente.HistorialMedico.AtencionHospitalaria.OtraIntervencionQuirurgica;
            }

            if (paciente.HistorialMedico?.AtencionHospitalaria?.CausaHemorragiaId is int causaHemorragiaId)
            {
                foreach (IdNombreApiModel item in CausaHemorragiaComboBox.Items)
                    if (item.Id == causaHemorragiaId)
                    {
                        CausaHemorragiaComboBox.SelectedItem = item;
                        break;
                    }
            }

            if (paciente.HistorialMedico?.AtencionHospitalaria?.HemorragiaId is int hemorragiaId)
            {
                Hemorragia1raMitadRadioButton.Checked = hemorragiaId == (int)Hemorragia1raMitadRadioButton.Tag ? true : false;
                Hemorragia2daMitadRadioButton.Checked = hemorragiaId == (int)Hemorragia2daMitadRadioButton.Tag ? true : false;
                HemorragiaPospartoRadioButton.Checked = hemorragiaId == (int)HemorragiaPospartoRadioButton.Tag ? true : false;
            }

            if (paciente.HistorialMedico?.AtencionHospitalaria?.UsoOcitocicosFlags is int usoOcitocicos)
            {
                var flags = (UsoOcitocicos)usoOcitocicos;

                AcidoTranexamicoCheckBox.Checked = flags.HasFlag((UsoOcitocicos)AcidoTranexamicoCheckBox.Tag);
                OcitocinaCheckBox.Checked = flags.HasFlag((UsoOcitocicos)OcitocinaCheckBox.Tag);
                ErgonovinaCheckBox.Checked = flags.HasFlag((UsoOcitocicos)ErgonovinaCheckBox.Tag);
                MisoprostolCheckBox.Checked = flags.HasFlag((UsoOcitocicos)MisoprostolCheckBox.Tag);
            }

            if (paciente.HistorialMedico?.AtencionHospitalaria?.UsoSulfatoMagnesio is bool usoSulfatoMagnesio)
            {
                SulfatoMagnesioSiRadioButton.Checked = usoSulfatoMagnesio;
                SulfatoMagnesioNoRadioButton.Checked = !usoSulfatoMagnesio;
            }

            #endregion

            #region Egreso

            if (paciente.HistorialMedico?.Egreso?.Fallecida is bool fallecida)
            {
                EgresoVivaRadioButton.Checked = !fallecida;
                EgresoFallecidaRadioButton.Checked = fallecida;
            }

            if (paciente.HistorialMedico?.Egreso?.Fecha is DateTime fechaEgreso)
            {
                FechaEgresoDateTimePicker.Checked = true;
                FechaEgresoDateTimePicker.Value = fechaEgreso;
                DateTimePicker_CheckedChanged(FechaEgresoDateTimePicker);
            }

            if (paciente.EstadiaHospitalaria is int estadiaHospitalaria)
                EstadiaHospitalariaValueLabel.Text = estadiaHospitalaria.ToString();

            if (paciente.HistorialMedico?.Egreso?.RecienNacido?.Fallecido is bool recienNacidoFallecido)
            {
                RecienNacidoVivoRadioButton.Checked = !recienNacidoFallecido;
                RecienNacidoFallecidoRadioButton.Checked = recienNacidoFallecido;
            }

            if (paciente.HistorialMedico?.Egreso?.RecienNacido?.EdadGestacional is int edadGestacional)
                EGNud.Value = edadGestacional;

            if (paciente.HistorialMedico?.Egreso?.RecienNacido?.Peso is int peso)
                PesoNud.Value = peso;

            if (paciente.HistorialMedico?.Egreso?.RecienNacido?.Apgar1 is int apgar1)
                Apgar1Nud.Value = apgar1;

            if (paciente.HistorialMedico?.Egreso?.RecienNacido?.Apgar2 is int apgar2)
                Apgar2Nud.Value = apgar2;

            if (paciente.HistorialMedico?.Egreso?.CausaMuerteDirectaId is int causaMuerteDirectaId)
            {
                foreach (IdNombreApiModel item in CausaMuerteDirectaComboBox.Items)
                    if (item.Id == causaMuerteDirectaId)
                    {
                        CausaMuerteDirectaComboBox.SelectedItem = item;
                        break;
                    }
            }

            if (paciente.HistorialMedico?.Egreso?.CausaMuerteIndirectaId is int causaMuerteIndirectaId)
            {
                foreach (IdNombreApiModel item in CausaMuerteIndirectaComboBox.Items)
                    if (item.Id == causaMuerteIndirectaId)
                    {
                        CausaMuerteIndirectaComboBox.SelectedItem = item;
                        break;
                    }
            }

            #endregion
        }

        #region DateTimePickerAdjust

        void DateTimePicker_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Space)
            {
                e.Handled = true;
                var dateTimePicker = sender as DateTimePicker;
                dateTimePicker.Checked = !dateTimePicker.Checked;

                DateTimePicker_CheckedChanged(dateTimePicker);
            }
        }

        void DateTimePicker_DropDown(object sender, EventArgs e)
            => DateTimePicker_CheckedChanged(sender as DateTimePicker);

        void DateTimePicker_MouseDown(object sender, MouseEventArgs e)
            => DateTimePicker_CheckedChanged(sender as DateTimePicker);

        void DateTimePicker_CheckedChanged(DateTimePicker dateTimePicker)
        {
            if (dateTimePicker.Checked && dateTimePicker.Value == dateTimePicker.MinDate)
                dateTimePicker.Value = DateTime.Now;

            dateTimePicker.CustomFormat = dateTimePicker.Checked ? EnabledDateFormat : DisabledDateFormat;
        }

        #endregion
    }
}
