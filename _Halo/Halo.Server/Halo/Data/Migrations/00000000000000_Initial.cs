using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Halo.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 128, nullable: true),
                    Valor = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CausasMuerteDirecta",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 128, nullable: true),
                    Valor = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CausasMuerteDirecta", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CausasMuerteIndirecta",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 128, nullable: true),
                    Valor = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CausasMuerteIndirecta", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Escolaridades",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 128, nullable: true),
                    Valor = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Escolaridades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IndicesMasaCorporal",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 128, nullable: true),
                    Valor = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndicesMasaCorporal", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MorbilidadPartos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 128, nullable: true),
                    Valor = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MorbilidadPartos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ocupaciones",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 128, nullable: true),
                    Valor = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ocupaciones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Partos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 128, nullable: true),
                    Valor = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Provincias",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provincias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Hospitales",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 128, nullable: false),
                    ProvinciaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hospitales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hospitales_Provincias_ProvinciaId",
                        column: x => x.ProvinciaId,
                        principalTable: "Provincias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Municipios",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Codigo = table.Column<int>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 32, nullable: false),
                    ProvinciaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Municipios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Municipios_Provincias_ProvinciaId",
                        column: x => x.ProvinciaId,
                        principalTable: "Provincias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pacientes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Edad = table.Column<int>(nullable: true),
                    EscolaridadId = table.Column<int>(nullable: true),
                    FechaIngreso = table.Column<DateTime>(nullable: true),
                    HistoriaClinica = table.Column<int>(nullable: true),
                    HospitalId = table.Column<int>(nullable: true),
                    Nombre = table.Column<string>(maxLength: 64, nullable: true),
                    OcupacionId = table.Column<int>(nullable: true),
                    Traslado1HospitalId = table.Column<int>(nullable: true),
                    Traslado2HospitalId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pacientes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pacientes_Escolaridades_EscolaridadId",
                        column: x => x.EscolaridadId,
                        principalTable: "Escolaridades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pacientes_Hospitales_HospitalId",
                        column: x => x.HospitalId,
                        principalTable: "Hospitales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pacientes_Ocupaciones_OcupacionId",
                        column: x => x.OcupacionId,
                        principalTable: "Ocupaciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pacientes_Hospitales_Traslado1HospitalId",
                        column: x => x.Traslado1HospitalId,
                        principalTable: "Hospitales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pacientes_Hospitales_Traslado2HospitalId",
                        column: x => x.Traslado2HospitalId,
                        principalTable: "Hospitales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    HospitalId = table.Column<int>(nullable: true),
                    ProvinciaId = table.Column<int>(nullable: true),
                    Visualizador = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_Hospitales_HospitalId",
                        column: x => x.HospitalId,
                        principalTable: "Hospitales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Usuarios_Provincias_ProvinciaId",
                        column: x => x.ProvinciaId,
                        principalTable: "Provincias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Direcciones",
                columns: table => new
                {
                    PacienteId = table.Column<int>(nullable: false),
                    AreaId = table.Column<int>(nullable: true),
                    MunicipioId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Direcciones", x => x.PacienteId);
                    table.ForeignKey(
                        name: "FK_Direcciones_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Direcciones_Municipios_MunicipioId",
                        column: x => x.MunicipioId,
                        principalTable: "Municipios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Direcciones_Pacientes_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Pacientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HistorialesMedicos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialesMedicos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistorialesMedicos_Pacientes_Id",
                        column: x => x.Id,
                        principalTable: "Pacientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AntecedentesGinecoObstetricos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Abortos = table.Column<int>(nullable: true),
                    Cesarias = table.Column<int>(nullable: true),
                    Ectopicos = table.Column<int>(nullable: true),
                    Gestaciones = table.Column<int>(nullable: true),
                    Molas = table.Column<int>(nullable: true),
                    Muertos = table.Column<int>(nullable: true),
                    PartosVaginales = table.Column<int>(nullable: true),
                    UltimaGestacion = table.Column<DateTime>(nullable: true),
                    Vivos = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AntecedentesGinecoObstetricos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AntecedentesGinecoObstetricos_HistorialesMedicos_Id",
                        column: x => x.Id,
                        principalTable: "HistorialesMedicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AtencionesHospitalarias",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    MorbilidadPartoId = table.Column<int>(nullable: true),
                    PartoId = table.Column<int>(nullable: true),
                    UsoSulfatoMagnesio = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AtencionesHospitalarias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AtencionesHospitalarias_HistorialesMedicos_Id",
                        column: x => x.Id,
                        principalTable: "HistorialesMedicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AtencionesHospitalarias_MorbilidadPartos_MorbilidadPartoId",
                        column: x => x.MorbilidadPartoId,
                        principalTable: "MorbilidadPartos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AtencionesHospitalarias_Partos_PartoId",
                        column: x => x.PartoId,
                        principalTable: "Partos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AtencionesPrenatales",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    ControlesPrenatales = table.Column<int>(nullable: true),
                    EvaluadoComoRiesgo = table.Column<bool>(nullable: true),
                    IndiceMasaCorporalId = table.Column<int>(nullable: true),
                    Reevaluacion = table.Column<bool>(nullable: true),
                    SemanasCaptacion = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AtencionesPrenatales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AtencionesPrenatales_HistorialesMedicos_Id",
                        column: x => x.Id,
                        principalTable: "HistorialesMedicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AtencionesPrenatales_IndicesMasaCorporal_IndiceMasaCorporalId",
                        column: x => x.IndiceMasaCorporalId,
                        principalTable: "IndicesMasaCorporal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Egresos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CausaMuerteDirectaId = table.Column<int>(nullable: true),
                    CausaMuerteIndirectaId = table.Column<int>(nullable: true),
                    Fallecida = table.Column<bool>(nullable: true),
                    Fecha = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Egresos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Egresos_CausasMuerteDirecta_CausaMuerteDirectaId",
                        column: x => x.CausaMuerteDirectaId,
                        principalTable: "CausasMuerteDirecta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Egresos_CausasMuerteIndirecta_CausaMuerteIndirectaId",
                        column: x => x.CausaMuerteIndirectaId,
                        principalTable: "CausasMuerteIndirecta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Egresos_HistorialesMedicos_Id",
                        column: x => x.Id,
                        principalTable: "HistorialesMedicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CausasMorbilidad",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    ComplicacionEnfermedadExistente = table.Column<bool>(nullable: false),
                    ComplicacionesAborto = table.Column<bool>(nullable: false),
                    ComplicacionesHemorragicas = table.Column<bool>(nullable: false),
                    OtraCausa = table.Column<bool>(nullable: false),
                    SepsisOrigenNoObstetrico = table.Column<bool>(nullable: false),
                    SepsisOrigenObstetrico = table.Column<bool>(nullable: false),
                    SepsisOrigenPulmonar = table.Column<bool>(nullable: false),
                    TrastornosHipertensivos = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CausasMorbilidad", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CausasMorbilidad_AtencionesHospitalarias_Id",
                        column: x => x.Id,
                        principalTable: "AtencionesHospitalarias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CriteriosMorbilidad",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CriteriosMorbilidad", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CriteriosMorbilidad_AtencionesHospitalarias_Id",
                        column: x => x.Id,
                        principalTable: "AtencionesHospitalarias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Hemorragias",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Aborto = table.Column<bool>(nullable: false),
                    Posparto = table.Column<bool>(nullable: false),
                    SegundaMitad = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hemorragias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hemorragias_AtencionesHospitalarias_Id",
                        column: x => x.Id,
                        principalTable: "AtencionesHospitalarias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IntervencionesQuirurgicas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    HisterectomiaSubTotal = table.Column<bool>(nullable: false),
                    HisterectomiaTotal = table.Column<bool>(nullable: false),
                    LigadurasArterialesSelectivas = table.Column<bool>(nullable: false),
                    LigadurasArteriasHipogastricas = table.Column<bool>(nullable: false),
                    SalpingectomiaTotalBilateral = table.Column<bool>(nullable: false),
                    SalpingectomiaTotalUnilateral = table.Column<bool>(nullable: false),
                    SuturasCompresivas = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntervencionesQuirurgicas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IntervencionesQuirurgicas_AtencionesHospitalarias_Id",
                        column: x => x.Id,
                        principalTable: "AtencionesHospitalarias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LugaresIngreso",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CuidadosPerinatales = table.Column<bool>(nullable: false),
                    UnidadCuidadosIntensivos = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LugaresIngreso", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LugaresIngreso_AtencionesHospitalarias_Id",
                        column: x => x.Id,
                        principalTable: "AtencionesHospitalarias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ocitocicos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    AcidoTranexamico = table.Column<bool>(nullable: false),
                    Ergonovina = table.Column<bool>(nullable: false),
                    Misoprostol = table.Column<bool>(nullable: false),
                    Ocitocina = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ocitocicos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ocitocicos_AtencionesHospitalarias_Id",
                        column: x => x.Id,
                        principalTable: "AtencionesHospitalarias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Hemoglobinas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    PrimerTrimestre = table.Column<double>(nullable: true),
                    SegundoTrimestre = table.Column<double>(nullable: true),
                    TercerTrimestre = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hemoglobinas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hemoglobinas_AtencionesPrenatales_Id",
                        column: x => x.Id,
                        principalTable: "AtencionesPrenatales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orinas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    PrimerTrimestre = table.Column<bool>(nullable: true),
                    SegundoTrimestre = table.Column<bool>(nullable: true),
                    TercerTrimestre = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orinas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orinas_AtencionesPrenatales_Id",
                        column: x => x.Id,
                        principalTable: "AtencionesPrenatales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Riesgos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Anemia = table.Column<bool>(nullable: false),
                    Asma = table.Column<bool>(nullable: false),
                    DiabetesMellitus = table.Column<bool>(nullable: false),
                    Eclampsia = table.Column<bool>(nullable: false),
                    EdadExtrema = table.Column<bool>(nullable: false),
                    Gemelaridad = table.Column<bool>(nullable: false),
                    HabitosToxicos = table.Column<bool>(nullable: false),
                    HipertensionArterial = table.Column<bool>(nullable: false),
                    InfeccionTransmisionSexual = table.Column<bool>(nullable: false),
                    InfeccionUrinaria = table.Column<bool>(nullable: false),
                    InfeccionVaginal = table.Column<bool>(nullable: false),
                    Malnutricion = table.Column<bool>(nullable: false),
                    Otros = table.Column<bool>(nullable: false),
                    PreEclampsia = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Riesgos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Riesgos_AtencionesPrenatales_Id",
                        column: x => x.Id,
                        principalTable: "AtencionesPrenatales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UltrasonidosGeneticos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Semana13 = table.Column<bool>(nullable: false),
                    Semana22 = table.Column<bool>(nullable: false),
                    Semana26 = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UltrasonidosGeneticos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UltrasonidosGeneticos_AtencionesPrenatales_Id",
                        column: x => x.Id,
                        principalTable: "AtencionesPrenatales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecienNacidos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Apgar = table.Column<int>(nullable: true),
                    Apgar2 = table.Column<int>(nullable: true),
                    Fallecido = table.Column<bool>(nullable: true),
                    Multiplicidad = table.Column<bool>(nullable: true),
                    Peso = table.Column<int>(nullable: true),
                    Peso2 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecienNacidos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecienNacidos_Egresos_Id",
                        column: x => x.Id,
                        principalTable: "Egresos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EnfermedadesEspecificas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Eclampsia = table.Column<bool>(nullable: false),
                    ShockHipovolemico = table.Column<bool>(nullable: false),
                    ShockSeptico = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnfermedadesEspecificas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnfermedadesEspecificas_CriteriosMorbilidad_Id",
                        column: x => x.Id,
                        principalTable: "CriteriosMorbilidad",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FallasOrganicas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Cardiaca = table.Column<bool>(nullable: false),
                    Cerebral = table.Column<bool>(nullable: false),
                    Coagulacion = table.Column<bool>(nullable: false),
                    Hepatica = table.Column<bool>(nullable: false),
                    Metabolica = table.Column<bool>(nullable: false),
                    Renal = table.Column<bool>(nullable: false),
                    Respiratoria = table.Column<bool>(nullable: false),
                    Vascular = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FallasOrganicas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FallasOrganicas_CriteriosMorbilidad_Id",
                        column: x => x.Id,
                        principalTable: "CriteriosMorbilidad",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Manejos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Cirugia = table.Column<bool>(nullable: false),
                    Transfusion = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manejos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Manejos_CriteriosMorbilidad_Id",
                        column: x => x.Id,
                        principalTable: "CriteriosMorbilidad",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AtencionesHospitalarias_MorbilidadPartoId",
                table: "AtencionesHospitalarias",
                column: "MorbilidadPartoId");

            migrationBuilder.CreateIndex(
                name: "IX_AtencionesHospitalarias_PartoId",
                table: "AtencionesHospitalarias",
                column: "PartoId");

            migrationBuilder.CreateIndex(
                name: "IX_AtencionesPrenatales_IndiceMasaCorporalId",
                table: "AtencionesPrenatales",
                column: "IndiceMasaCorporalId");

            migrationBuilder.CreateIndex(
                name: "IX_Direcciones_AreaId",
                table: "Direcciones",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Direcciones_MunicipioId",
                table: "Direcciones",
                column: "MunicipioId");

            migrationBuilder.CreateIndex(
                name: "IX_Egresos_CausaMuerteDirectaId",
                table: "Egresos",
                column: "CausaMuerteDirectaId");

            migrationBuilder.CreateIndex(
                name: "IX_Egresos_CausaMuerteIndirectaId",
                table: "Egresos",
                column: "CausaMuerteIndirectaId");

            migrationBuilder.CreateIndex(
                name: "IX_Hospitales_ProvinciaId",
                table: "Hospitales",
                column: "ProvinciaId");

            migrationBuilder.CreateIndex(
                name: "IX_Municipios_ProvinciaId",
                table: "Municipios",
                column: "ProvinciaId");

            migrationBuilder.CreateIndex(
                name: "IX_Pacientes_EscolaridadId",
                table: "Pacientes",
                column: "EscolaridadId");

            migrationBuilder.CreateIndex(
                name: "IX_Pacientes_HospitalId",
                table: "Pacientes",
                column: "HospitalId");

            migrationBuilder.CreateIndex(
                name: "IX_Pacientes_OcupacionId",
                table: "Pacientes",
                column: "OcupacionId");

            migrationBuilder.CreateIndex(
                name: "IX_Pacientes_Traslado1HospitalId",
                table: "Pacientes",
                column: "Traslado1HospitalId");

            migrationBuilder.CreateIndex(
                name: "IX_Pacientes_Traslado2HospitalId",
                table: "Pacientes",
                column: "Traslado2HospitalId");

            migrationBuilder.CreateIndex(
                name: "IX_Provincias_Nombre",
                table: "Provincias",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_HospitalId",
                table: "Usuarios",
                column: "HospitalId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_ProvinciaId",
                table: "Usuarios",
                column: "ProvinciaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AntecedentesGinecoObstetricos");

            migrationBuilder.DropTable(
                name: "CausasMorbilidad");

            migrationBuilder.DropTable(
                name: "Direcciones");

            migrationBuilder.DropTable(
                name: "EnfermedadesEspecificas");

            migrationBuilder.DropTable(
                name: "FallasOrganicas");

            migrationBuilder.DropTable(
                name: "Hemoglobinas");

            migrationBuilder.DropTable(
                name: "Hemorragias");

            migrationBuilder.DropTable(
                name: "IntervencionesQuirurgicas");

            migrationBuilder.DropTable(
                name: "LugaresIngreso");

            migrationBuilder.DropTable(
                name: "Manejos");

            migrationBuilder.DropTable(
                name: "Ocitocicos");

            migrationBuilder.DropTable(
                name: "Orinas");

            migrationBuilder.DropTable(
                name: "RecienNacidos");

            migrationBuilder.DropTable(
                name: "Riesgos");

            migrationBuilder.DropTable(
                name: "UltrasonidosGeneticos");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Areas");

            migrationBuilder.DropTable(
                name: "Municipios");

            migrationBuilder.DropTable(
                name: "CriteriosMorbilidad");

            migrationBuilder.DropTable(
                name: "Egresos");

            migrationBuilder.DropTable(
                name: "AtencionesPrenatales");

            migrationBuilder.DropTable(
                name: "AtencionesHospitalarias");

            migrationBuilder.DropTable(
                name: "CausasMuerteDirecta");

            migrationBuilder.DropTable(
                name: "CausasMuerteIndirecta");

            migrationBuilder.DropTable(
                name: "IndicesMasaCorporal");

            migrationBuilder.DropTable(
                name: "HistorialesMedicos");

            migrationBuilder.DropTable(
                name: "MorbilidadPartos");

            migrationBuilder.DropTable(
                name: "Partos");

            migrationBuilder.DropTable(
                name: "Pacientes");

            migrationBuilder.DropTable(
                name: "Escolaridades");

            migrationBuilder.DropTable(
                name: "Hospitales");

            migrationBuilder.DropTable(
                name: "Ocupaciones");

            migrationBuilder.DropTable(
                name: "Provincias");
        }
    }
}
