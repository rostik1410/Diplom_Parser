create Table Product_Info(
Id int primary key identity(1,1) not null,
Category varchar(50) not null,
Product_name varchar(500) not null,
Product_link varchar(500) not null,
Product_img_link varchar(500) not null
);

drop Table Product_Info

delete  from Product_Info
select * from Product_Info where Category='phone'