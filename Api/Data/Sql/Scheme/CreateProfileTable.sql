CREATE TABLE IF NOT EXISTS profile (
    id serial PRIMARY KEY,
    username VARCHAR (100) UNIQUE NULL,
    name VARCHAR (50) NULL,
    password VARCHAR (100) NOT NULL,
    photo VARCHAR(100000) NULL,
    about VARCHAR(1000) NULL,
    role VARCHAR(10) NOT NULL
);