SELECT * FROM customers;

CREATE TABLE customers(
id serial PRIMARY KEY,
username VARCHAR(50) UNIQUE NOT NULL,
password VARCHAR(50) NOT NULL,
email VARCHAR(355) UNIQUE NOT NULL,
name VARCHAR(50) NOT NULL,
last_name VARCHAR(50) NOT NULL,
vip_customer BOOLEAN NOT NULL,
created_on TIMESTAMP NOT NULL,
last_login TIMESTAMP
);


SELECT * FROM products;

CREATE TABLE products(
id serial PRIMARY KEY,
name VARCHAR(150) UNIQUE NOT NULL,
amount int NOT NULL,
product_price int NOT NULL
);


SELECT * FROM orders;

CREATE TABLE orders(
order_id serial PRIMARY KEY,
customer_id integer NOT NULL REFERENCES customers(id), 
product_id integer NOT NULL REFERENCES product(id),
date TIMESTAMP NOT NULL
);


SELECT * FROM order_lines;

CREATE TABLE order_lines(
order_id INT, 
product_id INT, 
employee_id INT, 
amount integer NOT NULL,
PRIMARY KEY (order_id, product_id),
FOREIGN KEY (order_id) REFERENCES orders(order_id),
FOREIGN KEY (product_id) REFERENCES products(id),
FOREIGN KEY (employee_id) REFERENCES employee(id)
);


SELECT * FROM employee;

CREATE TABLE employee(
id serial PRIMARY KEY,
username VARCHAR(50) UNIQUE NOT NULL,
password VARCHAR(50) NOT NULL,
email VARCHAR(355) UNIQUE NOT NULL,
name VARCHAR(50) NOT NULL,
last_name VARCHAR(50) NOT NULL,
address VARCHAR (350) NOT NULL,
salary INT NOT NULL
);


SELECT * FROM order_tracking;

CREATE TABLE order_tracking(
status VARCHAR(50),
eta VARCHAR(50),
courier VARCHAR(50),
order_id INT,
product_id INT,
PRIMARY KEY(order_id, product_id),
FOREIGN KEY(order_id, product_id) REFERENCES order_lines(order_id, product_id)
);
jk