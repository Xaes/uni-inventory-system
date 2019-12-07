CREATE DATABASE inventorydb;
USE inventorydb;

/** CREANDO TABLAS **/

IF NOT EXISTS (SELECT  * FROM sysobjects WHERE name='Bodega' and xtype='U')
    CREATE TABLE Bodega
    (
		Bodega_ID int IDENTITY PRIMARY KEY,
        Codigo VARCHAR(5) NOT NULL UNIQUE,
        Nombre VARCHAR(30) NOT NULL
    );

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Pasillo' and xtype='U')
    CREATE TABLE Pasillo
    (
		Pasillo_ID int IDENTITY,
        Codigo VARCHAR(5),
        Bodega_ID int NOT NULL FOREIGN KEY REFERENCES Bodega(Bodega_ID),

        CONSTRAINT PK_Pasillo PRIMARY KEY NONCLUSTERED (Pasillo_ID),
        CONSTRAINT UK_Pasillo UNIQUE (Bodega_ID, Codigo)
    );

IF NOT EXISTS (SELECT  * FROM sysobjects WHERE name='Estante' and xtype='U')
    CREATE TABLE Estante
    (
        Estante_ID int IDENTITY,
        Codigo VARCHAR(5),
        Pasillo_ID int FOREIGN KEY REFERENCES Pasillo(Pasillo_ID),
        Secuencia_Loc int,

        CONSTRAINT PK_Estante PRIMARY KEY NONCLUSTERED (Estante_ID),
        CONSTRAINT UK_Estante UNIQUE (Pasillo_ID, Codigo)
    );

IF NOT EXISTS (SELECT  * FROM sysobjects WHERE name='Localizacion' and xtype='U')
    CREATE TABLE Localizacion
    (
        Codigo VARCHAR(25),
        Estante_ID int FOREIGN KEY REFERENCES Estante(Estante_ID),

        CONSTRAINT PK_Localizacion PRIMARY KEY NONCLUSTERED (Codigo),
        CONSTRAINT UK_Localizacion UNIQUE (Estante_ID, Codigo)
    );