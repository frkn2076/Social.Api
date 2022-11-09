CREATE TABLE IF NOT EXISTS profile (
    id serial PRIMARY KEY,
    username VARCHAR (100) UNIQUE NULL,
    email VARCHAR (100) UNIQUE NULL,
    name VARCHAR (20) NULL,
    surname VARCHAR (20) NULL,
    password VARCHAR (100) NOT NULL,
    photo VARCHAR(1000) NULL,
    about VARCHAR(1000) NULL,
    refreshToken VARCHAR(1000) NULL,
    expireDate timestamp default NULL,
    role VARCHAR(10) NOT NULL
);