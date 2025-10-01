-- ============================================
-- 1. Create user if it does not exist
-- ============================================
DO
$$
BEGIN
   IF NOT EXISTS (SELECT FROM pg_roles WHERE rolname = 'FIN_DbUser') THEN
      CREATE USER "FIN_DbUser" WITH PASSWORD 'FiNw0rd$';
   ELSE
      ALTER USER "FIN_DbUser" WITH PASSWORD 'FiNw0rd$';
   END IF;
END
$$;

-- ============================================
-- 2. Create database if it does not exist
-- ============================================
-- Trick: use SELECT with \gexec (psql feature) so it only executes when needed
-- This works both during Docker init and on re-runs.
SELECT 'CREATE DATABASE "financial_db" OWNER "FIN_DbUser";'
WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = 'financial_db')
\gexec

-- ============================================
-- 3. Ensure privileges and ownership (safe re-run)
-- ============================================
ALTER DATABASE "financial_db" OWNER TO "FIN_DbUser";
GRANT ALL PRIVILEGES ON DATABASE "financial_db" TO "FIN_DbUser";
