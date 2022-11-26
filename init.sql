CREATE TABLE IF NOT EXISTS public.profile (
    id serial PRIMARY KEY,
    username VARCHAR (100) UNIQUE NULL,
    name VARCHAR (50) NULL,
    password VARCHAR (100) NOT NULL,
    photo VARCHAR(100000) NULL,
    about VARCHAR(1000) NULL,
    role VARCHAR(10) NOT NULL
);

CREATE TABLE IF NOT EXISTS public.activity (
    id serial PRIMARY KEY,
    title VARCHAR (100) NOT NULL,
    detail VARCHAR (1000) NOT NULL,
    location VARCHAR(100) NULL,
    date timestamp default NULL,
    phonenumber VARCHAR(100) NULL,
    ownerprofileid NUMERIC (7) NOT NULL,
    capacity NUMERIC(7) NULL
);

CREATE TABLE IF NOT EXISTS profile_activity (
    id serial PRIMARY KEY,
    activityId NUMERIC (7) NOT NULL,
    profileId NUMERIC (7) NOT NULL
);