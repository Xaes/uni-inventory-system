CREATE DATABASE inventorydb;
USE inventorydb;

/** CREATING TABLES **/

IF NOT EXISTS (SELECT  * FROM sysobjects WHERE name='Storage' and xtype='U')
    CREATE TABLE Storage
    (
        ID int IDENTITY(1,1) PRIMARY KEY,
        Code VARCHAR(30) NOT NULL,
        Name VARCHAR(30) NOT NULL
    );