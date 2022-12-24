CREATE TABLE IF NOT EXISTS public.profile (
    id serial PRIMARY KEY,
    username VARCHAR (100) UNIQUE NULL,
    name VARCHAR (50) NULL,
    password VARCHAR (100) NOT NULL,
    photo VARCHAR(10000000) NULL,
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
    capacity NUMERIC(7) NULL,
    category VARCHAR(100) NOT NULL
);

CREATE TABLE IF NOT EXISTS profile_activity (
    id serial PRIMARY KEY,
    activityId NUMERIC (7) NOT NULL,
    profileId NUMERIC (7) NOT NULL
);

CREATE TABLE IF NOT EXISTS public.chat_message (
    id serial PRIMARY KEY,
    authorId VARCHAR (50) NOT NULL,
    firstName VARCHAR (50) NOT NULL,
    lastName VARCHAR (50) NULL,
    createdAt NUMERIC (15) NOT NULL,
    messageId VARCHAR (50) NULL,
    status VARCHAR (50) NOT NULL,
    text VARCHAR (1000) NULL,
    type VARCHAR (50) NULL,
    height NUMERIC (7) NULL,
    width NUMERIC (7) NULL,
    image_name VARCHAR (200) NULL,
    size NUMERIC (7) NULL,
    uri VARCHAR (200) NULL,
    activityId NUMERIC (7) NOT NULL
);