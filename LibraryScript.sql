drop database Library;
create database Library;

use Library;

create table Librarians(
idLibrarian int auto_increment primary key,
fioLibrarian varchar(100),
contactNumber varchar(30),
email varchar(30),
passwordLibrarian varchar(30));

create table Readers(
idReader int auto_increment primary key,
fioReader varchar(40),
contactNumber varchar(30),
email varchar(30));

create table Autors(
idAutor int auto_increment primary key,
fioAutor varchar(100),
biography text);

create table Books(
idBook  int auto_increment primary key,
nameBook varchar(30),
idAutor int,
countInStock int,
category varchar(30),
picture varchar(30),
yearOfIssue int,
foreign key(idAutor) references Autors(idAutor) on delete cascade on update cascade);

create table Instances(
idInstance  int primary key,
idBook int,
foreign key(idBook) references Books(idBook) on delete cascade on update cascade);

create table BookDelivery(
idBookDelivery int auto_increment primary key,
idReader int,
idInstance int,
idLibrarian int,
dateOfIssue date,
returnDate date,
foreign key (idReader) references Readers(idReader) on delete cascade on update cascade,
foreign key (idInstance) references Instances(idInstance) on delete cascade on update cascade,
foreign key (idLibrarian) references Librarians(idLibrarian) on delete cascade on update cascade);

delimiter //

create trigger AfterBookDeliveryInsert
after insert on BookDelivery for each row
begin
declare addedIdBook integer;

set addedIdBook = (select idBook from Instances where idInstance = new.idInstance);

update Books set countInStock = countInStock - 1 where idBook = addedIdBook;
end//

create trigger AfterBookDeliveryUpdate
after update on BookDelivery for each row
begin
declare addedIdBook integer;

set addedIdBook = (select idBook from Instances where idInstance = new.idInstance);

update Books set countInStock = countInStock + 1 where idBook = addedIdBook;
end//

create trigger AfterInstancesInsert
after insert on Instances for each row
begin
declare addedIdBook integer;

set addedIdBook = new.idBook;

update Books set countInStock = countInStock + 1 where idBook = addedIdBook;
end//

create trigger AfterInstancesDelete
after delete on Instances for each row
begin
declare addedIdBook integer;

set addedIdBook = old.idBook;

update Books set countInStock = countInStock - 1 where idBook = addedIdBook;
end//

delimiter ;

insert into Librarians values(default,"Драпун Александр Игоревич","+375447533262","dr.sasha2602@mail.ru","123");

insert into Readers values(default,"Винничук Родион Тарасович","+375298984353","vinnichuk.rodion@mail.ru"),
(default,"Куракин Алексей Тарасович","+375298984353","vinnichuk.rodion@mail.ru"),
(default,"Козлов Александр Григорьевич","+375298984353","vinnichuk.rodion@mail.ru"),
(default,"Павлов Алексей Семеныч","+375298984353","vinnichuk.rodion@mail.ru");

insert into Autors values(default,"Еврем Иван Николаевич","Жил долго");

insert into Books values(default,"Библия",1,0,"Новый завет","1.jpg",0),
(default,"Библия2",1,0,"Новый завет","1.jpg",0),
(default,"Библия3",1,0,"Новый завет","1.jpg",0),
(default,"Библия4",1,0,"Новый завет","1.jpg",0),
(default,"Библия5",1,0,"Новый завет","1.jpg",0);

insert into Instances values(1,1),(2,1),(3,2),(4,3),(5,4),(6,5);

insert into BookDelivery values(default,1,1,1,date(now()),null);


/*Все из выдачи книг*/
select fioReader,nameBook,fioLibrarian,dateOfIssue,returnDate,BookDelivery.idInstance
from BookDelivery inner join Books inner join Readers
inner join Librarians inner join Instances
on BookDelivery.idReader = Readers.idReader
and BookDelivery.idInstance = Instances.idInstance
and BookDelivery.idLibrarian = Librarians.idLibrarian;

/*Все из библиотекарей*/
select fioLibrarian,contactNumber,email,passwordLibrarian from Librarians;

/*Все из читателей*/
select fioReader,contactNumber,email from Readers;

/*Все из авторов*/
select fioAutor,biography from Autors;

/*Все из книг*/
select nameBook,fioAutor,countInStock,category,picture,yearOfIssue
from Books inner join Autors
on Books.idAutor = Autors.idAutor;

select nameBook,fioAutor,countInStock,category,picture,yearOfIssue 
from Books inner join Autors on Books.idAutor = Autors.idAutor where countInStock Like "%";

select * from Books;

/*Все экземпляры, которые есть на складе для определенной книги*/
select idInstance from Instances
inner join Books 
on Instances.idBook = Books.idBook
where not exists(Select * from BookDelivery where BookDelivery.idInstance = Instances.idInstance and
returnDate is null)
and nameBook = "Библия2";

/*Все фамилии у кого есть книжки на руках*/
select fioReader from Readers
where exists(Select * from BookDelivery where BookDelivery.idReader = Readers.idReader
and returnDate is null);

select * from BookDelivery;










