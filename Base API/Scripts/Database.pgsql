CREATE DATABASE "app_db";
CREATE USER "app_db_user" WITH ENCRYPTED PASSWORD 'AppDbUser@123';

GRANT ALL PRIVILEGES ON DATABASE "app_db" TO "app_db_user";
ALTER DATABASE "app_db" OWNER TO "app_db_user";

