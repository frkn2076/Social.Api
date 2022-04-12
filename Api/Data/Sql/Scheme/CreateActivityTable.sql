CREATE TABLE IF NOT EXISTS activity (
    id serial PRIMARY KEY,
    title VARCHAR (100) NOT NULL,
    detail VARCHAR (100) NOT NULL,
    location VARCHAR(100) NULL,
    date timestamp default NULL
);