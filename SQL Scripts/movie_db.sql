/* Check whether database exists and drop it if it does */
IF EXISTS(SELECT 1 FROM master.dbo.sysdatabases WHERE NAME = 'movie_db')
BEGIN
	DROP DATABASE movie_db
	print '' print '*** dropping database movie_db ***'
END
GO

print '' print '*** creating movie_db ***'
GO
CREATE DATABASE movie_db
GO

print '' print '*** using database movie_db ***'
GO
USE [movie_db]
GO

print '' print '*** creating account table ***'
GO
CREATE TABLE [dbo].[Account]
(
	[AccountID]		[INT] 			IDENTITY(100,1) 	NOT NULL,
	[Email]			[NVARCHAR](100) 		   			NOT NULL,
	[FirstName]		[NVARCHAR](50) 		  	  			NOT NULL,
	[LastName]		[NVARCHAR](100)		  				NOT NULL,
	[PhoneNumber]	[NVARCHAR](15)		  	  			NULL,
	[PasswordHash]	[NVARCHAR](100)		  				NOT NULL	DEFAULT 
		'9C9064C59F1FFA2E174EE754D2979BE80DD30DB552EC03E7E327E9B1A4BD594E', /* Hash Code */
	[Active]		[BIT]					  			NOT NULL	DEFAULT 1,
	
	CONSTRAINT [pk_AccountID] 	PRIMARY KEY([AccountID] ASC),
	CONSTRAINT [ak_Email] 		UNIQUE([Email] ASC)
)
GO

print '' print '*** creating account test data ***'
GO
INSERT INTO [dbo].[Account] 
		([Email], [FirstName], [LastName], [PhoneNumber])
	VALUES
		('Scott@Webber.com', 'Scott', 'Webber', '319-555-1111'),
		('Shannon@Webber.com', 'Shannon', 'Webber', '319-555-2222'),
		('Nathaniel@Webber.com', 'Nathaniel', 'Webber', '319-555-3333'),
		('Teddie@Webber.com', 'Teddie', 'Webber', '319-555-4444'),
		('Anna@Webber.com', 'Anna', 'Webber', '319-555-5555')
GO

print '' print '*** creating role table ***'
GO
CREATE TABLE [dbo].[Role]
(
	[RoleID]		[NVARCHAR](25) 						NOT NULL,
	[Description]	[NVARCHAR](250)						NULL,

	CONSTRAINT [pk_RoleID]	PRIMARY KEY([RoleID] ASC)
)
GO

print '' print '*** creating role test data ***'
GO
INSERT INTO [dbo].[Role]
		([RoleID], [Description])
	VALUES
		('Admin', 'User Administrator'),
		('User', 'Normal User'),
		('IT', 'Upkeep Management')
GO

print '' print '*** creating accountRole table ***'
GO
CREATE TABLE [dbo].[AccountRole]
(
	[AccountID]		[INT]								NOT NULL,
	[RoleID]		[NVARCHAR](25)						NOT NULL,
	
	CONSTRAINT [pk_AccountID_RoleID] 	PRIMARY KEY([AccountID], [RoleID]),
	CONSTRAINT [fk_AccountID]			FOREIGN KEY([AccountID])	REFERENCES [dbo].[Account]([AccountID]),
	CONSTRAINT [fk_RoleID]				FOREIGN KEY([RoleID])		REFERENCES [dbo].[Role]([RoleID])
	ON UPDATE CASCADE
)
GO

print '' print '*** adding sample account records ***'
GO
INSERT INTO [dbo].[AccountRole]
		([AccountID], [RoleID])
	VALUES
		(100, 'Admin'),
		(101, 'User'),
		(102, 'IT'),
		(103, 'User'),
		(104, 'User')
GO

print '' print '*** adding genre table ***'
GO
CREATE TABLE [dbo].[Genre]
(
	GenreName		[NVARCHAR] (100)					NOT NULL
	
	CONSTRAINT [pk_GenreName]	PRIMARY KEY([GenreName] ASC)
)
GO

print '' print '*** adding sample genre data ***'
GO
INSERT INTO [dbo].[Genre]
		([GenreName])
	VALUES
		('Action'),
		('Comedy'),
		('Romance')
GO	

print '' print '*** adding movie table ***'
GO
CREATE TABLE [dbo].[Movie]
(
	MovieID				[INT]		IDENTITY(01, 1)		NOT NULL,
	MovieTitle			[NVARCHAR] (256)				NOT NULL,
	MovieDate			[INT]							NOT NULL,
	MovieGenre			[NVARCHAR] (100)				NULL,
	MovieStarringFName	[NVARCHAR] (100)				NULL,
	MovieStarringLName	[NVARCHAR] (100)				NULL,
	MovieStatusID		[NVARCHAR] (50)	DEFAULT ('Available')				NOT NULL,
	Active				[BIT]		DEFAULT 1			NOT NULL,
	FirstName			[NVARCHAR](50)		DEFAULT(USER)				NULL,
	
	
	/*CONSTRAINT [fk_FirstName]			FOREIGN KEY([FirstName])	REFERENCES [dbo].[Account]([FirstName]),*/
	CONSTRAINT [pk_MovieID] 			PRIMARY KEY ([MovieID] ASC)
)
GO

print '' print '*** creating MovieStatus table ***'
GO
CREATE TABLE [dbo].[MovieStatus](
	[MovieStatusID]	[NVARCHAR](50)	NOT NULL,
	CONSTRAINT [pk_movieStatusID] PRIMARY KEY([MovieStatusID] ASC)
)
GO

print '' print '*** creating MovieStatus test records ***'
GO
INSERT INTO [dbo].[MovieStatus]
		([MovieStatusID])
	VALUES
		('Available'),
		('Rented')
GO

print '' print '*** adding sample movie data ***' 
GO
INSERT INTO [dbo].[Movie]
		([MovieTitle], [MovieDate], [MovieGenre], 
		[MovieStarringFName], [MovieStarringLName], 
		[Active])
	VALUES
	('300', 2006, 'Action', 'Gerard', 'Butler', 1)
GO

print '' print '' print '*** USER PROCEDURES FOR USERS ***'
GO

print '' print '*** Creating sp_authenticate_user ***'
GO
CREATE PROCEDURE [dbo].[sp_authenticate_user] 
(
	@Email			[NVARCHAR](100),
	@PasswordHash	[NVARCHAR](100)
)
AS
	BEGIN
		SELECT COUNT (Email)
		FROM 	Account
		WHERE 	Email = @Email
		AND 	PasswordHash = @PasswordHash
		AND 	Active = 1
	END
GO

print '' print '*** Creating sp_update_passwordhash ***'
GO
CREATE PROCEDURE [dbo].[sp_update_passwordhash] 
(
	@Email				[NVARCHAR](100),
	@OldPasswordHash	[NVARCHAR](100),
	@NewPasswordHash	[NVARCHAR](100)
)
AS
	BEGIN
		UPDATE 		Account
			SET 	PasswordHash = @NewPasswordHash
			WHERE 	Email = @Email
			AND 	PasswordHash = @OldPasswordHash
		RETURN @@ROWCOUNT
	END
GO

print '' print '*** Creating sp_select_user_by_email ***'
GO
CREATE PROCEDURE [dbo].[sp_select_user_by_email] 
(
	@Email				[NVARCHAR](100)
)
AS
	BEGIN
		SELECT 	AccountID, Email, FirstName, LastName, PhoneNumber, Active
		FROM 	Account
		WHERE 	Email = @Email
	END
GO

print '' print '*** Creating sp_select_roles_by_accountID ***'
GO
CREATE PROCEDURE [dbo].[sp_select_roles_by_accountID] 
(
	@AccountID				[INT]
)
AS
	BEGIN
		SELECT 	RoleID
		FROM 	AccountRole
		WHERE 	AccountID = @AccountID
	END
GO

print '' print '*** Creating sp_update_account_profile_data ***'
GO
CREATE PROCEDURE [dbo].[sp_update_employee_profile_data] 
(
	@AccountID				[INT],
	
	@NewEmail				[NVARCHAR](100),
	@NewFirstName			[NVARCHAR](50),
	@NewLastName			[NVARCHAR](100),
	@NewPhoneNumber			[NVARCHAR](15),
	
	@OldEmail				[NVARCHAR](100),
	@OldFirstName			[NVARCHAR](50),
	@OldLastName			[NVARCHAR](50),
	@OldPhoneNumber			[NVARCHAR](15)
	
)
AS
	BEGIN
		UPDATE 		Account
			SET 	Email = @NewEmail,
					FirstName = @NewFirstName,
					LastName = @NewLastName,
					PhoneNumber = @NewPhoneNumber
			WHERE 	AccountID = @AccountID
			AND 	Email = @OldEmail
			AND		FirstName = @OldFirstName
			AND		LastName = @OldLastName
			AND		PhoneNumber = @OldPhoneNumber
		RETURN @@ROWCOUNT
	END
GO

print '' print '' print '*** USER PROCEDURES FOR ADMINS ***'
GO

/* INSERT */
print '' print '*** creating sp_insert_new_user ***'
GO
CREATE PROCEDURE [dbo].[sp_insert_new_user]
(
	@Email				[NVARCHAR](100),
	@FirstName			[NVARCHAR](50),
	@LastName			[NVARCHAR](100),
	@PhoneNumber		[NVARCHAR](15)
)
AS
	BEGIN
		INSERT INTO [dbo].[Account] 
			([Email], [FirstName], [LastName], [PhoneNumber])
		VALUES	
			(@Email, @FirstName, @LastName, @PhoneNumber)
		SELECT SCOPE_IDENTITY()
	END
GO

/* SELECT LIST */
print '' print '*** creating sp_select_all_users ***'
GO
CREATE PROCEDURE [dbo].[sp_select_all_users]
AS
	BEGIN
		SELECT AccountID, Email, FirstName, LastName, PhoneNumber, Active
		FROM Account
		ORDER BY LastName ASC
	END
GO

/* SELECT ONE */
print '' print '*** creating sp_select_users_by_active ***'
GO
CREATE PROCEDURE [dbo].[sp_select_users_by_active]
(
	@Active			[BIT]
)
AS
	BEGIN
		SELECT AccountID, Email, FirstName, LastName, PhoneNumber, Active
		FROM Account
		WHERE Active = @Active
		ORDER BY LastName ASC
	END
GO

/* SELECT FILTERED LIST */
print '' print '*** Creating sp_select_user_by_id ***'
GO
CREATE PROCEDURE [dbo].[sp_select_user_by_id] 
(
	@AccountID				[INT]
)
AS
	BEGIN
		SELECT 	AccountID, Email, FirstName, LastName, PhoneNumber, Active
		FROM 	Account
		WHERE 	AccountID = @AccountID
	END
GO

/* UPDATE SPECIFIC */
print '' print '*** Creating sp_reset_passwordhash ***'
GO
CREATE PROCEDURE [dbo].[sp_reset_passwordhash] 
(
	@Email				[NVARCHAR](100),
	@NewPasswordHash	[NVARCHAR](100)
)
AS
	BEGIN
		UPDATE 		Account
			SET 	PasswordHash = 
				'9C9064C59F1FFA2E174EE754D2979BE80DD30DB552EC03E7E327E9B1A4BD594E' /* Translates to 'newuser' */
			WHERE 	Email = @Email
		RETURN @@ROWCOUNT
	END
GO

/* DEACTIVATE */
print '' print '*** Creating sp_deactivate_user ***'
GO
CREATE PROCEDURE [dbo].[sp_deactivate_user] 
(
	@AccountID			[INT]
)
AS
	BEGIN
		UPDATE 		Account
			SET 	Active = 0
			WHERE 	AccountID = @AccountID
		RETURN @@ROWCOUNT
	END
GO

/* REACTIVATE */
print '' print '*** Creating sp_reactivate_user ***'
GO
CREATE PROCEDURE [dbo].[sp_reactivate_user] 
(
	@AccountID			[INT]
)
AS
	BEGIN
		UPDATE 		Account
			SET 	Active = 1
			WHERE 	AccountID = @AccountID
		RETURN @@ROWCOUNT
	END
GO

/* ADD ROLE */
print '' print '*** Creating sp_add_accountrole ***'
GO
CREATE PROCEDURE [dbo].[sp_add_accountrole] 
	(
		@AccountID		[INT],
		@RoleID			[NVARCHAR](25)
	)
AS
	BEGIN
		INSERT INTO AccountRole
			([AccountID], [RoleID])
		VALUES
			(@AccountID, @RoleID)
		RETURN @@ROWCOUNT
	END
GO

/* SELECT ALL ROLES */
print '' print '*** Creating sp_select_all_roles ***'
GO
CREATE PROCEDURE [dbo].[sp_select_all_roles]
AS
	BEGIN	
		SELECT RoleID
		FROM Role
		ORDER BY RoleID ASC
		RETURN @@ROWCOUNT
	END
GO

/* SAFELY REMOVE EMPLOYEE ROLL */
print '' print '*** Creating sp_safely_remove_employeerole ***'
GO
CREATE PROCEDURE [dbo].[sp_safely_remove_Accountrole]
	(
		@AccountID		[INT],
		@RoleID 			[NVARCHAR](25)
	)
AS
	BEGIN
		DECLARE @Admins INT;
		
		SELECT @Admins = COUNT(RoleID)
		FROM AccountRole
		WHERE RoleID = 'Admin'
		
		IF @RoleID = 'Admin' AND @Admins = 1
			BEGIN
				RETURN 0
			END
		ELSE
			BEGIN
				DELETE FROM AccountRole
					WHERE AccountID = @AccountID
					AND RoleID = @RoleID
				RETURN @@ROWCOUNT
			END
	END
GO

/* INSERT */
print '' print '*** creating sp_insert_new_movie ***'
GO
CREATE PROCEDURE [dbo].[sp_insert_new_movie]
(
	@MovieTitle			[NVARCHAR] (256),
	@MovieDate			[INT],
	@MovieGenre			[NVARCHAR] (100),
	@MovieStarringFName	[NVARCHAR] (100),
	@MovieStarringLName	[NVARCHAR] (100)
)
AS
	BEGIN
		INSERT INTO [dbo].[Movie] 
			([MovieTitle], [MovieDate], [MovieGenre], [MovieStarringFName], [MovieStarringLName])
		VALUES	
			(@MovieTitle, @MovieDate, @MovieGenre, @MovieStarringFName, @MovieStarringLName)
		SELECT SCOPE_IDENTITY()
	END
GO

/* RENT */
print '' print '*** Creating sp_deactivate_movie ***'
GO
CREATE PROCEDURE [dbo].[sp_deactivate_movie] 
(
	@MovieTitle			[NVARCHAR] (256)
)
AS
	BEGIN
		UPDATE 		Movie
			SET 	Active = 0
			WHERE 	MovieTitle = @MovieTitle
		RETURN @@ROWCOUNT
	END
GO

print '' print '*** creating sp_update_movie_status ***'
GO
CREATE PROCEDURE [dbo].[sp_update_movie_status]
	(
		@MovieID			[INT],
		@OldMovieStatusID	[NVARCHAR](50),
		@NewMovieStatusID	[NVARCHAR](50)
	)
AS
	BEGIN
		UPDATE Movie
			SET MovieStatusID = @NewMovieStatusID
			WHERE MovieID = @MovieID
			  AND MovieStatusID = @OldMovieStatusID
		RETURN @@ROWCOUNT
	END
GO

print '' print '*** creating sp_select_movie_by_rented ***'
GO
CREATE PROCEDURE [dbo].[sp_select_movie_by_rented]
	(
		@MovieStatusID	[NVARCHAR] (50)
	)
AS
	BEGIN
	
		SELECT 	MovieTitle,  Account.FirstName
		FROM 	Movie JOIN Account ON Movie.FirstName = Account.FirstName
		WHERE 	Movie.MovieStatusID = @MovieStatusID
	END
GO

print '' print '*** creating sp_select_movie_by_status ***'
GO
CREATE PROCEDURE [dbo].[sp_select_movie_by_status]
	(
		@MovieStatusID		[NVARCHAR](50)
	)
AS
	BEGIN
		SELECT		MovieID, MovieTitle, MovieDate, MovieGenre, MovieStarringFName, MovieStarringLName, MovieStatusID, FirstName
		FROM		Movie 
		WHERE		MovieStatusID = @MovieStatusID
		ORDER BY 	MovieID ASC
	END
GO

print '' print '*** creating sp_update_movie ***'
GO
CREATE PROCEDURE [dbo].[sp_update_movie]
	(
		@MovieID				[INT],
		
		@NewMovieTitle			[NVARCHAR] (256),
		@NewMovieDate			[INT],
		@NewMovieGenre			[NVARCHAR] (100),
		@NewMovieStarringFName	[NVARCHAR] (100),
		@NewMovieStarringLName	[NVARCHAR] (100),
		
		@OldMovieTitle			[NVARCHAR] (256),
		@OldMovieDate			[INT],
		@OldMovieGenre			[NVARCHAR] (100),
		@OldMovieStarringFName	[NVARCHAR] (100),
		@OldMovieStarringLName	[NVARCHAR] (100)
	)
AS
	BEGIN
		UPDATE 	Movie
			SET 	MovieTitle = @NewMovieTitle,
					MovieDate = @NewMovieDate,
					MovieGenre = @NewMovieGenre,
					MovieStarringFName = @NewMovieStarringFName,
					MovieStarringLName = @NewMovieStarringLName
			WHERE 	MovieID = @MovieID
			AND		MovieTitle = @OldMovieTitle
			AND		MovieDate = @OldMovieDate
			AND		MovieGenre = @OldMovieGenre
			AND		MovieStarringFName = @OldMovieStarringFName
			AND 	MovieStarringLName = @OldMovieStarringLName
		RETURN @@ROWCOUNT
	END
GO
	
print '' print '*** creating sp_retreive_movie_by_id ***'
GO
CREATE PROCEDURE [dbo].[sp_retreive_movie_by_id]
	(
		@MovieID		[INT]
	)
AS
	BEGIN
		SELECT MovieID, MovieTitle, MovieDate, MovieGenre, MovieStarringFName, MovieStarringLName, MovieStatusID, FirstName
		FROM Movie
		WHERE MovieID = @MovieID
	END
GO
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		


