CREATE DATABASE "brokerhood";
CREATE USER "brokerhood_user" WITH ENCRYPTED PASSWORD 'Brokerhood@123';

GRANT ALL PRIVILEGES ON DATABASE "brokerhood" TO "brokerhood_user";
ALTER DATABASE "brokerhood" OWNER TO "brokerhood_user";

