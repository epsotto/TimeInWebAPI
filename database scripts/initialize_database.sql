USE master;
GO

IF DB_ID (N'TimeIn') IS NULL
CREATE DATABASE TimeIn;
GO

USE TimeIn

IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'Users'))
BEGIN
    --Do Stuff
	CREATE TABLE [TimeIn].[dbo].[Users] (
	UserKey INT PRIMARY KEY IDENTITY(10000,1)
	,FirstName VARCHAR(250) NOT NULL
	,LastName VARCHAR(250) NOT NULL
	,UserName VARCHAR(250) NOT NULL
	,UserPassword VARCHAR(50) NOT NULL
	,IsActive BIT NOT NULL
	,CreateDttm DATETIME NOT NULL
	,CreateUserId VARCHAR(250) NOT NULL
	,UpdateDttm DATETIME NOT NULL
	,UpdateUserId VARCHAR(250) NOT NULL
	)
END

IF(NOT EXISTS(SELECT * 
				FROM INFORMATION_SCHEMA.TABLES
				WHERE TABLE_SCHEMA = 'dbo'
				AND TABLE_NAME = 'Activity'))
BEGIN
	CREATE TABLE [TimeIn].[dbo].[Activity] (
	ActivityId INT PRIMARY KEY IDENTITY(10000,1)
	,ActivityNm VARCHAR(250) NOT NULL
	,IsActive BIT NOT NULL
	,CreateDttm DATETIME NOT NULL
	,CreateUserId VARCHAR(250) NOT NULL
	,UpdateDttm DATETIME NOT NULL
	,UpdateUserId VARCHAR(250) NOT NULL
	)
END

IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'DailyTimeIn'))
BEGIN
    --Do Stuff
	CREATE TABLE [TimeIn].[dbo].[DailyTimeIn] (
	TimeInId INT PRIMARY KEY IDENTITY(10000,1)
	,EmployeeId INT NOT NULL
	,ActivityCd INT NOT NULL
	,IsActive BIT NOT NULL
	,TimeInDttm DATETIME NOT NULL
	,CreateDttm DATETIME NOT NULL
	,CreateUserId VARCHAR(250) NOT NULL
	,UpdateDttm DATETIME NOT NULL
	,UpdateUserId VARCHAR(250) NOT NULL
	,CONSTRAINT FK_User_UserKey FOREIGN KEY (EmployeeId)
		REFERENCES [TimeIn].[dbo].[Users] (UserKey)
	,CONSTRAINT FK_Activity_ActivityId FOREIGN KEY (ActivityCd)
		REFERENCES [TimeIn].[dbo].[Activity] (ActivityId)
	)
END

IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'DailyTimeOut'))
BEGIN
    --Do Stuff
	CREATE TABLE [TimeIn].[dbo].[DailyTimeOut] (
	TimeInId INT PRIMARY KEY IDENTITY(10000,1)
	,EmployeeId INT NOT NULL
	,ActivityCd INT NOT NULL
	,IsActive BIT NOT NULL
	,TimeOutDttm DATETIME NOT NULL
	,CreateDttm DATETIME NOT NULL
	,CreateUserId VARCHAR(250) NOT NULL
	,UpdateDttm DATETIME NOT NULL
	,UpdateUserId VARCHAR(250) NOT NULL
	,CONSTRAINT FK_User_UserKey_TimeOut FOREIGN KEY (EmployeeId)
		REFERENCES [TimeIn].[dbo].[Users] (UserKey)
	,CONSTRAINT FK_Activity_ActivityId_TimeOut FOREIGN KEY (ActivityCd)
		REFERENCES [TimeIn].[dbo].[Activity] (ActivityId)
	)
END