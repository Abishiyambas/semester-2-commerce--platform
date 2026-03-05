-- Delete old data  
DROP TABLE IF EXISTS price_history;  
DROP TABLE IF EXISTS discounts;  
DROP TABLE IF EXISTS reviews;  
DROP TABLE IF EXISTS products;  
DROP TABLE IF EXISTS suppliers;  
DROP TABLE IF EXISTS categories;  
DROP TABLE IF EXISTS employees;  

-- Create new tables  
CREATE TABLE employees(  
  employee_id SERIAL PRIMARY KEY,  
  name varchar(30) NOT NULL,  
  password varchar(256) NOT NULL,  
  authorized BOOLEAN NOT NULL DEFAULT false  
);  

CREATE TABLE categories(  
  category_id SERIAL PRIMARY KEY,  
  name VARCHAR(256) UNIQUE NOT NULL  
);  

CREATE TABLE suppliers(  
  supplier_id SERIAL PRIMARY KEY,  
  name VARCHAR(256) UNIQUE NOT NULL,  
  phone VARCHAR(50) NOT NULL  
);  

CREATE OR REPLACE FUNCTION generate_unique_sku()
RETURNS TEXT AS $$
DECLARE
    new_sku TEXT;
    exists BOOLEAN;
BEGIN
    LOOP
        new_sku := lpad((trunc(random()*900)+100)::text, 3, '0') || '.' ||          -- 1. random positive float under 1 (0.someNumbers)
               lpad((trunc(random()*900)+100)::text, 3, '0') || '.' ||              -- 2. this float * 900, the value can now have a max just under 900, so 899,99999
               lpad((trunc(random()*90)+10)::text, 2, '0');                         -- 3. trunc remove the decimal part, giving us an integer insted
        SELECT EXISTS(SELECT 1 FROM products WHERE sku = new_sku) INTO exists;      -- 4. the value + 100 bumps its max up to 999
        EXIT WHEN NOT exists;                                                       -- 5. the ::text, 3, '0', turns the value into text, then pads the text with 0 until the text is 3 char long
    END LOOP;                                                                       -- 6. finally we check if the sku already exists in another product
    RETURN new_sku;
END;
$$ LANGUAGE plpgsql;

CREATE TABLE products(  
  product_id SERIAL PRIMARY KEY,  
  name VARCHAR(256) NOT NULL,  
  description TEXT,  
  sku VARCHAR(50)  DEFAULT generate_unique_sku(),
  status BOOLEAN NOT NULL DEFAULT TRUE,  
  price DECIMAL(10,2) NOT NULL CHECK (price >= 0),  
  max_stock INT NOT NULL CHECK (max_stock >= 0),  
  current_stock INT DEFAULT 0 CHECK (current_stock <= max_stock AND current_stock >= 0),  
  created_at TIMESTAMP DEFAULT now(),  
  updated_at TIMESTAMP DEFAULT now(),  
  category_id INT,  
  supplier_id INT NOT NULL,  
  FOREIGN KEY (category_id) REFERENCES categories(category_id) ON DELETE SET NULL,  
  FOREIGN KEY (supplier_id) REFERENCES suppliers(supplier_id) ON DELETE RESTRICT  
);  

CREATE TABLE discounts(  
  discount_id SERIAL PRIMARY KEY,  
  discount_percentage INT NOT NULL CHECK (discount_percentage BETWEEN 0 AND 100),  
  start_date TIMESTAMPTZ NOT NULL,  
  end_date TIMESTAMPTZ NOT NULL CHECK (end_date > start_date),  
  product_id INT NULL, 
  category_id INT NULL,
  FOREIGN KEY (product_id) REFERENCES products(product_id) ON DELETE CASCADE,
  FOREIGN KEY (category_id) REFERENCES categories(category_id) ON DELETE CASCADE,

  CONSTRAINT one_target_only
      CHECK (
          (product_id IS NOT NULL AND category_id IS NULL)
              OR
          (product_id IS NULL AND category_id IS NOT NULL)
          )
);  

CREATE TABLE price_history(  
  price_history_id SERIAL PRIMARY KEY,  
  changed_at TIMESTAMPTZ DEFAULT now(),  
  new_price DECIMAL(10,2) NOT NULL,  
  old_price DECIMAL(10,2) NOT NULL,  
  product_id INT NOT NULL,  
  FOREIGN KEY (product_id) REFERENCES products(product_id) ON DELETE CASCADE  
);  

CREATE TABLE reviews(  
  review_id SERIAL PRIMARY KEY,  
  comment TEXT,  
  rating INT NOT NULL CHECK (rating BETWEEN 1 AND 5),  
  product_id INT NOT NULL,  
  FOREIGN KEY (product_id) REFERENCES products(product_id) ON DELETE CASCADE  
);  

-- Create test data  
INSERT INTO categories (name) VALUES  
('Laptops'),  
('TVs'),  
('Headphones'),  
('Smartwatches'),  
('Accessories'),
('hardware'),
('desktops');

INSERT INTO employees(name, password, authorized)  
VALUES ('Arne', 'password', true),  
('Gustav Skou', 'password', false);  

INSERT INTO suppliers (name, phone) VALUES  
('TechSupply', '123456789'),  
('GadgetStore', '987654321'),  
('ElectronicWorld', '555123456');  

INSERT INTO products (name, description, sku, status, price, max_stock, current_stock, category_id, supplier_id) VALUES  
('Laptop 15" 16GB RAM', 'High performance laptop with 16GB RAM and 512GB SSD', '111,111,11', TRUE, 8999.95, 25, 25, 1, 1),  
('4K TV 55"', 'Ultra HD 4K television with HDR support', '222,222,22', TRUE, 4999.95, 10, 3,  2, 2),  
('Bluetooth Headphones', 'Noise-cancelling Bluetooth headphones', '333,333,33', TRUE, 1499.95, 50, 40, 3, 3),  
('Smartwatch Series 5', 'Smartwatch with fitness tracking and heart rate monitor', '444,444,44', TRUE, 2499.95, 40, 40, 4, 1),  
('Wireless Mouse', 'Ergonomic wireless mouse with 2-year battery life', '555,555,55', TRUE, 299.95, 100, 4,  5, 2),  
('Gaming Laptop 17" 32GB RAM', 'High-end gaming laptop with 32GB RAM and 1TB SSD', '666,666,66', TRUE, 12999.95, 15, 13,  1, 3),  
('Soundbar 2.1', 'Surround sound system with subwoofer', '777,777,77', TRUE, 3499.95, 20, 20,  2, 1),  
('Wireless Charging Pad', 'Qi wireless charging pad for smartphones', '888,888,88', TRUE, 499.95, 60, 7, 5, 3),

('SSD 1TB', 'Fast 1TB solid state drive for desktops and laptops', '999,999,99', TRUE, 1099.95, 30, 30, 6, 1),
('Vortex VFX graphics card', 'Whirlwind of power with 16GB VRAM', '101,101,10', TRUE, 6200, 25, 10, 6, 2),

('Gaming Desktop', 'High-end gaming desktop with RTX 4080', '202,202,20', TRUE, 18999.95, 10, 5, 7, 3),
('Office Desktop', 'Reliable office desktop with Intel i5 and 16GB RAM', '303,303,30', TRUE, 5999.95, 20, 10, 7, 1);

-- PRODUCT DISCOUNTS
INSERT INTO discounts (discount_percentage, start_date, end_date, product_id) VALUES
  -- ——— Past ———
  (10, '2025-02-01', '2025-03-01', 1),
  (15, '2025-03-01', '2025-03-15', 2),
  (5,  '2025-03-10', '2025-03-20', 3),

  -- ——— Active ——— (May 1 2025 is between start_date and end_date)
  (20, '2025-04-01', '2025-06-01', 4),
  (25, '2025-04-15', '2025-05-15', 5),
  (30, '2025-04-20', '2025-05-10', 6),
  (12, '2025-03-20', '2025-05-20', 7),

  -- ——— Upcoming ——— (start_date > May 1 2025)
  (35, '2025-06-05', '2025-07-05', 8),
  (40, '2025-08-01', '2025-09-01', 1);

-- CATEGORY DISCOUNTS
INSERT INTO discounts (discount_percentage, start_date, end_date, category_id) VALUES
   -- ——— Past ———
   (12, '2025-02-01', '2025-03-01', 1),  -- Laptops
   (18, '2025-03-01', '2025-04-01', 2),  -- TVs

   -- ——— Active ———
   (22, '2025-04-01', '2025-06-01', 3),  -- Headphones
   (28, '2025-04-15', '2025-05-15', 4),  -- Smartwatches
   (32, '2025-04-20', '2025-05-10', 5),  -- Accessories

   -- ——— Upcoming ———
   (38, '2025-06-10', '2025-07-10', 1),  -- Laptops again
   (42, '2025-07-01', '2025-08-01', 2);  -- TVs again


INSERT INTO price_history (new_price, old_price, product_id, changed_at) VALUES  
(10000, 10229, 1, '2025-03-01'),  
(9495, 10000, 1, '2025-03-02'),  
(8999.95, 9495, 1, '2025-04-11'),  
(1499.95, 1595, 3, '2025-04-11'),  
(2499.95, 2695, 4, '2025-04-11'),  
(299.95, 345, 5, '2025-04-11'),  
(12995.95, 13995, 6, '2025-04-11'),  
(3495.95, 3995, 7, '2025-04-11'),  
(499.95, 595, 8, '2025-04-11');  

INSERT INTO reviews (comment, rating, product_id) VALUES  
('Fantastisk laptop! Hurtig og pålidelig.', 5, 1),  
('', 5, 2),  
('', 4, 3),  
('Elsker min nye smartwatch, perfekt til træning.', 5, 4),  
('Musen er behagelig at bruge, men lidt dyr.', 3, 5),  
('', 5, 6),  
('Lyden er god, men subwooferen kunne være kraftigere.', 4, 7),  
('Virkelig praktisk oplader, men langsom opladning.', 3, 8),  
('', 3, 2),  
('  ', 5, 1);