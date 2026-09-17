CREATE DATABASE RIFADB;
GO

USE RIFADB;
GO

CREATE SEQUENCE seqplanning
    START WITH 1
    INCREMENT BY 1;
GO

CREATE TABLE tmst_planning (
    planning_cd   VARCHAR(50)  NULL PRIMARY KEY,
    emp_nm        VARCHAR(150) NULL,
    total         INT          NULL,
    created_at    DATETIME     NULL DEFAULT GETDATE(),
    isactive      BIT          NULL DEFAULT 0
);
GO

CREATE TABLE tplanning_user (
    planning_cd   VARCHAR(50) NULL PRIMARY KEY,
    senin         INT NULL,
    selasa        INT NULL,
    rabu          INT NULL,
    kamis         INT NULL,
    jumat         INT NULL,
    sabtu         INT NULL,
    minggu        INT NULL,
    CONSTRAINT fk_user_planning FOREIGN KEY (planning_cd)
        REFERENCES tmst_planning(planning_cd)
);
GO

CREATE TABLE tplanning_recommendation (
    planning_cd   VARCHAR(50) NULL PRIMARY KEY,
    senin         INT NULL,
    selasa        INT NULL,
    rabu          INT NULL,
    kamis         INT NULL,
    jumat         INT NULL,
    sabtu         INT NULL,
    minggu        INT NULL,
    CONSTRAINT fk_rekomendasi_planning FOREIGN KEY (planning_cd)
        REFERENCES tmst_planning(planning_cd)
);
GO