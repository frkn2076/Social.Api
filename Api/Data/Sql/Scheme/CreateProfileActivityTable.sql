CREATE TABLE IF NOT EXISTS profile_activity (
    id serial PRIMARY KEY,
    activityId NUMERIC (7) NOT NULL,
    profileId NUMERIC (7) NOT NULL
);