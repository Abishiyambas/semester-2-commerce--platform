CREATE TABLE IF NOT EXISTS customers(
id serial PRIMARY KEY,
username VARCHAR(50) UNIQUE NOT NULL,
password VARCHAR(50) NOT NULL,
email VARCHAR(355) UNIQUE NOT NULL,
created_on TIMESTAMP NOT NULL,
last_login TIMESTAMP
);

CREATE TABLE IF NOT EXISTS products(
id serial PRIMARY KEY,
name VARCHAR(150) UNIQUE NOT NULL,
amount int NOT NULL,
price int NOT NULL,
);

CREATE TABLE IF NOT EXISTS orders(
order_id serial PRIMARY KEY,
customer_id integer NOT NULL REFERENCES customers(id),
product_id integer NOT NULL REFERENCES products(id),
price integer NOT NULL,
date TIMESTAMP NOT NULL
);

CREATE TABLE IF NOT EXISTS order_lines (
order_id integer, 
product_id integer, 
amount integer NOT NULL
CONSTRAINT order_lines_id PRIMARY KEY (order_id, product_id),
FOREIGN KEY (order_id) REFERENCES orders(order_id),
FOREIGN KEY (product_id) REFERENCES products(product_id)
);

CREATE TABLE IF NOT EXISTS order_tracking (
  current_status VARCHAR(50),
  courier VARCHAR(50),
  order_id int,
  product_id int,
  CONSTRAINT order_tracking_id PRIMARY KEY (order_id, product_id),
  FOREIGN KEY(order_id, product_id) REFERENCES order_lines(order_id, product_id)
);

ALTER TABLE orders DROP COLUMN date;
ALTER TABLE orders ADD COLUMN product_id integer NOT NULL REFERENCES products(id);
ALTER TABLE orders ADD COLUMN price integer;
ALTER TABLE orders ADD COLUMN date TIMESTAMP;

INSERT INTO customers (id, username, password, email, created_on, last_login) VALUES
(2, 'Bob', 'qwerty123', 'BobBuilder@bogusemail.com',  CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
(3, 'Charlie', 'letmein99', 'CharlieBrown@bogusemail.com',  CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
(4, 'Dana', '123abcDEF', 'DanaWhite@bogusemail.com',  CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
(5, 'Eve', 'hunter2!', 'EveHacker@bogusemail.com',  CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
(6, 'Frank', 'ilovepizza', 'FrankCastle@bogusemail.com',  CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
(7, 'Grace', 'password321', 'GraceHopper@bogusemail.com',  CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
(8, 'Hank', 'sunshine22', 'HankHill@bogusemail.com',  CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
(9, 'Ivy', 'welcome1', 'IvyLeague@bogusemail.com',  CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
(10, 'Jack', 'monkey123', 'JackSparrow@bogusemail.com',  CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);


ALTER TABLE products DROP COLUMN price;
ALTER TABLE products ADD COLUMN price int;

INSERT INTO products (id, name, amount, price) VALUES
(1, 'Laptop', 34, 3500),
(2, 'Smartphone', 58, 999),
(3, 'Oven', 12, 450),
(4, 'Tablet', 27, 650),
(5, 'Playstation', 19, 1200),
(6, 'Smartwatch', 44, 299),
(7, 'Refrigerator', 7, 1800),
(8, 'Monitor', 25, 400),
(9, 'Headphones', 63, 150),
(10, 'Microwave', 15, 320);

INSERT INTO orders (order_id, customer_id, product_id, price, date) VALUES (1,1,1,3500,CURRENT_TIMESTAMP), 
(2,2,2,999,CURRENT_TIMESTAMP),
(3,4,6,299,CURRENT_TIMESTAMP),
(4,7,3,450,CURRENT_TIMESTAMP),
(5,9,1,3500,CURRENT_TIMESTAMP),
(6,2,10,320,CURRENT_TIMESTAMP),
(7,5,5,1200,CURRENT_TIMESTAMP),
(8,1,2,999,CURRENT_TIMESTAMP),
(9,3,2,999,CURRENT_TIMESTAMP),
(10,6,2,999,CURRENT_TIMESTAMP);

INSERT INTO order_lines (order_id, product_id, amount) VALUES (1,1,1),
(2,2,3),
(3,6,3),
(4,3,3),
(5,2,1),
(6,10,1),
(7,5,3),
(8,2,2),
(9,2,3),
(10,2,1);

INSERT INTO order_tracking (current_status, courier, order_id, product_id) VALUES ('Delivered', 'GLS', 1,1),
('Delivered', 'GLS', 2,2),
('Delivered', 'GLS', 3,6),
('Delivered', 'Postnord', 4,3),
('Delivered', 'Postnord', 5,2),
('En route', 'GLS', 6,10),
('En route', 'GLS', 7,5),
('En route', 'Postnord', 8,2),
('Packing', 'Postnord', 9,2),
('Packing', 'Postnord', 10,2);

CREATE TABLE employee (
id SERIAL PRIMARY KEY,
username VARCHAR(50) NOT NULL,
name VARCHAR(100) NOT NULL,
mail VARCHAR(100) UNIQUE NOT NULL,
address TEXT,
password VARCHAR(100) NOT NULL
);

INSERT INTO employee (username, name, mail, address, password)
VALUES
    ('jsmith', 'John Smith', 'john.smith@example.com', '123 Main St, New York', 'pass123'),
    ('ajensen', 'Anna Jensen', 'anna.jensen@example.com', '45 Elm St, Copenhagen', 'secret456'),
    ('mkumar', 'Mohan Kumar', 'mohan.kumar@example.com', '9 Tech Park Rd, Bangalore', 'mohan789'),
    ('admin', 'admin-1', 'admin@example.com', '123 Main St, New York', 'admin123');


SELECT * FROM customers;
SELECT * FROM products;
SELECT * FROM orders;
SELECT * FROM order_lines;
SELECT * FROM order_tracking;

SELECT orders.order_id, orders.customer_id, orders.product_id, order_lines.amount, order_tracking.current_status, orders.price, orders.date FROM orders
  INNER JOIN order_lines ON orders.order_id = order_lines.order_id
  INNER JOIN order_tracking ON orders.order_id = order_tracking.order_id;
