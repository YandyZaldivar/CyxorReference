using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Halo.Data.Migrations
{
    [DbContext(typeof(HaloDbContext))]
    [Migration("00000000000000_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("Halo.Models.AntecedenteGinecoObstetrico", b =>
                {
                    b.Property<int>("Id");

                    b.Property<int?>("Abortos");

                    b.Property<int?>("Cesarias");

                    b.Property<int?>("Ectopicos");

                    b.Property<int?>("Gestaciones");

                    b.Property<int?>("Molas");

                    b.Property<int?>("Muertos");

                    b.Property<int?>("PartosVaginales");

                    b.Property<DateTime?>("UltimaGestacion");

                    b.Property<int?>("Vivos");

                    b.HasKey("Id");

                    b.ToTable("AntecedentesGinecoObstetricos");
                });

            modelBuilder.Entity("Halo.Models.Area", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Nombre")
                        .HasMaxLength(128);

                    b.Property<string>("Valor")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.ToTable("Areas");
                });

            modelBuilder.Entity("Halo.Models.AtencionHospitalaria", b =>
                {
                    b.Property<int>("Id");

                    b.Property<int?>("MorbilidadPartoId");

                    b.Property<int?>("PartoId");

                    b.Property<bool?>("UsoSulfatoMagnesio");

                    b.HasKey("Id");

                    b.HasIndex("MorbilidadPartoId");

                    b.HasIndex("PartoId");

                    b.ToTable("AtencionesHospitalarias");
                });

            modelBuilder.Entity("Halo.Models.AtencionPrenatal", b =>
                {
                    b.Property<int>("Id");

                    b.Property<int?>("ControlesPrenatales");

                    b.Property<bool?>("EvaluadoComoRiesgo");

                    b.Property<int?>("IndiceMasaCorporalId");

                    b.Property<bool?>("Reevaluacion");

                    b.Property<int?>("SemanasCaptacion");

                    b.HasKey("Id");

                    b.HasIndex("IndiceMasaCorporalId");

                    b.ToTable("AtencionesPrenatales");
                });

            modelBuilder.Entity("Halo.Models.CausaMorbilidad", b =>
                {
                    b.Property<int>("Id");

                    b.Property<bool>("ComplicacionEnfermedadExistente");

                    b.Property<bool>("ComplicacionesAborto");

                    b.Property<bool>("ComplicacionesHemorragicas");

                    b.Property<bool>("OtraCausa");

                    b.Property<bool>("SepsisOrigenNoObstetrico");

                    b.Property<bool>("SepsisOrigenObstetrico");

                    b.Property<bool>("SepsisOrigenPulmonar");

                    b.Property<bool>("TrastornosHipertensivos");

                    b.HasKey("Id");

                    b.ToTable("CausasMorbilidad");
                });

            modelBuilder.Entity("Halo.Models.CausaMuerteDirecta", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Nombre")
                        .HasMaxLength(128);

                    b.Property<string>("Valor")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.ToTable("CausasMuerteDirecta");
                });

            modelBuilder.Entity("Halo.Models.CausaMuerteIndirecta", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Nombre")
                        .HasMaxLength(128);

                    b.Property<string>("Valor")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.ToTable("CausasMuerteIndirecta");
                });

            modelBuilder.Entity("Halo.Models.CriterioMorbilidad", b =>
                {
                    b.Property<int>("Id");

                    b.HasKey("Id");

                    b.ToTable("CriteriosMorbilidad");
                });

            modelBuilder.Entity("Halo.Models.Direccion", b =>
                {
                    b.Property<int>("PacienteId");

                    b.Property<int?>("AreaId");

                    b.Property<int?>("MunicipioId");

                    b.HasKey("PacienteId");

                    b.HasIndex("AreaId");

                    b.HasIndex("MunicipioId");

                    b.ToTable("Direcciones");
                });

            modelBuilder.Entity("Halo.Models.Egreso", b =>
                {
                    b.Property<int>("Id");

                    b.Property<int?>("CausaMuerteDirectaId");

                    b.Property<int?>("CausaMuerteIndirectaId");

                    b.Property<bool?>("Fallecida");

                    b.Property<DateTime?>("Fecha");

                    b.HasKey("Id");

                    b.HasIndex("CausaMuerteDirectaId");

                    b.HasIndex("CausaMuerteIndirectaId");

                    b.ToTable("Egresos");
                });

            modelBuilder.Entity("Halo.Models.EnfermedadEspecifica", b =>
                {
                    b.Property<int>("Id");

                    b.Property<bool>("Eclampsia");

                    b.Property<bool>("ShockHipovolemico");

                    b.Property<bool>("ShockSeptico");

                    b.HasKey("Id");

                    b.ToTable("EnfermedadesEspecificas");
                });

            modelBuilder.Entity("Halo.Models.Escolaridad", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Nombre")
                        .HasMaxLength(128);

                    b.Property<string>("Valor")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.ToTable("Escolaridades");
                });

            modelBuilder.Entity("Halo.Models.FallaOrganica", b =>
                {
                    b.Property<int>("Id");

                    b.Property<bool>("Cardiaca");

                    b.Property<bool>("Cerebral");

                    b.Property<bool>("Coagulacion");

                    b.Property<bool>("Hepatica");

                    b.Property<bool>("Metabolica");

                    b.Property<bool>("Renal");

                    b.Property<bool>("Respiratoria");

                    b.Property<bool>("Vascular");

                    b.HasKey("Id");

                    b.ToTable("FallasOrganicas");
                });

            modelBuilder.Entity("Halo.Models.Hemoglobina", b =>
                {
                    b.Property<int>("Id");

                    b.Property<double?>("PrimerTrimestre");

                    b.Property<double?>("SegundoTrimestre");

                    b.Property<double?>("TercerTrimestre");

                    b.HasKey("Id");

                    b.ToTable("Hemoglobinas");
                });

            modelBuilder.Entity("Halo.Models.Hemorragia", b =>
                {
                    b.Property<int>("Id");

                    b.Property<bool>("Aborto");

                    b.Property<bool>("Posparto");

                    b.Property<bool>("SegundaMitad");

                    b.HasKey("Id");

                    b.ToTable("Hemorragias");
                });

            modelBuilder.Entity("Halo.Models.HistorialMedico", b =>
                {
                    b.Property<int>("Id");

                    b.HasKey("Id");

                    b.ToTable("HistorialesMedicos");
                });

            modelBuilder.Entity("Halo.Models.Hospital", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<int>("ProvinciaId");

                    b.HasKey("Id");

                    b.HasIndex("ProvinciaId");

                    b.ToTable("Hospitales");
                });

            modelBuilder.Entity("Halo.Models.IndiceMasaCorporal", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Nombre")
                        .HasMaxLength(128);

                    b.Property<string>("Valor")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.ToTable("IndicesMasaCorporal");
                });

            modelBuilder.Entity("Halo.Models.IntervencionQuirurgica", b =>
                {
                    b.Property<int>("Id");

                    b.Property<bool>("HisterectomiaSubTotal");

                    b.Property<bool>("HisterectomiaTotal");

                    b.Property<bool>("LigadurasArterialesSelectivas");

                    b.Property<bool>("LigadurasArteriasHipogastricas");

                    b.Property<bool>("SalpingectomiaTotalBilateral");

                    b.Property<bool>("SalpingectomiaTotalUnilateral");

                    b.Property<bool>("SuturasCompresivas");

                    b.HasKey("Id");

                    b.ToTable("IntervencionesQuirurgicas");
                });

            modelBuilder.Entity("Halo.Models.LugarIngreso", b =>
                {
                    b.Property<int>("Id");

                    b.Property<bool>("CuidadosPerinatales");

                    b.Property<bool>("UnidadCuidadosIntensivos");

                    b.HasKey("Id");

                    b.ToTable("LugaresIngreso");
                });

            modelBuilder.Entity("Halo.Models.Manejo", b =>
                {
                    b.Property<int>("Id");

                    b.Property<bool>("Cirugia");

                    b.Property<bool>("Transfusion");

                    b.HasKey("Id");

                    b.ToTable("Manejos");
                });

            modelBuilder.Entity("Halo.Models.MorbilidadParto", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Nombre")
                        .HasMaxLength(128);

                    b.Property<string>("Valor")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.ToTable("MorbilidadPartos");
                });

            modelBuilder.Entity("Halo.Models.Municipio", b =>
                {
                    b.Property<int>("Id");

                    b.Property<int>("Codigo");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<int>("ProvinciaId");

                    b.HasKey("Id");

                    b.HasIndex("ProvinciaId");

                    b.ToTable("Municipios");
                });

            modelBuilder.Entity("Halo.Models.Ocitocico", b =>
                {
                    b.Property<int>("Id");

                    b.Property<bool>("AcidoTranexamico");

                    b.Property<bool>("Ergonovina");

                    b.Property<bool>("Misoprostol");

                    b.Property<bool>("Ocitocina");

                    b.HasKey("Id");

                    b.ToTable("Ocitocicos");
                });

            modelBuilder.Entity("Halo.Models.Ocupacion", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Nombre")
                        .HasMaxLength(128);

                    b.Property<string>("Valor")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.ToTable("Ocupaciones");
                });

            modelBuilder.Entity("Halo.Models.Orina", b =>
                {
                    b.Property<int>("Id");

                    b.Property<bool?>("PrimerTrimestre");

                    b.Property<bool?>("SegundoTrimestre");

                    b.Property<bool?>("TercerTrimestre");

                    b.HasKey("Id");

                    b.ToTable("Orinas");
                });

            modelBuilder.Entity("Halo.Models.Paciente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("Edad");

                    b.Property<int?>("EscolaridadId");

                    b.Property<DateTime?>("FechaIngreso");

                    b.Property<int?>("HistoriaClinica");

                    b.Property<int?>("HospitalId");

                    b.Property<string>("Nombre")
                        .HasMaxLength(64);

                    b.Property<int?>("OcupacionId");

                    b.Property<int?>("Traslado1HospitalId");

                    b.Property<int?>("Traslado2HospitalId");

                    b.HasKey("Id");

                    b.HasIndex("EscolaridadId");

                    b.HasIndex("HospitalId");

                    b.HasIndex("OcupacionId");

                    b.HasIndex("Traslado1HospitalId");

                    b.HasIndex("Traslado2HospitalId");

                    b.ToTable("Pacientes");
                });

            modelBuilder.Entity("Halo.Models.Parto", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Nombre")
                        .HasMaxLength(128);

                    b.Property<string>("Valor")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.ToTable("Partos");
                });

            modelBuilder.Entity("Halo.Models.Provincia", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.HasKey("Id");

                    b.HasIndex("Nombre")
                        .IsUnique();

                    b.ToTable("Provincias");
                });

            modelBuilder.Entity("Halo.Models.RecienNacido", b =>
                {
                    b.Property<int>("Id");

                    b.Property<int?>("Apgar");

                    b.Property<int?>("Apgar2");

                    b.Property<bool?>("Fallecido");

                    b.Property<bool?>("Multiplicidad");

                    b.Property<int?>("Peso");

                    b.Property<int?>("Peso2");

                    b.HasKey("Id");

                    b.ToTable("RecienNacidos");
                });

            modelBuilder.Entity("Halo.Models.Riesgo", b =>
                {
                    b.Property<int>("Id");

                    b.Property<bool>("Anemia");

                    b.Property<bool>("Asma");

                    b.Property<bool>("DiabetesMellitus");

                    b.Property<bool>("Eclampsia");

                    b.Property<bool>("EdadExtrema");

                    b.Property<bool>("Gemelaridad");

                    b.Property<bool>("HabitosToxicos");

                    b.Property<bool>("HipertensionArterial");

                    b.Property<bool>("InfeccionTransmisionSexual");

                    b.Property<bool>("InfeccionUrinaria");

                    b.Property<bool>("InfeccionVaginal");

                    b.Property<bool>("Malnutricion");

                    b.Property<bool>("Otros");

                    b.Property<bool>("PreEclampsia");

                    b.HasKey("Id");

                    b.ToTable("Riesgos");
                });

            modelBuilder.Entity("Halo.Models.UltrasonidoGenetico", b =>
                {
                    b.Property<int>("Id");

                    b.Property<bool>("Semana13");

                    b.Property<bool>("Semana22");

                    b.Property<bool>("Semana26");

                    b.HasKey("Id");

                    b.ToTable("UltrasonidosGeneticos");
                });

            modelBuilder.Entity("Halo.Models.Usuario", b =>
                {
                    b.Property<int>("Id");

                    b.Property<int?>("HospitalId");

                    b.Property<int?>("ProvinciaId");

                    b.Property<bool>("Visualizador");

                    b.HasKey("Id");

                    b.HasIndex("HospitalId");

                    b.HasIndex("ProvinciaId");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("Halo.Models.AntecedenteGinecoObstetrico", b =>
                {
                    b.HasOne("Halo.Models.HistorialMedico", "HistorialMedico")
                        .WithOne("AntecedenteGinecoObstetrico")
                        .HasForeignKey("Halo.Models.AntecedenteGinecoObstetrico", "Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Halo.Models.AtencionHospitalaria", b =>
                {
                    b.HasOne("Halo.Models.HistorialMedico", "HistorialMedico")
                        .WithOne("AtencionHospitalaria")
                        .HasForeignKey("Halo.Models.AtencionHospitalaria", "Id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Halo.Models.MorbilidadParto", "MorbilidadParto")
                        .WithMany("AtencionesHospitalarias")
                        .HasForeignKey("MorbilidadPartoId");

                    b.HasOne("Halo.Models.Parto", "Parto")
                        .WithMany("AtencionesHospitalarias")
                        .HasForeignKey("PartoId");
                });

            modelBuilder.Entity("Halo.Models.AtencionPrenatal", b =>
                {
                    b.HasOne("Halo.Models.HistorialMedico", "HistorialMedico")
                        .WithOne("AtencionPrenatal")
                        .HasForeignKey("Halo.Models.AtencionPrenatal", "Id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Halo.Models.IndiceMasaCorporal", "IndiceMasaCorporal")
                        .WithMany("AtencionesPrenatales")
                        .HasForeignKey("IndiceMasaCorporalId");
                });

            modelBuilder.Entity("Halo.Models.CausaMorbilidad", b =>
                {
                    b.HasOne("Halo.Models.AtencionHospitalaria", "AtencionHospitalaria")
                        .WithOne("CausaMorbilidad")
                        .HasForeignKey("Halo.Models.CausaMorbilidad", "Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Halo.Models.CriterioMorbilidad", b =>
                {
                    b.HasOne("Halo.Models.AtencionHospitalaria", "AtencionHospitalaria")
                        .WithOne("CriterioMorbilidad")
                        .HasForeignKey("Halo.Models.CriterioMorbilidad", "Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Halo.Models.Direccion", b =>
                {
                    b.HasOne("Halo.Models.Area", "Area")
                        .WithMany("Direcciones")
                        .HasForeignKey("AreaId");

                    b.HasOne("Halo.Models.Municipio", "Municipio")
                        .WithMany("Direcciones")
                        .HasForeignKey("MunicipioId");

                    b.HasOne("Halo.Models.Paciente", "Paciente")
                        .WithOne("Direccion")
                        .HasForeignKey("Halo.Models.Direccion", "PacienteId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Halo.Models.Egreso", b =>
                {
                    b.HasOne("Halo.Models.CausaMuerteDirecta", "CausaMuerteDirecta")
                        .WithMany("Egresos")
                        .HasForeignKey("CausaMuerteDirectaId");

                    b.HasOne("Halo.Models.CausaMuerteIndirecta", "CausaMuerteIndirecta")
                        .WithMany("Egresos")
                        .HasForeignKey("CausaMuerteIndirectaId");

                    b.HasOne("Halo.Models.HistorialMedico", "HistorialMedico")
                        .WithOne("Egreso")
                        .HasForeignKey("Halo.Models.Egreso", "Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Halo.Models.EnfermedadEspecifica", b =>
                {
                    b.HasOne("Halo.Models.CriterioMorbilidad", "CriterioMorbilidad")
                        .WithOne("EnfermedadEspecifica")
                        .HasForeignKey("Halo.Models.EnfermedadEspecifica", "Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Halo.Models.FallaOrganica", b =>
                {
                    b.HasOne("Halo.Models.CriterioMorbilidad", "CriterioMorbilidad")
                        .WithOne("FallaOrganica")
                        .HasForeignKey("Halo.Models.FallaOrganica", "Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Halo.Models.Hemoglobina", b =>
                {
                    b.HasOne("Halo.Models.AtencionPrenatal", "AtencionPrenatal")
                        .WithOne("Hemoglobina")
                        .HasForeignKey("Halo.Models.Hemoglobina", "Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Halo.Models.Hemorragia", b =>
                {
                    b.HasOne("Halo.Models.AtencionHospitalaria", "AtencionHospitalaria")
                        .WithOne("Hemorragia")
                        .HasForeignKey("Halo.Models.Hemorragia", "Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Halo.Models.HistorialMedico", b =>
                {
                    b.HasOne("Halo.Models.Paciente", "Paciente")
                        .WithOne("HistorialMedico")
                        .HasForeignKey("Halo.Models.HistorialMedico", "Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Halo.Models.Hospital", b =>
                {
                    b.HasOne("Halo.Models.Provincia", "Provincia")
                        .WithMany()
                        .HasForeignKey("ProvinciaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Halo.Models.IntervencionQuirurgica", b =>
                {
                    b.HasOne("Halo.Models.AtencionHospitalaria", "AtencionHospitalaria")
                        .WithOne("IntervencionQuirurgica")
                        .HasForeignKey("Halo.Models.IntervencionQuirurgica", "Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Halo.Models.LugarIngreso", b =>
                {
                    b.HasOne("Halo.Models.AtencionHospitalaria", "AtencionHospitalaria")
                        .WithOne("LugarIngreso")
                        .HasForeignKey("Halo.Models.LugarIngreso", "Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Halo.Models.Manejo", b =>
                {
                    b.HasOne("Halo.Models.CriterioMorbilidad", "CriterioMorbilidad")
                        .WithOne("Manejo")
                        .HasForeignKey("Halo.Models.Manejo", "Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Halo.Models.Municipio", b =>
                {
                    b.HasOne("Halo.Models.Provincia", "Provincia")
                        .WithMany("Municipios")
                        .HasForeignKey("ProvinciaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Halo.Models.Ocitocico", b =>
                {
                    b.HasOne("Halo.Models.AtencionHospitalaria", "AtencionHospitalaria")
                        .WithOne("Ocitocico")
                        .HasForeignKey("Halo.Models.Ocitocico", "Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Halo.Models.Orina", b =>
                {
                    b.HasOne("Halo.Models.AtencionPrenatal", "AtencionPrenatal")
                        .WithOne("Orina")
                        .HasForeignKey("Halo.Models.Orina", "Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Halo.Models.Paciente", b =>
                {
                    b.HasOne("Halo.Models.Escolaridad", "Escolaridad")
                        .WithMany("Pacientes")
                        .HasForeignKey("EscolaridadId");

                    b.HasOne("Halo.Models.Hospital", "Hospital")
                        .WithMany("Pacientes")
                        .HasForeignKey("HospitalId");

                    b.HasOne("Halo.Models.Ocupacion", "Ocupacion")
                        .WithMany("Pacientes")
                        .HasForeignKey("OcupacionId");

                    b.HasOne("Halo.Models.Hospital", "Traslado1Hospital")
                        .WithMany()
                        .HasForeignKey("Traslado1HospitalId");

                    b.HasOne("Halo.Models.Hospital", "Traslado2Hospital")
                        .WithMany()
                        .HasForeignKey("Traslado2HospitalId");
                });

            modelBuilder.Entity("Halo.Models.RecienNacido", b =>
                {
                    b.HasOne("Halo.Models.Egreso", "Egreso")
                        .WithOne("RecienNacido")
                        .HasForeignKey("Halo.Models.RecienNacido", "Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Halo.Models.Riesgo", b =>
                {
                    b.HasOne("Halo.Models.AtencionPrenatal", "AtencionPrenatal")
                        .WithOne("Riesgo")
                        .HasForeignKey("Halo.Models.Riesgo", "Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Halo.Models.UltrasonidoGenetico", b =>
                {
                    b.HasOne("Halo.Models.AtencionPrenatal", "AtencionPrenatal")
                        .WithOne("UltrasonidoGenetico")
                        .HasForeignKey("Halo.Models.UltrasonidoGenetico", "Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Halo.Models.Usuario", b =>
                {
                    b.HasOne("Halo.Models.Hospital", "Hospital")
                        .WithMany()
                        .HasForeignKey("HospitalId");

                    b.HasOne("Halo.Models.Provincia", "Provincia")
                        .WithMany()
                        .HasForeignKey("ProvinciaId");
                });
        }
    }
}
