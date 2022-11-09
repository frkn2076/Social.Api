CREATE TABLE IF NOT EXISTS public.activity (
    id serial PRIMARY KEY,
    title VARCHAR (100) NOT NULL,
    detail VARCHAR (100) NOT NULL,
    location VARCHAR(100) NULL,
    date timestamp default NULL
);

CREATE TABLE IF NOT EXISTS public.activity (
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

CREATE TABLE IF NOT EXISTS public.activity (
    id serial PRIMARY KEY,
    activityId NUMERIC (7) NOT NULL,
    profileId NUMERIC (7) NOT NULL
);