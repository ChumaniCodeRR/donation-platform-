USE [master]
GO
/****** Object:  Database [PPSDonation]    Script Date: 2018/10/23 1:34:54 PM ******/
CREATE DATABASE [PPSDonation]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'PPSDonation', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL11.SQL2012\MSSQL\DATA\PPSDonation.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'PPSDonation_log', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL11.SQL2012\MSSQL\DATA\PPSDonation_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [PPSDonation] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [PPSDonation].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [PPSDonation] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [PPSDonation] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [PPSDonation] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [PPSDonation] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [PPSDonation] SET ARITHABORT OFF 
GO
ALTER DATABASE [PPSDonation] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [PPSDonation] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [PPSDonation] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [PPSDonation] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [PPSDonation] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [PPSDonation] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [PPSDonation] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [PPSDonation] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [PPSDonation] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [PPSDonation] SET  DISABLE_BROKER 
GO
ALTER DATABASE [PPSDonation] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [PPSDonation] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [PPSDonation] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [PPSDonation] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [PPSDonation] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [PPSDonation] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [PPSDonation] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [PPSDonation] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [PPSDonation] SET  MULTI_USER 
GO
ALTER DATABASE [PPSDonation] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [PPSDonation] SET DB_CHAINING OFF 
GO
ALTER DATABASE [PPSDonation] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [PPSDonation] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [PPSDonation]
GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 2018/10/23 1:34:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ContextKey] [nvarchar](300) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 2018/10/23 1:34:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 2018/10/23 1:34:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
	[User_Id] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 2018/10/23 1:34:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[UserId] [nvarchar](128) NOT NULL,
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 2018/10/23 1:34:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](128) NOT NULL,
	[RoleId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 2018/10/23 1:34:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](128) NOT NULL,
	[UserName] [nvarchar](max) NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[Discriminator] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Donation]    Script Date: 2018/10/23 1:34:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Donation](
	[DonationID] [int] IDENTITY(1,1) NOT NULL,
	[DonationName] [varchar](max) NOT NULL,
	[DonorID] [int] NOT NULL,
	[DonationReference] [varchar](max) NULL,
	[GUID] [uniqueidentifier] NOT NULL,
	[TransactionDate] [datetime] NOT NULL,
	[DonationStatusID] [int] NULL,
	[Amount] [float] NULL,
	[FeedbackDate] [datetime] NULL,
	[InsertDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_Donation] PRIMARY KEY CLUSTERED 
(
	[DonationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DonationCertificate]    Script Date: 2018/10/23 1:34:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DonationCertificate](
	[CertificateID] [int] IDENTITY(1,1) NOT NULL,
	[CertificateDate] [datetime] NOT NULL,
	[DonationID] [int] NOT NULL,
	[SendDate] [datetime] NULL,
	[SendStatus] [varchar](50) NULL,
	[CertificateReference] [varchar](100) NULL,
	[InsertDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NULL,
	[Comment] [varchar](max) NULL,
	[SentEmail] [varchar](50) NULL,
 CONSTRAINT [PK_DonationCertificate] PRIMARY KEY CLUSTERED 
(
	[CertificateID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DonationStatus]    Script Date: 2018/10/23 1:34:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DonationStatus](
	[DonationStatusID] [int] IDENTITY(1,1) NOT NULL,
	[StatusName] [varchar](150) NOT NULL,
	[Description] [varchar](max) NULL,
	[InsertDate] [datetime] NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_DonationStatus] PRIMARY KEY CLUSTERED 
(
	[DonationStatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Donor]    Script Date: 2018/10/23 1:34:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Donor](
	[DonorID] [int] IDENTITY(1,1) NOT NULL,
	[DonorTypeID] [int] NOT NULL,
	[ContactName] [varchar](150) NOT NULL,
	[ContactEmail] [varchar](100) NULL,
	[TaxNumber] [varchar](50) NULL,
	[FirstName] [varchar](50) NULL,
	[LastName] [varchar](50) NULL,
	[IsMember] [bit] NOT NULL,
	[MembershipNumber] [varchar](150) NULL,
	[State] [varchar](50) NULL,
	[Country] [varchar](50) NOT NULL,
	[City] [varchar](50) NULL,
	[PostCode] [varchar](50) NULL,
	[Address1] [varchar](200) NULL,
	[Address2] [varchar](200) NULL,
	[ContactPhoneNumber] [varchar](50) NULL,
	[Gender] [varchar](50) NULL,
	[RegistrationNumber] [varchar](50) NULL,
	[OrganizationEmail] [varchar](50) NULL,
	[OrganizationName] [varchar](150) NULL,
	[InsertDate] [datetime] NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_Donor] PRIMARY KEY CLUSTERED 
(
	[DonorID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DonorType]    Script Date: 2018/10/23 1:34:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DonorType](
	[DonorTypeID] [int] IDENTITY(1,1) NOT NULL,
	[DonorTypeName] [varchar](100) NOT NULL,
	[Description] [varchar](100) NULL,
	[UpdateDate] [datetime] NULL,
	[InsertDate] [datetime] NULL,
	[Updatedby] [varchar](100) NULL,
 CONSTRAINT [PK_DonorType] PRIMARY KEY CLUSTERED 
(
	[DonorTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Donation] ON 

INSERT [dbo].[Donation] ([DonationID], [DonationName], [DonorID], [DonationReference], [GUID], [TransactionDate], [DonationStatusID], [Amount], [FeedbackDate], [InsertDate], [ModifiedDate]) VALUES (5, N'PPS Individual Donation', 5, NULL, N'44eee437-41ac-488a-8390-4e444a2ca3c8', CAST(N'2018-10-23T10:39:58.517' AS DateTime), 1, 1500, NULL, CAST(N'2018-10-23T10:39:58.500' AS DateTime), NULL)
INSERT [dbo].[Donation] ([DonationID], [DonationName], [DonorID], [DonationReference], [GUID], [TransactionDate], [DonationStatusID], [Amount], [FeedbackDate], [InsertDate], [ModifiedDate]) VALUES (6, N'PPS Individual Donation', 6, NULL, N'7af209f8-549e-4301-aaf0-75e59c22714e', CAST(N'2018-10-23T11:57:33.473' AS DateTime), 1, 1500, NULL, CAST(N'2018-10-23T11:57:33.473' AS DateTime), NULL)
INSERT [dbo].[Donation] ([DonationID], [DonationName], [DonorID], [DonationReference], [GUID], [TransactionDate], [DonationStatusID], [Amount], [FeedbackDate], [InsertDate], [ModifiedDate]) VALUES (7, N'PPS Individual Donation', 7, NULL, N'b65ba1b1-0482-43ba-a12e-b74fac1bc7f8', CAST(N'2018-10-23T12:14:17.327' AS DateTime), 1, 1500, NULL, CAST(N'2018-10-23T12:14:17.327' AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[Donation] OFF
SET IDENTITY_INSERT [dbo].[DonationStatus] ON 

INSERT [dbo].[DonationStatus] ([DonationStatusID], [StatusName], [Description], [InsertDate], [ModifiedDate]) VALUES (1, N'CREATED', N'Transaction started but not processed yet', CAST(N'2018-10-22T10:57:20.930' AS DateTime), NULL)
INSERT [dbo].[DonationStatus] ([DonationStatusID], [StatusName], [Description], [InsertDate], [ModifiedDate]) VALUES (2, N'PENDING', N'Request sent to payfast and awaiting payment', CAST(N'2018-10-22T10:59:55.407' AS DateTime), NULL)
INSERT [dbo].[DonationStatus] ([DonationStatusID], [StatusName], [Description], [InsertDate], [ModifiedDate]) VALUES (3, N'CANCELLED', N'Transaction Cancelled', CAST(N'2018-10-22T11:00:28.777' AS DateTime), NULL)
INSERT [dbo].[DonationStatus] ([DonationStatusID], [StatusName], [Description], [InsertDate], [ModifiedDate]) VALUES (4, N'SUCCESSFUL', N'Payment Successfull', CAST(N'2018-10-22T11:00:54.910' AS DateTime), NULL)
INSERT [dbo].[DonationStatus] ([DonationStatusID], [StatusName], [Description], [InsertDate], [ModifiedDate]) VALUES (5, N'FAILED', N'Payfast send a notification showing that the payment was unsuccessful', CAST(N'2018-10-22T11:03:03.417' AS DateTime), NULL)
INSERT [dbo].[DonationStatus] ([DonationStatusID], [StatusName], [Description], [InsertDate], [ModifiedDate]) VALUES (6, N'CERTIFIED', N'Certificate sent to the donor', CAST(N'2018-10-22T11:04:07.603' AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[DonationStatus] OFF
SET IDENTITY_INSERT [dbo].[Donor] ON 

INSERT [dbo].[Donor] ([DonorID], [DonorTypeID], [ContactName], [ContactEmail], [TaxNumber], [FirstName], [LastName], [IsMember], [MembershipNumber], [State], [Country], [City], [PostCode], [Address1], [Address2], [ContactPhoneNumber], [Gender], [RegistrationNumber], [OrganizationEmail], [OrganizationName], [InsertDate], [ModifiedDate]) VALUES (1, 1, N'Marc Hurwitz', N'gilbertnganduk@gmail.com', N'9887654651', N'Marc', N'Hurwitz', 0, N'45654654', N'Gauteng', N'South Africa', N'Pretoria', N'2195', N'18 Lubi Street, Randburg', N'South Africa', N'+27729981533', N'Male', NULL, N'', NULL, CAST(N'2018-10-23T09:53:24.143' AS DateTime), NULL)
INSERT [dbo].[Donor] ([DonorID], [DonorTypeID], [ContactName], [ContactEmail], [TaxNumber], [FirstName], [LastName], [IsMember], [MembershipNumber], [State], [Country], [City], [PostCode], [Address1], [Address2], [ContactPhoneNumber], [Gender], [RegistrationNumber], [OrganizationEmail], [OrganizationName], [InsertDate], [ModifiedDate]) VALUES (2, 1, N'Marc Hurwitz', N'gilbertnganduk@gmail.com', N'9887654651', N'Marc', N'Hurwitz', 0, N'45654654', N'Gauteng', N'South Africa', N'Pretoria', N'2195', N'18 Lubi Street, Randburg', N'South Africa', N'+27729981533', N'Male', NULL, N'', NULL, CAST(N'2018-10-23T10:10:09.200' AS DateTime), NULL)
INSERT [dbo].[Donor] ([DonorID], [DonorTypeID], [ContactName], [ContactEmail], [TaxNumber], [FirstName], [LastName], [IsMember], [MembershipNumber], [State], [Country], [City], [PostCode], [Address1], [Address2], [ContactPhoneNumber], [Gender], [RegistrationNumber], [OrganizationEmail], [OrganizationName], [InsertDate], [ModifiedDate]) VALUES (3, 1, N'Marc Hurwitz', N'gilbertnganduk@gmail.com', N'9887654651', N'Marc', N'Hurwitz', 0, N'45654654', N'Gauteng', N'South Africa', N'Pretoria', N'2195', N'18 Lubi Street, Randburg', N'South Africa', N'+27729981533', N'Male', NULL, N'', NULL, CAST(N'2018-10-23T10:14:44.997' AS DateTime), NULL)
INSERT [dbo].[Donor] ([DonorID], [DonorTypeID], [ContactName], [ContactEmail], [TaxNumber], [FirstName], [LastName], [IsMember], [MembershipNumber], [State], [Country], [City], [PostCode], [Address1], [Address2], [ContactPhoneNumber], [Gender], [RegistrationNumber], [OrganizationEmail], [OrganizationName], [InsertDate], [ModifiedDate]) VALUES (4, 1, N'Marc Hurwitz', N'gilbertnganduk@gmail.com', N'9887654651', N'Marc', N'Hurwitz', 0, N'45654654', N'Gauteng', N'South Africa', N'Pretoria', N'2195', N'18 Lubi Street, Randburg', N'South Africa', N'+27729981533', N'Male', NULL, N'', NULL, CAST(N'2018-10-23T10:16:39.207' AS DateTime), NULL)
INSERT [dbo].[Donor] ([DonorID], [DonorTypeID], [ContactName], [ContactEmail], [TaxNumber], [FirstName], [LastName], [IsMember], [MembershipNumber], [State], [Country], [City], [PostCode], [Address1], [Address2], [ContactPhoneNumber], [Gender], [RegistrationNumber], [OrganizationEmail], [OrganizationName], [InsertDate], [ModifiedDate]) VALUES (5, 1, N'Marc Hurwitz', N'gilbertnganduk@gmail.com', N'9887654651', N'Marc', N'Hurwitz', 0, N'45654654', N'Gauteng', N'South Africa', N'Pretoria', N'2195', N'18 Lubi Street, Randburg', N'South Africa', N'+27729981533', N'Male', NULL, N'', NULL, CAST(N'2018-10-23T10:39:56.807' AS DateTime), NULL)
INSERT [dbo].[Donor] ([DonorID], [DonorTypeID], [ContactName], [ContactEmail], [TaxNumber], [FirstName], [LastName], [IsMember], [MembershipNumber], [State], [Country], [City], [PostCode], [Address1], [Address2], [ContactPhoneNumber], [Gender], [RegistrationNumber], [OrganizationEmail], [OrganizationName], [InsertDate], [ModifiedDate]) VALUES (6, 1, N'Marc Hurwitz', N'gilbertnganduk@gmail.com', N'9887654651', N'Marc', N'Hurwitz', 0, N'45654654', N'Gauteng', N'South Africa', N'Pretoria', N'2195', N'18 Lubi Street, Randburg', N'South Africa', N'+27729981533', N'Male', NULL, N'', NULL, CAST(N'2018-10-23T11:57:33.443' AS DateTime), NULL)
INSERT [dbo].[Donor] ([DonorID], [DonorTypeID], [ContactName], [ContactEmail], [TaxNumber], [FirstName], [LastName], [IsMember], [MembershipNumber], [State], [Country], [City], [PostCode], [Address1], [Address2], [ContactPhoneNumber], [Gender], [RegistrationNumber], [OrganizationEmail], [OrganizationName], [InsertDate], [ModifiedDate]) VALUES (7, 1, N'Marc Hurwitz', N'gilbertnganduk@gmail.com', N'9887654651', N'Marc', N'Hurwitz', 0, N'45654654', N'Gauteng', N'South Africa', N'Pretoria', N'2195', N'18 Lubi Street, Randburg', N'South Africa', N'+27729981533', N'Male', NULL, N'', NULL, CAST(N'2018-10-23T12:14:17.313' AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[Donor] OFF
SET IDENTITY_INSERT [dbo].[DonorType] ON 

INSERT [dbo].[DonorType] ([DonorTypeID], [DonorTypeName], [Description], [UpdateDate], [InsertDate], [Updatedby]) VALUES (1, N'Individual', N'Individual donation', NULL, CAST(N'2018-10-23T05:06:45.300' AS DateTime), NULL)
INSERT [dbo].[DonorType] ([DonorTypeID], [DonorTypeName], [Description], [UpdateDate], [InsertDate], [Updatedby]) VALUES (2, N'Organization', N'Organization donation', NULL, CAST(N'2018-10-23T05:06:54.897' AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[DonorType] OFF
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_User_Id]    Script Date: 2018/10/23 1:34:57 PM ******/
CREATE NONCLUSTERED INDEX [IX_User_Id] ON [dbo].[AspNetUserClaims]
(
	[User_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_UserId]    Script Date: 2018/10/23 1:34:57 PM ******/
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[AspNetUserLogins]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_RoleId]    Script Date: 2018/10/23 1:34:57 PM ******/
CREATE NONCLUSTERED INDEX [IX_RoleId] ON [dbo].[AspNetUserRoles]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_UserId]    Script Date: 2018/10/23 1:34:57 PM ******/
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[AspNetUserRoles]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Donation]    Script Date: 2018/10/23 1:34:57 PM ******/
ALTER TABLE [dbo].[Donation] ADD  CONSTRAINT [IX_Donation] UNIQUE NONCLUSTERED 
(
	[DonationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [UniqueGUID]    Script Date: 2018/10/23 1:34:57 PM ******/
ALTER TABLE [dbo].[Donation] ADD  CONSTRAINT [UniqueGUID] UNIQUE NONCLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Donation] ADD  CONSTRAINT [DF_Donation_GUID]  DEFAULT (newid()) FOR [GUID]
GO
ALTER TABLE [dbo].[DonationCertificate] ADD  CONSTRAINT [DF_DonationCertificateInsertDate]  DEFAULT (getdate()) FOR [InsertDate]
GO
ALTER TABLE [dbo].[DonationStatus] ADD  CONSTRAINT [DF_DonationStatus_InsertDate]  DEFAULT (getdate()) FOR [InsertDate]
GO
ALTER TABLE [dbo].[Donor] ADD  CONSTRAINT [DF_InsertDate]  DEFAULT (getdate()) FOR [InsertDate]
GO
ALTER TABLE [dbo].[DonorType] ADD  CONSTRAINT [DF_DonorTypeInsertDate]  DEFAULT (getdate()) FOR [InsertDate]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_User_Id] FOREIGN KEY([User_Id])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_User_Id]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[Donation]  WITH CHECK ADD FOREIGN KEY([DonationStatusID])
REFERENCES [dbo].[DonationStatus] ([DonationStatusID])
GO
ALTER TABLE [dbo].[Donation]  WITH CHECK ADD FOREIGN KEY([DonorID])
REFERENCES [dbo].[Donor] ([DonorID])
GO
ALTER TABLE [dbo].[DonationCertificate]  WITH CHECK ADD FOREIGN KEY([DonationID])
REFERENCES [dbo].[Donation] ([DonationID])
GO
ALTER TABLE [dbo].[Donor]  WITH CHECK ADD FOREIGN KEY([DonorTypeID])
REFERENCES [dbo].[DonorType] ([DonorTypeID])
GO
USE [master]
GO
ALTER DATABASE [PPSDonation] SET  READ_WRITE 
GO
