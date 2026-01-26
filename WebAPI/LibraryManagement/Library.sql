--create Database
sqlite3 LibraryDb.db

-- Create tables
--create Authors table
CREATE TABLE Authors (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    Country TEXT
);

--create Books table
CREATE TABLE Books (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Title TEXT NOT NULL,
    ISBN TEXT UNIQUE NOT NULL,
    AuthorId INTEGER NOT NULL,
    PublishedYear INTEGER,
    FOREIGN KEY (AuthorId) REFERENCES Authors(Id)
);

--create Borrowers table
CREATE TABLE Borrowers (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    Email TEXT UNIQUE NOT NULL
);


--Insert sample data into tables
-- Authors
INSERT INTO Authors (Name, Country) VALUES 
('J.K. Rowling', 'UK'),
('George R.R. Martin', 'USA'),
('Haruki Murakami', 'Japan');

-- Books
INSERT INTO Books (Title, ISBN, AuthorId, PublishedYear) VALUES
('Harry Potter and the Philosopher''s Stone', '9780747532743', 1, 1997),
('Harry Potter and the Chamber of Secrets', '9780747538486', 1, 1998),
('A Game of Thrones', '9780553103540', 2, 1996),
('A Clash of Kings', '9780553108033', 2, 1998),
('Kafka on the Shore', '9781400079278', 3, 2002);

-- Borrowers
INSERT INTO Borrowers (Name, Email) VALUES
('Alice Johnson', 'alice@example.com'),
('Bob Smith', 'bob@example.com'),
('Charlie Brown', 'charlie@example.com');


--query to retrive all books
select * from Books;

--query to retrive book from particular writer
select b.Title, b.ISBN, b.PublishedYear FROM Books b join Authors a on b.AuthorId =a.Id where a.Name='Haruki Murakami';

--query to update a borrowers email
update Borrowers SET Email='james@yahoo.com' WHERE Id=1;

--query to delete a book by its Id
delete from Book where Id=1;

--we can see the Id as primary key in all tables i.e Books,Borrowers,Authors. Foreign key is seen in Books table i.e AuthorId  which represents the Author table.
--The tables Books, Authors, Borrowers are in 3rd Normal form(3NF). 
--1st Normal Form (1NF): Each table's columns contain single, atomic values. 
--2nd Normal Form (2NF): Each non-primary key column is fully dependent on the entire primary key.
--3rd Normal Form (3NF):All non-primary key attributes are directly dependent on the primary key.
