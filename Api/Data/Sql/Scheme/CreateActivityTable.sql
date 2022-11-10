CREATE TABLE IF NOT EXISTS activity (
    id serial PRIMARY KEY,
    title VARCHAR (100) NOT NULL,
    detail VARCHAR (1000) NOT NULL,
    location VARCHAR(100) NULL,
    date timestamp default NULL,
    phonenumber VARCHAR(100) NULL,
    ownerprofileid NUMERIC (7) NOT NULL
);