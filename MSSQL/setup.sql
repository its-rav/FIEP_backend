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
	IsDeleted BIT DEFAULT 0 NOT NULL,
	AvatarUrl VARCHAR(256),
	CreateDate DATETIME DEFAULT GETDATE() ,
	ModifyDate DATETIME DEFAULT GETDATE(),
	FOREIGN KEY (RoleID) REFERENCES Role(RoleID),
);

CREATE table UserFCMToken(
	UserFCMId int IDENTITY(1, 1) primary key,	
	FCMToken VARCHAR(256) unique,
	UserID UNIQUEIDENTIFIER NOT NULL,
	FOREIGN KEY (UserID) REFERENCES UserInformation(UserID),
);

CREATE TABLE GroupInformation (
	GroupID INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	GroupImageUrl VARCHAR(256),
	IsDeleted BIT DEFAULT 0 NOT NULL,
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
	IsDeleted BIT DEFAULT 0 NOT NULL,
	PRIMARY KEY (GroupID,UserID),
	SubscriptionType INT NOT NULL, --1 : sub follow, 2 : sub admin
);

CREATE TABLE ActivityType (
	ActivityTypeId INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	ActivityTypeName VARCHAR(256),
	IsDeleted BIT DEFAULT 0 NOT NULL,
);



CREATE TABLE Event (
	EventID INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	EventName VARCHAR(128),
	GroupID INT NOT NULL,
	FOREIGN KEY (GroupID) REFERENCES GroupInformation(GroupID),
	Location VARCHAR(128),
	ApprovalState INT DEFAULT 0, --1: NOT_APPROVED, 0 : NOT_YET_APPROVED, 1 : APPROVED
	ImageUrl VARCHAR(256),
	IsDeleted BIT DEFAULT 0 NOT NULL,
	TimeOccur DATETIME,
	IsExpired BIT DEFAULT 0 NOT NULL,
	CreateDate DATETIME DEFAULT GETDATE() ,
	ModifyDate DATETIME DEFAULT GETDATE()
);

CREATE table Notification(
	NotificationID UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	Title NVARCHAR(256),
	Body NVARCHAR(256),
	ImageUrl NVARCHAR(256),
	UserFCMTokens VARCHAR(MAX) DEFAULT NULL,
	EventID INT ,
	FOREIGN KEY (EventID) REFERENCES Event(EventID),
	GroupID INT ,
	FOREIGN KEY (GroupId) REFERENCES GroupInformation(GroupID),
	CreateDate DATETIME DEFAULT GETDATE(),
	ModifyDate DATETIME DEFAULT GETDATE()
);

CREATE TABLE EventActivity (
	ActivityID INT NOT NULL IDENTITY(1,1),
	EventID INT NOT NULL,
	FOREIGN KEY (EventID) REFERENCES Event(EventID),
	ActivityTypeId INT NOT NULL ,
	IsDeleted BIT DEFAULT 0 NOT NULL,
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
	IsDeleted BIT DEFAULT 0,
	PRIMARY KEY (EventID,UserID)
);

CREATE TABLE Post (
	PostID UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	EventID INT NOT NULL,
	FOREIGN KEY (EventID) REFERENCES Event(EventID),
	PostContent NVARCHAR(256) NOT NULL,
	ImageUrl VARCHAR(256),
	IsDeleted BIT DEFAULT 0 NOT NULL,
	CreateDate DATETIME DEFAULT GETDATE() ,
	ModifyDate DATETIME DEFAULT GETDATE()
);

CREATE TABLE Comment (
	CommentID VARCHAR(128) NOT NULL PRIMARY KEY,
	PostID UNIQUEIDENTIFIER NOT NULL,
	FOREIGN KEY (PostID) REFERENCES Post(PostID),
	Content VARCHAR(256) NOT NULL,
	IsDeleted BIT DEFAULT 0 NOT NULL,
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
INSERT INTO UserInformation(UserID,RoleID,Email,Fullname,AvatarUrl)
VALUES('1d8c8527-e1f4-4a77-85ee-68c15f927817',1,'nhanvtse130478@fpt.edu.vn','Vo Thanh Nhan','https://firebasestorage.googleapis.com/v0/b/fiep-e6602.appspot.com/o/usericon.png?alt=media&token=4526116b-dc87-4d06-ae47-1d4fb730474c');
INSERT INTO UserInformation(UserID,RoleID,Email,Fullname,AvatarUrl)
VALUES('dc70a164-619f-4502-887e-2a04465f288f',2,'tuannase130462@fpt.edu.vn','Nguyen Anh Tuan','https://firebasestorage.googleapis.com/v0/b/fiep-e6602.appspot.com/o/usericon.png?alt=media&token=4526116b-dc87-4d06-ae47-1d4fb730474c');
INSERT INTO UserInformation(UserID,RoleID,Email,Fullname,AvatarUrl)
VALUES('d6076f10-bed3-4d76-9745-120a3794e8f7',3,'thanhncse130743@fpt.edu.vn','Nguyen Chanh Thanh','https://firebasestorage.googleapis.com/v0/b/fiep-e6602.appspot.com/o/usericon.png?alt=media&token=4526116b-dc87-4d06-ae47-1d4fb730474c');
INSERT INTO UserInformation(UserID,RoleID,Email,Fullname,AvatarUrl)
VALUES('f90e94fa-6b29-4a5a-993c-94b153ef81b2',1,'nguyenhoanghuy@fpt.edu.vn','Nguyen Hoang Huy','https://firebasestorage.googleapis.com/v0/b/fiep-e6602.appspot.com/o/usericon.png?alt=media&token=4526116b-dc87-4d06-ae47-1d4fb730474c');
INSERT INTO UserInformation(UserID,RoleID,Email,Fullname,AvatarUrl)
VALUES('daf78774-feb8-46ab-b8ab-de1439559ed8',3,'thanquocbinh@fpt.edu.vn','Thanh Quoc Binh','https://firebasestorage.googleapis.com/v0/b/fiep-e6602.appspot.com/o/usericon.png?alt=media&token=4526116b-dc87-4d06-ae47-1d4fb730474c');

--GroupInformation--
INSERT INTO GroupInformation(GroupName,GroupImageUrl)
VALUES('F-Code','https://firebasestorage.googleapis.com/v0/b/fiep-e6602.appspot.com/o/f-code.jpg?alt=media&token=b09869eb-02aa-472a-bbb5-c46d7da0a833');
INSERT INTO GroupInformation(GroupName,GroupImageUrl)
VALUES('FPT Event Club','https://firebasestorage.googleapis.com/v0/b/fiep-e6602.appspot.com/o/fev.jpg?alt=media&token=83d059b9-e682-476e-874f-c807b92f13c2');
INSERT INTO GroupInformation(GroupName,GroupImageUrl)
VALUES('FPT Instrument Club','https://firebasestorage.googleapis.com/v0/b/fiep-e6602.appspot.com/o/fpt-Intrument.jpg?alt=media&token=8cda66dd-d818-442f-a614-c9efe71f0ca8');
INSERT INTO GroupInformation(GroupName,GroupImageUrl)
VALUES('FPT Chess Club','https://firebasestorage.googleapis.com/v0/b/fiep-e6602.appspot.com/o/F-chess.png?alt=media&token=e2f88c71-98f7-41ca-a780-1e2c5508d057');
INSERT INTO GroupInformation(GroupName,GroupImageUrl)
VALUES('FPT Guitar Club','https://firebasestorage.googleapis.com/v0/b/fiep-e6602.appspot.com/o/fpt-guitar.jpg?alt=media&token=a40bc833-687e-4356-8f4d-0ba57add92f4');
INSERT INTO GroupInformation(GroupName,GroupImageUrl)
VALUES('Fpt Vovinam Club','https://firebasestorage.googleapis.com/v0/b/fiep-e6602.appspot.com/o/fpt-vovinam.jpg?alt=media&token=d46aa6b8-fab9-40bb-b753-9e82013b1801');

--GroupSubscription--
INSERT INTO GroupSubscription(GroupID,UserID,SubscriptionType)
VALUES(1,'1d8c8527-e1f4-4a77-85ee-68c15f927817',1);
INSERT INTO GroupSubscription(GroupID,UserID,SubscriptionType)
VALUES(2,'f90e94fa-6b29-4a5a-993c-94b153ef81b2',1);
INSERT INTO GroupSubscription(GroupID,UserID,SubscriptionType)
VALUES(2,'1d8c8527-e1f4-4a77-85ee-68c15f927817',1);
INSERT INTO GroupSubscription(GroupID,UserID,SubscriptionType)
VALUES(1,'daf78774-feb8-46ab-b8ab-de1439559ed8',2);

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
INSERT INTO Event(GroupID,EventName,Location,TimeOccur,ImageUrl,ApprovalState)
VALUES(1,'ACM Contest','Hall Of FPT University', '06-06-2020','https://firebasestorage.googleapis.com/v0/b/fiep-e6602.appspot.com/o/event-acm-fcode.png?alt=media&token=adfcf71c-10c8-48fa-a49a-f75b38572aab',1);
INSERT INTO Event(GroupID,EventName,Location,TimeOccur,ImageUrl,ApprovalState)
VALUES(3,'Tiktok conpetition','F-Tech Tower', '06-06-2020','https://firebasestorage.googleapis.com/v0/b/fiep-e6602.appspot.com/o/event-tiktok-intrument.jpg?alt=media&token=3eec5742-57c7-429b-9832-4c1574d25969',1);
INSERT INTO Event(GroupID,EventName,Location,TimeOccur,ImageUrl,ApprovalState)
VALUES(2,'Club celebration','FSOFT-Town-1', '06-06-2020','https://firebasestorage.googleapis.com/v0/b/fiep-e6602.appspot.com/o/event-daitho-fev.jpg?alt=media&token=336524d9-cd9d-4200-80ca-df75fabb32d2',1);
INSERT INTO Event(GroupID,EventName,Location,TimeOccur,ImageUrl,ApprovalState)
VALUES(5,'Guitar free style','FSOFT-Town-2', '06-06-2020','https://firebasestorage.googleapis.com/v0/b/fiep-e6602.appspot.com/o/event-yongguitar-guitar.jpg?alt=media&token=726b0bee-b68a-463f-9a87-51bc6a6bdcfd',1);
INSERT INTO Event(GroupID,EventName,Location,TimeOccur,ImageUrl,ApprovalState)
VALUES(2,'Reduce plastic together','FSOFT-Town-3', '06-06-2020','https://firebasestorage.googleapis.com/v0/b/fiep-e6602.appspot.com/o/event-plastic-fev.png?alt=media&token=0f72964b-c031-485b-a98d-ac01e31c3724',1);
INSERT INTO Event(GroupID,EventName,Location,TimeOccur,ImageUrl,ApprovalState)
VALUES(6,'Martial art for women day','FPT Greenwich', '06-06-2020','https://firebasestorage.googleapis.com/v0/b/fiep-e6602.appspot.com/o/event-womanday-vovinam.jpg?alt=media&token=e47a61a1-9c26-467a-9e78-eaeb0ff6378b',1);

--EventActivity--
INSERT INTO EventActivity(EventID,ActivityTypeId,EventActivityDescription)
VALUES(1,1,'Great show about music');
INSERT INTO EventActivity(EventID,ActivityTypeId,EventActivityDescription)
VALUES(1,2,'Teambuilding with newcomers');
INSERT INTO EventActivity(EventID,ActivityTypeId,EventActivityDescription)
VALUES(2,2,'Get along with newcomers');
INSERT INTO EventActivity(EventID,ActivityTypeId,EventActivityDescription)
VALUES(3,4,'Try be the best of yourself');
	
--User FCM Tokens
INSERT INTO UserFCMToken(UserID,FCMToken)
VALUES('1d8c8527-e1f4-4a77-85ee-68c15f927817','dK1pPIODQdC5w6Ww7d2uQi:APA91bGkkfiB8LUhPJFo7wybBUx6RFNdh3RcEgcjpCeVrxPUxGxKqribb8N6lQqYuMSMxmsobsxMTvci4aFeScm8D6mPEY9Yta1F7ppQ6cSziJCedHjDGwY9_z8g9IQxzatCbArBM1Ol');

--EventSubscription--
INSERT INTO EventSubscription(EventID,UserID)
VALUES(1,'1d8c8527-e1f4-4a77-85ee-68c15f927817');
INSERT INTO EventSubscription(EventID,UserID)
VALUES(2,'f90e94fa-6b29-4a5a-993c-94b153ef81b2');
INSERT INTO EventSubscription(EventID,UserID)
VALUES(3,'dc70a164-619f-4502-887e-2a04465f288f');

--Post--
INSERT INTO Post(PostID,EventID,PostContent,ImageUrl)
VALUES('88e898ce-d163-4873-8943-c0f828b92d33',1,'Luôn có đồ ăn hỗ trợ cho các bạn nhé hehe!','https://firebasestorage.googleapis.com/v0/b/fiep-e6602.appspot.com/o/post-acm-fcode.jpg?alt=media&token=f5d89f28-c64f-4437-8e28-40af5e8a89ac');
INSERT INTO Post(PostID,EventID,PostContent,ImageUrl)
VALUES('a67e6424-c865-4e13-b271-d8ac9a337517',2,'Các bạn đã chuẩn bị tới đâu rồi nào!','https://firebasestorage.googleapis.com/v0/b/fiep-e6602.appspot.com/o/post-tiktok-intru.jpg?alt=media&token=685615f2-daeb-453c-b24f-28bedbd0ff90');
INSERT INTO Post(PostID,EventID,PostContent,ImageUrl)
VALUES('97da7c08-78a8-4c18-bca3-422c42f7778a',3,'Một tấm hình đẹp kỷ niệm event lần này','https://firebasestorage.googleapis.com/v0/b/fiep-e6602.appspot.com/o/post-daitho-fev.jpg?alt=media&token=ecdfdb65-4189-4ac9-a2e3-03c3031f1dce');

--Comment--
INSERT INTO Comment(CommentID,PostID,Content,CommentOwnerId)
VALUES('826ef468-a3ed-4d41-af50-e13ff4004e1f','88e898ce-d163-4873-8943-c0f828b92d33','This is a comment','f90e94fa-6b29-4a5a-993c-94b153ef81b2');
INSERT INTO Comment(CommentID,PostID,Content,CommentOwnerId)
VALUES('aabcb833-4108-4a6e-a203-2acaa4d52763','a67e6424-c865-4e13-b271-d8ac9a337517','This is a comment','dc70a164-619f-4502-887e-2a04465f288f');