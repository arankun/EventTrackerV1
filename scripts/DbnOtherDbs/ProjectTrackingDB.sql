USE [master]
GO

/****** Object:  Database [ProjectTrackingDB]    Script Date: 10/16/2015 1:49:20 AM ******/
DROP DATABASE [ProjectTrackingDB]
GO

/****** Object:  Database [ProjectTrackingDB]    Script Date: 10/16/2015 1:49:21 AM ******/
CREATE DATABASE [ProjectTrackingDB] ON  PRIMARY 
( NAME = N'ProjectTrackingDB', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL10.SQLEXPRESS\MSSQL\DATA\ProjectTrackingDB.mdf' , SIZE = 2304KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'ProjectTrackingDB_log', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL10.SQLEXPRESS\MSSQL\DATA\ProjectTrackingDB_log.LDF' , SIZE = 504KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO

ALTER DATABASE [ProjectTrackingDB] SET COMPATIBILITY_LEVEL = 100
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ProjectTrackingDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [ProjectTrackingDB] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [ProjectTrackingDB] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [ProjectTrackingDB] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [ProjectTrackingDB] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [ProjectTrackingDB] SET ARITHABORT OFF 
GO

ALTER DATABASE [ProjectTrackingDB] SET AUTO_CLOSE ON 
GO

ALTER DATABASE [ProjectTrackingDB] SET AUTO_CREATE_STATISTICS ON 
GO

ALTER DATABASE [ProjectTrackingDB] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [ProjectTrackingDB] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [ProjectTrackingDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [ProjectTrackingDB] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [ProjectTrackingDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [ProjectTrackingDB] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [ProjectTrackingDB] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [ProjectTrackingDB] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [ProjectTrackingDB] SET  ENABLE_BROKER 
GO

ALTER DATABASE [ProjectTrackingDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [ProjectTrackingDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [ProjectTrackingDB] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [ProjectTrackingDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [ProjectTrackingDB] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [ProjectTrackingDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [ProjectTrackingDB] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [ProjectTrackingDB] SET RECOVERY SIMPLE 
GO

ALTER DATABASE [ProjectTrackingDB] SET  MULTI_USER 
GO

ALTER DATABASE [ProjectTrackingDB] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [ProjectTrackingDB] SET DB_CHAINING OFF 
GO

ALTER DATABASE [ProjectTrackingDB] SET  READ_WRITE 
GO

