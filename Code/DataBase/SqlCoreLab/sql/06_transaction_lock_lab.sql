-- Transaction, lock, and concurrency lab.
-- These blocks should be run manually in two different database sessions.

-- ============================================================
-- Lab 1: row lock wait
-- Session A:
-- BEGIN;
-- UPDATE lab_devices
-- SET device_name = device_name || '_A'
-- WHERE id = 1;
-- Keep this transaction open.
-- Later:
-- COMMIT;
-- or:
-- ROLLBACK;

-- Session B:
-- BEGIN;
-- UPDATE lab_devices
-- SET device_name = device_name || '_B'
-- WHERE id = 1;
-- This waits until Session A commits or rolls back.
-- COMMIT;

-- ============================================================
-- Lab 2: deadlock
-- Session A:
-- BEGIN;
-- UPDATE lab_devices SET version = version + 1 WHERE id = 1;
-- Wait, then run:
-- UPDATE lab_devices SET version = version + 1 WHERE id = 2;
-- COMMIT;

-- Session B:
-- BEGIN;
-- UPDATE lab_devices SET version = version + 1 WHERE id = 2;
-- Wait, then run:
-- UPDATE lab_devices SET version = version + 1 WHERE id = 1;
-- COMMIT;

-- ============================================================
-- Lab 3: optimistic lock pattern
-- Read current version first.
SELECT id, device_code, status, version
FROM lab_devices
WHERE id = 1;

-- Assume the version read by the application is 1.
-- UPDATE lab_devices
-- SET status = 'maintenance',
--     version = version + 1
-- WHERE id = 1
--   AND version = 1;

-- If affected rows = 0, another transaction changed the row first.

-- ============================================================
-- Lab 4: pessimistic lock pattern
-- BEGIN;
-- SELECT id, device_code, status, version
-- FROM lab_devices
-- WHERE id = 1
-- FOR UPDATE;
-- Do a short protected update.
-- UPDATE lab_devices
-- SET status = 'idle',
--     version = version + 1
-- WHERE id = 1;
-- COMMIT;
