CREATE TABLE IF NOT EXISTS profile_activity (
    id serial PRIMARY KEY,
    profileId NUMERIC (7) NOT NULL,
    activityId NUMERIC (7) NOT NULL
);