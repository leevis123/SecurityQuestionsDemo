CREATE TABLE "SecurityQuestion" (
	"Id"	INTEGER NOT NULL UNIQUE,
	"Question"	TEXT NOT NULL UNIQUE,
	PRIMARY KEY("Id" AUTOINCREMENT)
)

INSERT INTO SecurityQuestion (Question)
VALUES ('In what city were you born?'),
('What is the name of your favorite pet?'),
('What is your mother''s maiden name?'),
('What high school did you attend?'),
('What was the mascot of your high school?'),
('What was the make of your first car?'),
('What was your favorite toy as a child?'),
('Where did you meet your spouse?'),
('What is your favorite meal?'),
('What is the airspeed velocity of an unladen swallow?')

CREATE TABLE "User" (
	"Id"	INTEGER NOT NULL UNIQUE,
	"Name"	TEXT NOT NULL UNIQUE,
	PRIMARY KEY("Id" AUTOINCREMENT)
)

CREATE TABLE "UserSecurityQuestion" (
	"Id"	INTEGER NOT NULL UNIQUE,
	"UserId"	INTEGER NOT NULL,
	"SecurityQuestionId"	INTEGER NOT NULL,
	"Answer"	TEXT NOT NULL,
	PRIMARY KEY("Id")
	FOREIGN KEY("UserId") REFERENCES User(Id),
	FOREIGN KEY("SecurityQuestionId") REFERENCES SecurityQuestion(Id),
	UNIQUE("UserId","SecurityQuestionId")
)