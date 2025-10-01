-- ============================================
-- 1. Create user if it does not exist
-- ============================================
DO
$$
BEGIN
   IF NOT EXISTS (SELECT FROM pg_roles WHERE rolname = 'UM_DbUser') THEN
      CREATE USER "UM_DbUser" WITH PASSWORD 'P455w0rd$';
   ELSE
      ALTER USER "UM_DbUser" WITH PASSWORD 'P455w0rd$';
   END IF;
END
$$;

-- ============================================
-- 2. Create database if it does not exist
-- ============================================
-- Trick: use SELECT with \gexec (psql feature) so it only executes when needed
-- This works both during Docker init and on re-runs.
SELECT 'CREATE DATABASE "usermanagement_db" OWNER "UM_DbUser";'
WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = 'usermanagement_db')
\gexec

-- ============================================
-- 3. Ensure privileges and ownership (safe re-run)
-- ============================================
ALTER DATABASE "usermanagement_db" OWNER TO "UM_DbUser";
GRANT ALL PRIVILEGES ON DATABASE "usermanagement_db" TO "UM_DbUser";
