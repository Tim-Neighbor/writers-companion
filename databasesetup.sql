DROP TABLE IF EXISTS Note;
DROP TABLE IF EXISTS Category;
DROP TABLE IF EXISTS Project;

CREATE TABLE Project (
	name VARCHAR(40) NOT NULL,
	PRIMARY KEY (name)
);

CREATE TABLE Category (
	name VARCHAR(40) NOT NULL,
	projectName VARCHAR(40) NOT NULL,
	PRIMARY KEY (name, projectName),
	FOREIGN KEY (projectName) 
		REFERENCES Project (name)
		ON DELETE CASCADE
		ON UPDATE CASCADE
);

CREATE TABLE Note (
	name VARCHAR(40) NOT NULL,
	categoryName VARCHAR(40) NOT NULL,
	projectName VARCHAR(40) NOT NULL,
	content LONGTEXT,
	PRIMARY KEY (name, categoryName, projectName),
	FOREIGN KEY (categoryname, projectName) 
		REFERENCES Category (name, projectName)
		ON DELETE CASCADE
		ON UPDATE CASCADE
);

/*
INSERT INTO Project (name) VALUES ('TEST PROJECT');

INSERT INTO Category (name, projectName) VALUES ('TEST CATEGORY', 'TEST PROJECT');
INSERT INTO Category (name, projectName) 
	VALUES ('OTHER TEST CATEGORY', 'TEST PROJECT');
INSERT INTO Category (name, projectName) 
	VALUES ('EVEN OTHER TEST CATEGORY', 'TEST PROJECT');

INSERT INTO Note (name, categoryName, projectName, content) 
	VALUES ('TEST NOTE', 'TEST CATEGORY', 
		'TEST PROJECT', 'bopity boo dippity doo dah boyuyyyyyyyyyyyy');
INSERT INTO Note (name, categoryName, projectName, content) 
	VALUES ('EVEN OTHER TEST NOTE', 'TEST CATEGORY', 
		'TEST PROJECT', 'zzzzzzzzzzzZZZZZZZZZZZZZZZzzzzzzzz');
INSERT INTO Note (name, categoryName, projectName, content) 
	VALUES ('ANOTHER TEST NOTE', 'EVEN OTHER TEST CATEGORY', 
		'TEST PROJECT', 'YEAHHHHHHHHHHHHHHHH Hello');
INSERT INTO Note (name, categoryName, projectName, content) 
	VALUES ('VERY OTHER TEST NOTE', 'EVEN OTHER TEST CATEGORY', 
		'TEST PROJECT', 'looptylooptylooptyloopty loopty looptylooptylooptyloopty loopty looptylooptylooptyloopty loopty');
INSERT INTO Note (name, categoryName, projectName, content) 
	VALUES ('THIS OTHER TEST NOTE', 'OTHER TEST CATEGORY', 
		'TEST PROJECT', 'spiderman spiderman does whatever a spiter can');
*/













