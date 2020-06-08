CREATE DATABASE FIEP;
GO

USE FIEP;
GO

CREATE TABLE Role (
	RoleID INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	Rolename VARCHAR(128) NOT NULL,
);

CREATE TABLE UserInformation (
	UserID UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
	RoleID INT NOT NULL,
	Email VARCHAR(128) NOT NULL UNIQUE,
	Fullname VARCHAR(128) NOT NULL,
	IsDeleted BIT DEFAULT 0,
	AvatarUrl VARCHAR(256),
	CreateDate DATETIME DEFAULT GETDATE() ,
	ModifyDate DATETIME DEFAULT GETDATE(),
	FOREIGN KEY (RoleID) REFERENCES Role(RoleID),
);


CREATE TABLE GroupInformation (
	GroupID INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	GroupImageUrl VARCHAR(128),
	GroupManagerId UNIQUEIDENTIFIER NOT NULL,
	FOREIGN KEY (GroupManagerId) REFERENCES UserInformation(UserID),
	GroupName VARCHAR(128) NOT NULL,
	CreateDate DATETIME DEFAULT GETDATE() ,
	ModifyDate DATETIME DEFAULT GETDATE()
);

CREATE TABLE GroupSubscription (
	GroupID INT NOT NULL,
	FOREIGN KEY (GroupId) REFERENCES GroupInformation(GroupID),
	UserID UNIQUEIDENTIFIER NOT NULL,
	FOREIGN KEY (UserId) REFERENCES UserInformation(UserID),
	CreateDate DATETIME DEFAULT GETDATE() ,
	ModifyDate DATETIME DEFAULT GETDATE(),
	PRIMARY KEY (GroupID,UserID)
);

CREATE TABLE ActivityType (
	ActivityTypeId INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	ActivityTypeName VARCHAR(256),
);



CREATE TABLE Event (
	EventID INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	EventName VARCHAR(128),
	GroupID INT NOT NULL,
	FOREIGN KEY (GroupID) REFERENCES GroupInformation(GroupID),
	Location VARCHAR(128),
	ApprovalState INT DEFAULT 0, --1: NOT_APPROVED, 0 : NOT_YET_APPROVED, 1 : APPROVED
	ImageUrl VARCHAR(256),
	TimeOccur DATETIME,
	CreateDate DATETIME DEFAULT GETDATE() ,
	ModifyDate DATETIME DEFAULT GETDATE()
);

CREATE TABLE EventActivity (
	ActivityID INT NOT NULL IDENTITY(1,1),
	EventID INT NOT NULL,
	FOREIGN KEY (EventID) REFERENCES Event(EventID),
	ActivityTypeId INT NOT NULL ,
	FOREIGN KEY (ActivityTypeId) REFERENCES ActivityType(ActivityTypeId),
	EventActivityDescription VARCHAR(256),
);



CREATE TABLE EventSubscription (
	EventID INT NOT NULL,
	FOREIGN KEY (EventID) REFERENCES Event(EventID),
	UserID UNIQUEIDENTIFIER NOT NULL,
	FOREIGN KEY (UserID) REFERENCES UserInformation(UserID),
	CreateDate DATETIME DEFAULT GETDATE() ,
	ModifyDate DATETIME DEFAULT GETDATE(),
	PRIMARY KEY (EventID,UserID)
);

CREATE TABLE Post (
	PostID UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	EventID INT NOT NULL,
	FOREIGN KEY (EventID) REFERENCES Event(EventID),
	PostContent VARCHAR(256) NOT NULL,
	ImageUrl VARCHAR(256),
	CreateDate DATETIME DEFAULT GETDATE() ,
	ModifyDate DATETIME DEFAULT GETDATE()
);

CREATE TABLE Comment (
	CommentID VARCHAR(128) NOT NULL PRIMARY KEY,
	PostID UNIQUEIDENTIFIER NOT NULL,
	FOREIGN KEY (PostID) REFERENCES Post(PostID),
	Content VARCHAR(256) NOT NULL,
	CommentOwnerId UNIQUEIDENTIFIER NOT NULL,
	FOREIGN KEY (CommentOwnerId) REFERENCES UserInformation(UserID),
	CreateDate DATETIME DEFAULT GETDATE() ,
	ModifyDate DATETIME DEFAULT GETDATE()
);


GO

--Role--
INSERT INTO Role(Rolename)  VALUES ('admin');
INSERT INTO Role(Rolename) VALUES ('user');
INSERT INTO Role(Rolename) VALUES ('groupmanager');

--UserInformation--
INSERT INTO UserInformation(UserID,RoleID,Email,Fullname)
VALUES('1d8c8527-e1f4-4a77-85ee-68c15f927817',1,'vothanhnhan@fpt.edu.vn','Vo Thanh Nhan');
INSERT INTO UserInformation(UserID,RoleID,Email,Fullname)
VALUES('dc70a164-619f-4502-887e-2a04465f288f',2,'nguyenanhtuan@fpt.edu.vn','Nguyen Anh Tuan');
INSERT INTO UserInformation(UserID,RoleID,Email,Fullname)
VALUES('d6076f10-bed3-4d76-9745-120a3794e8f7',3,'nguyenchanhthanh@fpt.edu.vn','Nguyen Chanh Thanh');
INSERT INTO UserInformation(UserID,RoleID,Email,Fullname)
VALUES('f90e94fa-6b29-4a5a-993c-94b153ef81b2',1,'nguyenhoanghuy@fpt.edu.vn','Nguyen Hoang Huy');
INSERT INTO UserInformation(UserID,RoleID,Email,Fullname)
VALUES('daf78774-feb8-46ab-b8ab-de1439559ed8',3,'thanquocbinh@fpt.edu.vn','Thanh Quoc Binh');

--GroupInformation--
INSERT INTO GroupInformation(GroupManagerId,GroupName)
VALUES('d6076f10-bed3-4d76-9745-120a3794e8f7','F-Code');
INSERT INTO GroupInformation(GroupManagerId,GroupName)
VALUES('daf78774-feb8-46ab-b8ab-de1439559ed8','FPT Event Club');
INSERT INTO GroupInformation(GroupManagerId,GroupName)
VALUES('d6076f10-bed3-4d76-9745-120a3794e8f7','FPT Instrument Club');
INSERT INTO GroupInformation(GroupManagerId,GroupName)
VALUES('daf78774-feb8-46ab-b8ab-de1439559ed8','FPT Chess Club');
INSERT INTO GroupInformation(GroupManagerId,GroupName)
VALUES('d6076f10-bed3-4d76-9745-120a3794e8f7','FPT Guitar Club');
INSERT INTO GroupInformation(GroupManagerId,GroupName)
VALUES('daf78774-feb8-46ab-b8ab-de1439559ed8','Fpt Vovinam Club');

--GroupSubscription--
INSERT INTO GroupSubscription(GroupID,UserID)
VALUES(1,'1d8c8527-e1f4-4a77-85ee-68c15f927817');
INSERT INTO GroupSubscription(GroupID,UserID)
VALUES(2,'dc70a164-619f-4502-887e-2a04465f288f');
INSERT INTO GroupSubscription(GroupID,UserID)
VALUES(2,'f90e94fa-6b29-4a5a-993c-94b153ef81b2');
INSERT INTO GroupSubscription(GroupID,UserID)
VALUES(2,'1d8c8527-e1f4-4a77-85ee-68c15f927817');
INSERT INTO GroupSubscription(GroupID,UserID)
VALUES(1,'daf78774-feb8-46ab-b8ab-de1439559ed8');

--ActivityType--
INSERT INTO ActivityType(ActivityTypeName)
VALUES('Music Festival');
INSERT INTO ActivityType(ActivityTypeName)
VALUES('Welcome Newcomers');
INSERT INTO ActivityType(ActivityTypeName)
VALUES('Chess Tournament');
INSERT INTO ActivityType(ActivityTypeName)
VALUES('HACKATHON');

--Event--
INSERT INTO Event(GroupID,EventName,Location,TimeOccur)
VALUES(1,'Welcome FPT','Hall Of FPT University', '06-06-2020');
INSERT INTO Event(GroupID,EventName,Location,TimeOccur)
VALUES(3,'Welcome FPT','F-Tech Tower', '06-06-2020');
INSERT INTO Event(GroupID,EventName,Location,TimeOccur)
VALUES(2,'Welcome FPT','FSOFT-Town-1', '06-06-2020');
INSERT INTO Event(GroupID,EventName,Location,TimeOccur)
VALUES(5,'Welcome FPT','FSOFT-Town-2', '06-06-2020');
INSERT INTO Event(GroupID,EventName,Location,TimeOccur)
VALUES(2,'Welcome FPT','FSOFT-Town-3', '06-06-2020');
INSERT INTO Event(GroupID,EventName,Location,TimeOccur)
VALUES(6,'Welcome FPT','FPT Greenwich', '06-06-2020');

--EventActivity--
INSERT INTO EventActivity(EventID,ActivityTypeId,EventActivityDescription)
VALUES(1,1,'Great show about music');
INSERT INTO EventActivity(ActivityID,ActivityTypeId,EventActivityDescription)
VALUES(1,2,'Teambuilding with newcomers');
INSERT INTO EventActivity(ActivityID,ActivityTypeId,EventActivityDescription)
VALUES(2,2,'Get along with newcomers');
INSERT INTO EventActivity(ActivityID,ActivityTypeId,EventActivityDescription)
VALUES(3,4,'Try be the best of yourself');
	

--EventSubscription--
INSERT INTO EventSubscription(EventID,UserID)
VALUES(1,'1d8c8527-e1f4-4a77-85ee-68c15f927817');
INSERT INTO EventSubscription(EventID,UserID)
VALUES(2,'f90e94fa-6b29-4a5a-993c-94b153ef81b2');
INSERT INTO EventSubscription(EventID,UserID)
VALUES(3,'dc70a164-619f-4502-887e-2a04465f288f');

--Post--
INSERT INTO Post(PostID,EventID,PostContent)
VALUES('88e898ce-d163-4873-8943-c0f828b92d33',1,'This is a content of a post');
INSERT INTO Post(PostID,EventID,PostContent)
VALUES('a67e6424-c865-4e13-b271-d8ac9a337517',2,'This is a content of a post');
INSERT INTO Post(PostID,EventID,PostContent)
VALUES('97da7c08-78a8-4c18-bca3-422c42f7778a',3,'This is a content of a post');

--Comment--
INSERT INTO Comment(CommentID,PostID,Content,CommentOwnerId)
VALUES('826ef468-a3ed-4d41-af50-e13ff4004e1f','88e898ce-d163-4873-8943-c0f828b92d33','This is a comment','f90e94fa-6b29-4a5a-993c-94b153ef81b2');
INSERT INTO Comment(CommentID,PostID,Content,CommentOwnerId)
VALUES('aabcb833-4108-4a6e-a203-2acaa4d52763','a67e6424-c865-4e13-b271-d8ac9a337517','This is a comment','dc70a164-619f-4502-887e-2a04465f288f');