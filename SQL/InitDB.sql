/** CREATE DATABASE inventorydb; **/

/** CREANDO TABLAS **/
/** DOMINIO: LOCALIZACION **/

IF NOT EXISTS (SELECT  * FROM sysobjects WHERE name='Bodega' and xtype='U')
    CREATE TABLE Bodega
    (
		Bodega_ID int IDENTITY PRIMARY KEY,
        Codigo VARCHAR(5) NOT NULL,
        Nombre VARCHAR(30) NOT NULL UNIQUE,

        CONSTRAINT UK_Bodega UNIQUE (Codigo)
    );

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Pasillo' and xtype='U')
    CREATE TABLE Pasillo
    (
		Pasillo_ID int IDENTITY,
        Codigo VARCHAR(5) NOT NULL,
        FK_Bodega_ID int NOT NULL FOREIGN KEY REFERENCES Bodega(Bodega_ID),

        CONSTRAINT PK_Pasillo PRIMARY KEY NONCLUSTERED (Pasillo_ID),
        CONSTRAINT UK_Pasillo UNIQUE (Codigo)
    );

IF NOT EXISTS (SELECT  * FROM sysobjects WHERE name='Estante' and xtype='U')
    CREATE TABLE Estante
    (
        Estante_ID int IDENTITY,
        Codigo VARCHAR(5) NOT NULL,
        FK_Pasillo_ID int FOREIGN KEY REFERENCES Pasillo(Pasillo_ID),
        Secuencia_Loc int,

        CONSTRAINT PK_Estante PRIMARY KEY NONCLUSTERED (Estante_ID),
        CONSTRAINT UK_Estante UNIQUE (Codigo)
    );

IF NOT EXISTS (SELECT  * FROM sysobjects WHERE name='Localizacion' and xtype='U')
    CREATE TABLE Localizacion
    (
        Codigo VARCHAR(25) NOT NULL UNIQUE,
        FK_Estante_ID int FOREIGN KEY REFERENCES Estante(Estante_ID),

        CONSTRAINT PK_Localizacion PRIMARY KEY NONCLUSTERED (Codigo),
        CONSTRAINT UK_Localizacion UNIQUE (Codigo)
    );

/** DOMINIO: PRODUCTO **/

IF NOT EXISTS (SELECT  * FROM sysobjects WHERE name='MarcaRepuesto' and xtype='U')
    CREATE TABLE MarcaRepuesto
    (
        MarcaRepuesto_ID int IDENTITY,
        EsGenerica bit NOT NULL,
        Nombre VARCHAR(30) NOT NULL UNIQUE,

        CONSTRAINT PK_MarcaRepuesto PRIMARY KEY NONCLUSTERED (MarcaRepuesto_ID)
    )

IF NOT EXISTS (SELECT  * FROM sysobjects WHERE name='Categoria' and xtype='U')
    CREATE TABLE Categoria
    (
        Categoria_ID int IDENTITY,
        Nombre VARCHAR(30) UNIQUE,
        Descripcion VARCHAR(120)

        CONSTRAINT PK_Categoria PRIMARY KEY NONCLUSTERED (Categoria_ID)
    )

IF NOT EXISTS (SELECT  * FROM sysobjects WHERE name='SubCategoria' and xtype='U')
    CREATE TABLE SubCategoria
    (
        SubCategoria_ID int IDENTITY,
        FK_Categoria_ID int FOREIGN KEY REFERENCES Categoria(Categoria_ID),
        Nombre VARCHAR(30) UNIQUE,
        Descripcion VARCHAR(120)

        CONSTRAINT PK_SubCategoria PRIMARY KEY NONCLUSTERED (SubCategoria_ID)
    )

IF NOT EXISTS (SELECT  * FROM sysobjects WHERE name='ModeloVehiculo' and xtype='U')
    CREATE TABLE ModeloVehiculo
    (
        ModeloVehiculo_ID int IDENTITY,
        NombreModelo VARCHAR(30) NOT NULL,
        Marca VARCHAR(30) NOT NULL,
        Version VARCHAR(30)

        CONSTRAINT PK_ModeloVehiculo PRIMARY KEY NONCLUSTERED (ModeloVehiculo_ID)
    )

IF NOT EXISTS (SELECT  * FROM sysobjects WHERE name='Repuesto' and xtype='U')
    CREATE TABLE Repuesto
    (
        Repuesto_ID int IDENTITY,
        FK_MarcaRepuesto_ID int FOREIGN KEY REFERENCES MarcaRepuesto(MarcaRepuesto_ID),
        FK_SubCategoria_ID int FOREIGN KEY REFERENCES SubCategoria(SubCategoria_ID),
        NumeroParteCompatible int NOT NULL,
        NumeroParte int NOT NULL,
        UnidadesPorPaquete int,
        UnidadesEmpaque int,
        CodigoRepuesto VARCHAR(30) NOT NULL,
        UnidadMedida VARCHAR(10),
        Descripcion VARCHAR(120),
        Nombre VARCHAR(50) NOT NULL UNIQUE,

        CONSTRAINT PK_Repuesto PRIMARY KEY NONCLUSTERED (Repuesto_ID),
        CONSTRAINT UK_Repuesto UNIQUE (CodigoRepuesto, Repuesto_ID)
    )

IF NOT EXISTS (SELECT  * FROM sysobjects WHERE name='RepuestoCompatibilidad' and xtype='U')
    CREATE TABLE RepuestoCompatibilidad
    (
        FK_Repuesto_ID int FOREIGN KEY REFERENCES Repuesto(Repuesto_ID),
        FK_ModeloVehiculo_ID int FOREIGN KEY REFERENCES ModeloVehiculo(ModeloVehiculo_ID)

        CONSTRAINT PK_RepuestoCompatibilidad PRIMARY KEY NONCLUSTERED (FK_Repuesto_ID, FK_ModeloVehiculo_ID)
    )

/** DOMINIO: INVENTARIO **/

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Periodo' and xtype='U')
    CREATE TABLE Periodo
    (
        Periodo_Id int IDENTITY,
        Periodo_Fiscal int NOT NULL,
        FechaInicio date NOT NULL,
        FechaFinal date NOT NULL,
        Estado VARCHAR(15) NOT NULL CHECK (Estado IN('ABIERTO', 'CERRADO', 'NOUSADO')),
        Nombre VARCHAR(20) NOT NULL UNIQUE,

        CONSTRAINT PK_Periodo PRIMARY KEY NONCLUSTERED (Periodo_Id),
        CONSTRAINT UK_Periodo UNIQUE (Periodo_Fiscal)
    )

IF NOT EXISTS (SELECT  * FROM sysobjects WHERE name='Movimiento' and xtype='U')
    CREATE TABLE Movimiento
    (
        Movimiento_ID int IDENTITY,
        FK_LocalizacionInicial_ID VARCHAR(25) FOREIGN KEY REFERENCES Localizacion(Codigo),
        FK_LocalizacionFinal_ID VARCHAR(25) FOREIGN KEY REFERENCES Localizacion(Codigo),
        FK_Periodo_ID int FOREIGN KEY REFERENCES Periodo(Periodo_Id),
        CostoTotal numeric(12, 2) NOT NULL,
        CostoUnitario numeric(12, 2) NOT NULL,
        PrecioVentaUnitario numeric(12, 2) NOT NULL,
        Unidades int NOT NULL,
        Fecha datetime NOT NULL,
        TipoTransaccion VARCHAR(20) NOT NULL

        CONSTRAINT PK_Movimiento PRIMARY KEY NONCLUSTERED (Movimiento_ID)
    )

IF NOT EXISTS (SELECT  * FROM sysobjects WHERE name='Costo' and xtype='U')
    CREATE TABLE Costo
    (
        Costo_ID int IDENTITY,
        FK_Repuesto_ID int NOT NULL FOREIGN KEY REFERENCES Repuesto(Repuesto_ID),
        FechaEntrada datetime NOT NULL,
        CostoUnitario numeric(12, 2) NOT NULL,
        Unidades int NOT NULL

        CONSTRAINT PK_Costo PRIMARY KEY NONCLUSTERED (Costo_ID)
    )

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Existencia' and xtype='U')
    CREATE TABLE Existencia
    (
        Existecia_ID int IDENTITY,
        FK_Repuesto_ID int NOT NULL FOREIGN KEY REFERENCES Repuesto(Repuesto_ID),
        FK_Localizacion_ID VARCHAR(25) NOT NULL FOREIGN KEY REFERENCES Localizacion(Codigo),
        Unidades int NOT NULL,

        CONSTRAINT PK_Existencia PRIMARY KEY NONCLUSTERED (Existecia_ID)
    )

/** DOMINIO: PROVEEDOR **/

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Proveedor' and xtype='U')
    CREATE TABLE Proveedor
    (
        Proveedor_ID int IDENTITY,
        Nombre varchar(30) NOT NULL UNIQUE,

        CONSTRAINT PK_Proveedor PRIMARY KEY NONCLUSTERED (Proveedor_ID)
    )

/** DOMINIO: DOCUMENTO **/

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='TipoDocumento' and xtype='U')
    CREATE TABLE TipoDocumento
    (
        TipoDocumento_ID int IDENTITY,
        UltimoNumDoc VARCHAR(30),
        Nombre VARCHAR(30) UNIQUE,
        CambiaUnidades bit NOT NULL,
        CambiaCosto bit NOT NULL

        CONSTRAINT PK_TipoDocumento PRIMARY KEY NONCLUSTERED (TipoDocumento_ID)
    )

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Documento' and xtype='U')
    CREATE TABLE Documento
    (
        Documento_ID int IDENTITY,
        FK_ProveedorID int FOREIGN KEY REFERENCES Proveedor(Proveedor_ID),
        FK_TipoDocumentoID int NOT NULL FOREIGN KEY REFERENCES TipoDocumento(TipoDocumento_ID),
        NumeroDoc int NOT NULL,
        Fecha datetime NOT NULL

        CONSTRAINT PK_Documento PRIMARY KEY NONCLUSTERED (Documento_ID)
    )

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='LineaDocumento' and xtype='U')
    CREATE TABLE LineaDocumento
    (
        LineaDocumento_ID int IDENTITY,
        FK_DocumentoID int FOREIGN KEY REFERENCES Documento(Documento_ID),
        FK_MovimientoID int FOREIGN KEY REFERENCES Movimiento(Movimiento_ID),
        FK_RepuestoID int FOREIGN KEY REFERENCES Repuesto(Repuesto_ID),
        FK_BodegaID int FOREIGN KEY REFERENCES Bodega(Bodega_ID),
        Unidades int NOT NULL,
        UnidadesNoRecibidas int,
        UnidadesDanadas int,
        CantidadPaquetes int,
        CostoUnitario numeric(12, 2) NOT NULL,
        PrecioVentaUnitario numeric(12, 2)

        CONSTRAINT PK_LineaDocumento PRIMARY KEY NONCLUSTERED (LineaDocumento_ID)
    )

