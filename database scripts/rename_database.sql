SELECT * FROM dbo.Users

USE [master];
GO
ALTER DATABASE [dbo.TimeIn] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
GO
EXEC sp_renamedb N'dbo.TimeIn', N'TimeIn'

USE [master];
DROP DATABASE TimeIn
GO
