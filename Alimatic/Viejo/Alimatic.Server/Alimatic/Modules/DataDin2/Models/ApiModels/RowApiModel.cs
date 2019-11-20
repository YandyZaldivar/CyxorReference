﻿namespace Alimatic.DataDin2.Models
{
    public class RowApiModel
    {
        public int Id { get; set; }
        public int ModelId { get; set; }
        public string Description { get; set; }

        public static RowApiModel[] Rows { get; } = new RowApiModel[]
        {
            /*
            #region 5920
            new RowApiModel { Id = 1, ModelId = 5920, Description = "Activos Circulantes" },
            new RowApiModel { Id = 2, ModelId = 5920, Description = "Efectivo en Caja (101-108)" },
            new RowApiModel { Id = 3, ModelId = 5920, Description = "Efectivo en Banco y en Otras Instituciones (109-119)" },
            new RowApiModel { Id = 4, ModelId = 5920, Description = "Inversiones a Corto Plazo o Temporales (120-129)" },
            new RowApiModel { Id = 5, ModelId = 5920, Description = "Efectos por Cobrar a Corto Plazo (130-133)" },
            new RowApiModel { Id = 6, ModelId = 5920, Description = "Menos: Efectos por Cobrar Descontados (365)" },
            new RowApiModel { Id = 7, ModelId = 5920, Description = "Cuenta en Participación (134)" },
            new RowApiModel { Id = 8, ModelId = 5920, Description = "Cuentas por Cobrar a Corto Plazo (135-139 y 154)" },
            new RowApiModel { Id = 9, ModelId = 5920, Description = "Menos: Provisión para Cuentas Incobrables (369)" },
            new RowApiModel { Id = 10, ModelId = 5920, Description = "Pagos por Cuenta de Terceros (140)" },
            new RowApiModel { Id = 11, ModelId = 5920, Description = "Participación de Reaseguradores por Siniestros Pendientes (141)" },
            new RowApiModel { Id = 12, ModelId = 5920, Description = "Préstamos y Otras Operaciones Crediticias a Cobrar a Corto Plazo (142)" },
            new RowApiModel { Id = 13, ModelId = 5920, Description = "Suscriptores de Bonos (143)" },
            new RowApiModel { Id = 14, ModelId = 5920, Description = "Pagos Anticipados a Suministradores (146-149)" },
            new RowApiModel { Id = 15, ModelId = 5920, Description = "Pagos Anticipados del Proceso Inversionista (150-152)" },
            new RowApiModel { Id = 16, ModelId = 5920, Description = "Materiales Anticipados del Proceso Inversionista (153)" },
            new RowApiModel { Id = 17, ModelId = 5920, Description = "Anticipos a Justificar (161-163)" },
            new RowApiModel { Id = 18, ModelId = 5920, Description = "Adeudos del Presupuesto del Estado (164-166)" },
            new RowApiModel { Id = 19, ModelId = 5920, Description = "Adeudos del Órgano u Organismo (167-170)" },
            new RowApiModel { Id = 20, ModelId = 5920, Description = "Ingresos Acumulados por Cobrar (173-180)" },
            new RowApiModel { Id = 21, ModelId = 5920, Description = "Dividendos y Participaciones por Cobrar (181)" },
            new RowApiModel { Id = 22, ModelId = 5920, Description = "Ingresos Acumulados por Cobrar – Reaseguros Aceptados (182)" },
            new RowApiModel { Id = 23, ModelId = 5920, Description = "Total de Inventarios" },
            new RowApiModel { Id = 24, ModelId = 5920, Description = "Materias Primas y Materiales (183)" },
            new RowApiModel { Id = 25, ModelId = 5920, Description = "Combustibles y Lubricantes (184)" },
            new RowApiModel { Id = 26, ModelId = 5920, Description = "Partes y Piezas de Repuesto (185)" },
            new RowApiModel { Id = 27, ModelId = 5920, Description = "Envases y Embalajes (186)" },
            new RowApiModel { Id = 28, ModelId = 5920, Description = "Útiles, Herramientas y Otros (187)" },
            new RowApiModel { Id = 29, ModelId = 5920, Description = "Menos: Desgaste de Útiles y Herramientas (373)" },
            new RowApiModel { Id = 30, ModelId = 5920, Description = "Producción Terminada (188)" },
            new RowApiModel { Id = 31, ModelId = 5920, Description = "Mercancías para la Venta (189)" },
            new RowApiModel { Id = 32, ModelId = 5920, Description = "Menos: Descuento Comercial e Impuesto (370-372)" },
            new RowApiModel { Id = 33, ModelId = 5920, Description = "Medicamentos (190)" },
            new RowApiModel { Id = 34, ModelId = 5920, Description = "Base Material de Estudio (191)" },
            new RowApiModel { Id = 35, ModelId = 5920, Description = "Menos: Desgaste de Base Material de Estudio (366)" },
            new RowApiModel { Id = 36, ModelId = 5920, Description = "Vestuario y Lencería (192)" },
            new RowApiModel { Id = 37, ModelId = 5920, Description = "Menos: Desgaste de Vestuario y Lencería (367)" },
            new RowApiModel { Id = 38, ModelId = 5920, Description = "Alimentos (193)" },
            new RowApiModel { Id = 39, ModelId = 5920, Description = "Inventarios de Mercancías de Importación (194)" },
            new RowApiModel { Id = 40, ModelId = 5920, Description = "Inventarios de Mercancías de Exportación (195)" },
            new RowApiModel { Id = 41, ModelId = 5920, Description = "Producciones  para Insumo o Autoconsumo (196)" },
            new RowApiModel { Id = 42, ModelId = 5920, Description = "Otros Inventarios (205-207)" },
            new RowApiModel { Id = 43, ModelId = 5920, Description = "Inventarios Ociosos (208)" },
            new RowApiModel { Id = 44, ModelId = 5920, Description = "Inventarios de Lento Movimiento (209)" },
            new RowApiModel { Id = 45, ModelId = 5920, Description = "Producción en Proceso (700-724)" },
            new RowApiModel { Id = 46, ModelId = 5920, Description = "Producción Propia para Insumos (725)" },
            new RowApiModel { Id = 47, ModelId = 5920, Description = "Reparaciones Capitales con Medios Propios (726)" },
            new RowApiModel { Id = 48, ModelId = 5920, Description = "Inversiones con Medios Propios Activos Fijos Intangibles (727)" },
            new RowApiModel { Id = 49, ModelId = 5920, Description = "Inversiones con Medios Propios (728)" },
            new RowApiModel { Id = 50, ModelId = 5920, Description = "Créditos Documentarios (211)" },
            new RowApiModel { Id = 51, ModelId = 5920, Description = "Activos a Largo Plazo" },
            new RowApiModel { Id = 52, ModelId = 5920, Description = "Efectos por Cobrar a Largo Plazo (215-217)" },
            new RowApiModel { Id = 53, ModelId = 5920, Description = "Cuentas por Cobrar a Largo Plazo (218-220)" },
            new RowApiModel { Id = 54, ModelId = 5920, Description = "Préstamos Concedidos a Cobrar a Largo Plazo (221-224)" },
            new RowApiModel { Id = 55, ModelId = 5920, Description = "Inversiones a Largo Plazo o Permanentes (225-234)" },
            new RowApiModel { Id = 56, ModelId = 5920, Description = "Activos Fijos" },
            new RowApiModel { Id = 57, ModelId = 5920, Description = "Activos Fijos Tangibles (240-251)" },
            new RowApiModel { Id = 58, ModelId = 5920, Description = "Menos: Depreciación de Activos Fijos Tangibles (375-388)" },
            new RowApiModel { Id = 59, ModelId = 5920, Description = "Fondos Bibliotecarios (252)" },
            new RowApiModel { Id = 60, ModelId = 5920, Description = "Medios y Equipos para Alquilar (253)" },
            new RowApiModel { Id = 61, ModelId = 5920, Description = "Menos: Desgaste de Medios y Equipos para Alquilar (389)" },
            new RowApiModel { Id = 62, ModelId = 5920, Description = "Monumentos y Obras de Arte (254)" },
            new RowApiModel { Id = 63, ModelId = 5920, Description = "Activos Fijos Intangibles (255 – 263)" },
            new RowApiModel { Id = 64, ModelId = 5920, Description = "Activos Fijos Intangibles en Proceso (264)" },
            new RowApiModel { Id = 65, ModelId = 5920, Description = "Menos: Amortización de Activos Fijos Intangibles (390-399)" },
            new RowApiModel { Id = 66, ModelId = 5920, Description = "Inversiones en Proceso (265-278)" },
            new RowApiModel { Id = 67, ModelId = 5920, Description = "Plan de Preparación de Inversiones (279)" },
            new RowApiModel { Id = 68, ModelId = 5920, Description = "Equipos por Instalar y Materiales para el Proceso Inversionista (280-289)" },
            new RowApiModel { Id = 69, ModelId = 5920, Description = "Activos Diferidos" },
            new RowApiModel { Id = 70, ModelId = 5920, Description = "Gastos de Producción y Servicios Diferidos (300-305)" },
            new RowApiModel { Id = 71, ModelId = 5920, Description = "Gastos Financieros Diferidos (306-307)" },
            new RowApiModel { Id = 72, ModelId = 5920, Description = "Gastos del Proceso Inversionista Diferidos (310-311)" },
            new RowApiModel { Id = 73, ModelId = 5920, Description = "Gastos por Faltantes y Pérdidas Diferidos (312)" },
            new RowApiModel { Id = 74, ModelId = 5920, Description = "Otros Activos" },
            new RowApiModel { Id = 75, ModelId = 5920, Description = "Pérdidas en Investigación (330-331)" },
            new RowApiModel { Id = 76, ModelId = 5920, Description = "Faltantes de Bienes en Investigación (332-333)" },
            new RowApiModel { Id = 77, ModelId = 5920, Description = "Cuentas por Cobrar Diversas – Operaciones Corrientes (334-341)" },
            new RowApiModel { Id = 78, ModelId = 5920, Description = "Cuentas por Cobrar – Compra de Monedas (342)" },
            new RowApiModel { Id = 79, ModelId = 5920, Description = "Cuentas por Cobrar del Proceso Inversionista (343-345)" },
            new RowApiModel { Id = 80, ModelId = 5920, Description = "Efectos por Cobrar en Litigio (346)" },
            new RowApiModel { Id = 81, ModelId = 5920, Description = "Cuentas por Cobrar en Litigio (347)" },
            new RowApiModel { Id = 82, ModelId = 5920, Description = "Efectos por Cobrar Protestados (348)" },
            new RowApiModel { Id = 83, ModelId = 5920, Description = "Cuentas por Cobrar en Proceso Judicial (349)" },
            new RowApiModel { Id = 84, ModelId = 5920, Description = "Depósitos y Fianzas (354-355)" },
            new RowApiModel { Id = 85, ModelId = 5920, Description = "Fondo de Amortización de Bonos – Efectivo y Valores (364)" },
            new RowApiModel { Id = 86, ModelId = 5920, Description = "Menos: Otras Provisiones Reguladoras de Activos (374)" },
            new RowApiModel { Id = 87, ModelId = 5920, Description = "TOTAL DEL ACTIVO" },
            new RowApiModel { Id = 88, ModelId = 5920, Description = "Pasivos Circulantes" },
            new RowApiModel { Id = 89, ModelId = 5920, Description = "Sobregiro Bancario (400)" },
            new RowApiModel { Id = 90, ModelId = 5920, Description = "Efectos por Pagar a Corto Plazo (401-404)" },
            new RowApiModel { Id = 91, ModelId = 5920, Description = "Cuentas por Pagar a Corto Plazo (405-415)" },
            new RowApiModel { Id = 92, ModelId = 5920, Description = "Cobros por Cuenta de Terceros (416)" },
            new RowApiModel { Id = 93, ModelId = 5920, Description = "Dividendos y Participaciones por Pagar (417)" },
            new RowApiModel { Id = 94, ModelId = 5920, Description = "Cuentas en Participación (418-420)" },
            new RowApiModel { Id = 95, ModelId = 5920, Description = "Cuentas por Pagar - Activos Fijos Tangibles (421-424)" },
            new RowApiModel { Id = 96, ModelId = 5920, Description = "Cuentas por Pagar del Proceso Inversionista (425-429)" },
            new RowApiModel { Id = 97, ModelId = 5920, Description = "Cobros Anticipados (430-433)" },
            new RowApiModel { Id = 98, ModelId = 5920, Description = "Materiales Recibidos de Forma Anticipada (434)" },
            new RowApiModel { Id = 99, ModelId = 5920, Description = "Depósitos Recibidos (435-439)" },
            new RowApiModel { Id = 100, ModelId = 5920, Description = "Obligaciones con el Presupuesto del Estado (440-449)" },
            new RowApiModel { Id = 101, ModelId = 5920, Description = "Obligaciones con el Órgano u Organismo (450-453)" },
            new RowApiModel { Id = 102, ModelId = 5920, Description = "Nóminas por Pagar (455-459)" },
            new RowApiModel { Id = 103, ModelId = 5920, Description = "Retenciones por Pagar (460-469)" },
            new RowApiModel { Id = 104, ModelId = 5920, Description = "Préstamos Recibidos y Otras Operaciones Crediticias por Pagar (470-479)" },
            new RowApiModel { Id = 105, ModelId = 5920, Description = "Gastos Acumulados por Pagar (480-489)" },
            new RowApiModel { Id = 106, ModelId = 5920, Description = "Provisión para Vacaciones (492)" },
            new RowApiModel { Id = 107, ModelId = 5920, Description = "Otras Provisiones Operacionales (493-499)" },
            new RowApiModel { Id = 108, ModelId = 5920, Description = "Provisión para Pagos de los Subsidios de Seguridad Social a Corto Plazo (500)" },
            new RowApiModel { Id = 109, ModelId = 5920, Description = "Fondo de Compensación para Desbalances Financieros (501) uso exclusivo de las OSDE" },
            new RowApiModel { Id = 110, ModelId = 5920, Description = "Pasivos a Largo Plazo" },
            new RowApiModel { Id = 111, ModelId = 5920, Description = "Efectos por Pagar a Largo Plazo (510-514)" },
            new RowApiModel { Id = 112, ModelId = 5920, Description = "Cuentas por Pagar a Largo Plazo (515-518)" },
            new RowApiModel { Id = 113, ModelId = 5920, Description = "Préstamos Recibidos por Pagar a Largo Plazo (520-524)" },
            new RowApiModel { Id = 114, ModelId = 5920, Description = "Obligaciones a Largo Plazo (525-532)" },
            new RowApiModel { Id = 115, ModelId = 5920, Description = "Otras Provisiones a Largo Plazo (533-539)" },
            new RowApiModel { Id = 116, ModelId = 5920, Description = "Bonos por Pagar (540)-(144)-(363)" },
            new RowApiModel { Id = 117, ModelId = 5920, Description = "Bonos Suscritos (541)" },
            new RowApiModel { Id = 118, ModelId = 5920, Description = "Pasivos Diferidos" },
            new RowApiModel { Id = 119, ModelId = 5920, Description = "Ingresos Diferidos (545-548)" },
            new RowApiModel { Id = 120, ModelId = 5920, Description = "Ingresos Diferidos por Donaciones Recibidas (549)" },
            new RowApiModel { Id = 121, ModelId = 5920, Description = "Otros Pasivos" },
            new RowApiModel { Id = 122, ModelId = 5920, Description = "Sobrantes en Investigación (555-564)" },
            new RowApiModel { Id = 123, ModelId = 5920, Description = "Cuentas por Pagar Diversas (565-568)" },
            new RowApiModel { Id = 124, ModelId = 5920, Description = "Cuentas por Pagar – Compra de Monedas (569)" },
            new RowApiModel { Id = 125, ModelId = 5920, Description = "Ingresos de Períodos Futuros (570-574)" },
            new RowApiModel { Id = 126, ModelId = 5920, Description = "Obligación con el Presupuesto del Estado por Garantía Activada (575)" },
            new RowApiModel { Id = 127, ModelId = 5920, Description = "TOTAL DEL PASIVO" },
            new RowApiModel { Id = 128, ModelId = 5920, Description = "Inversión Estatal (600-612) Sector Público" },
            new RowApiModel { Id = 129, ModelId = 5920, Description = "Patrimonio y Fondo Común (600) Sector Privado" },
            new RowApiModel { Id = 130, ModelId = 5920, Description = "Capital Social Suscrito y Pagado" },
            new RowApiModel { Id = 131, ModelId = 5920, Description = "Recursos Recibidos (617-618) Sector Público" },
            new RowApiModel { Id = 132, ModelId = 5920, Description = "Donaciones Recibidas – Nacionales (620)" },
            new RowApiModel { Id = 133, ModelId = 5920, Description = "Donaciones Recibidas – Exterior (621)" },
            new RowApiModel { Id = 134, ModelId = 5920, Description = "Utilidades Retenidas (630-634)" },
            new RowApiModel { Id = 135, ModelId = 5920, Description = "Subvención por Pérdida (635-639)" },
            new RowApiModel { Id = 136, ModelId = 5920, Description = "Reservas para Contingencias (645)" },
            new RowApiModel { Id = 137, ModelId = 5920, Description = "Otras Reservas Patrimoniales (646-654)" },
            new RowApiModel { Id = 138, ModelId = 5920, Description = "Fondo de Contravalor para proyectos de Inversión (688)" },
            new RowApiModel { Id = 139, ModelId = 5920, Description = "Menos: Recursos Entregados (619) Sector Público" },
            new RowApiModel { Id = 140, ModelId = 5920, Description = "Donaciones Entregadas –Nacionales (626)" },
            new RowApiModel { Id = 141, ModelId = 5920, Description = "Donaciones Entregadas – Exterior (627)" },
            new RowApiModel { Id = 142, ModelId = 5920, Description = "Pago a Cuenta de las Utilidades (690)" },
            new RowApiModel { Id = 143, ModelId = 5920, Description = "Pago a Cuenta de los Dividendos (691)" },
            new RowApiModel { Id = 144, ModelId = 5920, Description = "Pérdida (640-644)" },
            new RowApiModel { Id = 145, ModelId = 5920, Description = "Más o Menos: Revalorización de Activos Fijos Tangibles (613 - 615)" },
            new RowApiModel { Id = 146, ModelId = 5920, Description = "Otras Operaciones de Capital (616 - 619) Sector Privado" },
            new RowApiModel { Id = 147, ModelId = 5920, Description = "Revaluación de Inventarios (697)" },
            new RowApiModel { Id = 148, ModelId = 5920, Description = "Ganancia o Pérdida no Realizada (698)" },
            new RowApiModel { Id = 149, ModelId = 5920, Description = "Resultado del Período" },
            new RowApiModel { Id = 150, ModelId = 5920, Description = "TOTAL DE PATRIMONIO NETO" },
            new RowApiModel { Id = 151, ModelId = 5920, Description = "TOTAL DEL PASIVO Y PATRIMONIO NETO O CAPITAL CONTABLE" },
            #endregion

            #region 5921
            new RowApiModel { Id = 1, ModelId = 5921, Description = "Ventas (900 - 913)" },
            new RowApiModel { Id = 2, ModelId = 5921, Description = "Más: Ventas de Bienes con destino a la Exportación (914)" },
            new RowApiModel { Id = 3, ModelId = 5921, Description = "Ventas  por Exportación de Servicios (915)" },
            new RowApiModel { Id = 4, ModelId = 5921, Description = "Subvenciones (916 - 919)" },
            new RowApiModel { Id = 5, ModelId = 5921, Description = "Menos: Devoluciones y Rebajas en Ventas (800 – 804)" },
            new RowApiModel { Id = 6, ModelId = 5921, Description = "Impuesto por las Ventas (805 – 809)" },
            new RowApiModel { Id = 7, ModelId = 5921, Description = "Ventas Netas" },
            new RowApiModel { Id = 8, ModelId = 5921, Description = "Menos: Costo de Ventas de la Producción (810 – 813)" },
            new RowApiModel { Id = 9, ModelId = 5921, Description = "Costo de Ventas de Mercancías (814 – 817)" },
            new RowApiModel { Id = 10, ModelId = 5921, Description = "Costo por Exportación de Servicios (818)" },
            new RowApiModel { Id = 11, ModelId = 5921, Description = "Utilidad o Pérdida Bruta en Ventas" },
            new RowApiModel { Id = 12, ModelId = 5921, Description = "Menos: Gastos de Distribución y Ventas (819 – 821)" },
            new RowApiModel { Id = 13, ModelId = 5921, Description = "Utilidad o Pérdida Neta en Ventas" },
            new RowApiModel { Id = 14, ModelId = 5921, Description = "Menos: Gastos Generales y de Administración (822 – 824)" },
            new RowApiModel { Id = 15, ModelId = 5921, Description = "Gastos de Operación (826 – 833)" },
            new RowApiModel { Id = 16, ModelId = 5921, Description = "Gastos de Administración de la OSDE (834)" },
            new RowApiModel { Id = 17, ModelId = 5921, Description = "Utilidad o Pérdida en Operaciones" },
            new RowApiModel { Id = 18, ModelId = 5921, Description = "Menos: Gastos de Proyectos (825)" },
            new RowApiModel { Id = 19, ModelId = 5921, Description = "Gastos Financieros (835 – 838)" },
            new RowApiModel { Id = 20, ModelId = 5921, Description = "Gastos por Pérdidas en Tasa de Cambio (839)" },
            new RowApiModel { Id = 21, ModelId = 5921, Description = "Financiamiento Entregado a la OSDE (840)" },
            new RowApiModel { Id = 22, ModelId = 5921, Description = "Gastos por Estadía – Importadores (841)" },
            new RowApiModel { Id = 23, ModelId = 5921, Description = "Gastos por Estadía – Otras Entidades (843)" },
            new RowApiModel { Id = 24, ModelId = 5921, Description = "Gastos por Pérdidas (845 – 848)" },
            new RowApiModel { Id = 25, ModelId = 5921, Description = "Gastos por Pérdidas-Desastres (849)" },
            new RowApiModel { Id = 26, ModelId = 5921, Description = "Gastos por Faltantes de Bienes (850 – 854)" },
            new RowApiModel { Id = 27, ModelId = 5921, Description = "Otros Impuestos, Tasas y Contribuciones (855-864)" },
            new RowApiModel { Id = 28, ModelId = 5921, Description = "Otros Gastos (865 – 866)" },
            new RowApiModel { Id = 29, ModelId = 5921, Description = "Gastos de Eventos (867)" },
            new RowApiModel { Id = 30, ModelId = 5921, Description = "Gastos de Recuperación de Desastres (873)" },
            new RowApiModel { Id = 31, ModelId = 5921, Description = "Más: Ingresos Financieros (920 – 922)" },
            new RowApiModel { Id = 32, ModelId = 5921, Description = "Financiamiento Recibido de las Empresas (923)" },
            new RowApiModel { Id = 33, ModelId = 5921, Description = "Ingresos por Variación de Tasa de Cambio (924)" },
            new RowApiModel { Id = 34, ModelId = 5921, Description = "Ingresos por Dividendos Ganados (925)" },
            new RowApiModel { Id = 35, ModelId = 5921, Description = "Ingresos por Estadía (navieras y operadores) (926– 927)" },
            new RowApiModel { Id = 36, ModelId = 5921, Description = "Ingresos por Recobro de Estadía (importadores y otras entidades) (928 – 929)" },
            new RowApiModel { Id = 37, ModelId = 5921, Description = "Ingresos por Sobrantes  (930 – 939)" },
            new RowApiModel { Id = 38, ModelId = 5921, Description = "Otros Ingresos (950 – 952)" },
            new RowApiModel { Id = 39, ModelId = 5921, Description = "Ingresos por Donaciones Recibidas (953)" },
            new RowApiModel { Id = 40, ModelId = 5921, Description = "Utilidad o Pérdida antes del Impuesto." },
            #endregion

            #region 5924
            new RowApiModel { Id = 1, ModelId = 5924, Description = "Materias Primas y Materiales" },
            new RowApiModel { Id = 2, ModelId = 5924, Description = "Combustibles y Lubricantes" },
            new RowApiModel { Id = 3, ModelId = 5924, Description = "Energía" },
            new RowApiModel { Id = 4, ModelId = 5924, Description = "Salario" },
            new RowApiModel { Id = 5, ModelId = 5924, Description = "De ellos: Salario Escala" },
            new RowApiModel { Id = 6, ModelId = 5924, Description = "Pagos Adicional del Perfeccionamiento Empresarial" },
            new RowApiModel { Id = 7, ModelId = 5924, Description = "Otros Pagos Adicionales" },
            new RowApiModel { Id = 8, ModelId = 5924, Description = "Pago por Resultado" },
            new RowApiModel { Id = 9, ModelId = 5924, Description = "Acumulación de Vacaciones (9.09%)" },
            new RowApiModel { Id = 10, ModelId = 5924, Description = "Depreciación y Amortización" },
            new RowApiModel { Id = 11, ModelId = 5924, Description = "Otros Gastos Monetarios" },
            new RowApiModel { Id = 12, ModelId = 5924, Description = "De ellos: Servicios Comprados entre entidades" },
            new RowApiModel { Id = 13, ModelId = 5924, Description = "Servicios de Mantenimiento y Reparación Constructivo" },
            new RowApiModel { Id = 14, ModelId = 5924, Description = "Reparación y Mantenimiento de Viales" },
            new RowApiModel { Id = 15, ModelId = 5924, Description = "Otros Servicios de Mantenimiento y Reparaciones Corrientes" },
            new RowApiModel { Id = 16, ModelId = 5924, Description = "Gastos por importación de servicios" },
            new RowApiModel { Id = 17, ModelId = 5924, Description = "Viáticos" },
            new RowApiModel { Id = 18, ModelId = 5924, Description = "TOTAL DE GASTOS POR ELEMENTOS" },
            #endregion

            #region 5925
            new RowApiModel { Id = 1, ModelId = 5925, Description = "SECCIÓN I : INVERSIONES" },
            new RowApiModel { Id = 2, ModelId = 5925, Description = "Inversiones en Proceso (265 – 278)" },
            new RowApiModel { Id = 3, ModelId = 5925, Description = "Construcción y Montaje (0010)" },
            new RowApiModel { Id = 4, ModelId = 5925, Description = "Equipos (0020)" },
            new RowApiModel { Id = 5, ModelId = 5925, Description = "Otros Gastos (0030)" },
            new RowApiModel { Id = 6, ModelId = 5925, Description = "Fomentos Agrícolas (0050)" },
            new RowApiModel { Id = 7, ModelId = 5925, Description = "Fomentos y Desarrollo Mineros (0060)" },
            new RowApiModel { Id = 8, ModelId = 5925, Description = "Fomentos y Desarrollo Forestales (0070)" },
            new RowApiModel { Id = 9, ModelId = 5925, Description = "Otros No Especificados (0100)" },
            new RowApiModel { Id = 10, ModelId = 5925, Description = "Fondo de Fomento Desarrollo Ganadero (0200)" },
            new RowApiModel { Id = 11, ModelId = 5925, Description = "Adquisición de Activos Fijos Tangibles Nuevos (290 – 0100 y 0300)" },
            new RowApiModel { Id = 12, ModelId = 5925, Description = "Compra de Activos Fijos Tangibles de Uso (291 – 0100 y 0300)" },
            new RowApiModel { Id = 13, ModelId = 5925, Description = "Compra de Activos Fijos Intangibles (292 – 0100 y 0300)" },
            new RowApiModel { Id = 14, ModelId = 5925, Description = "SECCIÓN II: INVERSIONES FINANCIERAS" },
            new RowApiModel { Id = 15, ModelId = 5925, Description = "Inversiones a Largo Plazo o Permanentes (225 – 234)" },
            new RowApiModel { Id = 16, ModelId = 5925, Description = "La fila 16 no aparece en el DataDin pero sí en los modelos de empresa" },
            #endregion

            #region 5926
            new RowApiModel { Id = 1, ModelId = 5926, Description = "Ventas o Ingresos Netos" },
            new RowApiModel { Id = 2, ModelId = 5926, Description = "Más: Financiamiento Recibido de las Empresas (923)" },
            new RowApiModel { Id = 3, ModelId = 5926, Description = "Más: Saldo de la Cuenta Producción en Proceso (700 – 724)" },
            new RowApiModel { Id = 4, ModelId = 5926, Description = "Menos: Saldo de la Cuenta Producción en Proceso al Inicio del Año (700 – 724)" },
            new RowApiModel { Id = 5, ModelId = 5926, Description = "Más: Saldo de la Cuenta Producción Terminada (188)" },
            new RowApiModel { Id = 6, ModelId = 5926, Description = "Menos: Saldo de la Cuenta Producción Terminada al Inicio del Año (188)" },
            new RowApiModel { Id = 7, ModelId = 5926, Description = "Menos: Aumento de la existencia de Producción Terminada por conceptos distintos al de producción y entrega (188 – 0040)" },
            new RowApiModel { Id = 8, ModelId = 5926, Description = "Más: Disminución de la existencia de Producción Terminada por conceptos distintos al de producción y entrega (188 – 0050)" },
            new RowApiModel { Id = 9, ModelId = 5926, Description = "Más: Gastos diferidos del período relacionados con procesos productivos y de servicios (300 – 305 – 0020)" },
            new RowApiModel { Id = 10, ModelId = 5926, Description = "Más: Gastos del período de las producciones destinadas al insumo (725 – 0020)" },
            new RowApiModel { Id = 11, ModelId = 5926, Description = "Más: Saldo de las cuentas de Otros Ingresos e Ingresos Financieros (Excepto variación de tasas de cambio y los dividendos) (920 – 922 + 950 – 952)" },
            new RowApiModel { Id = 12, ModelId = 5926, Description = "Menos: Gastos incorporados a las producciones en proceso proveniente del almacén de las producciones de insumo o autoconsumo (196 – 0030)" },
            new RowApiModel { Id = 13, ModelId = 5926, Description = "Más: Gastos del período de Reparaciones Capitales con Medios Propios (726 – 0020)" },
            new RowApiModel { Id = 14, ModelId = 5926, Description = "Más: Gastos del período de las Inversiones con Medios Propios Activos Fijos Intangibles (727 - 0020)" },
            new RowApiModel { Id = 15, ModelId = 5926, Description = "Más: Gastos del período por Inversiones con Medios Propios (728 – 0020)" },
            new RowApiModel { Id = 16, ModelId = 5926, Description = "Menos: Saldo de la cuenta Costo de Ventas de Mercancías (814 – 817)" },
            new RowApiModel { Id = 17, ModelId = 5926, Description = "PRODUCCIÓN DE BIENES Y SERVICIOS" },
            new RowApiModel { Id = 18, ModelId = 5926, Description = "Gasto Material" },
            new RowApiModel { Id = 19, ModelId = 5926, Description = "Otros Gastos Monetarios" },
            new RowApiModel { Id = 20, ModelId = 5926, Description = "Financiamiento entregado a la OSDE (840)" },
            new RowApiModel { Id = 21, ModelId = 5926, Description = "CONSUMO INTERMEDIO" },
            new RowApiModel { Id = 22, ModelId = 5926, Description = "VALOR AGREGADO BRUTO CREADO" },
            #endregion
            */
        };
    }
}
